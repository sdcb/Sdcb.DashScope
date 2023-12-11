using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Helper class representing task performance metrics.
/// </summary>
public record TaskMetrics
{
    /// <summary>
    /// Gets or initializes the total number of tasks.
    /// </summary>
    [JsonPropertyName("TOTAL")]
    public int Total { get; init; }

    /// <summary>
    /// Gets or initializes the number of tasks that succeeded.
    /// </summary>
    [JsonPropertyName("SUCCEEDED")]
    public int Succeeded { get; init; }

    /// <summary>
    /// Gets or initializes the number of tasks that failed.
    /// </summary>
    [JsonPropertyName("FAILED")]
    public int Failed { get; init; }
}
