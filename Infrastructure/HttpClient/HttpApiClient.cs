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

        public static void ProcessResponse(HttpResponseMessage response)
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