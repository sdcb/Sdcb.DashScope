namespace Sdcb.DashScope.FaceChains;

/// <summary>
/// The GeneratePortraitRequest class defines a request for generating a portrait.
/// </summary>
public record GeneratePortraitRequest
{
    /// <summary>
    /// Resource ID from the finetuned output of a successful model customization task.
    /// For example, "realistic_v1_12345"; the "realistic_v1" prefix should be consistent
    /// with the prefix of the corresponding style supported.
    /// <para>This field is required.</para>
    /// </summary>
    public required string ResourceId { get; init; }

    /// <summary>
    /// The style of the output image. Currently supported styles include:
    /// <list type="bullet">
    /// <item><c>f_idcard_male</c> ID photo, male (证件照·男)</item>
    /// <item><c>f_business_male</c> Business photo, male (商务写真·男)</item>
    /// <item><c>f_idcard_female</c> ID photo, female (证件照·女)</item>
    /// <item><c>f_business_female</c> Business photo, female (商务写真·女)</item>
    /// <item><c>m_springflower_female</c> Spring garden (春日花园·女)</item>
    /// <item><c>f_summersport_female</c> Summer sports (夏日运动·女)</item>
    /// <item><c>f_autumnleaf_female</c> Autumn impression (秋日印象·女)</item>
    /// <item><c>m_winterchinese_female</c> Winter Chinese style (冬日国风·女)</item>
    /// <item><c>f_hongkongvintage_female</c> Hong Kong vintage (港风复古·女)</item>
    /// <item><c>f_lightportray_female</c> Light portrait (轻写真·女)</item>
    /// </list>
    /// This field is optional.
    /// </summary>
    public required string Style { get; init; }

    /// <summary>
    /// The resolution of the generated image. Currently, '768*1024' is supported.
    /// <para>This field is optional.</para>
    /// </summary>
    public string? Size { get; init; }

    /// <summary>
    /// The number of images to generate. Currently supports 1 to 4 images with a default of 4.
    /// <para>This field is optional.</para>
    /// </summary>
    public int? N { get; init; }
}