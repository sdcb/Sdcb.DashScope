using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

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

/// <summary>
/// Represents the parameters section of the request.
/// </summary>
public record Text2ImageParams
{
    /// <summary>
    /// <para>The resolution for the generated image.</para>
    /// <para>For "stable-diffusion-v1.5" the size must be null or "512*512".</para>
    /// <para>
    /// For "stable-diffusion-xl", allowed values are combinations of width and height between 512 and 1024 in increments of 128 (e.g., "512*1024", "1024*768"),
    /// The default value is "1024*1024".
    /// </para>
    /// </summary>
    [JsonPropertyName("size")]
    public string? Size { get; init; }

    /// <summary>
    /// The number of images to generate for the request.
    /// The allowed range is 1 to 4 inclusive, with the default being 1.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; init; }

    /// <summary>
    /// The number of denoising steps to apply during image generation.
    /// More steps generally result in higher image quality but slower inference time.
    /// The default value is 50, and users can adjust it between 1 and 500.
    /// </summary>
    [JsonPropertyName("steps")]
    public int? Steps { get; init; }

    /// <summary>
    /// The scale parameter influencing how closely the generated image adheres to the input prompt.
    /// Higher values make the outcome more closely match the provided prompt.
    /// The default value is 10, adjustable between 1 and 15.
    /// </summary>
    [JsonPropertyName("scale")]
    public int? Scale { get; init; }

    /// <summary>
    /// The seed value used for image generation.
    /// If not provided, the algorithm uses a randomly generated number as the seed.
    /// If provided, the seed will increment based on the batch quantity (e.g., "seed", "seed+1", "seed+2", "seed+3").
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; init; }
}