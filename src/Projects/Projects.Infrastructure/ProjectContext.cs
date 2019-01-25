using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Projects.Domain.AggregatesModel;
using Projects.Domain.SeedWork;
using Projects.Infrastructure.EntityConfigurations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Projects.Infrastructure
{
    public class ProjectContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        public ProjectContext(DbContextOptions<ProjectContext> options,IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectContributor> Contributors { get; set; }

        public DbSet<ProjectViewer> Viewers { get; set; }

        public DbSet<ProjectVisibleRule> VisibleRules { get; set; }


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //分派领域事件集合
            // 选择：
            // A) 在将数据（EF SaveChange）提交到数据库之前，将执行单个事务
            // B）在将数据( EF SaveChange) 提交到数据库之后，将立即执行多个事务。
            //    在任何处理程序出现故障时，您都需要处理最终一致性和补偿操作。
            await _mediator.DispatchDomainEventAsync(this);

            // 执行这一行后，所有更改（来自命令处理程序和领域事件处理程序）通过DbContext执行将被激活
            var result = await base.SaveChangesAsync();
            return true;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectViewerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectContributorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVisibleRuleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectPropertyEntityTypeConfiguration());
        }

    }
}
