using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.FineTunes;

/// <summary>
/// Represents the possible states of a fine tune job.
/// </summary>
[JsonConverter(typeof(FineTuneJobStatusConverter))]
public enum FineTuneJobStatus
{
    /// <summary>
    /// The job is queued and waiting to be processed.
    /// </summary>
    Queuing,

    /// <summary>
    /// The job is currently being processed.
    /// </summary>
    Running,

    /// <summary>
    /// The job has been successfully completed.
    /// </summary>
    Succeeded,

    /// <summary>
    /// The job has failed.
    /// </summary>
    Failed,
}

/// <summary>
/// Converter class for the Enum Type <see cref="FineTuneJobStatus"/>. It provides the custom serialization and deserialization logic
/// from `<see cref="FineTuneJobStatus"/>` to json and from json to `<see cref="FineTuneJobStatus"/>`.
/// </summary>
public class FineTuneJobStatusConverter : JsonConverter<FineTuneJobStatus>
{
    /// <inheritdoc/>
    public override FineTuneJobStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (Enum.TryParse(value, true, out FineTuneJobStatus status))
        {
            return status;
        }
        throw new JsonException($"Invalid value '{value}' for {nameof(FineTuneJobStatus)}.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, FineTuneJobStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
