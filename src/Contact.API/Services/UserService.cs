using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Http;
using ServiceDiscovery.Consul;

namespace Contact.API.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IHttpClient _httpClient;
        private readonly IDnsQuery _dns;
        private readonly ServiceDiscoveryOptions _options;
        private readonly DependencyServiceDiscoverySettings _dependencyService;

        public UserService(ILogger<UserService> logger, 
            IHttpClient httpClient, 
            IDnsQuery dns, 
            IOptions<ServiceDiscoveryOptions> options, 
            IOptions<DependencyServiceDiscoverySettings> dependencyService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _dns = dns;
            _options = options.Value;
            _dependencyService = dependencyService.Value;
        }
        public async Task<UserIdentity> GetBaseUserInfoAsync(int userId)
        {

            var hostEntries = await _dns.ResolveServiceAsync("service.consul", _dependencyService.UserServiceName);
            if (hostEntries == null || hostEntries.Length <= 0)
            {
                var msg = $"在Service.consul:{_options.Consul.DnsEndpoint.ToIPEndPoint()} 中未找到 ServiceName={_dependencyService.UserServiceName}";
                _logger.LogWarning(msg);
                throw new ArgumentNullException(nameof(_dependencyService.UserServiceName), msg);
            }
            var hostEntry = hostEntries.First();

            // TBD 需要传Token

            var result = await _httpClient.GetStringAsync($"http://{hostEntry.HostName}:{hostEntry.Port}/api/users/baseInfo/"+userId);


            return  JsonConvert.DeserializeObject<UserIdentity>(result);  

        }
    }
}
