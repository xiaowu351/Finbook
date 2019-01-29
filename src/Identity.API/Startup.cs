using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsClient;
using Identity.API.Authentication;
using Identity.API.Exceptions; 
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
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;
using Resilience.Http.DependencyInjection.Extensions;
using Zipkin.Extensions;

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
            
            //注册Consul服务配置
            services.AddConsulServiceDiscovery(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));

            //定义HttpClient创建使用的Handler,以便Zipkin收集跟踪日志
            //var applicationName = "IdentityAPI"; //Configuration["applicationName"];
            //services.AddHttpClient(applicationName)
            //        .AddHttpMessageHandler(sp => TracingHandler.WithoutInnerHandler(applicationName));
            //services.AddResilienceHttpClient(applicationName);

            services.AddZipkin(Configuration.GetSection(nameof(ZipkinOptions)));

            services.AddScoped<IAuthCodeService, SmsAuthCodeServie>()
                    .AddScoped<IUserService, UserService>();

            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime lifetime,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //UseZipkin(lifetime, loggerFactory);
            //app.UseTracing("IdentityAPI");//Configuration["applicationName"]);

            app.UseZipkin(); 
            app.UseConsulRegisterService(env);
            app.UseIdentityServer(); 
            app.UseMvc();
        } 
    }
}
