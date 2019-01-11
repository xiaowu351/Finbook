using Contact.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public interface IContactApplyRequestRepository
    {
        /// <summary>
        /// 添加 申请好友的请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddRequestAsync(ContactApplyRequest request);


        /// <summary>
        /// 通过好友的请求
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int userId,int applierId);

        /// <summary>
        /// 好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ContactApplyRequest>> GetReuestListAsync(int userId);
    }
}
