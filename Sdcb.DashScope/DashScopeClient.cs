using Sdcb.DashScope.FaceChains;
using Sdcb.DashScope.FineTunes;
using Sdcb.DashScope.StableDiffusion;
using Sdcb.DashScope.TextEmbedding;
using Sdcb.DashScope.TextGeneration;
using Sdcb.DashScope.TrainingFiles;
using Sdcb.DashScope.WanXiang;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.DashScope;

/// <summary>
/// Represents a client for interacting with the DashScope API.
/// </summary>
public class DashScopeClient : IDisposable
{
    internal readonly HttpClient HttpClient = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="DashScopeClient"/> class with the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key used for authentication.</param>
    /// <param name="httpClient">The HTTP client used for making requests. If null, a new instance of <see cref="System.Net.Http.HttpClient"/> will be created.</param>
    [SetsRequiredMembers]
    public DashScopeClient(string apiKey, HttpClient? httpClient = null)
    {
        HttpClient = httpClient ?? new HttpClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        TrainingFiles = new TrainingFilesClient(this);
        FaceChains = new FaceChainsClient(this);
        StableDiffusion = new StableDiffusionClient(this);
        WanXiang = new WanXiangClient(this);
        FineTunes = new FineTunesClient(this);
        TextGeneration = new TextGenerationClient(this);
        TextEmbedding = new TextEmbeddingClient(this);
    }

    /// <summary>
    /// Model customization file management service, you can manage your training files in a unified way;
    /// a single upload allows for multiple reuses in model customization tasks.
    /// </summary>
    public TrainingFilesClient TrainingFiles { get; }

    /// <summary>
    /// FaceChain Portrait Generation requires only two photos of a person to train and obtain a unique image of that individual,
    /// which can be used to mass-produce portraits in various styles.
    /// <para>
    /// FaceChain leverages the image generation capabilities of diffusion models,
    /// combined with LoRA training for the integration of portraits and styles,
    /// and layers a series of post-processing abilities to achieve portrait generation that encompasses similarity, realism, and aesthetic appeal.
    /// </para>
    /// </summary>
    public FaceChainsClient FaceChains { get; }

    /// <summary>
    /// The Stable Diffusion API provides a series of AI models that can be used to generate images from text.
    /// </summary>
    public StableDiffusionClient StableDiffusion { get; }

    /// <summary>
    /// Tongyi Wanxiang is a large-scale AI painting creation model based on the self-developed Composer generative framework,
    /// offering a range of image generation capabilities.
    /// </summary>
    public WanXiangClient WanXiang { get; }

    /// <summary>
    /// DashScope client for fine-tuning models.
    /// </summary>
    public FineTunesClient FineTunes { get; }

    /// <summary>
    /// LLM models clients that supports Qwen/open source LLMs.
    /// </summary>
    public TextGenerationClient TextGeneration { get; }

    /// <summary>
    /// Text embedding client.
    /// </summary>
    public TextEmbeddingClient TextEmbedding { get; }

    /// <summary>
    /// Queries the status of a task using the specified task ID.
    /// </summary>
    /// <param name="taskId">The ID of the task to query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task status response.</returns>
    public async Task<TaskStatusResponse> QueryTaskStatus(string taskId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage resp = await HttpClient.GetAsync($@"https://dashscope.aliyuncs.com/api/v1/tasks/{taskId}", cancellationToken);
        return await ReadWrapperResponse<TaskStatusResponse>(resp, cancellationToken);
    }

    /// <summary>
    /// Disposes the underlying HTTP client.
    /// </summary>
    public void Dispose() => HttpClient.Dispose();

    internal async Task<T> ReadWrapperResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return (await ReadResponse<ResponseWrapper<T, ImageTaskUsage>>(response, cancellationToken)).Output;
    }

    internal static async Task<T> ReadResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new DashScopeException(await response.Content.ReadAsStringAsync());
        }

        try
        {
            var debug = await response.Content.ReadAsStringAsync();
            return (await response.Content.ReadFromJsonAsync<T>(options: null, cancellationToken))!;
        }
        catch (Exception e) when (e is NotSupportedException or JsonException)
        {
            throw new DashScopeException($"failed to convert following json into: {typeof(T).Name}: {await response.Content.ReadAsStringAsync()}", e);
        }
    }
}
