using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience.Http;

namespace Recommend.API.Infrastructure
{
    public class ResilienceHttpClientFactory : IResilienceHttpClientFactory
    {

        private readonly ILogger<ResilienceHttpClient> _logger;
        private readonly int _retryCount;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilienceHttpClientFactory(ILogger<ResilienceHttpClient> logger, IHttpContextAccessor httpContextAccessor, int retryCount, int exceptionsAllowedBeforeBreaking)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
        }

        public ResilienceHttpClient CreateResilienceHttpClient()
        {
            return new ResilienceHttpClient(origin => CreatePolicies(), _logger, _httpContextAccessor);
        }

        private IEnumerable<Policy> CreatePolicies()
        {
            return new List<Policy> {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(_retryCount,retryAttempt=> TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),
                (ex,timespan,retryCount,context)=>{
                    var msg = $"第 {retryCount} 重试，重试策略" +
                    $" of {context.PolicyKey}"+
                    $" at {context.OperationKey}"+
                    $" due to:{ex}";

                    _logger.LogWarning(msg);
                    //_logger.LogDebug(msg);
                }),
                Policy.Handle<HttpRequestException>()
                      .CircuitBreakerAsync(_exceptionsAllowedBeforeBreaking,TimeSpan.FromMinutes(1),
                      (ex,duration)=>{
                          _logger.LogTrace("熔断器打开");
                      },
                      ()=>{
                          _logger.LogTrace("断路器复位");
                      })
            };
        }
    }
}
