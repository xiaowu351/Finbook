using Projects.Domain.Events;
using Projects.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Domain.AggregatesModel
{
    public class Project : Entity, IAggregateRoot
    {
        public int UserId { get; set; }

        /// <summary>
        /// 项目Logo
        /// </summary>
        public string Avatar { get; set; }

        public string Company { get; set; }

        /// <summary>
        /// 原BP文件地址
        /// </summary>
        public string OriginBPFile { get; set; }

        /// <summary>
        /// 转换后BP文件地址
        /// </summary>
        public string FormatBPFile { get; set; }

        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        public bool ShowSecurityInfo { get; set; }

        /// <summary>
        /// 公司省的ID
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 公司省的名称
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 公司所在城市ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 公司所在城市名称
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 公司成立时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 项目简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 出让股份比例
        /// </summary>
        public string FinPercentage { get; set; }

        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }

        /// <summary>
        /// 融资金额 单位(万)
        /// </summary>
        public int FinMoney { get; set; }

        /// <summary>
        /// 收入  单位(万)
        /// </summary>
        public int Income { get; set; }

        /// <summary>
        /// 利润 单位(万)
        /// </summary>
        public int Revenue { get; set; }

        /// <summary>
        /// 估值  单位（万）
        /// </summary>
        public int Valuation { get; set; }

        /// <summary>
        /// 佣金分配方式:线下商议、等比例分配
        /// </summary>
        public int BrokerageOptions { get; set; }

        /// <summary>
        /// 是否委托给finbook
        /// </summary>
        public bool OnPlatform { get; set; }

        /// <summary>
        /// 项目的范围设置
        /// </summary>
        public ProjectVisibleRule VisibleRules { get; set; }

        /// <summary>
        /// 根引用项目ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 上级引用项目ID
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// 项目标签，展示用以逗号分隔
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 项目属性：行业领域、融资币种
        /// </summary>
        public List<ProjectProperty> Properties { get; set; }

        /// <summary>
        /// 贡献者列表
        /// </summary>
        public List<ProjectContributor> Contributors { get; set; }

        /// <summary>
        /// 查看者列表
        /// </summary>
        public List<ProjectViewer> Viewers { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatetTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        public Project()
        {
            Viewers = new List<ProjectViewer>();
            Contributors = new List<ProjectContributor>();

            AddDomainEvent(new ProjectCreatedDomainEvent {  Project = this});
        }

        /// <summary>
        /// 添加查看者
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="avatar"></param>
        public void AddViewer(int userId,string userName,string avatar)
        {
            //检查是否已存在
            var viewer = new ProjectViewer {
                UserId = userId,
                UserName = userName,
                Avatar = avatar,
                CreateTime = DateTime.Now 
            };
            
            if(!Viewers.Any(v=>v.UserId == userId))
            {
                Viewers.Add(viewer);

                AddDomainEvent(new ProjectViewerCreatedDomainEvent { Viewer = viewer });
            }

        }

        /// <summary>
        /// 添加贡献者
        /// </summary>
        /// <param name="contributor"></param>
        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.UserId == UserId))
            {
                Contributors.Add(contributor);
                AddDomainEvent(new ProjectContributorCreatedDomainEvent { Contributor = contributor });
            }
        }
    }
}
