using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.DashScope.TextEmbedding;

/// <summary>
/// Provides services related to text embedding operations by communicating with DashScope API.
/// </summary>
public class TextEmbeddingClient
{
    internal TextEmbeddingClient(DashScopeClient parent)
    {
        Parent = parent;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// Asynchronously retrieves text embeddings for the given request parameters.
    /// </summary>
    /// <param name="request">An <see cref="EmbeddingRequest"/> object containing the request parameters for text embedding.</param>
    /// <param name="cancellationToken">A cancellation token used to propagate notification that the operation should be canceled.</param>
    /// <returns>A <see cref="ResponseWrapper{TData, TMetadata}"/> containing the embeddings output and usage information.</returns>
    public async Task<ResponseWrapper<EmbeddingOutput, EmbeddingUsage>> GetEmbeddings(EmbeddingRequest request, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, @"https://dashscope.aliyuncs.com/api/v1/services/embeddings/text-embedding/text-embedding")
        {
            Content = JsonContent.Create((object)request.ToRequest()),
        };
        HttpResponseMessage resp = await Parent.HttpClient.SendAsync(httpRequest, cancellationToken);
        return await DashScopeClient.ReadResponse<ResponseWrapper<EmbeddingOutput, EmbeddingUsage>>(resp, cancellationToken);
    }
}

/// <summary>
/// Represents the output of a text embedding operation, containing an array of embedding items.
/// </summary>
public record EmbeddingOutput
{
    /// <summary>
    /// An array of <see cref="EmbeddingItem"/> objects representing the embedding results for each input text.
    /// </summary>
    [JsonPropertyName("embeddings")]
    public required EmbeddingItem[] Embeddings { get; init; }
}

/// <summary>
/// Represents a single text embedding result, including the original text's index and the computed embedding vector.
/// </summary>
public record EmbeddingItem
{
    /// <summary>
    /// The index of the text in the original input list for which this embedding was generated.
    /// </summary>
    [JsonPropertyName("text_index")]
    public int TextIndex { get; init; }

    /// <summary>
    /// The computed embedding vector for the associated text.
    /// </summary>
    [JsonPropertyName("embedding")]
    public required double[] Embedding { get; init; }
}

/// <summary>
/// Represents usage information of the text embedding service, e.g., the total number of tokens processed.
/// </summary>
public record EmbeddingUsage
{
    /// <summary>
    /// The total number of tokens that were processed to generate text embeddings.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }
}

/// <summary>
/// Represents a request for generating text embeddings, specifying the model version, text inputs, and embedding type.
/// </summary>
public record EmbeddingRequest
{
    /// <summary>
    /// Specifies the version of the text embedding model to be used, known models:
    /// <list type="bullet">
    /// <item>
    /// <c>text-embedding-v1</c>: The initial version of the model with embedding capabilities for core languages including Chinese, English, Spanish, French, Portuguese, and Indonesian.
    /// </item>
    /// <item>
    /// <c>text-embedding-v2</c>: Enhanced capabilities including additional language support and improved overall performance from advanced training strategies and normalization processes.
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>text-embedding-v2</c> model is an upgraded version of the <c>text-embedding-v1</c> and includes language expansions, 
    /// offering vectorization support for languages such as Japanese, Korean, German, and Russian. 
    /// </para>
    /// <para>
    /// It also features improvements in the underlying pretrained models and Sparse Fine-tuning (SFT) strategies, 
    /// resulting in better performance as evidenced by public data benchmarks such as MTEB and CMTEB. 
    /// </para>
    /// Moreover, the output vectors from <c>text-embedding-v2</c> are normalized by default.
    /// </remarks>
    public required string Model { get; init; }

    /// <summary>
    /// The list of input texts for which embeddings are requested.
    /// </summary>
    public required IReadOnlyList<string> InputTexts { get; init; }

    /// <summary>
    /// Specifies the type of text embeddings to generate, such as document-level or query-level embeddings.
    /// </summary>
    public EmbeddingType EmbeddingType { get; init; } = EmbeddingType.Document;

    internal RequestWrapper ToRequest() => RequestWrapper.Create(Model, new { texts = InputTexts }, new { text_type = EmbeddingType.ToString().ToLowerInvariant() });

    /// <summary>
    /// Creates an <see cref="EmbeddingRequest"/> instance configured for the version 1 text embedding model (text-embedding-v1).
    /// This model has embedding capabilities for core languages including Chinese, English, Spanish, French, Portuguese, and Indonesian.
    /// </summary>
    /// <param name="texts">The collection of input texts for which embeddings are to be generated.</param>
    /// <param name="embeddingType">The type of embeddings to be generated, defaulted to Document.</param>
    /// <returns>An instance of <see cref="EmbeddingRequest"/> configured for the text-embedding-v1 model.</returns>
    public static EmbeddingRequest FromV1(IReadOnlyList<string> texts, EmbeddingType embeddingType = EmbeddingType.Document)
    {
        return new EmbeddingRequest
        {
            Model = "text-embedding-v1", 
            InputTexts = texts, 
            EmbeddingType = embeddingType
        };
    }

    /// <summary>
    /// Creates an <see cref="EmbeddingRequest"/> instance configured for the version 2 text embedding model (text-embedding-v2).
    /// This upgraded model includes language expansions and improved performance, supporting additional languages such as Japanese, Korean, German, and Russian.
    /// </summary>
    /// <param name="texts">The collection of input texts for which embeddings are to be generated.</param>
    /// <param name="embeddingType">The type of embeddings to be generated, defaulted to Document.</param>
    /// <returns>An instance of <see cref="EmbeddingRequest"/> configured for the text-embedding-v2 model.</returns>
    public static EmbeddingRequest FromV2(IReadOnlyList<string> texts, EmbeddingType embeddingType = EmbeddingType.Document)
    {
        return new EmbeddingRequest
        {
            Model = "text-embedding-v2",
            InputTexts = texts,
            EmbeddingType = embeddingType
        };
    }
}

/// <summary>
/// Defines the possible types of embeddings that can be generated, indicating the context in which the text is used.
/// </summary>
public enum EmbeddingType
{
    /// <summary>
    /// Indicates that the embedding is for a document, suggesting that the text is expected to be longer and may require a representation that captures more context.
    /// </summary>
    Document,

    /// <summary>
    /// Indicates that the embedding is for a query, suggesting that the text is typically shorter and focused, such as a search query or a keyword.
    /// </summary>
    Query,
}