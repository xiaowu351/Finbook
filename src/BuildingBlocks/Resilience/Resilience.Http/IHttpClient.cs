using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilience.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer");
    }
}
