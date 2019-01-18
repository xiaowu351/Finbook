using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ServiceDiscovery.Consul
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConsulRegisterService(this IApplicationBuilder app, IHostingEnvironment env)
        {

            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            var consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var serviceDiscoveryOptions = app.ApplicationServices.GetService<IOptions<ServiceDiscoveryOptions>>().Value;

            var logger = loggerFactory.CreateLogger("ServiceDiscoveryBuilder");

            if (string.IsNullOrWhiteSpace(serviceDiscoveryOptions.ServiceName))
            {
                throw new ArgumentException("Service Name must be configured", nameof(serviceDiscoveryOptions.ServiceName));
            }

            IEnumerable<Uri> addresses = null;
            if (serviceDiscoveryOptions.Endpoints != null && serviceDiscoveryOptions.Endpoints.Length > 0)
            {
                logger.LogInformation($"Using {serviceDiscoveryOptions.Endpoints.Length} configured endpoints for service registration.");
                addresses = serviceDiscoveryOptions.Endpoints.Select(p => new Uri(p));
            }
            else
            {
                logger.LogInformation($"Trying to use server.Features to figure out the service endpoints for service registration.");
                var features = app.Properties["server.Features"] as FeatureCollection;//server.Features 只有在使用 kestrel托管服务时，才可用
                addresses = features.Get<IServerAddressesFeature>()
                                      .Addresses.Select(p => new Uri(p));
            }

            logger.LogInformation($"Found {addresses.Count()} endpoints:{string.Join(",", addresses.Select(p => p.OriginalString))}.");
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceDiscoveryOptions.ServiceName}_{address.Host}:{address.Port}";

                logger.LogInformation($"Registering service {serviceId} for address {address}.");

                var serviceChecks = new List<AgentServiceCheck>();
                //强制必须配置HealthCheckTemplate，否则不向Consul注册服务
                if (!string.IsNullOrWhiteSpace(serviceDiscoveryOptions.HealthCheckTemplate))
                {
                    var healthCheckUri = new Uri(address, serviceDiscoveryOptions.HealthCheckTemplate).OriginalString;
                    var serviceCheck =  new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                        Interval = TimeSpan.FromSeconds(30),
                        HTTP = healthCheckUri
                    };
                    if (env.IsDevelopment())
                    {
                        serviceCheck.DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(30);
                        serviceCheck.Interval = TimeSpan.FromSeconds(30);
                    }

                    serviceChecks.Add(serviceCheck);
                    logger.LogInformation($"Adding healthcheck for service {serviceId},checking {healthCheckUri}");


                    var registration = new AgentServiceRegistration()
                    {
                        Checks = serviceChecks.ToArray(),
                        Address = address.Host,
                        ID = serviceId,
                        Name = serviceDiscoveryOptions.ServiceName,
                        Port = address.Port
                    };

                    consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
                    lifetime.ApplicationStopping.Register(() =>
                    {
                        consul.Agent.ServiceDeregister(registration.ID).GetAwaiter().GetResult();
                    });
                }
            }

            return app;
        }
    }
}
