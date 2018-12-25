using Identity.API.Authentication;
using Identity.API.Services;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Identity.API.Authentication
{
    public class SmsExtensionGrantValidator : IExtensionGrantValidator
    { 
        private readonly IUserService _userService;
        private readonly IAuthCodeService _authCodeService;

        public string GrantType => "sms_auth_code";

        public SmsExtensionGrantValidator(IAuthCodeService authCodeService,IUserService userService)
        { 
            _authCodeService = authCodeService;
            _userService = userService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw.Get("phone");
            var authCode = context.Request.Raw.Get("auth_code");

            var errorGrantValidatorResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            if(string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(authCode))
            {
                context.Result = errorGrantValidatorResult;
                return;
            }
            //检查验证码
            if(!await _authCodeService.ValidateAsync(phone, authCode))
            {
                context.Result = errorGrantValidatorResult;
                return;
            }

            //完成用户注册与登录
            var userId = await _userService.CheckOrAddUserAsync(phone);
            if (userId <= 0)
            {
                context.Result = errorGrantValidatorResult;
                return;
            }

            context.Result = new GrantValidationResult(userId.ToString(), GrantType);
            return; 
        }
    }
}
