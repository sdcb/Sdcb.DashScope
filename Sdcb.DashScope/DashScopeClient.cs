using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sdcb.DashScope;

public class DashScopeClient
{
    HttpClient _http = new();

    public DashScopeApi(string apiKey)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public void Dispose() => _http.Dispose();

    private async Task<T> ReadResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new DashScopeException(await response.Content.ReadAsStringAsync());
        }

        try
        {
            return (await response.Content.ReadFromJsonAsync<T>())!;
        }
        catch (Exception e) when (e is NotSupportedException or JsonException)
        {
            throw new DashScopeException($"failed to convert following json into: {typeof(T).Name}: {await response.Content.ReadAsStringAsync()}", e);
        }
    }
}
