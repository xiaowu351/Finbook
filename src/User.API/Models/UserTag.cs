using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class UserTag
    {
        public int AppUserId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
