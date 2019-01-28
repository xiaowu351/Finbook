using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resilience.Http.DependencyInjection
{
    public interface IResilienceHttpClientFactory
    {
        ResilienceHttpClient CreateResilienceHttpClient();
    }
}
