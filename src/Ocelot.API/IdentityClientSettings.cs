using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ocelot.API
{
    /// <summary>
    /// IdentityServer4 Client配置信息
    /// </summary>
    public class IdentityClientSettings
    {
    //    "AuthenticationScheme": "finbookKey",
    //"Authority": "http://localhost:8000",
    //"ApiName": "gateway_api",
    //"ApiSecret": "secret"

        public string AuthenticationScheme { get; set; }
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
    }
}
