using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Represents the response for a Text to Image request.
/// </summary>
public class DashScopeTaskWrapper
{
    /// <summary>
    /// Gets or sets the output response, including the job ID and status.
    /// </summary>
    [JsonPropertyName("output")]
    public required DashScopeTask Output { get; set; }

    /// <summary>
    /// Gets or sets the unique system identifier for the current request.
    /// </summary>
    [JsonPropertyName("request_id")]
    public required string RequestId { get; set; }
}

/// <summary>
/// Represents the output section of the Text to Image response.
/// </summary>
public class DashScopeTask
{
    /// <summary>
    /// Gets or sets the job ID of the asynchronous task for the current request.
    /// The actual job result needs to be obtained through the asynchronous task query interface.
    /// </summary>
    [JsonPropertyName("task_id")]
    public string TaskId { get; init; } = null!;

    /// <summary>
    /// Gets or sets the job status after submitting the asynchronous task.
    /// </summary>
    [JsonPropertyName("task_status")]
    public TaskStatus TaskStatus { get; init; }
}
