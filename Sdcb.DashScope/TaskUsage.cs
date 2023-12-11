using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Additional information regarding the resource usage of the task.
/// </summary>
public class TaskUsage
{
    [JsonPropertyName("image_count")]
    public int ImageCount { get; set; }
}
