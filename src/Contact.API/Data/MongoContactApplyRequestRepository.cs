using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactApplyRequestRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        /// <summary>
        ///  添加 申请好友的请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddRequestAsync(ContactApplyRequest request)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(c => c.UserId == request.UserId
                                            && c.ApplierId == request.ApplierId);
            
             
            if (_contactContext.ContactApplyRequests.Find(filter).CountDocuments()>0)
            {
                var update = Builders<ContactApplyRequest>.Update.Set(c => c.ApplyTime, DateTime.Now);
                var result = await _contactContext.ContactApplyRequests.UpdateOneAsync(filter, update);
                return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
            }

            await _contactContext.ContactApplyRequests.InsertOneAsync(request);

            return true;
        }

        /// <summary>
        /// 通过好友的请求
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        public async Task<bool> ApprovalAsync(int userId, int applierId)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(c => c.UserId == userId
                                            && c.ApplierId == applierId);
            var update = Builders<ContactApplyRequest>.Update
                                                      .Set(c => c.HandleTime, DateTime.Now)
                                                      .Set(c => c.Approvaled, 1);
            var result = await _contactContext.ContactApplyRequests
                                .UpdateOneAsync(filter, update);

            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }

        /// <summary>
        /// 好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ContactApplyRequest>> GetReuestListAsync(int userId)
        {
            //var filter = Builders<ContactApplyRequest>.Filter.Where(c => c.UserId == userId);
            //return await _contactContext.ContactApplyRequests.Find(filter).ToListAsync();

            return await _contactContext.ContactApplyRequests.Find(c => c.UserId == userId).ToListAsync();
        }
    }
}
