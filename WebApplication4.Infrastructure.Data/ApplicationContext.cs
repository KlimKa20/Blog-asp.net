﻿
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Domain.Core;

namespace WebApplication4.Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<Profile>
    {
        public ApplicationContext()
        {
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Tag>()
                .HasMany(c => c.Articles)
                .WithOne(e => e.Tag);
            builder.Entity<Profile>()
                .HasMany(c => c.Articles)
                .WithOne(e => e.Profile);
            builder.Entity<Profile>()
                .HasMany(c => c.Comments)
                .WithOne(e => e.Profile);
            builder.Entity<Article>()
                .HasMany(c => c.Comments)
                .WithOne(e => e.Article);
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
