using System;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.FineTunes;

/// <summary>
/// Represents the response details of a fine tune request.
/// </summary>
public record FineTuneJob
{
    /// <summary>
    /// The task ID of the fine tune job.
    /// </summary>
    [JsonPropertyName("job_id")]
    public required string JobId { get; init; }

    /// <summary>
    ///  The status of the fine tune job.
    /// </summary>
    [JsonPropertyName("status")]
    public required DashScopeTaskStatus Status { get; init; }

    /// <summary>
    /// Gets the training type of the custom job created by the request.
    /// </summary>
    [JsonPropertyName("training_type")]
    public required string TrainingType { get; init; }

    /// <summary>
    /// Gets the create time of the custom job created by the request.
    /// </summary>
    [JsonPropertyName("create_time"), JsonConverter(typeof(DashScopeDateTimeConverter))]
    public DateTime CreateTime { get; init; }
}
