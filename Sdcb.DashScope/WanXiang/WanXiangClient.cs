using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Sdcb.DashScope.StableDiffusion;

namespace Sdcb.DashScope.WanXiang;

/// <summary>
/// Tongyi Wanxiang is a large-scale AI painting creation model based on the self-developed Composer generative framework,
/// offering a range of image generation capabilities.
/// </summary>
public class WanXiangClient
{
    internal WanXiangClient(DashScopeClient parent)
    {
        Parent = parent;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// Initiates an asynchronous request to replicate a style from a given input image using the defined AI model.
    /// </summary>
    /// <param name="input">The input information including the image to replicate the style from and other relevant data.</param>
    /// <param name="model">The model identifier indicating the model to be used, with the fixed value "wanx-style-repaint-v1".</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
    /// <returns>A Task representing the asynchronous operation that returns a <see cref="DashScopeTask"/> which contains information about the replication task.</returns>
    public async Task<DashScopeTask> StyleReplicate(StyleReplicationInput input, string model = "wanx-style-repaint-v1", CancellationToken cancellationToken = default)
    {
        HttpRequestMessage msg = new(HttpMethod.Post, "https://dashscope.aliyuncs.com/api/v1/services/aigc/image-generation/generation")
        {
            Content = JsonContent.Create(RequestWrapper.Create(model, input), options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };
        msg.Headers.TryAddWithoutValidation("X-DashScope-Async", "enable");
        HttpResponseMessage resp = await Parent.HttpClient.SendAsync(msg, cancellationToken);
        return await Parent.ReadWrapperResponse<DashScopeTask>(resp, cancellationToken);
    }

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
