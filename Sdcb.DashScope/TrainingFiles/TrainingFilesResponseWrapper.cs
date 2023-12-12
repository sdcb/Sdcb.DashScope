using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TrainingFiles;

internal record TrainingFilesResponseWrapper<T>
{
    [JsonPropertyName("request_id")]
    public required string RequestId { get; init; }

    [JsonPropertyName("data")]
    public required T Data { get; init; }
}