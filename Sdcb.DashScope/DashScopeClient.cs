using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
    private readonly HttpClient _httpClient = null!;

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
            Content = JsonContent.Create(RequestWrapper.Create(model, prompt, parameters)),
        };
        httpRequest.Headers.TryAddWithoutValidation("X-DashScope-Async", "enable");
        HttpResponseMessage resp = await _httpClient.SendAsync(httpRequest, cancellationToken);
        return await ReadResponse<DashScopeTask>(resp, cancellationToken);
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
            Content = JsonContent.Create(RequestWrapper.Create(model, input))
        };
        msg.Headers.TryAddWithoutValidation("X-DashScope-Async", "enable");
        HttpResponseMessage resp = await _httpClient.SendAsync(msg, cancellationToken);
        return await ReadResponse<DashScopeTask>(resp, cancellationToken);
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
        return await ReadResponse<TaskStatusResponse>(resp, cancellationToken);
    }

    /// <summary>
    /// Disposes the underlying HTTP client.
    /// </summary>
    public void Dispose() => _httpClient.Dispose();

    private async Task<T> ReadResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new DashScopeException(await response.Content.ReadAsStringAsync());
        }

        try
        {
            return (await response.Content.ReadFromJsonAsync<ResponseWrapper<T>>(cancellationToken))!.Output;
        }
        catch (Exception e) when (e is NotSupportedException or JsonException)
        {
            throw new DashScopeException($"failed to convert following json into: {typeof(T).Name}: {await response.Content.ReadAsStringAsync()}", e);
        }
    }
}
