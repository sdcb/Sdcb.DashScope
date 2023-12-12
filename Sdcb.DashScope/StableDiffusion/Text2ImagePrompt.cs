using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.StableDiffusion;

/// <summary>
/// Represents the input section of the request.
/// </summary>
public record Text2ImagePrompt
{
    /// <summary>
    /// Initializes a new instance of the Text2ImagePrompt class without parameters.
    /// </summary>
    public Text2ImagePrompt() { }

    /// <summary>
    /// Initializes a new instance of the Text2ImagePrompt class with the required and optional prompt properties.
    /// </summary>
    /// <param name="prompt">The primary text prompt for the image generation.</param>
    /// <param name="negativePrompt">(Optional) A negative prompt to avoid certain concepts in the image.</param>
    [SetsRequiredMembers]
    public Text2ImagePrompt(string prompt, string? negativePrompt = null)
    {
        Prompt = prompt;
        NegativePrompt = negativePrompt;
    }

    /// <summary>
    /// The text prompt that describes the desired image.
    /// Only supports English and is limited to 75 words. Excess words will be truncated.
    /// Example: "a running cat"
    /// </summary>
    [JsonPropertyName("prompt")]
    public required string Prompt { get; init; }

    /// <summary>
    /// An optional negative text prompt to guide the image away from certain concepts.
    /// Only supports English.
    /// Example: "yellow cat"
    /// </summary>
    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt { get; init; }
}
