using Contact.API.Dtos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Models
{
    /// <summary>
    /// 联系人
    /// </summary>
    [BsonIgnoreExtraElements]// 忽略Mongo系统字段objectId_id 
    public class Contact
    {
        public Contact()
        {
            Tags = new List<string>();
        }

        public Contact(UserIdentity userInfo):this()
        {
            UserId = userInfo.UserId;
            Name = userInfo.Name;
            Company = userInfo.Company;
            Title = userInfo.Title;
            Avatar = userInfo.Avatar;
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

        public List<string> Tags { get; set; }
    }
}
