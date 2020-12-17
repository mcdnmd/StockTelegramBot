using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IHttpClient
    {
        public Task<string> Get(string url);
    }
}