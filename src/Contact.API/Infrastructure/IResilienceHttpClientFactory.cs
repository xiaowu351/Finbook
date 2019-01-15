using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Infrastructure
{
    public interface IResilienceHttpClientFactory
    {
        ResilienceHttpClient CreateResilienceHttpClient();
    }
}
