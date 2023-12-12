using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.StableDiffusion;

/// <summary>
/// The request class for generating images from text using a specific model.
/// </summary>
public record Text2ImageRequest
{
    /// <summary>
    /// Initializes a new default instance of the Text2ImageRequest class.
    /// </summary>
    public Text2ImageRequest() { }

    /// <summary>
    /// Initializes a new instance of the Text2ImageRequest class with both a primary prompt and a negative prompt to guide image generation.
    /// </summary>
    /// <param name="prompt">The primary text prompt for the image generation.</param>
    /// <param name="negativePrompt">An additional optional negative prompt to steer the generation away from certain concepts or elements.</param>

    [SetsRequiredMembers]
    public Text2ImageRequest(string prompt, string? negativePrompt = null)
    {
        InputPrompt = new Text2ImagePrompt(prompt, negativePrompt) { };
    }

    /// <summary>
    /// The name of the model to use for image generation.
    /// Allowed values are "stable-diffusion-xl" or "stable-diffusion-v1.5".
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; } = "stable-diffusion-xl";

    /// <summary>
    /// The input prompt information for the image generation.
    /// </summary>
    [JsonPropertyName("input")]
    public required Text2ImagePrompt InputPrompt { get; init; } = null!;

    /// <summary>
    /// The parameters for image generation, such as resolution, quantity, and quality settings.
    /// </summary>
    [JsonPropertyName("parameters")]
    public required Text2ImageParams Parameters { get; init; } = new Text2ImageParams();
}
