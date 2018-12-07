using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace User.API.Data
{
    public class AppUserContextSeed
    { 
        public static void  SeedData(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory, int? retry = 0)
        {
            var retryForAvaiability = retry.Value;
            var logger = loggerFactory.CreateLogger<AppUserContextSeed>();
            try
            {
                using (var scope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    AppUserContext context = scope.ServiceProvider.GetService<AppUserContext>();
                    //ILogger<AppUserContextSeed> logger = scope.ServiceProvider.GetService<ILogger<AppUserContextSeed>>();
                    logger.LogDebug("Begin AppUserContextSeed SeedData");
                    context.Database.Migrate();
                    if (!context.AppUsers.Any())
                    {
                        context.AppUsers.Add(new Models.AppUser { Name = "test" });
                         context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    logger.LogError(ex, "AppUserContextSeed SeedData Error");

                    SeedData(applicationBuilder, loggerFactory, retryForAvaiability);
                }
            }
        }
    }
}
