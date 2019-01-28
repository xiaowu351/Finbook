 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Services
{
    public interface IUserService
    {
        Task<Dtos.UserIdentity> GetBaseUserInfoAsync(int userId);
    }
}
