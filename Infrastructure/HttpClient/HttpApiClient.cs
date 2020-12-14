using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HttpApiClient
    {
        private readonly HttpClient client;
        private readonly string Url;

        public HttpApiClient(string url)
        {
            client = new HttpClient();
            Url = url;
        }


        public async Task<string> Get()
        {
            var response = await client.GetAsync(Url);
            ProcessResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        public static void ProcessResponse(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException)
            {
                Console.WriteLine(response.RequestMessage);
            }
        }
    }

}