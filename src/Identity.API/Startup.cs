using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsClient;
using Identity.API.Authentication;
using Identity.API.Infrastructure;
using Identity.API.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience.Http;
using ServiceDiscovery.Consul;

namespace Identity.API
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
            services.Configure<DependencyServiceDiscoverySettings>(Configuration.GetSection(nameof(DependencyServiceDiscoverySettings)));

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddExtensionGrantValidator<SmsExtensionGrantValidator>()
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddInMemoryIdentityResources(Config.GetIdentityResources());

            services.AddScoped<IProfileService, ProfileService>();

            services.AddSingleton<IResilienceHttpClientFactory, ResilienceHttpClientFactory>(sp => {
                var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 6;
                var exceptionsAllowedBeforeBreaking = 5;
                return new ResilienceHttpClientFactory(logger,httpContextAccessor, retryCount, exceptionsAllowedBeforeBreaking);
            });
            services.AddSingleton<IHttpClient, ResilienceHttpClient>(sp=> sp.GetService<IResilienceHttpClientFactory>().CreateResilienceHttpClient());
            //注册Consul服务配置
            services.AddConsulServiceDiscovery(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));

            services.AddScoped<IAuthCodeService, SmsAuthCodeServie>()
                    .AddScoped<IUserService, UserService>();

            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseConsulRegisterService();
            app.UseIdentityServer(); 
            app.UseMvc();
        }
    }
}
