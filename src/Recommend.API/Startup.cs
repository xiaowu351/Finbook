using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.API.Data;
using ServiceDiscovery.Consul;
using Finbook.BuildingBlocks.EventBus.RabbitMQ.Extensions;
using Recommend.API.IntegrationEvents.Events;
using Recommend.API.IntegrationEvents.EventHandling;
using Recommend.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Recommend.API.Extensions;
using Resilience.Http.DependencyInjection.Extensions;
using Zipkin.Extensions;

namespace Recommend.API
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
            services.AddDbContext<ProjectRecommendContext>(options => {
                options.UseMySQL(Configuration.GetConnectionString("MysqlProjectRecommend"));
            }); 

            services.Configure<DependencyServiceDiscoverySettings>(Configuration.GetSection(nameof(DependencyServiceDiscoverySettings)));

            services.AddConsulServiceDiscovery(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));
            services.AddZipkin(Configuration.GetSection(nameof(ZipkinOptions)));
            services.AddTransient<ProjectCreatedIntegrationEventHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddResilienceHttpClient();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactService, ContactService>();
            
            services.AddEventBus();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.Audience = "projectrecommend_api";
                        options.Authority = "http://localhost:8000";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseZipkin();
            app.UseAuthentication();
            app.UseMvc();
            app.UseEventBus(evtBus => {
                evtBus.Subscribe<ProjectCreatedIntegrationEvent, ProjectCreatedIntegrationEventHandler>();
            });
            app.UseConsulRegisterService(env);
        }
    }
}
