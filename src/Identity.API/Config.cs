using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API
{
    public class Config
    {
        /// <summary>
        /// 配置api资源名称 
        /// 哪些API可以使用这个Authorization Server
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("gateway_api","Gateway API")
            };
        }

        /// <summary>
        /// 配置客户端：即第三方应用程序
        /// 
        /// 哪些客户端Client（应用）可以使用这个 Authorization Server
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                new Client(){
                     ClientId="android",
                     ClientName="android",  
                    AllowedGrantTypes = {"sms_auth_code" },
                     ClientSecrets = new List<Secret>{
                         new Secret("secret".Sha256())
                     },
                     
                     AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        "gateway_api" //Client拿到Token时，可以访问gateway_api的服务
                    },
                     AllowOfflineAccess = true, 
                     AlwaysIncludeUserClaimsInIdToken = true 
                },

            };
        }

        /// <summary>
        /// 配置密码模式（resource owner password credentials）的用户名和密码
        /// 
        /// 指定可以使用Authorization Server授权的用户。这里是供学习和测试使用的用户类型，实际环境需根据项目而定
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser (){
                     SubjectId="10000",
                     Username="zhangsan",
                     Password="123456"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() 
            };
        }

    }
}
