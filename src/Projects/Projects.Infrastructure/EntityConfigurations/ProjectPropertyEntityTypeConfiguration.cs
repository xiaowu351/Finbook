using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projects.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projects.Infrastructure.EntityConfigurations
{
    public class ProjectPropertyEntityTypeConfiguration : IEntityTypeConfiguration<ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<ProjectProperty> projectPropertyConfiguration)
        {
            projectPropertyConfiguration.ToTable("projectProperties")
                               .HasKey(p => new { p.ProjectId, p.Key, p.Value });

            projectPropertyConfiguration.Property(p => p.Key).HasMaxLength(100);
            projectPropertyConfiguration.Property(p => p.Value).HasMaxLength(100);
            projectPropertyConfiguration.Property(p => p.Text).HasMaxLength(512); 
        }
    }
}
