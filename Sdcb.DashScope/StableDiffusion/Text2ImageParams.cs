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
    /// <para>
    /// For WanXiang, allowed values: 1024*1024, 720*1280, 1280*720
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
    /// <remarks>This options is only available in StableDiffusion API.</remarks>
    [JsonPropertyName("steps")]
    public int? Steps { get; init; }

    /// <summary>
    /// The style of generated images, allowed values:
    /// <list type="bullet">
    /// <item>"&lt;auto&gt;" - auto - 自动</item>
    /// <item>"Enhance" - enhance - 增强</item>
    /// <item>"Anime" - anime - 动漫</item>
    /// <item>"Photographic" - photographic - 摄影</item>
    /// <item>"Digital Art" - digital art - 数字艺术</item>
    /// <item>"Comic Book" - comic book - 漫画书</item>
    /// <item>"Fantasy Art" - fantasy art - 幻想艺术</item>
    /// <item>"Analog Film" - analog film - 胶片</item>
    /// <item>"Neon Punk" - neon punk - 霓虹朋克</item>
    /// <item>"Isometric" - isometric - 等距</item>
    /// <item>"Low Poly" - low poly - 低多边形</item>
    /// <item>"Origami" - origami - 折纸</item>
    /// <item>"Line Art" - line art - 线条艺术</item>
    /// <item>"Craft Clay" - craft clay - 彩泥工艺</item>
    /// <item>"Cinematic" - cinematic - 电影的</item>
    /// <item>"3D Model" - 3d model - 三维模型</item>
    /// <item>"Pixel Art" - pixel art - 像素艺术</item>
    /// <item>"Texture" - texture - 纹理</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// This option is only available in WanXiang text-to-image request.
    /// </remarks>
    [JsonPropertyName("style")]
    public string? Style { get; init; }

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