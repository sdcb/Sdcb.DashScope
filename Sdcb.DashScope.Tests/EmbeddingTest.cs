using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TextEmbedding;
using System.Text.Json;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class EmbeddingTest
{
    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    public EmbeddingTest(ITestOutputHelper console)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .Build();
        _apiKey = config["DashScopeApiKey"] ?? throw new Exception("DashScopeApiKey is not set in user secrets.");
        _console = console;
    }

    [Fact]
    public async Task V1Test()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<EmbeddingOutput, EmbeddingUsage> result = await c.TextEmbedding.GetEmbeddings(EmbeddingRequest.FromV1(["你好"]));
        _console.WriteLine(JsonSerializer.Serialize(result.Output.Embeddings));
    }

    [Fact]
    public async Task V2Test()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<EmbeddingOutput, EmbeddingUsage> result = await c.TextEmbedding.GetEmbeddings(EmbeddingRequest.FromV2(["你好"]));
        _console.WriteLine(JsonSerializer.Serialize(result.Output.Embeddings));
    }

    [Fact]
    public async Task V2QueryTest()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<EmbeddingOutput, EmbeddingUsage> result = await c.TextEmbedding.GetEmbeddings(EmbeddingRequest.FromV2(["你好"], EmbeddingType.Query));
        _console.WriteLine(JsonSerializer.Serialize(result.Output.Embeddings));
    }

    [Fact]
    public async Task V2MultipleQueryTest()
    {
        using DashScopeClient c = new(_apiKey);
        ResponseWrapper<EmbeddingOutput, EmbeddingUsage> result = await c.TextEmbedding.GetEmbeddings(EmbeddingRequest.FromV2(["你好", "Hello"], EmbeddingType.Query));
        _console.WriteLine(JsonSerializer.Serialize(result.Output.Embeddings));
    }
}
