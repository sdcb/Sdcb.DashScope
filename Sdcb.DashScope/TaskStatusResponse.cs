using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Base output class containing common properties for responding to text-to-image operations.
/// </summary>
public record TaskStatusResponse
{
    /// <summary>
    /// Unique identifier of the task.
    /// </summary>
    [JsonPropertyName("task_id")]
    public required string TaskId { get; init; }

    /// <summary>
    /// The current status of the task.
    /// </summary>
    [JsonPropertyName("task_status")]
    public required DashScopeTaskStatus TaskStatus { get; init; }

    /// <summary>
    /// Metrics for the task such as duration and resource consumption, only available in text2image request.
    /// </summary>
    [JsonPropertyName("task_metrics")]
    public TaskMetrics? TaskMetrics { get; init; }

    /// <summary>
    /// The time when the task was submitted.
    /// </summary>
    [JsonPropertyName("submit_time"), JsonConverter(typeof(DashScopeDateTimeConverter))]
    public DateTime SubmitTime { get; init; }

    /// <summary>
    /// The time when the task is scheduled to run.
    /// </summary>
    [JsonPropertyName("scheduled_time"), JsonConverter(typeof(DashScopeDateTimeConverter))]
    public DateTime ScheduledTime { get; init; }

    /// <summary>
    /// Additional data associated with the task in a key-value format.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; init; }

    /// <summary>
    /// Converts the current instance to a <see cref="SuccessTaskResponse"/> if the task is in a succeeded state.
    /// Throws <see cref="InvalidOperationException"/> if the operation is invalid.
    /// </summary>
    /// <returns>Instance of <see cref="SuccessTaskResponse"/>.</returns>
    public SuccessTaskResponse AsSuccess()
    {
        if (this.TaskStatus != DashScopeTaskStatus.Succeeded || ExtraData == null)
        {
            throw new InvalidOperationException($"Text2ImageBaseOutput is not in succeed status.");
        }

        return JsonSerializer.Deserialize<SuccessTaskResponse>(JsonSerializer.Serialize(this))!;
    }

    /// <summary>
    /// Converts the current instance to a <see cref="FailedTaskResponse"/> if the task is in a failed state.
    /// Throws <see cref="InvalidOperationException"/> if the operation is invalid.
    /// </summary>
    /// <returns>Instance of <see cref="FailedTaskResponse"/>.</returns>
    public FailedTaskResponse AsFailed()
    {
        if (this.TaskStatus != DashScopeTaskStatus.Failed || ExtraData == null)
        {
            throw new InvalidOperationException($"Text2ImageBaseOutput is not in failed status.");
        }

        return JsonSerializer.Deserialize<FailedTaskResponse>(JsonSerializer.Serialize(this))!;
    }
}

/// <summary>
/// Output class providing details when a text-to-image task has failed.
/// </summary>
public record FailedTaskResponse : TaskStatusResponse
{
    /// <summary>
    /// The time when the task ended.
    /// </summary>
    [JsonPropertyName("end_time"), JsonConverter(typeof(DashScopeDateTimeConverter))]
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Error code associated with the failure.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    /// <summary>
    /// Descriptive message about the failure.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}

/// <summary>
/// Output class representing the successful completion of a text-to-image task.
/// </summary>
public record SuccessTaskResponse : TaskStatusResponse
{
    /// <summary>
    /// List of image results produced by the task.
    /// </summary>
    [JsonPropertyName("results")]
    public required List<ImageResult> Results { get; init; }

    /// <summary>
    /// The time when the task successfully ended.
    /// </summary>
    [JsonPropertyName("end_time"), JsonConverter(typeof(DashScopeDateTimeConverter))]
    public DateTime EndTime { get; init; }
}