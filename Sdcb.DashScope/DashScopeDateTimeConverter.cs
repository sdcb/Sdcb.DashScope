using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Converts a <see cref="DateTime"/> to and from a string in the format "yyyy-MM-dd HH:mm:ss.fff".
/// </summary>
public class DashScopeDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] AllowedDateTimeFormats = 
    [
        "yyyy-MM-dd HH:mm:ss.fff",
        "yyyy-MM-dd HH:mm:ss"
    ];

    /// <inheritdoc/>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();
        foreach (string format in AllowedDateTimeFormats)
        {
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
        }
        throw new JsonException("Failed to parse datetime string.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(AllowedDateTimeFormats[0], CultureInfo.InvariantCulture));
    }
}
