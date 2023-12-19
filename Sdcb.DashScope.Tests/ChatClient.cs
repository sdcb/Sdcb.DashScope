using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TextGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class ChatClient
{
    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    public ChatClient(ITestOutputHelper console)
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
        ResponseWrapper<ChatOutput, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("湖南省的省会是？")]);
        _console.WriteLine(result.ToString());
    }

    [Fact]
    public async Task OnlineChatTest()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatOutput, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-turbo", [ChatMessage.FromUser("今天长沙天气如何？")], new ChatParameters
        {
            EnableSearch = true
        });
        _console.WriteLine(result.ToString());
    }

    [Fact]
    public async Task OpenSourceChatTest()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<ChatOutput, ChatTokenUsage> result = await c.TextGeneration.Chat("qwen-72b-chat", [ChatMessage.FromUser("湖南省的省会是？")], new ChatParameters
        {
        });
        _console.WriteLine(result.ToString());
    }

    [Fact]
    public async Task FastChatStreamed()
    {
        using DashScopeClient c = new(_apiKey);
        await foreach (ResponseWrapper<ChatOutput, ChatTokenUsage> item in c.TextGeneration.ChatStreamed("qwen-turbo", [ChatMessage.FromUser("湖南省的省会是？")]))
        {
            _console.WriteLine(item.ToString());
        }
    }
}
