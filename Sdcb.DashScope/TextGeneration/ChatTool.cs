using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// Represents a tool used in chat.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(FunctionChatTool))]
public abstract record ChatTool
{
    /// <summary>
    /// Gets the type of the chat tool.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    internal ChatTool(string type) => Type = type;

    /// <summary>
    /// Creates a new instance of <see cref="FunctionChatTool"/>.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="parameters">The parameters of the function.</param>
    /// <returns>A new instance of <see cref="FunctionChatTool"/>.</returns>
    public static ChatTool CreateFunction(string name, string? description = null, IReadOnlyList<FunctionParameter>? parameters = null)
    {
        return new FunctionChatTool(name, description, parameters);
    }
}

/// <summary>
/// Represents a function, including its name, description, and parameters.
/// </summary>
internal record FunctionChatTool() : ChatTool("function")
{
    /// <summary>
    /// Gets the function definition.
    /// </summary>
    [JsonPropertyName("function")]
    public required FunctionDef Function { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionChatTool"/> class.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="parameters">The parameters of the function.</param>
    [SetsRequiredMembers]
    public FunctionChatTool(string name, string? description, IReadOnlyList<FunctionParameter>? parameters = null) : this()
    {
        Function = new FunctionDef(name, description, parameters);
    }
}

/// <summary>
/// Represents the definition of a function.
/// </summary>
internal record FunctionDef
{
    /// <summary>
    /// Gets the name of the function.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the description of the function.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets the parameters of the function.
    /// </summary>
    [JsonPropertyName("parameters")]
    public IReadOnlyList<FunctionParameterDto>? Parameters { get; init; }

    /// <summary>
    /// Gets the required parameters of the function.
    /// </summary>
    [JsonPropertyName("required")]
    public string[]? Required { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionDef"/> class.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="parameters">The parameters of the function.</param>
    [SetsRequiredMembers]
    public FunctionDef(string name, string? description = null, IReadOnlyList<FunctionParameter>? parameters = null)
    {
        Name = name;
        Description = description;
        Parameters = parameters?.Select(x => x.ToDto()).ToArray();
        Required = parameters?.Where(x => x.Required).Select(x => x.Name).ToArray();
    }
}
