using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Models
{
    public class ProjectRecommend
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FromUserId { get; set; }

        public string FromUserName { get; set; }

        /// <summary>
        /// 推荐类型： 1，好友推荐； 2，系统推荐；3，二度好友推荐
        /// </summary>
        public EnumRecommendType RecommendType { get; set; }

        public int ProjectId { get; set; }

        public string ProjectAvatar { get; set; }

        public string Company { get; set; }

        public string Introduction { get; set; }

        public string Tags { get; set; }

        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime RecommendTime { get; set; } 

    }
}
