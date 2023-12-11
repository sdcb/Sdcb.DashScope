using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

/// <summary>
/// Result object which may contain a URL for a successfully generated image or error details.
/// </summary>
public record ImageResult
{
    /// <summary>
    /// Gets or sets the URL of the image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// Gets or sets the code associated with the image.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    /// <summary>
    /// Gets a value indicating whether the image generation was successful.
    /// </summary>
    public bool IsSuccess => Code == null && Message == null;
}
