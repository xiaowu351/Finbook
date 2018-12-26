using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Extensions
{
    public static class ConsulRegisterExtensions
    {
        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {

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

            return app;
        }
    }
}
