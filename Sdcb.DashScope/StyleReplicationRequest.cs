using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

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
    /// Retro Comic style.
    /// </summary>
    RetroComic = 0,

    /// <summary>
    /// 3D Fairy Tale style.
    /// </summary>
    _3DFairyTale,

    /// <summary>
    /// Anime style.
    /// </summary>
    Anime,

    /// <summary>
    /// Fresh and Clean (小清新) style.
    /// </summary>
    FreshAndClean,

    /// <summary>
    /// Future Tech style.
    /// </summary>
    FutureTech,

    /// <summary>
    /// 3D Realism style.
    /// </summary>
    _3DRealism
}