using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Represents a single choice in the output.
/// </summary>
public record ChatChoice
{
    /// <summary>
    /// Gets the reason for the completion of the choice.
    /// There are 4 cases:
    /// <list type="bullet">
    /// <item><c>null</c> when generating</item>
    /// <item><c>stop</c> when stopped</item>
    /// <item><c>length</c> when content is too long</item>
    /// <item><c>tool_calls</c> when calling tools</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public required string FinishReason { get; init; }

    /// <summary>
    /// Gets the message details associated with the choice.
    /// </summary>
    [JsonPropertyName("message")]
    public required ContentUpdateMessage Message { get; init; }
}

/// <summary>
/// Represents a message in a choice.
/// </summary>
public record ContentUpdateMessage
{
    /// <summary>
    /// Gets the role of the message sender.
    /// </summary>
    [JsonPropertyName("role")]
    public required string Role { get; init; }

    /// <summary>
    /// Gets the tool calls made within the message.
    /// </summary>
    [JsonPropertyName("tool_calls")]
    public ChatToolCall[]? ToolCalls { get; init; }

    /// <summary>
    /// Gets the content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public required string Content { get; init; }
}

/// <summary>
/// Represents a call to a tool within a message.
/// </summary>
public record ChatToolCall
{
    /// <summary>
    /// Gets the function details of the tool call.
    /// </summary>
    [JsonPropertyName("function")]
    public required Function Function { get; init; }

    /// <summary>
    /// Gets the index of the tool call.
    /// </summary>
    [JsonPropertyName("index")]
    public required int Index { get; init; }

    /// <summary>
    /// Gets the identifier of the tool call.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// Gets the type of the tool call.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}

/// <summary>
/// Represents the function details of a tool call.
/// </summary>
public record Function
{
    /// <summary>
    /// Gets the name of the function being called.
    /// </summary>
    /// <remarks>
    /// maybe null when <see cref="ChatParameters.IncrementalOutput"/> is <c>false</c>.
    /// </remarks>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the arguments provided to the function call, in JSON format.
    /// </summary>
    /// <remarks>
    /// maybe null when <see cref="ChatParameters.IncrementalOutput"/> is <c>false</c>.
    /// </remarks>
    [JsonPropertyName("arguments")]
    public string? Arguments { get; init; }
}
