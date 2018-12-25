using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 检查用户是否存在，如果存在，则直接返回，否则创建一个新用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<int> CheckOrAddUserAsync(string phone);
    }
}
