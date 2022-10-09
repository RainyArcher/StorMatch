using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace StorMatch;
public class HttpServer
{
    public async Task<Dictionary<string, object>> Get(string url, string key="", string value="")
    {
        Dictionary<string, object> data = null;
        try
        {
            var client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add(key, value);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong", e);
            throw;
        }
        return data;
    }
}
