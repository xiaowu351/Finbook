using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience.Http;
using ServiceDiscovery.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class UserService : IUserService
    { 
        private readonly ILogger<UserService> _logger;
        private readonly IHttpClient _httpClient;
        private readonly IDnsQuery _dns;
        private readonly ServiceDiscoveryOptions _options;
        private readonly DependencyServiceDiscoverySettings _dependencyService;

        public UserService(ILogger<UserService> logger,IHttpClient httpClient,IDnsQuery dns,IOptions<ServiceDiscoveryOptions> options,IOptions<DependencyServiceDiscoverySettings> dependencyService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _dns = dns;
            _options = options.Value;
            _dependencyService = dependencyService.Value;
        }


        public async Task<int> CheckOrAddUserAsync(string phone)
        {

            var hostEntries = await _dns.ResolveServiceAsync("service.consul", _dependencyService.UserServiceName);
            if(hostEntries == null || hostEntries.Length <= 0)
            {
                var msg = $"在Service.consul:{_options.Consul.DnsEndpoint.ToIPEndPoint()} 中未找到 ServiceName={_dependencyService.UserServiceName}";
                _logger.LogWarning(msg);
                throw new ArgumentNullException(nameof(_dependencyService.UserServiceName),msg);
            }
            var hostEntry = hostEntries.First();
                   
            var data = new Dictionary<string, string>() { { "phone", phone } };
            var response =await _httpClient.PostAsync($"http://{hostEntry.HostName}:{hostEntry.Port}/api/users/check-or-create", data);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<int>();
            } 
            return 0; 
        }
    }
}
