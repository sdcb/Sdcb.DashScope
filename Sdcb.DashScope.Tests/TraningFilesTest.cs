using Microsoft.Extensions.Configuration;
using Sdcb.DashScope.TrainingFiles;
using Xunit.Abstractions;

namespace Sdcb.DashScope.Tests;

public class TrainingFilesTest
{
    public TrainingFilesTest(ITestOutputHelper console)
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
    public async Task List()
    {
        DashScopeClient c = new(_apiKey);
        PaginatedFiles r = await c.TrainingFiles.List();
        _console.WriteLine(r.ToString());
        foreach (var file in r.Files)
        {
            _console.WriteLine(file.ToString());
        }
    }

    [Fact]
    public async Task ListAndDeleteAll()
    {
        DashScopeClient c = new(_apiKey);
        int pageSize = 100;
        for (int pageNo = 1; ; ++pageNo)
        {
            PaginatedFiles r = await c.TrainingFiles.List(pageNo, pageSize);

            foreach (string id in r.Files.Select(x => x.FileId))
            {
                await c.TrainingFiles.Delete(id);
                _console.WriteLine($"deleted: {id}");
            }

            if (r.Files.Count < pageSize) break;
        }
    }

    [Fact]
    public async Task UploadAndDelete()
    {
        DashScopeClient c = new(_apiKey);
        UploadedResponse r = await c.TrainingFiles.Upload(new TrainingFile[]
        {
            TrainingFile.FromText("test.txt", "This is a test file."),
            TrainingFile.FromText("test2.txt", "This is another test file."),
        });
        foreach (UploadedFile file in r.UploadedFiles)
        {
            _console.WriteLine($"uploaded: {file}");
        }
        string[] ids = r.UploadedFiles.Select(x => x.FileId).ToArray();

        TrainingFileInfo item = await c.TrainingFiles.GetFileInfo(ids[1]);
        _console.WriteLine(item.ToString());

        foreach (string id in ids)
        {
            await c.TrainingFiles.Delete(id);
            _console.WriteLine($"deleted: {id}");
        }
    }
}
