using Projects.Domain.SeedWork;

namespace Projects.Domain.AggregatesModel
{
    /// <summary>
    /// 项目可见范围
    /// </summary>
    public class ProjectVisibleRule:Entity

    {

        public int ProjectId { get; set; }

        public bool Visible { get; set; }

        public string Tags { get; set; }
    }
}