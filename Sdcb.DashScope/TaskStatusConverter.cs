using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Converter class for the Enum Type <see cref="DashScopeTaskStatus"/>. It provides the custom serialization and deserialization logic
/// from `<see cref="DashScopeTaskStatus"/>` to json and from json to `<see cref="DashScopeTaskStatus"/>`.
/// </summary>
public class TaskStatusConverter : JsonConverter<DashScopeTaskStatus>
{
    /// <inheritdoc/>
    public override DashScopeTaskStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (Enum.TryParse(value, true, out DashScopeTaskStatus status))
        {
            return status;
        }
        throw new JsonException($"Invalid value '{value}' for {nameof(DashScopeTaskStatus)}.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DashScopeTaskStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
