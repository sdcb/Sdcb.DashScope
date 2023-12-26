using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.DashScope.TextEmbedding;

/// <summary>
/// Text embedding client.
/// </summary>
public class TextEmbeddingClient
{
    internal TextEmbeddingClient(DashScopeClient parent)
    {
        Parent = parent;
    }

    internal DashScopeClient Parent { get; }

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

public record EmbeddingOutput
{
    [JsonPropertyName("embeddings")]
    public required EmbeddingItem[] Embeddings { get; init; }
}

public record EmbeddingItem
{
    [JsonPropertyName("text_index")]
    public int TextIndex { get; init; }

    [JsonPropertyName("embedding")]
    public required double[] Embedding { get; init; }
}

public record EmbeddingUsage
{
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }
}

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

    public required IReadOnlyList<string> InputTexts { get; init; }

    public EmbeddingType EmbeddingType { get; init; } = EmbeddingType.Document;

    internal RequestWrapper ToRequest() => RequestWrapper.Create(Model, new { texts = InputTexts }, new { text_type = EmbeddingType.ToString().ToLowerInvariant() });

    public static EmbeddingRequest FromV1(IReadOnlyList<string> texts, EmbeddingType embeddingType = EmbeddingType.Document)
    {
        return new EmbeddingRequest
        {
            Model = "text-embedding-v1", 
            InputTexts = texts, 
            EmbeddingType = embeddingType
        };
    }

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

public enum EmbeddingType
{
    Document,
    Query,
}
