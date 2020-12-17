using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HttpApiClient : IHttpClient
    {
        private readonly HttpClient client;
        private readonly string Url;

        public HttpApiClient()
        {
            client = new HttpClient();
        }


        public async Task<string> Get(string url)
        {
            var response = await client.GetAsync(url);
            try
            {
                ProcessResponse(response);
            }
            catch (Exception e)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }

        private static void ProcessResponse(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine(e + " " + response.RequestMessage);
                throw;
            }
        }
    }

}