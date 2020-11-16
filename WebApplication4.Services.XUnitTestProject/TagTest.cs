//using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Controllers;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;
using Xunit;
using System.Security.Permissions;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using WebApplication4.Domain.Interfaces;


namespace WebApplication4.Services.XUnitTestProject
{
    public class TagTest
    {
        private TagRepository repository;
        public static DbContextOptions<ApplicationContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext12-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static TagTest()
        {

            dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public TagTest()
        {
            var context = new ApplicationContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            repository = new TagRepository(context);
        }
        [Fact]
        public async void TestFindAllTag()
        {
            var Tag = await repository.FindAll();
            Assert.NotNull(Tag);
            Assert.IsType<List<Tag>>(Tag);
            Assert.Equal(4, Tag.Count);
        }
        [Fact]
        public async void TestFirstOrDefaultAsyncTag()
        {
            var Tag = await repository.FirstOrDefaultAsync(3);
            Assert.NotNull(Tag);
            Assert.IsType<Tag>(Tag);
            Assert.Equal("Relationship", Tag.TagName);
        }
       

    }
}
