using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.DashScope.TextGeneration;

/// <summary>
/// LLM models clients that supports Qwen/open source LLMs.
/// </summary>
public class TextGenerationClient
{
    internal TextGenerationClient(DashScopeClient parent)
    {
        Parent = parent;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// Sends a chat interaction to the DashScope large language model and returns a response.
    /// </summary>
    /// <param name="model">The specified model identifier for processing the chat interaction.</param>
    /// <param name="messages">A read-only list of chat messages representing the conversation history.</param>
    /// <param name="parameters">Optional parameters to customize the chat behavior.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// <see cref="ResponseWrapper{TOutput, TUsage}"/> object with the <see cref="ChatOutput"/> as the result of the interaction and 
    /// <see cref="ChatTokenUsage"/> that provides metadata about the token usage for the conversation.
    /// </returns>
    public async Task<ResponseWrapper<ChatOutput, ChatTokenUsage>> Chat(string model, IReadOnlyList<ChatMessage> messages, ChatParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, @"https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation")
        {
            Content = JsonContent.Create(RequestWrapper.Create(model, new
            {
                messages, 
            }, parameters), options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }),
        };
        HttpResponseMessage resp = await Parent.HttpClient.SendAsync(httpRequest, cancellationToken);
        return await DashScopeClient.ReadResponse<ResponseWrapper<ChatOutput, ChatTokenUsage>>(resp, cancellationToken);
    }

    /// <summary>
    /// Sends a chat interaction to the DashScope large language model and streams back responses as they are ready.
    /// </summary>
    /// <param name="model">The specified model identifier for processing the chat interaction.</param>
    /// <param name="messages">A read-only list of chat messages representing the conversation history.</param>
    /// <param name="parameters">Optional parameters to customize the chat behavior.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// Streamed <see cref="ResponseWrapper{TOutput, TUsage}"/> object with the <see cref="ChatOutput"/> as the result of the interaction and 
    /// <see cref="ChatTokenUsage"/> that provides metadata about the token usage for the conversation.
    /// </returns>
    public async IAsyncEnumerable<ResponseWrapper<ChatOutput, ChatTokenUsage>> ChatStreamed(string model, 
        IReadOnlyList<ChatMessage> messages, 
        ChatParameters? parameters = null, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        HttpRequestMessage httpRequest = new(HttpMethod.Post, @"https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation")
        {
            Content = JsonContent.Create(RequestWrapper.Create(model, new
            {
                messages,
            }, parameters), options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }),
        };
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        httpRequest.Headers.TryAddWithoutValidation("X-DashScope-SSE", "enable");

        using HttpResponseMessage resp = await Parent.HttpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        using StreamReader reader = new (await resp.Content.ReadAsStreamAsync(), Encoding.UTF8);
        while (!reader.EndOfStream)
        {
            if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

            string line = await reader.ReadLineAsync();
            if (line.StartsWith("data:"))
            {
                string data = line["data:".Length..];
                yield return JsonSerializer.Deserialize<ResponseWrapper<ChatOutput, ChatTokenUsage>>(data)!;
            }
        }
    }
}
