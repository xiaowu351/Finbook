using Microsoft.EntityFrameworkCore;
using Projects.Domain.AggregatesModel;
using Projects.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProjectRepository(ProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<Project> AddProjectAsync(Project project)
        {
            return (await _context.Projects.AddAsync(project)).Entity;
        }

        public async Task<Project> GetAsync(int projectId)
        {
            return await _context.Projects
                           .Include(p => p.Contributors)
                           .Include(p => p.VisibleRules)
                           .Include(p => p.Viewers)
                           .SingleOrDefaultAsync(p => p.Id == projectId);
                    
        }

        public void UpdateProject(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
        }
    }
}
