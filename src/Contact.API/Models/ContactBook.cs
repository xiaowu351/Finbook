using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Models
{
    /// <summary>
    /// 通讯录
    /// </summary>
    [BsonIgnoreExtraElements]// 忽略Mongo系统字段objectId_id 
    public class ContactBook
    {
        public ContactBook()
        {
            Contacts = new List<Contact>();
        }
        /// <summary>
        /// 用户Id，表明属于那个用户的通讯录
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 联系人列表
        /// </summary>
        public List<Contact> Contacts { get; set; }

    }
}
