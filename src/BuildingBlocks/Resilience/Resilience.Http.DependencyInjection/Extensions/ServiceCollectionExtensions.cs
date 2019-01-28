 
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resilience.Http.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册ResilienceHTTPClient（采用Pollicy库实现）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="exceptionsAllowedBeforeBreaking">在发生了exceptionsAllowedBeforeBreaking次数时，熔断打开</param>
        /// <returns></returns>
        public static IServiceCollection AddResilienceHttpClient(this IServiceCollection services,int retryCount=6,int exceptionsAllowedBeforeBreaking=5)
        {
             
            services.AddSingleton<IResilienceHttpClientFactory, ResilienceHttpClientFactory>(sp => {
                var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>(); 
                 
                return new ResilienceHttpClientFactory(logger, httpContextAccessor, retryCount, exceptionsAllowedBeforeBreaking);
            });
            services.AddSingleton<IHttpClient, ResilienceHttpClient>(sp => sp.GetService<IResilienceHttpClientFactory>().CreateResilienceHttpClient());

            return services;
        } 
    }
}
