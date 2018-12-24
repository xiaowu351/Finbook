using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Authentication
{
    /// <summary>
    /// Mock手机验证码校验
    /// </summary>
    public class TestAuthCodeServie : IAuthCodeService
    {
        public async Task<bool> ValidateAsync(string phone, string authCode)
        {
            return await Task.FromResult<bool>(true);
        }
    }
}
