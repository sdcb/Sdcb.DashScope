using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.StableDiffusion;

/// <summary>
/// The Stable Diffusion API provides a series of AI models that can be used to generate images from text.
/// </summary>
public class StableDiffusionClient
{
    internal StableDiffusionClient(DashScopeClient parent)
    {
        Parent = parent;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// Initiates an asynchronous request to generate images from text using the specified AI model.
    /// </summary>
    /// <param name="prompt">The textual description or prompts that guide the image generation process.</param>
    /// <param name="parameters">Optional parameters for fine-tuning the image generation such as resolution, aspect ratio, etc.</param>
    /// <param name="model">The name of the model to use for image generation. Allowed values are "stable-diffusion-xl" or "stable-diffusion-v1.5".</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
    /// <returns>A Task representing the asynchronous operation that returns a <see cref="DashScopeTask"/> which contains information about the generation task.</returns>
    public async Task<DashScopeTask> Text2Image(Text2ImagePrompt prompt, Text2ImageParams? parameters = null, string model = "stable-diffusion-xl", CancellationToken cancellationToken = default)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, @"https://dashscope.aliyuncs.com/api/v1/services/aigc/text2image/image-synthesis")
        {
            Content = JsonContent.Create(RequestWrapper.Create(model, prompt, parameters), options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }),
        };
        httpRequest.Headers.TryAddWithoutValidation("X-DashScope-Async", "enable");
        HttpResponseMessage resp = await Parent.HttpClient.SendAsync(httpRequest, cancellationToken);
        return await Parent.ReadWrapperResponse<DashScopeTask>(resp, cancellationToken);
    }
}
