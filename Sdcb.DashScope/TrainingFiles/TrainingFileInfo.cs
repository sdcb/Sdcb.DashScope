using System;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Represents an individual file informations in DashScope traning storage.
/// </summary>
public record TrainingFileInfo
{
    /// <summary>
    /// The unique file identifier.
    /// </summary>
    [JsonPropertyName("file_id")]
    public required string FileId { get; init; }

    /// <summary>
    /// The name of the file.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The description of the file.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// The size of the file.
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; init; }

    /// <summary>
    /// The MD5 hash of the file.
    /// </summary>
    [JsonPropertyName("md5")]
    public required string MD5 { get; init; }

    /// <summary>
    /// The creation date of the file.
    /// </summary>
    [JsonPropertyName("gmt_create"), JsonConverter(typeof(DashScopeDateTimeConverter))]
    public required DateTime CreateTime { get; init; }

    /// <summary>
    /// The URL for downloading the file.
    /// </summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}