using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class UnitTest1
{
    public UnitTest1(ITestOutputHelper console)
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
    public async Task Text2ImageTest()
    {
        using DashScopeClient c = new(_apiKey);
        Text2ImagePrompt prompt = new()
        {
            Prompt = "standing, ultra detailed, official art, 4k 8k wallpaper, soft light and shadow, hand detail, eye high detail, 8K, (best quality:1.5), pastel color, soft focus, masterpiece, studio, hair high detail, (pure background:1.2), (head fully visible, full body shot)",
            NegativePrompt = "EasyNegative, nsfw,(low quality, worst quality:1.4),lamp, missing shoe, missing head,mutated hands and fingers,deformed,bad anatomy,extra limb,ugly,poorly drawn hands,disconnected limbs,missing limb,missing head,camera"
        };
        DashScopeTask task = await c.Text2Image(prompt);
        _console.WriteLine(task.TaskId);

        while (true)
        {
            TaskStatusResponse resp = await c.QueryTaskStatus(task.TaskId);
            _console.WriteLine(resp.TaskStatus.ToString());

            if (resp.TaskStatus == DashScopeTaskStatus.Succeeded)
            {
                SuccessTaskResponse success = resp.AsSuccess();
                using HttpClient client = new();
                for (int i = 0; i < success.Results.Count; i++)
                {
                    string url = success.Results[i].Url!;
                    _console.WriteLine(url);
                    using HttpResponseMessage imgResp = await client.GetAsync(url);
                    using Stream imgStream = await imgResp.Content.ReadAsStreamAsync();
                    using FileStream fs = new($"{nameof(Text2ImageTest)}-{i}.png", FileMode.Create);
                    await imgStream.CopyToAsync(fs);
                }
                break;
            }
            else if (resp.TaskStatus == DashScopeTaskStatus.Failed)
            {
                FailedTaskResponse failed = resp.AsFailed();
                _console.WriteLine($"Failed!");
                _console.WriteLine($"reason: {failed.Code} {failed.Message}");
                break;
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }

    [Fact]
    public async Task StyleReplicateTest()
    {
        using DashScopeClient c = new(_apiKey);
        DashScopeTask task = await c.StyleReplicate(new StyleReplicationInput()
        { 
            ImageUrl = "https://avatars.githubusercontent.com/u/1317141",
            Style = RepliationStyle.FreshAndClean,
        });
        _console.WriteLine(task.TaskId);

        while (true)
        {
            TaskStatusResponse resp = await c.QueryTaskStatus(task.TaskId);
            _console.WriteLine(resp.TaskStatus.ToString());

            if (resp.TaskStatus == DashScopeTaskStatus.Succeeded)
            {
                SuccessTaskResponse success = resp.AsSuccess();
                using HttpClient client = new();
                for (int i = 0; i < success.Results.Count; i++)
                {
                    string url = success.Results[i].Url!;
                    _console.WriteLine(url);
                    using HttpResponseMessage imgResp = await client.GetAsync(url);
                    using Stream imgStream = await imgResp.Content.ReadAsStreamAsync();
                    using FileStream fs = new($"{nameof(StyleReplicateTest)}-{i}.png", FileMode.Create);
                    await imgStream.CopyToAsync(fs);
                }
                break;
            }
            else if (resp.TaskStatus == DashScopeTaskStatus.Failed)
            {
                FailedTaskResponse failed = resp.AsFailed();
                _console.WriteLine($"Failed!");
                _console.WriteLine($"reason: {failed.Code} {failed.Message}");
                break;
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }
}