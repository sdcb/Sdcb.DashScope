using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TextGeneration;
using System.Text;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class ChatClientTest
{
    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    public ChatClientTest(ITestOutputHelper console)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .Build();
        _apiKey = config["DashScopeApiKey"] ?? throw new Exception("DashScopeApiKey is not set in user secrets.");
        _console = console;
    }

    [Fact]
    public async Task FastChat()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatResponse, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("湖南省的省会是？")]);
        _console.WriteLine(result.ToString());
    }

    [Fact]
    public async Task OnlineChatTest()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatResponse, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("今天长沙天气如何？")], new ChatParameters
        {
            EnableSearch = true
        });
        _console.WriteLine(result.ToString());
    }

    [Fact]
    public async Task OpenSourceChatTest()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatResponse, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-72b-chat", [ChatMessage.FromUser("湖南省的省会是？")], new ChatParameters
        {
        });
        _console.WriteLine(result.ToString());
    }

    [Fact]
    public async Task FastChatStreamed()
    {
        using DashScopeClient c = new(_apiKey);
        await foreach (ResponseWrapper<ChatResponse, ChatTokenUsage> item in c.TextGeneration.ChatStreamed("qwen-turbo", [ChatMessage.FromUser("湖南省的省会是？")]))
        {
            _console.WriteLine(item.ToString());
        }
    }

    [Fact]
    public async Task FastChatStreamedIncremental()
    {
        using DashScopeClient c = new(_apiKey);
        ChatMessage msg = ChatMessage.FromUser("湖南省的省会是？请使用JSON字符串表示，不需要其它输出信息");
        StringBuilder sb = new();
        await foreach (ResponseWrapper<ChatResponse, ChatTokenUsage> item in c.TextGeneration.ChatStreamed("qwen-turbo", [msg], new()
        {
            IncrementalOutput = true
        }))
        {
            if (item.Output.Choices[0].FinishReason == "stop") break;
            sb.Append(item.Output.Choices[0].Message.Content);
        }
        _console.WriteLine(sb.ToString());
    }

    [Fact]
    public async Task FastChatStreamedOnline()
    {
        using DashScopeClient c = new(_apiKey);
        ChatMessage msg = ChatMessage.FromUser("长沙今天天气如何？");
        string finalResult = null!;
        await foreach (ResponseWrapper<ChatResponse, ChatTokenUsage> item in c.TextGeneration.ChatStreamed("qwen-turbo", [msg], new()
        {
            EnableSearch = true,
        }))
        {
            _console.WriteLine(item.Output.Choices[0].Message.Content);
            if (item.Output.Choices[0].FinishReason == "stop")
            {
                finalResult = item.Output.Choices[0].Message.Content;
                break;
            }
        }
        _console.WriteLine($"最终结果：");
        _console.WriteLine(finalResult);
    }
}
