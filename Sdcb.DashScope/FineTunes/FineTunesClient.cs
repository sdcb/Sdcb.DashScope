using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sdcb.DashScope.FineTunes;

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
    public async Task<FineTuneJob> FineTunes(string[] fileIds, string model, Dictionary<string, object>? hyperParameters = null, CancellationToken cancellationToken = default)
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
}
