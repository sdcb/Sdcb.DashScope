using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Additional information regarding the resource usage of the task.
/// </summary>
public class ImageTaskUsage
{
    /// <summary>
    /// Image count
    /// </summary>
    [JsonPropertyName("image_count")]
    public int ImageCount { get; set; }
}
