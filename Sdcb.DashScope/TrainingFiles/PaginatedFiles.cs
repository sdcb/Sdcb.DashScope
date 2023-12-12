using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Represents a paginated list of files.
/// </summary>
public record PaginatedFiles
{
    /// <summary>
    /// The total number of records.
    /// </summary>
    [JsonPropertyName("total")]
    public required int Total { get; init; }

    /// <summary>
    /// The page size.
    /// </summary>
    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }

    /// <summary>
    /// The current page number.
    /// </summary>
    [JsonPropertyName("page_no")]
    public required int PageNo { get; init; }

    /// <summary>
    /// The list of files.
    /// </summary>
    [JsonPropertyName("files")]
    public required List<TrainingFileInfo> Files { get; init; } = [];
}
