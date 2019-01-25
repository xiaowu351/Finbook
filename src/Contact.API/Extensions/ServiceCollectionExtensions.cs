using Contact.API.Infrastructure;
using Contact.API.IntegrationEvents.EventHandling;
using Finbook.BuildingBlocks.EventBus;
using Finbook.BuildingBlocks.EventBus.Abstractions;
using Finbook.BuildingBlocks.EventBus.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.API.Exceptions
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

        /// <summary>
        /// 注册EventBus依赖项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            //var subscriptionClientName = configuration["SubscriptionClientName"];
            var queueName = "finbook_user_api";
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = "localhost"
                }; 
                var retryCount = 5;  
                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    var iLifetimeScope = sp.CreateScope();  //sp.GetRequiredService<IServiceScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(configuration["EventBusRetryCount"]);
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, retryCount);
                });
             

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<UserInfoChangedIntegrationEventHandler>(); 

            return services;
        }
    }
}
