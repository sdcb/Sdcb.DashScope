using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// The output of chat request.
/// </summary>
public record ChatOutput
{
    /// <summary>
    /// Output content of the model.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    /// <summary>
    /// There are 3 cases:
    /// <list type="bullet">
    /// <item><c>null</c> when generating</item>
    /// <item><c>stop</c> when stopped</item>
    /// <item><c>length</c> when content is too long</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public required string? FinishReason { get; init; }
}
