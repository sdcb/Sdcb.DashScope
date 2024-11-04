using System;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Represents the output details in the assistant's response.
/// </summary>
public record ChatResponse
{
    /// <summary>
    /// Gets the list of choices available in the output.
    /// </summary>
    [JsonPropertyName("choices")]
    public required ChatChoice[] Choices { get; init; }

    /// <summary>
    /// Gets the text content of the first choice's message.
    /// </summary>
    [Obsolete("use Choices[0].Message.Content")]
    public string Text => Choices[0].Message.Content;

    /// <summary>
    /// Gets the reason for the completion of the first choice.
    /// </summary>
    [Obsolete("use Choices[0].Message.FinishReason")]
    public string FinishReason => Choices[0].FinishReason;
}
