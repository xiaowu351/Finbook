using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Authentication
{
    public class ProfileService : IProfileService
    { 

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            context.IssuedClaims = claims;
            await Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            await Task.CompletedTask;
        }
    }
}
