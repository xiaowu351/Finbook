using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Dtos.Consul;

namespace User.API.Extensions
{
    public static class ConsulRegisterExtensions
    {
        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {

            #region 固定写法
            /*
            var consulClient = new ConsulClient();
            var port = 5000;
            var name = "User.API";
            var id = $"{name}:{port}";

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),//在服务实例被标记为不健康超过一分钟时自动取消注册
                Interval = TimeSpan.FromSeconds(10), //每10秒进行一次健康检查
                HTTP = $"http://localhost:{port}/healthcheck"
            };

            var tcpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),//在服务实例被标记为不健康超过一分钟时自动取消注册
                Interval = TimeSpan.FromSeconds(10), //每10秒进行一次健康检查
                TCP = $"localhost:{port}"
            };



            var registration = new AgentServiceRegistration()
            {
                ID = id,
                Name = name,
                Address = "127.0.0.1",
                Port = port,
                Checks = new[] { httpCheck, tcpCheck }

            }; 



            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            */
            #endregion

            var consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var serviceDiscoveryOptions = app.ApplicationServices.GetService<IOptions<ServiceDiscoveryOptions>>().Value;

            var features = app.Properties["server.Features"] as FeatureCollection;//server.Features 只有在使用 kestrel托管服务时，才可用
            var addresses = features.Get<IServerAddressesFeature>()
                                  .Addresses.Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceDiscoveryOptions.ServiceName}_{address.Host}:{address.Port}";
                var httpCheck = new AgentServiceCheck() {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString 
                };

                var registration = new AgentServiceRegistration() {
                    Check = httpCheck,
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceDiscoveryOptions.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).Wait();
                lifetime.ApplicationStopping.Register(()=> {
                    consul.Agent.ServiceDeregister(registration.ID).Wait();
                }); 
            }

            return app;
        }
    }
}
