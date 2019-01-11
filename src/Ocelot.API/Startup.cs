using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace Ocelot.API
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

            services.Configure<IdentityClientSettings>(Configuration.GetSection(nameof(IdentityClientSettings)));
            
            var clientSettings = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityClientSettings>>().Value;
            //这里启用IdentityServer4进行认证和鉴权
            services.AddAuthentication()
                    .AddIdentityServerAuthentication("finbookKey", o =>
                    {
                             
                        o.Authority = "http://localhost:8000";
                        o.ApiName = "gateway_api";
                        o.ApiSecret = "secret";
                        o.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
                        o.RequireHttpsMetadata = false;
                    });

            services.AddOcelot()
                    .AddConsul();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOcelot().Wait();
            //app.UseMvc();
        }
    }
}
