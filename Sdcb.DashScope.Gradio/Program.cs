using Dapper;
using Gradio.Net;
using Gradio.Net.Enums;
using Sdcb.DashScope;
using System.Data.SqlClient;
using DashScopeChatMessage = Sdcb.DashScope.TextGeneration.ChatMessage;

void Main()
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder();
    //builder.Logging.ClearProviders();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddGradio();
    WebApplication webApplication = builder.Build();
    IServiceProvider sp = webApplication.Services.GetRequiredService<IServiceProvider>();
    webApplication.UseGradio(CreateBlocks(sp));
    webApplication.Run();
}
Main();

static Blocks CreateBlocks(IServiceProvider sp)
{

    using Blocks blocks = gr.Blocks();
    Button sendButton, resetButton, regenerateButton;
    Textbox systemPrompt, userInput;
    Chatbot chatBot;
    Radio model;

    gr.Markdown("# 通义千问开源模型");
    model = gr.Radio(["qwen1.5-110b-chat", "qwen1.5-72b-chat", "qwen1.5-32b-chat", "qwen1.5-14b-chat", "qwen1.5-7b-chat", "qwen1.5-1.8b-chat", "qwen1.5-0.5b-chat", "codeqwen1.5-7b-chat"], label: "选择模型", value: "qwen1.5-110b-chat");
    using (gr.Row())
    {
        using (gr.Column(9))
        {
            systemPrompt = gr.Textbox("你是通义千问模型版本{model}，请仔细遵循用户指令，用markdown回复，当前日期：{date}", label: "系统Prompt");
        }
        resetButton = gr.Button("🔄重置聊天", variant: ButtonVariant.Stop);
    }
    chatBot = gr.Chatbot(label: "聊天窗口", height: 700, showCopyButton: true, placeholder: "这里显示聊天历史记录");
    using (gr.Row())
    {
        using (gr.Column(scale: 9))
        {
            userInput = gr.Textbox(label: "用户输入", placeholder: "请输入你的问题或指令...");
        }
        sendButton = gr.Button("✉️发送", variant: ButtonVariant.Primary);
        regenerateButton = gr.Button("🔃重新生成", variant: ButtonVariant.Secondary);
    }
    using (gr.Row())
    {
        gr.Markdown("""
		## Github: https://github.com/sdcb/Sdcb.DashScope
		""");

        gr.Markdown("""		
		## QQ: 495782587
		""");
    }

    sendButton.Click(streamingFn: i =>
    {
        string model = Radio.Payload(i.Data[0]).Single();
        string systemPrompt = Textbox.Payload(i.Data[1]);
        IList<ChatbotMessagePair> chatHistory = Chatbot.Payload(i.Data[2]);
        string userInput = Textbox.Payload(i.Data[3]);

        return Respond(model, systemPrompt, chatHistory, userInput, sp);
    }, inputs: [model, systemPrompt, chatBot, userInput], outputs: [userInput, chatBot]);
    regenerateButton.Click(streamingFn: i =>
    {
        string model = Radio.Payload(i.Data[0]).Single();
        string systemPrompt = Textbox.Payload(i.Data[1]);
        IList<ChatbotMessagePair> chatHistory = Chatbot.Payload(i.Data[2]);
        if (chatHistory.Count == 0)
        {
            throw new Exception("No chat history available for regeneration.");
        }
        string userInput = chatHistory[^1].HumanMessage.TextMessage;
        chatHistory.RemoveAt(chatHistory.Count - 1);

        return Respond(model, systemPrompt, chatHistory, userInput, sp);
    }, inputs: [model, systemPrompt, chatBot], outputs: [userInput, chatBot]);
    resetButton.Click(i => Task.FromResult(gr.Output(new ChatbotMessagePair[0], "")), outputs: [chatBot, userInput]);

    return blocks;
}

static async IAsyncEnumerable<Output> Respond(string model, string systemPrompt, IList<ChatbotMessagePair> chatHistory, string message, IServiceProvider sp)
{
    IConfiguration config = sp.GetRequiredService<IConfiguration>();
    string dashScopeApiKey = config["DashScope:ApiKey"] ?? throw new Exception("DashScope API key is not configured.");

    if (message == "")
    {
        yield return gr.Output("", chatHistory);
        yield break;
    }

    systemPrompt = systemPrompt.Replace("{model}", model).Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
    chatHistory.Add(new ChatbotMessagePair(message, ""));

    using DashScopeClient api = new(dashScopeApiKey);
    DashScopeChatMessage[] msgs =
    [
        DashScopeChatMessage.FromSystem(systemPrompt),
        ..chatHistory.SkipLast(1).SelectMany(p => new []
        {
            DashScopeChatMessage.FromUser(p.HumanMessage.TextMessage),
            DashScopeChatMessage.FromAssistant(p.AiMessage.TextMessage),
        }),
        DashScopeChatMessage.FromUser(message),
    ];
    await foreach (var item in api.TextGeneration.ChatStreamed(model, msgs, new()
    {
        //EnableSearch = true, 
        IncrementalOutput = true,
        Seed = (ulong)Random.Shared.Next()
    }))
    {
        chatHistory[^1].AiMessage.TextMessage += item.Output.Text;
        yield return gr.Output("", chatHistory);
        if (item.Output.FinishReason == "stop")
        {
            string? connectionString = config.GetValue<string>("ConnectionString");
            if (connectionString != null)
            {
                IHttpContextAccessor httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                string clientIp =
                    httpContextAccessor.HttpContext!.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString();
                DashScopeChatMessage[] combinedMessages = [.. msgs, DashScopeChatMessage.FromAssistant(chatHistory[^1].AiMessage.TextMessage)];
                LogClientMessage(clientIp, model, combinedMessages, connectionString);
            }
            break;
        }
    }
}

static void LogClientMessage(string clientIP, string model, DashScopeChatMessage[] combinedMessages, string connectionString)
{
    using SqlConnection conn = new(connectionString);
    int chatId = conn.ExecuteScalar<int>("""
        INSERT INTO Chat(Model, ClientIP, CreateTime) VALUES (@Model, @ClientIp, SYSDATETIME());
        SELECT SCOPE_IDENTITY();
        """, new
    {
        Model = model,
        ClientIp = clientIP
    });
    conn.Execute("INSERT INTO ChatMessage(ChatId, Role, [Content]) VALUES (@ChatId, @Role, @Content)", combinedMessages.Select((m) => new
    {
        ChatId = chatId,
        Role = m.Role,
        Content = m.Content
    }));
}
