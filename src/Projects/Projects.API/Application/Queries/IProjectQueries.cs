using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects.API.Application.Queries
{
    public interface IProjectQueries
    {
        /// <summary>
        /// 查看项目列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetProjectsByUserIdAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param> 
        /// <returns></returns>
        Task<dynamic> GetProjectDetailsAsync(int projectId);
    }
}
