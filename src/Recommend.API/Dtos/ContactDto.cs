 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Dtos
{
    /// <summary>
    /// 联系人
    /// </summary> 
    public class ContactDto
    {
        public ContactDto()
        { 
        } 
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; } 
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
 
    }
}
