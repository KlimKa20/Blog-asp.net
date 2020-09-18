using blog_project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class ApplicationContext : IdentityDbContext<Profile>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tag>()
        .HasMany(c => c.Articles)
        .WithOne(e => e.Tag);
            builder.Entity<Tag>().HasData(
            new Tag[]
            {
                new Tag { TagID=1,TagName="Style"},
                new Tag { TagID=2, TagName="Design"},
                new Tag { TagID=3, TagName="Relationship"},
                new Tag { TagID=4, TagName="Eat"}
            });
            base.OnModelCreating(builder);
        }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
