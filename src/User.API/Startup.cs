using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceDiscovery.Consul;
using User.API.Data;
using System.IdentityModel.Tokens.Jwt;
using User.API.IntegrationEvents;
using User.API.Extensions;
using Finbook.BuildingBlocks.EventBus.RabbitMQ.Extensions;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Transport.Http;
using zipkin4net.Tracers.Zipkin;
using Zipkin.Extensions;

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

            services.AddConsulServiceDiscovery(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));

            services.AddZipkin(Configuration.GetSection(nameof(ZipkinOptions)));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //注册身份认证服务使用Bearer方式
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.Audience = "user_api";
                        options.Authority = "http://localhost:8000";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddDbContext<AppUserContext>(options => options.UseMySQL(Configuration.GetConnectionString("MysqlUser")));

            services.AddEventBus();
            services.AddScoped<IUserIntegrationEventService, UserIntegrationEventService>();

            services
                .AddMvc(options => options.Filters.Add(typeof(GlobalExceptionFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //UseZipkin(lifetime, loggerFactory);
            //app.UseTracing(Configuration["applicationName"]);

            app.UseZipkin();
            app.UseAuthentication();
            app.UseMvc();
            app.UseConsulRegisterService(env);
            AppUserContextSeed.SeedData(app, loggerFactory);
        }

        /// <summary>
        /// 注册Zipkin
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="loggerFactory"></param>
        private void UseZipkin(IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            lifetime.ApplicationStarted.Register(() => {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender("http://localhost:9411", "application/json");

                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer());
                TraceManager.RegisterTracer(tracer);
                TraceManager.Start(logger);
            });



            lifetime.ApplicationStopped.Register(() => {
                TraceManager.Stop();
            });
        }
    }
}
