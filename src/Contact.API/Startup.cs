using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Exceptions;
using Contact.API.Extensions;
using Contact.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceDiscovery.Consul;
using Finbook.BuildingBlocks.EventBus.RabbitMQ.Extensions;
using Contact.API.IntegrationEvents.Events;
using Contact.API.IntegrationEvents.EventHandling;

namespace Contact.API
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

            services.Configure<AppSettings>(Configuration);
            services.AddScoped(typeof(ContactContext));
            services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();
            services.AddScoped<IContactRepository, MongoContactRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddConsulServiceDiscovery(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddEventBus()
                    .AddTransient<UserInfoChangedIntegrationEventHandler>();

            services.AddResilienceHttpClient(); 
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {

                        options.Audience = "contact_api";
                        options.Authority = "http://localhost:8000";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddMvc(options => options.Filters.Add(typeof(GlobalExceptionFilter)))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseEventBus(evtBus =>
            {
                evtBus.Subscribe<UserInfoChangedIntegrationEvent, UserInfoChangedIntegrationEventHandler>();
            });
            app.UseAuthentication();
            app.UseConsulRegisterService(env);
            app.UseMvc();
        }
    }
}
