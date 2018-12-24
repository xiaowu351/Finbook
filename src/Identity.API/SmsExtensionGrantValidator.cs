using Identity.API.Authentication;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Identity.API
{
    public class SmsExtensionGrantValidator : IExtensionGrantValidator
    { 
        private readonly IAuthCodeService _authCodeService;

        public string GrantType => "sms_auth_code";

        public SmsExtensionGrantValidator(IAuthCodeService authCodeService)
        {
            _authCodeService = authCodeService;
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

            if(!await _authCodeService.ValidateAsync(phone, authCode))
            {
                context.Result = errorGrantValidatorResult;
                return;
            }

            //HttpPost 请求user.api
            HttpClient client = new HttpClient();
            var bodyData = new Dictionary<string, string>();
            bodyData.Add("phone", phone);
            var from = new FormUrlEncodedContent(bodyData);
            var response = await client.PostAsync("http://localhost:5000/api/users/check-or-addUser", from);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }

            var result = await response.Content.ReadAsAsync<int>();

            context.Result = new GrantValidationResult(result.ToString(), GrantType);
            return; 
        }
    }
}
