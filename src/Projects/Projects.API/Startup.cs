using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Projects.API.Application.Queries;
using Projects.API.Application.Services;
using Projects.Domain.AggregatesModel;
using Projects.Infrastructure;
using Projects.Infrastructure.Repositories;
using ServiceDiscovery.Consul;
using Finbook.BuildingBlocks.EventBus.RabbitMQ.Extensions;
using Projects.API.Application.IntegrationEvents;

namespace Projects.API
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

            services.AddDbContext<ProjectContext>(options => {
                options.UseMySQL(Configuration.GetConnectionString("MysqlProject"),b=> {
                    var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
                    b.MigrationsAssembly(assemblyName);
                });
            });
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRecommendService, RecommendService>();
            services.AddScoped<IProjectQueries, ProjectQueries>(sp=> {
                var connectionString = Configuration.GetConnectionString("MysqlProject");
                return new ProjectQueries(connectionString);
            });
            services.AddScoped<IProjectIntegrationEventService, ProjectIntegrationEventService>();
            services.AddMediatR();

            services.AddEventBus();

            services.AddConsulServiceDiscovery(Configuration.GetSection(nameof(ServiceDiscoveryOptions)));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.Audience = "project_api";
                        options.Authority = "http://localhost:8000";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseConsulRegisterService(env);
        }
    }
}
