using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class UserService : IUserService
    {
        private readonly string _userServiceUrl = "http://localhost:5000";
        private readonly IHttpClient _httpClient;

        public UserService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<int> CheckOrAddUserAsync(string phone)
        {
            var data = new Dictionary<string, string>() { { "phone", phone } };
            var response =await _httpClient.PostAsync($"{_userServiceUrl}/api/users/check-or-create", data);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<int>();
            } 
            return 0; 
        }
    }
}
