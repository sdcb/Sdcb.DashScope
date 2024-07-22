using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.FineTunes;
using System.Text.Json;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class FaceChainTest
{
    public FaceChainTest(ITestOutputHelper console)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>()
            .Build();
        _apiKey = config["DashScopeApiKey"] ?? throw new Exception("DashScopeApiKey is not set in user secrets.");
        _console = console;
    }

    private readonly string _apiKey;
    private readonly ITestOutputHelper _console;

    [Fact]
    public async Task FaceChainCheckImageTest()
    {
        string[] urls =
        [
            "https://io.starworks.cc:88/cv-public/2023/1317141.jpg",
        ];
        DashScopeClient c = new(_apiKey);
        bool[] oks = await c.FaceChains.CheckImage(urls);
        _console.WriteLine(string.Join(",", oks));
    }

    [Fact]
    public async Task CreateFineTuneJobTest()
    {
        DashScopeClient c = new(_apiKey);
        FineTuneJob job = await c.FineTunes.StartFineTune(["47b086cb-702c-4f78-9e71-162c2d4ebf6e"], "facechain-finetune");
        _console.WriteLine(JsonSerializer.Serialize(job));
    }

    [Fact]
    public async Task GetFineTuneJobStatusTest()
    {
        DashScopeClient c = new(_apiKey);
        FineTuneJobDetailed resp = await c.FineTunes.GetJobStatus("ft-202312141750-3225");
        _console.WriteLine(JsonSerializer.Serialize(resp));
    }
}
