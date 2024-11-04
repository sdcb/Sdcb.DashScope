using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Represents the properties of a function parameter, including its type, description, and whether it is required.
/// </summary>
public record FunctionParameter
{
    /// <summary>
    /// Gets or init the name of the parameter.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the data type description of the parameter, default: <c>string</c>.
    /// </summary>
    public required string Type { get; init; } = "string";

    /// <summary>
    /// Gets a detailed description of the parameter.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Gets a value indicating whether the parameter is required.
    /// </summary>
    public required bool Required { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionParameter"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The data type of the parameter.</param>
    /// <param name="description">The description of the parameter.</param>
    /// <param name="required">value indicating whether the parameter is required.</param>
    [SetsRequiredMembers]
    public FunctionParameter(string name, string type, string description, bool required = true)
    {
        Name = name;
        Type = type;
        Description = description;
        Required = true;
    }

    internal FunctionParameterDto ToDto() => new(Name, Type, Description);
}

/// <summary>
/// Represents the properties of a function parameter, including its type, description, and whether it is required.
/// </summary>
internal record FunctionParameterDto
{
    /// <summary>
    /// Gets or init the name of the parameter.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the data type description of the parameter, default: <c>string</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; } = "string";

    /// <summary>
    /// Gets a detailed description of the parameter.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionParameter"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The data type of the parameter.</param>
    /// <param name="description">The description of the parameter.</param>
    [SetsRequiredMembers]
    public FunctionParameterDto(string name, string type, string description)
    {
        Name = name;
        Type = type;
        Description = description;
    }
}
