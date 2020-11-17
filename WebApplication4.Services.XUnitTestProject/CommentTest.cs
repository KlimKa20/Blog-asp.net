using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;
using Xunit;

namespace WebApplication4.Services.XUnitTestProject
{
    public class CommentTest
    {
        private CommentRepository repository;
        public static DbContextOptions<ApplicationContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext12-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static CommentTest()
        {
            
            dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public CommentTest()
        {
            var context = new ApplicationContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            repository = new CommentRepository(context);
        }

        [Fact]
        public void TestFindAllByArticle()
        {
            var Comments = repository.FindAllByArticle(1);
            Assert.NotNull(Comments);
            Assert.IsType<List<Comment>>(Comments);
            Assert.Equal(2, Comments.Count);
            Comments = repository.FindAllByArticle(2);
            Assert.NotNull(Comments);
            Assert.IsType<List<Comment>>(Comments);
            Assert.Single(Comments);
        }
        [Fact]
        public async void TestCreate()
        {
            var Comments = repository.FindAllByArticle(1);
            Assert.NotNull(Comments);
            Assert.IsType<List<Comment>>(Comments);
            Assert.Equal(2, Comments.Count);
            await repository.Create(new Comment { DateTime = DateTime.Now, ArticleID = 1, Text = "Test4" });
            Comments = repository.FindAllByArticle(1);
            Assert.NotNull(Comments);
            Assert.IsType<List<Comment>>(Comments);
            Assert.Equal(3, Comments.Count);
        }
    }
}
