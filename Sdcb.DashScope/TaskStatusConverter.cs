using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Converter class for the Enum Type <see cref="TaskStatus"/>. It provides the custom serialization and deserialization logic
/// from `TaskStatus` to json and from json to `TaskStatus`.
/// </summary>
public class TaskStatusConverter : JsonConverter<TaskStatus>
{
    /// <inheritdoc/>
    public override TaskStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (Enum.TryParse<TaskStatus>(value, true, out var status))
        {
            return status;
        }
        throw new JsonException($"Invalid value '{value}' for {nameof(TaskStatus)}.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TaskStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
