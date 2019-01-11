using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ServiceDiscovery.Consul
{
    /// <summary>
    /// 服务注册实体类
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set;}

        public ConsulOptions Consul { get; set; }

        /// <summary>
        /// 指定注册服务的自身实际使用的uri地址
        /// </summary>
        public string[] Endpoints { get; set; }

        public string HealthCheckTemplate { get; set; }

    }

    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }
    }

    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
