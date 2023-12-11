using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Represents the possible states of a Text to Image job.
/// </summary>
[JsonConverter(typeof(TaskStatusConverter))]
public enum DashScopeTaskStatus
{
    /// <summary>
    /// The job is queued and waiting to be processed.
    /// </summary>
    Pending,

    /// <summary>
    /// The job is currently being processed.
    /// </summary>
    Running,

    /// <summary>
    /// The job has been successfully completed.
    /// </summary>
    Succeeded,

    /// <summary>
    /// The job has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The job either does not exist or its status is unknown.
    /// </summary>
    Unknown,
}
