using Microsoft.EntityFrameworkCore;
using Recommend.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Data
{
    public class ProjectRecommendContext:DbContext
    {

        public ProjectRecommendContext(DbContextOptions<ProjectRecommendContext> contextOptions) 
            : base(contextOptions)
        {

        }

        public DbSet<ProjectRecommend> ProjectRecommends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProjectRecommend>()
                        .ToTable("ProjectRecommends")
                        .HasKey(pc=>pc.Id); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
