using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async Task<bool> AddContactInfoAsync(int userId, UserIdentity contact)
        {
            if (_contactContext.ContactBooks.CountDocuments(c=>c.UserId == userId) == 0)
            {//若用户未创建通讯录，则新创建一个
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook { UserId = userId });
            }

            var filter = Builders<ContactBook>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<ContactBook>.Update.AddToSet(c => c.Contacts, new Models.Contact(contact));

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update);
            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }
         
        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateContactInfoAsync(UserIdentity userInfo)
        {
            //1.首先找到信息变更的用户通讯录
            var contactBook = await _contactContext.ContactBooks.Find(cb => cb.UserId == userInfo.UserId).SingleOrDefaultAsync();

            if (contactBook == null)
            {//若未创建通讯录，则忽略
                return true;
            }
            //否则找出该用户的好友Id的列表,然后遍历修改好友Id通讯录中的UserInfo的信息

            //2.找出好友列表的UserId
            var contactIds = contactBook.Contacts.Select(c => c.UserId);

            var filter = Builders<ContactBook>.Filter
                .And(
                      Builders<ContactBook>.Filter.In(c => c.UserId, contactIds), //① 找出与用户userInfo是好友的记录
                      Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, contact => contact.UserId == userInfo.UserId) //② 在Contacts集合中找出userInfo的记录
                     );

            //3.定义更新的document
            var update = Builders<ContactBook>
                         .Update
                         .Set($"{contactBook.Contacts}.$.{nameof(userInfo.Name)}", userInfo.Name)
                         .Set($"{contactBook.Contacts}.$.{nameof(userInfo.Avatar)}", userInfo.Avatar)
                         .Set($"{contactBook.Contacts}.$.{nameof(userInfo.Company)}", userInfo.Company)
                         .Set($"{contactBook.Contacts}.$.{nameof(userInfo.Title)}", userInfo.Title);

            //4.执行批量更新
            var result = await  _contactContext.ContactBooks.UpdateManyAsync(filter, update);
            
            //5.判断返回结果
            return result.MatchedCount == result.ModifiedCount;

        }

        public async Task<List<Models.Contact>> GetContactsAsync(int userId)
        {
            var contackBook = await _contactContext.ContactBooks.Find(c => c.UserId == userId).SingleOrDefaultAsync();
            if (contackBook == null)
            {
                //log TBD
                return new List<Models.Contact>();
            }

            return contackBook.Contacts;
        }

        public async Task<bool> UpdateContactTagsAsync(int userId, int contactId, List<string> tags)
        {

            var contactBook = await _contactContext.ContactBooks.Find(c => c.UserId == userId).SingleOrDefaultAsync();
            if (contactBook == null)
            {
                // log TBD
                return true;
            }
            var contact = new Models.Contact() {   UserId = contactId};
            var filter = Builders<ContactBook>.Filter
                .And(Builders<ContactBook>.Filter.Eq(c => c.UserId, userId),
                      Builders<ContactBook>.Filter.Eq($"{nameof(contactBook.Contacts)}.$.{nameof(contact.UserId)}", contact.UserId));


            var update = Builders<ContactBook>
                         .Update
                         .Set($"{contactBook.Contacts}.$.{nameof(contact.Tags)}", contact.Tags);

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update);

            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }
    }
}
