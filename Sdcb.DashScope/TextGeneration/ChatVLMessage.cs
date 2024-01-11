using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// A single chat message within the conversation, with visual ability supported.
/// </summary>
public record ChatVLMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatVLMessage"/> class.
    /// </summary>
    public ChatVLMessage()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatVLMessage"/> class with specified role and content.
    /// </summary>
    /// <param name="role">The role of the message sender.</param>
    /// <param name="content">The content of the message.</param>
    [SetsRequiredMembers]
    public ChatVLMessage(string role, ContentItem[] content)
    {
        Role = role;
        Content = content;
    }

    /// <summary>
    /// Create a chat message from the system with specified content.
    /// </summary>
    /// <param name="content">The content of the message.</param>
    /// <returns>A new <see cref="ChatMessage"/> instance representing a system message.</returns>
    public static ChatVLMessage FromSystem(string content) => new("system", [ContentItem.FromText(content)]);

    /// <summary>
    /// Create a chat message from a user with specified contents.
    /// </summary>
    /// <param name="contents">The contents of the message.</param>
    /// <returns>A new <see cref="ChatMessage"/> instance representing a user message.</returns>
    public static ChatVLMessage FromUser(params ContentItem[] contents) => new("user", contents);

    /// <summary>
    /// Create a chat message from the assistant with specified content.
    /// </summary>
    /// <param name="content">The content of the message.</param>
    /// <returns>A new <see cref="ChatMessage"/> instance representing an assistant message.</returns>
    public static ChatVLMessage FromAssistant(string content) => new("assistant", [ContentItem.FromText(content)]);

    /// <summary>
    /// Gets or sets the role of the message sender, allowed values are "system", "user", "assistant".
    /// </summary>
    [JsonPropertyName("role")]
    public required string Role { get; set; }

    /// <summary>
    /// The content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public required ContentItem[] Content { get; set; }

    /// <summary>
    /// Returns the content of <c>"<see cref="Role"/>: <see cref="Content"/>(joined by ',')"</c>.
    /// </summary>
    public override string ToString()
    {
        return $"{Role}: {string.Join("\n", Content.Select(x => x.ToString()))}";
    }
}

/// <summary>
/// Represents a visual and textual content item within a chat message.
/// </summary>
[JsonConverter(typeof(ContentItemConverter))]
public abstract record ContentItem
{
    /// <summary>
    /// Creates a text content item with the specified text.
    /// </summary>
    /// <param name="text">The textual content of the item.</param>
    /// <returns>A new instance of <see cref="TextContentItem"/>.</returns>
    public static TextContentItem FromText(string text) => new() { Text = text };

    /// <summary>
    /// Creates an image content item with the specified image URL.
    /// </summary>
    /// <param name="imageUrl">The URL of the image to be included in the content.</param>
    /// <returns>A new instance of <see cref="ImageContentItem"/>.</returns>
    public static ImageContentItem FromImage(string imageUrl) => new() { Image = imageUrl };
}

/// <summary>
/// A content item that contains text.
/// </summary>
public record TextContentItem : ContentItem
{
    /// <summary>
    /// Gets or sets the text of this content item.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    /// <summary>
    /// Returns the content of <c>"<see cref="Text"/>"</c>.
    /// </summary>
    public override string ToString() => Text;
}

/// <summary>
/// A content item that contains an image URL.
/// </summary>
public record ImageContentItem : ContentItem
{
    /// <summary>
    /// Gets or sets the URL of the image in this content item.
    /// </summary>
    [JsonPropertyName("image")]
    public required string Image { get; init; }

    /// <returns>The content of <c>"image(<see cref="Image"/>)"</c>.</returns>
    public override string ToString() => $"image({Image})";
}

/// <summary>
/// A <see cref="JsonConverter{T}"/> that can convert a <see cref="ContentItem"/> to or from JSON.
/// </summary>
public class ContentItemConverter : JsonConverter<ContentItem>
{
    /// <inheritdoc/>
    public override ContentItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Create a JsonDocument out of the JsonElement which has the properties of the ContentItem
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        // Check the properties to figure out which type of content item to deserialize into
        if (root.TryGetProperty("text", out _))
        {
            return JsonSerializer.Deserialize<TextContentItem>(root.GetRawText(), options)!;
        }
        else if (root.TryGetProperty("image", out _))
        {
            return JsonSerializer.Deserialize<ImageContentItem>(root.GetRawText(), options)!;
        }

        throw new JsonException("Unknown type of ContentItem");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ContentItem value, JsonSerializerOptions options)
    {
        // Serialize the actual derived type
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}