using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace Sdcb.DashScope.FaceChains;

/// <summary>
/// FaceChain Portrait Generation requires only two photos of a person to train and obtain a unique image of that individual,
/// which can be used to mass-produce portraits in various styles.
/// <para>
/// FaceChain leverages the image generation capabilities of diffusion models,
/// combined with LoRA training for the integration of portraits and styles,
/// and layers a series of post-processing abilities to achieve portrait generation that encompasses similarity, realism, and aesthetic appeal.
/// </para>
/// </summary>
public class FaceChainsClient
{
    internal FaceChainsClient(DashScopeClient client)
    {
        Parent = client;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// <para>Detects whether the faces in the uploaded images meet the required standards for facechain fine-tuning.</para>
    /// <para>The detection dimensions include the number of faces, size, angle, illumination, clarity, etc.</para>
    /// <para>This model is not a mandatory step in the task flow and can be integrated as per business needs.</para>
    /// </summary>
    /// <param name="imageUrls">
    /// A string array of image URLs with the following specifications:
    /// <list type="bullet">
    /// <item>- Resolution between 256*256 and 4096*4096 pixels.</item>
    /// <item>- File size is no more than 5MB.</item>
    /// <item>- Supported formats: JPEG, PNG, JPG, WEBP.</item>
    /// </list>
    /// <para>The images should contain exactly one face; multi-face or no-face images are not supported.</para>
    /// <para>Face quality should preferably be a frontal face, larger than 128*128 pixels, with no obstructions such as sunglasses or hands, and without heavy makeup or excessive beautification.</para>
    /// <para>The image should not have complex lighting or shadows.</para>
    /// </param>
    /// <param name="model">The model parameter defines which model to use for detection, fixed to 'facechain-facedetect'.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>An array of boolean values indicating whether each image meets the face detection criteria.</returns>
    public async Task<bool[]> CheckImage(string[] imageUrls, string model = "facechain-facedetect", CancellationToken cancellationToken = default)
    {
        HttpRequestMessage msg = new(HttpMethod.Post, "https://dashscope.aliyuncs.com/api/v1/services/vision/facedetection/detect")
        {
            Content = JsonContent.Create(RequestWrapper.Create(model, new { images = imageUrls }), options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };
        HttpResponseMessage resp = await Parent.HttpClient.SendAsync(msg, cancellationToken);
        JsonElement result = await Parent.ReadWrapperResponse<JsonElement>(resp, cancellationToken);
        return result
            .GetProperty("is_face")
            .EnumerateArray()
            .Select(x => x.GetBoolean())
            .ToArray();
    }

}
