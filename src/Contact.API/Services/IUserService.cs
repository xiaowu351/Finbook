using Contact.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Services
{
    public interface IUserService
    {
        Task<BaseUserInfo> GetUserInfoAsync(int userId);
    }
}
