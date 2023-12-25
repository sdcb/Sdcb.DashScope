using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Represents the output of the Qwen-VL model.
/// </summary>
internal record ChatVLOutput
{
    /// <summary>
    /// The choices available in the output.
    /// </summary>
    [JsonPropertyName("choices")]
    public required MessageWrapper[] Choices { get; init; }

    // This field exists in API document but not present in actuall API response.
    ///// <summary>
    ///// There are 3 cases:
    ///// <list type="bullet">
    ///// <item><c>null</c> when generating</item>
    ///// <item><c>stop</c> when stopped</item>
    ///// <item><c>length</c> when content is too long</item>
    ///// </list>
    ///// </summary>
    //[JsonPropertyName("finish_reason")]
    //public required string? FinishReason { get; init; }
}

internal record MessageWrapper
{
    [JsonPropertyName("message")]
    public required ChatVLMessage Message { get; init; }
}