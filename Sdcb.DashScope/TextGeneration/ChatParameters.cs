using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Parameters for LLM execution.
/// </summary>
public record ChatParameters
{
    /// <summary>
    /// Format of the result - "text" for old text version, "message" for OpenAI compatible message.
    /// <para>This field must be "text" in for language model, not been used in VL model.</para>
    /// </summary>
    [JsonPropertyName("result_format")]
    public string? ResultFormat { get; set; }

    /// <summary>
    /// Seed for the random number generator to control the randomness of the model's generation.
    /// Using the same seed allows for the reproducibility of the model's output.
    /// <para>This field is optional. Default value is 1234.</para>
    /// </summary>
    [JsonPropertyName("seed")]
    public ulong? Seed { get; set; }

    /// <summary>
    /// Limits the number of tokens to generate. The limit sets the maximum but does not guarantee
    /// that exactly that many tokens will be generated. This field is optional.
    /// <para>qwen-turbo and qwen-max-longcontext have a maximum and default of 1500,</para>
    /// <para>qwen-max, qwen-max-1201, and qwen-plus have a maximum and default of 2048.</para>
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Probability threshold for nucleus sampling. Taking the value of 0.8, for example,
    /// only retains tokens with a cumulative probability sum greater than or equal to 0.8.
    /// <para>The value range is (0,1.0). The larger the value, the higher the randomness;</para>
    /// <para>the smaller the value, the lower the randomness. This field is optional.</para>
    /// <para>Default value is 0.8. Note that the value should not be greater than or equal to 1.</para>
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    /// <summary>
    /// Size of the candidate set for sampling. For instance, when set to 50, only the top 50 tokens
    /// will be considered for sampling. This field is optional. A larger value increases randomness;
    /// a smaller value increases determinism. Note: If top_k is null or greater than 100,
    /// the top_k strategy is not used, and only top_p is effective. The default is null.
    /// </summary>
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }

    /// <summary>
    /// Penalty to apply for repetition to reduce redundancy in the model's generation.
    /// A value of 1.0 means no penalty. This field is optional.
    /// <para>Default value is 1.1.</para>
    /// </summary>
    [JsonPropertyName("repetition_penalty")]
    public float? RepetitionPenalty { get; set; }

    /// <summary>
    /// Controls the randomness and diversity degree in text generation.
    /// High temperature values reduce the peakiness of the probability distribution,
    /// allowing more low-probability words to be chosen, resulting in more diverse output.
    /// <para>
    /// Low values increase peakiness, making high-probability words more likely to be chosen,
    /// resulting in more deterministic output. This field is optional.
    /// Value range is [0, 2). The system's default value is 1.0.
    /// </para>
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    /// <summary>
    /// Specifies the content that, upon generation, should stop the model from further output.
    /// <para>This can be a string or list of strings, a single list of token IDs or a list of token ID lists.</para>
    /// <para>For example, if the stop is set as "hello", generation stops before producing "hello";</para>
    /// <para>if set as [37763, 367], generation stops before producing the token IDs equivalent to "Observation".</para>
    /// <para>
    /// Note, this field is optional and list mode does not support mixing strings and token IDs;
    /// they should all be of the same type.
    /// </para>
    /// </summary>
    [JsonPropertyName("stop")]
    public object? Stop { get; set; }

    /// <summary>
    /// Controls whether to take search results into account during generation.
    /// <para>Note: enabling search does not guarantee that search results will be used.</para>
    /// <para>
    /// If search is enabled, the model will consider the search results as part of the prompt
    /// to potentially generate text that includes the results.
    /// </para>
    /// <para>This field is optional and defaults to false.</para>
    /// </summary>
    [JsonPropertyName("enable_search")]
    public bool? EnableSearch { get; set; }

    /// <summary>
    /// Controls whether to enable incremental output mode.
    /// <para>
    /// The default value is false, meaning subsequent outputs will contain already completed segments.
    /// When set to true, incremental output mode is activated, and subsequent outputs will not contain
    /// previous segments. The full output would need to be constructed incrementally by the user.
    /// </para>
    /// This field is optional and only applicable in streaming output modes.
    /// </summary>
    [JsonPropertyName("incremental_output")]
    public bool? IncrementalOutput { get; set; }
}