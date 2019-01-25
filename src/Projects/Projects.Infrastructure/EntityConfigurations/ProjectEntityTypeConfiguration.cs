using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projects.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Projects.Infrastructure.EntityConfigurations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> projectConfiguration)
        {
            projectConfiguration.ToTable("projects")
                                .HasKey(p => p.Id);
            
            //mysql下需要配置bool->short关系，或者其他的映射关系，若没有，oracle官方的驱动无法自动转换
            projectConfiguration.Property(p => p.ShowSecurityInfo).HasConversion(new BoolToZeroOneConverter<short>());
            projectConfiguration.Property(p => p.OnPlatform).HasConversion(new BoolToZeroOneConverter<short>());

            //projectConfiguration.OwnsMany(p => p.Properties);
            projectConfiguration.Ignore(b => b.DomainEvents);
        }
    }
}
