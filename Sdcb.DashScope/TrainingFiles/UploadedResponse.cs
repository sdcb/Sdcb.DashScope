using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Represents a collection of information about uploaded files and failed uploads.
/// </summary>
public record UploadedResponse
{
    /// <summary>
    /// Gets the information of files that were successfully uploaded.
    /// </summary>
    [JsonPropertyName("uploaded_files")]
    public List<UploadedFile> UploadedFiles { get; set; } = [];

    /// <summary>
    /// Gets the information of files that failed to upload.
    /// </summary>
    [JsonPropertyName("failed_uploads")]
    public List<FailedUpload> FailedUploads { get; set; } = [];
}
