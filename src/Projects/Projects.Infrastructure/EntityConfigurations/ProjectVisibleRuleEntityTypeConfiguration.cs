using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projects.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projects.Infrastructure.EntityConfigurations
{
    public class ProjectVisibleRuleEntityTypeConfiguration : IEntityTypeConfiguration<ProjectVisibleRule>
    {
        public void Configure(EntityTypeBuilder<ProjectVisibleRule> projectVisibleRuleConfiguration)
        {
            projectVisibleRuleConfiguration.ToTable("projectVisibleRules")
                                .HasKey(p => p.Id);
            //mysql下需要配置bool->short关系，或者其他的映射关系，若没有，oracle官方的驱动无法自动转换
            projectVisibleRuleConfiguration.Property(p => p.Visible).HasConversion(new BoolToZeroOneConverter<short>());
            //projectContributorConfiguration.Ignore(b => b.DomainEvents);
        }
    }
}
