using System.Text.Json.Serialization;

namespace Sdcb.DashScope.FineTunes;

/// <summary>
/// Represents the response details of a fine tune request.
/// </summary>
public record FineTuneJob
{
    /// <summary>
    /// Gets the unique identifier of the custom job created by the request.
    /// </summary>
    [JsonPropertyName("job_id")]
    public required string JobId { get; init; }

    /// <summary>
    /// Gets the status of the custom job created by the request.
    /// </summary>
    [JsonPropertyName("status")]
    public required string Status { get; init; }
}
