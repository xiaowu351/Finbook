using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Data
{
    public class AppUserContext : DbContext
    {
        
        public AppUserContext(DbContextOptions<AppUserContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<AppUser>().Property(u => u.Gender).HasColumnType("bit");

            modelBuilder.Entity<AppUser>()
                        .ToTable("Users")
                        .HasKey(u => u.Id);


            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                        .ToTable("UserProperties")
                        .HasKey(u => new { u.AppUserId, u.Key, u.Value });

            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTag>()
                        .ToTable("UserTags")
                        .HasKey(u => new { u.AppUserId, u.Tag });

            modelBuilder.Entity<BPFile>()
                        .ToTable("UserBPFiles")
                        .HasKey(b => b.Id);


            
        }

       
    }
}
