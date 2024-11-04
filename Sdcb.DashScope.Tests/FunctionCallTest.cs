using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TextGeneration;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class FunctionCallTest
{
    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    public FunctionCallTest(ITestOutputHelper console)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .Build();
        _apiKey = config["DashScopeApiKey"] ?? throw new Exception("DashScopeApiKey is not set in user secrets.");
        _console = console;
    }

    private readonly JsonSerializerOptions jsonSerializerOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    [Fact]
    public async Task FunctionCall()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatResponse, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("长沙明天天气如何？")], new ChatParameters()
        {
            Tools =
            [
                ChatTool.CreateFunction("query_weather", parameters:[
                    new FunctionParameter("city", "string", "城市"),
                    new FunctionParameter("date", "string", "日期，必须为yyyyMMdd的格式")
                    ]
                )
            ]
        });
        Assert.NotNull(result.Output.Choices[0].Message.ToolCalls);
        _console.WriteLine(JsonSerializer.Serialize(result.Output.Choices[0].Message.ToolCalls!, jsonSerializerOptions));
    }

    [Fact]
    public async Task FunctionCallStreamed()
    {
        using DashScopeClient c = new(_apiKey);
        await foreach (ResponseWrapper<ChatResponse, ChatTokenUsage>  result in c.TextGeneration.ChatStreamed("qwen-turbo", [ChatMessage.FromUser("长沙明天天气如何？")], new ChatParameters()
        {
            Tools =
            [
                ChatTool.CreateFunction("query_weather", parameters:[
                    new FunctionParameter("city", "string", "城市"),
                    new FunctionParameter("date", "string", "日期，必须为yyyyMMdd的格式")
                    ]
                )
            ],
            IncrementalOutput = true,
        }))
        {
            Assert.NotNull(result.Output.Choices[0].Message.ToolCalls);
            _console.WriteLine(JsonSerializer.Serialize(result.Output.Choices[0].Message.ToolCalls!, jsonSerializerOptions));
        }
    }
}
