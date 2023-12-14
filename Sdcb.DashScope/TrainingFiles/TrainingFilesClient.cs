using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.DashScope.TrainingFiles;

/// <summary>
/// Model customization file management service, you can manage your training files in a unified way;
/// a single upload allows for multiple reuses in model customization tasks.
/// </summary>
public class TrainingFilesClient
{
    internal TrainingFilesClient(DashScopeClient client)
    {
        Parent = client;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// Uploads a list of training files.
    /// </summary>
    /// <param name="files">A list of <see cref="TrainingFile"/> objects representing the training files to be uploaded.</param>
    /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task{UploadResponse}"/> representing the asynchronous operation, containing the upload status.</returns>
    /// <remarks>
    /// When using this service, there are several usage limitations:
    /// <list type="bullet">
    /// <item>The maximum size for individual files is 300MB.</item>
    /// <item>The total quota for valid (not deleted) storage space is 5GB.</item>
    /// <item>The total quota for the number of valid (not deleted) files is 100.</item>
    /// </list>
    /// </remarks>
    public async Task<UploadedResponse> Upload(IReadOnlyList<TrainingFile> files, CancellationToken cancellationToken = default)
    {
        using MultipartFormDataContent formData = [];
        foreach (TrainingFile file in files)
        {
            formData.Add(new StreamContent(file.Stream), "files", file.Name);
            if (file.Description is not null)
            {
                formData.Add(new StringContent(file.Description), "descriptions", file.Description);
            }
        }

        HttpResponseMessage resp = await Parent.HttpClient.PostAsync("https://dashscope.aliyuncs.com/api/v1/files", formData);
        return await ReadWrapperResponse<UploadedResponse>(resp, cancellationToken);
    }

    /// <summary>
    /// Retrieves a paginated list of TrainingFiles.
    /// </summary>
    /// <param name="pageNo">The current page number to retrieve. The minimum value is 1 and the default value is 1.</param>
    /// <param name="pageSize">The size of the page to retrieve. The minimum value is 1, maximum value is 100, and the default value is 10.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Returns a <see cref="PaginatedFiles"/> object containing the current list of TrainingFiles.</returns>
    public async Task<PaginatedFiles> List(int pageNo = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage resp = await Parent.HttpClient.GetAsync($"https://dashscope.aliyuncs.com/api/v1/files?page_no={pageNo}&page_size={pageSize}", cancellationToken);
        PaginatedFiles result = await ReadWrapperResponse<PaginatedFiles>(resp, cancellationToken);
        return result;
    }

    /// <summary>
    /// Retrieves file information for a given file ID.
    /// </summary>
    /// <param name="fileId">The unique identifier for the file.</param>
    /// <param name="cancellationToken">Optional. A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation and wraps the file information as a <see cref="TrainingFileInfo"/> object.</returns>

    public async Task<TrainingFileInfo> GetFileInfo(string fileId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage resp = await Parent.HttpClient.GetAsync($"https://dashscope.aliyuncs.com/api/v1/files/{fileId}", cancellationToken);
        TrainingFileInfo result = await ReadWrapperResponse<TrainingFileInfo>(resp, cancellationToken);
        return result;
    }

    /// <summary>
    /// Delete a file by its ID.
    /// </summary>
    /// <param name="fileId">The unique identifier for the file.</param>
    /// <param name="cancellationToken">Optional. A token to cancel the asynchronous operation.</param>
    public async Task Delete(string fileId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage resp = await Parent.HttpClient.DeleteAsync($"https://dashscope.aliyuncs.com/api/v1/files/{fileId}", cancellationToken);
        await DashScopeClient.ReadResponse<JsonDocument>(resp, cancellationToken);
    }

    internal async Task<T> ReadWrapperResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return (await DashScopeClient.ReadResponse<TrainingFilesResponseWrapper<T>>(response, cancellationToken)).Data;
    }
}
