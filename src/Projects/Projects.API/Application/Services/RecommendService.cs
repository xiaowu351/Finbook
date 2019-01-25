using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects.API.Application.Services
{
    public class RecommendService : IRecommendService
    {
        /// <summary>
        /// 是否是推荐的项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> IsRecommendProject(int projectId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}
