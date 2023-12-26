using System;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope;

internal record RequestWrapper
{
    public static RequestWrapper<TInput, TParameters> Create<TInput, TParameters>(string model, TInput input, TParameters? parameters = default) => new()
    {
        Model = model ?? throw new ArgumentNullException(nameof(model)),
        Input = input ?? throw new ArgumentNullException(nameof(input)),
        Parameters = parameters,
    };

    public static RequestWrapper<TInput, object> Create<TInput>(string model, TInput inputPrompt) => new()
    {
        Model = model ?? throw new ArgumentNullException(nameof(model)),
        Input = inputPrompt ?? throw new ArgumentNullException(nameof(inputPrompt)),
    };
}

internal record RequestWrapper<TInput, TParameters> : RequestWrapper
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("input")]
    public required TInput Input { get; init; }

    [JsonPropertyName("parameters")]
    public TParameters? Parameters { get; init; }
}