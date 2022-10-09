using System;
using System.Net.Http;

namespace StorMatch;
public static class HttpServer
{
    public static async Task<string> Get(string url)
    {
        try
        {
            var client = new HttpClient();
            string response = await client.GetStringAsync(url);
            Console.WriteLine(response);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong", e);
        }
        return null;
    }
}
