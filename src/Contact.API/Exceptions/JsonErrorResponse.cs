using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Exceptions
{
    /// <summary>
    /// 错误返回时的JSON对象
    /// </summary>
    public class JsonErrorResponse
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Messaage { get; set; }
        /// <summary>
        /// 开发者查看的错误消息
        /// </summary>
        public string DeveloperMessage { get; set; }
    }
}
