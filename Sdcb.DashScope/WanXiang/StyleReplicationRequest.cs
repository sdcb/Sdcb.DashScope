using System.Text.Json.Serialization;

namespace Sdcb.DashScope.WanXiang;

/// <summary>
/// Contains the specific input details for a style replication request.
/// </summary>
public record StyleReplicationInput
{
    /// <summary>
    /// <para>The image URL. The image should meet certain resolution requirements:</para>
    /// <para>Minimum resolution of 256x256 pixels, maximum of 5760x3240 pixels; aspect ratio should not exceed 1.78:1.</para>
    /// <para>To ensure quality output, optimal image resolution ranges from 512x512 to 2048x1024 pixels.</para>
    /// <para>Uploaded face photo should be clear, with a proper size of the face and devoid of exaggerated poses and expressions.</para>
    /// <para>Supported types: JPEG, PNG, JPG, BMP, WEBP. Maximum size: 10MB.</para>
    /// </summary>
    [JsonPropertyName("image_url")]
    public required string ImageUrl { get; init; }

    /// <summary>
    /// Gets or sets the style index, indicating the desired type of style to generate。
    /// </summary>
    [JsonPropertyName("style_index")]
    public RepliationStyle Style { get; init; }
}

/// <summary>
/// Enum for the different style types.
/// </summary>
public enum RepliationStyle
{
    /// <summary>
    /// Retro Comic style. (复古漫画)
    /// </summary>
    RetroComic = 0,

    /// <summary>
    /// 3D Fairy Tale style. (3D童话)
    /// </summary>
    _3DFairyTale = 1,

    /// <summary>
    /// Anime style. (二次元)
    /// </summary>
    Anime = 2,

    /// <summary>
    /// Fresh and Clean (小清新) style.
    /// </summary>
    FreshAndClean = 3,

    /// <summary>
    /// Future Tech style. (未来科技)
    /// </summary>
    FutureTech = 4,

    /// <summary>
    /// Traditional Chinese painting style. (国画古风)
    /// </summary>
    TraditionalChinese = 5,

    /// <summary>
    /// General in War style. (将军百战)
    /// </summary>
    GeneralInWar = 6,

    /// <summary>
    /// Colorful Cartoon style. (炫彩卡通)
    /// </summary>
    ColorfulCartoon = 7,

    /// <summary>
    /// Elegant and Traditional (清雅国风) style.
    /// </summary>
    ElegantTraditional = 8,

    /// <summary>
    /// Happy New Year style. (喜迎新年)
    /// </summary>
    HappyNewYear = 9
}