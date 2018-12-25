using Identity.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    /// <summary>
    /// Mock手机验证码校验
    /// </summary>
    public class SmsAuthCodeServie : IAuthCodeService
    {
        /// <summary>
        /// 暂时先返回True
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string phone, string authCode)
        {
            return await Task.FromResult<bool>(true);
        }
    }
}
