using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Represents an uploaded file's information.
/// </summary>
public record UploadedFile
{
    /// <summary>
    /// Gets or sets the unique identifier of the file.
    /// </summary>
    [JsonPropertyName("file_id")]
    public required string FileId { get; init; }

    /// <summary>
    /// Gets or sets the name of the uploaded file.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
