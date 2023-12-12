using Sdcb.DashScope.TrainingFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Sdcb.DashScope;

/// <summary>
/// Represents a client for interacting with the DashScope API.
/// </summary>
public class DashScopeClient : IDisposable
{
    internal readonly HttpClient _httpClient = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="DashScopeClient"/> class with the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key used for authentication.</param>
    /// <param name="httpClient">The HTTP client used for making requests. If null, a new instance of <see cref="HttpClient"/> will be created.</param>
    [SetsRequiredMembers]
    public DashScopeClient(string apiKey, HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        TrainingFiles = new TrainingFilesClient(this);
    }

    /// <summary>
    /// Model customization file management service, you can manage your training files in a unified way;
    /// a single upload allows for multiple reuses in model customization tasks.
    /// </summary>
    public TrainingFilesClient TrainingFiles { get; }

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
        HttpResponseMessage resp = await _httpClient.SendAsync(httpRequest, cancellationToken);
        return await ReadWrapperResponse<DashScopeTask>(resp, cancellationToken);
    }

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
        HttpResponseMessage resp = await _httpClient.SendAsync(msg, cancellationToken);
        return await ReadWrapperResponse<DashScopeTask>(resp, cancellationToken);
    }

    /// <summary>
    /// Queries the status of a task using the specified task ID.
    /// </summary>
    /// <param name="taskId">The ID of the task to query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task status response.</returns>
    public async Task<TaskStatusResponse> QueryTaskStatus(string taskId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage resp = await _httpClient.GetAsync($@"https://dashscope.aliyuncs.com/api/v1/tasks/{taskId}", cancellationToken);
        return await ReadWrapperResponse<TaskStatusResponse>(resp, cancellationToken);
    }

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
    public async Task<bool[]> FaceChainCheckImage(string[] imageUrls, string model = "facechain-facedetect", CancellationToken cancellationToken = default)
    {
        HttpRequestMessage msg = new(HttpMethod.Post, "https://dashscope.aliyuncs.com/api/v1/services/vision/facedetection/detect")
        {
            Content = JsonContent.Create(RequestWrapper.Create(model, new { images = imageUrls }), options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };
        HttpResponseMessage resp = await _httpClient.SendAsync(msg, cancellationToken);
        JsonElement result = await ReadWrapperResponse<JsonElement>(resp, cancellationToken);
        return result
            .GetProperty("is_face")
            .EnumerateArray()
            .Select(x => x.GetBoolean())
            .ToArray();
    }

    /// <summary>
    /// Disposes the underlying HTTP client.
    /// </summary>
    public void Dispose() => _httpClient.Dispose();

    internal async Task<T> ReadWrapperResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return (await ReadResponse<ResponseWrapper<T>>(response, cancellationToken)).Output;
    }

    internal async Task<T> ReadResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new DashScopeException(await response.Content.ReadAsStringAsync());
        }

        try
        {
            return (await response.Content.ReadFromJsonAsync<T>(options: null, cancellationToken))!;
        }
        catch (Exception e) when (e is NotSupportedException or JsonException)
        {
            throw new DashScopeException($"failed to convert following json into: {typeof(T).Name}: {await response.Content.ReadAsStringAsync()}", e);
        }
    }
}
