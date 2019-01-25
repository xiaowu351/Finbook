using Projects.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Domain.AggregatesModel
{
    public interface IProjectRepository:IRepository<Project>
    {
        Task<Project> GetAsync(int projectId);

        Task<Project> AddProjectAsync(Project project);

        void UpdateProject(Project project); 
    }
}
