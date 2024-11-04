using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TextGeneration;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class ResponseFormatTest
{
    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    public ResponseFormatTest(ITestOutputHelper console)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .Build();
        _apiKey = config["DashScopeApiKey"] ?? throw new Exception("DashScopeApiKey is not set in user secrets.");
        _console = console;
    }

    [Fact]
    public async Task EnsureJson()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatResponse, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("请随机生成一段json")], new ChatParameters
        {
            ResponseFormat = ChatResponseFormat.JsonObject
        });
        _console.WriteLine(result.Output.Choices[0].Message.Content);
        _ = JsonSerializer.Deserialize<JsonElement>(result.Output.Choices[0].Message.Content);
    }

    [Fact]
    public async Task EnsureText()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatResponse, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("请随机生成一段json")], new ChatParameters
        {
            ResponseFormat = ChatResponseFormat.Text
        });
        _console.WriteLine(result.Output.Choices[0].Message.Content);
        // expect exception
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<JsonElement>(result.Output.Choices[0].Message.Content));
    }
}
