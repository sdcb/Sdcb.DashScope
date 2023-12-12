using System.Text.Json.Serialization;

namespace Sdcb.DashScope.StableDiffusion;

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