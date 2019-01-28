using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommend.API.Dtos;

namespace Recommend.API.Services
{
    /// <summary>
    /// 通讯录相关服务接口
    /// </summary>
    public interface IContactService
    {
        Task<List<ContactDto>> GetContactsAsync(int userId);
    }
}
