using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TextGeneration;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class QwenVLTest
{
    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    public QwenVLTest(ITestOutputHelper console)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .Build();
        _apiKey = config["DashScopeApiKey"] ?? throw new Exception("DashScopeApiKey is not set in user secrets.");
        _console = console;
    }

    [Fact]
    public async Task FastChatStreamed()
    {
        using DashScopeClient c = new(_apiKey);
        ChatVLMessage[] messages = [ChatVLMessage.FromUser(
            ContentItem.FromImage("https://avatars.githubusercontent.com/u/1317141?v=4"),
            ContentItem.FromText("画面中有什么？") 
            )];
        await foreach (ResponseWrapper<string, ChatTokenUsage> item in c.TextGeneration.ChatVLStreamed("qwen-vl-plus", messages))
        {
            _console.WriteLine(item.ToString());
        }
    }

    [Fact]
    public async Task FastChatStreamedIncremental()
    {
        using DashScopeClient c = new(_apiKey);
        ChatVLMessage[] messages = [ChatVLMessage.FromUser(
            ContentItem.FromImage("https://avatars.githubusercontent.com/u/1317141?v=4"),
            ContentItem.FromText("画面中有什么？")
            )];
        await foreach (ResponseWrapper<string, ChatTokenUsage> item in c.TextGeneration.ChatVLStreamed("qwen-vl-plus", messages, new ChatParameters
        {
            IncrementalOutput = true
        }))
        {
            _console.WriteLine(item.ToString());
        }
    }
}
