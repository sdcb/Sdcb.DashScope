using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.FineTunes;

/// <summary>
/// DashScope client for fine-tuning models.
/// </summary>
public class FineTunesClient
{
    internal FineTunesClient(DashScopeClient client)
    {
        Parent = client;
    }

    internal DashScopeClient Parent { get; }

    /// <summary>
    /// Initiates a fine-tuning job for a provided base model using specified training files and optional hyperparameters.
    /// </summary>
    /// <param name="fileIds">An array of strings representing the file IDs for the training set.</param>
    /// <param name="model">The name of the base model for customization, or the output of another fine-tuned job ('finetuned_output').</param>
    /// <param name="hyperParameters">A dictionary representing the hyperparameters for model customization. If null, the system will use default values.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the asynchronous request.</param>
    /// <returns>an asynchronous <see cref="FineTuneJob"/> representing the initiated fine-tune job.</returns>
    public async Task<FineTuneJob> StartFineTune(string[] fileIds, string model, Dictionary<string, object>? hyperParameters = null, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage msg = new(HttpMethod.Post, "https://dashscope.aliyuncs.com/api/v1/fine-tunes")
        {
            Content = JsonContent.Create(new
            {
                model,
                training_file_ids = fileIds,
                hyper_parameters = hyperParameters,
            }, options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };
        HttpResponseMessage resp = await Parent.HttpClient.SendAsync(msg, cancellationToken);
        return await Parent.ReadWrapperResponse<FineTuneJob>(resp, cancellationToken);
    }

    /// <summary>
    /// Queries the status of a model fine-tuning task, and retrieves the results of the task upon completion.
    /// </summary>
    /// <param name="jobId">The identifier of the job to query. Example: "ft-202308291948-edc2"</param>
    /// <param name="cancellationToken">A token to cancel the operation, if needed.</param>
    /// <returns>The status of the fine-tuning job.</returns>
    public async Task<FineTuneJobDetailed> GetJobStatus(string jobId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage resp = await Parent.HttpClient.GetAsync($@"https://dashscope.aliyuncs.com/api/v1/fine-tunes/{jobId}", cancellationToken);
        return await Parent.ReadWrapperResponse<FineTuneJobDetailed>(resp, cancellationToken);
    }
}
