using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.FineTunes;

/// <summary>
/// Represents the output of a custom job.
/// </summary>
public record FineTuneJobDetailed : FineTuneJob
{
    /// <summary>
    /// The model produced by the custom job. This field is shown after the custom job succeeds; this model will be used for inference calls.
    /// </summary>
    [JsonPropertyName("finetuned_output")]
    public string? FinetunedOutput { get; init; }

    /// <summary>
    /// The base model of the custom job.
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// The training files used by the custom job.
    /// </summary>
    [JsonPropertyName("training_file_ids")]
    public required IList<string> TrainingFileIds { get; init; } = new List<string>();

    /// <summary>
    /// The validation files used by the custom job (may be empty).
    /// </summary>
    [JsonPropertyName("validation_file_ids")]
    public IList<string>? ValidationFileIds { get; init; }

    /// <summary>
    /// The hyperparameters used for the custom job (can be an empty object).
    /// </summary>
    [JsonPropertyName("hyper_parameters")]
    public IDictionary<string, object>? HyperParameters { get; init; }
}