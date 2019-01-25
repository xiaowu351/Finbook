using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projects.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projects.Infrastructure.EntityConfigurations
{
    public class ProjectViewerEntityTypeConfiguration : IEntityTypeConfiguration<ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<ProjectViewer> projectViewerConfiguration)
        {
            projectViewerConfiguration.ToTable("projectViewers")
                                .HasKey(p => p.Id); 
            
            //projectViewerConfiguration.Ignore(b => b.DomainEvents);
        }
    }
}
