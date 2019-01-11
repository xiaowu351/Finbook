using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Services
{
    public class UserService : IUserService
    {
        public async Task<BaseUserInfo> GetUserInfoAsync(int userId)
        {
            return await Task.FromResult<BaseUserInfo>(new BaseUserInfo
            {
                UserId = 1,
                Company = "dingxin",
                Name = "dingxin",
                Title = "CTO"
            });
        }
    }
}
