using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Token usage of the chat request.
/// </summary>
public record ChatTokenUsage
{
    /// <summary>
    /// Output token count of generated text.
    /// </summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; init; }

    /// <summary>
    /// Input token count of messages.
    /// </summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; init; }
}