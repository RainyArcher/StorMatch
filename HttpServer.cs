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
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                request.Headers.Add(key, value);
            }
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                System.Diagnostics.Debug.WriteLine("Succesful response");
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
