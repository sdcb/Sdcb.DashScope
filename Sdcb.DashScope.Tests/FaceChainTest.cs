using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public async Task StartFineTuneTest()
    {

    }
}
