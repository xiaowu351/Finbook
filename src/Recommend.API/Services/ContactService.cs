using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommend.API.Dtos;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Resilience.Http;
using DnsClient;
using Microsoft.Extensions.Options;
using ServiceDiscovery.Consul;

namespace Recommend.API.Services
{
    public class ContactService : IContactService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IHttpClient _httpClient;
        private readonly IDnsQuery _dns;
        private readonly ServiceDiscoveryOptions _options;
        private readonly DependencyServiceDiscoverySettings _dependencyService;

        public ContactService(ILogger<UserService> logger,
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
        public async Task<List<ContactDto>> GetContactsAsync(int userId)
        {

            var hostEntries = await _dns.ResolveServiceAsync("service.consul", _dependencyService.ContactServiceName);
            if (hostEntries == null || hostEntries.Length <= 0)
            {
                var msg = $"在Service.consul:{_options.Consul.DnsEndpoint.ToIPEndPoint()} 中未找到 ServiceName={_dependencyService.ContactServiceName}";
                _logger.LogWarning(msg);
                throw new ArgumentNullException(nameof(_dependencyService.ContactServiceName), msg);
            }
            var hostEntry = hostEntries.First();

            // TBD 需要传Token 
            var result = await _httpClient.GetStringAsync($"http://{hostEntry.HostName}:{hostEntry.Port}/api/contacts/" + userId);


            return JsonConvert.DeserializeObject<List<ContactDto>>(result);

        } 
    }
}
