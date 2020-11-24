using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HttpApiClient
    {
        private readonly HttpClient client;
        private readonly string URL;

        public HttpApiClient(string token)
        {
            client = new HttpClient();
            URL = $"https://cloud.iexapis.com/stable/stock/intc/quote/?token={token}";
        }

        private async Task<string> Get()
        {
            var response = await client.GetAsync(URL);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public string GetInfo()
        {
            try
            {
                return Get().Result;
            }
            catch (AggregateException e)
            {
                return e.GetBaseException().Message;
            }
        }
    }
}