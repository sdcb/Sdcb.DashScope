using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Generic base response class for image request async task.
/// </summary>
public record TaskStatusWrapper
{
    /// <summary>
    /// Gets or initiates the request id. This unique identifier corresponds to each individual request.
    /// </summary>
    [JsonPropertyName("request_id")]
    public required string RequestId { get; init; }

    /// <summary>
    /// Gets or initiates the output. This represents the processed task status response associated with the respective request.
    /// </summary>
    [JsonPropertyName("output")]
    public required TaskStatusResponse Output { get; init; }
}
