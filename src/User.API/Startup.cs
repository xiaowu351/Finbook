using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Data;
using User.API.Dtos.Consul;
using User.API.Extensions;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(sp => {
                var client = new ConsulClient();
                var serviceConfig = sp.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                if (!string.IsNullOrWhiteSpace(serviceConfig.Consul.HttpEndpoint))
                {
                    //如果未配置，client将是使用默认的值：127.0.0.1:8500
                    client.Config.Address = new Uri(serviceConfig.Consul.HttpEndpoint);
                }
                return client;
            });

            services.AddDbContext<AppUserContext>(options => options.UseMySQL(Configuration.GetConnectionString("MysqlUser")));
            services
                .AddMvc(options => options.Filters.Add(typeof(GlobalExceptionFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.RegisterWithConsul(lifetime); 
            AppUserContextSeed.SeedData(app, loggerFactory);
        } 
    }
}
