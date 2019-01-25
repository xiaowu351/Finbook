using Projects.Domain.SeedWork;
using System;

namespace Projects.Domain.AggregatesModel
{
    public class ProjectViewer:Entity
    {
        public int ProjectId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } 

        public string Avatar { get; set; }

        public DateTime CreateTime { get; set; }
    }
}