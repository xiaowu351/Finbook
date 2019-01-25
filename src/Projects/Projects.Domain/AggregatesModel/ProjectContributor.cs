using Projects.Domain.SeedWork;
using System;

namespace Projects.Domain.AggregatesModel
{
    /// <summary>
    /// 贡献者
    /// </summary>
    public class ProjectContributor:Entity
    {
        public int ProjectId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否为关闭者
        /// </summary>
        public bool IsCloser { get; set; }

        /// <summary>
        /// 贡献者类型：1，财务顾问；2，投资机构
        /// </summary>
        public int ContributorType { get; set; }
    }
}