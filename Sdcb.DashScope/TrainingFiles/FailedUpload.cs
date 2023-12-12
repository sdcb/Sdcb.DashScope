using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Represents the information of a failed file upload along with the error details.
/// </summary>
public record FailedUpload
{
    /// <summary>
    /// Gets or sets the name of the file that failed to upload.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the error code indicating the reason for failure.
    /// </summary>
    [JsonPropertyName("code")]
    public required string? Code { get; init; }

    /// <summary>
    /// Gets or sets the message providing further details on the failure.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}