using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API
{
    public class DependencyServiceDiscoverySettings
    {
        /// <summary>
        /// 用户服务在consul中注册的服务名称
        /// </summary>
        public string UserServiceName { get; set; }
    }
}
