using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Dtos.Consul
{
    /// <summary>
    /// 服务注册实体类
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set;}

        public ConsulOptions Consul { get; set; }
    }
}
