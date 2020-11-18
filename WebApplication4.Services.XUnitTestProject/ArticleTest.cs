
using System;
using System.Collections.Generic;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Services.XUnitTestProject
{
    public class ArticleTest
    {

        private ArticleRepository repository;
        public static DbContextOptions<ApplicationContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext123-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static ArticleTest()
        {

            dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public ArticleTest()
        {
            var context = new ApplicationContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            repository = new ArticleRepository(context);
        }

        [Fact]
        public async void TestFindAll()
        {
            var product = await repository.FindAll();

            Assert.NotNull(product);
            Assert.Equal(4, product.Count);
        }
        [Fact]
        public async void TestTag()
        {
            var TagArticle = await repository.FindAllbyTag(1);
            Assert.NotNull(TagArticle);
            Assert.Equal(2, TagArticle.Count);
            Assert.IsType<List<Article>>(TagArticle);
            TagArticle = await repository.FindAllbyTag(2);
            Assert.NotNull(TagArticle);
            Assert.Single(TagArticle);
            Assert.IsType<List<Article>>(TagArticle);
            Assert.Equal("Test2", TagArticle[0].Text);
            TagArticle = await repository.FindAllbyTag(14);
            Assert.IsType<List<Article>>(TagArticle);
            Assert.Empty(TagArticle);
        }
        
        [Fact]
        public void TestAny()
        {
            var Article = repository.Any(3);
            Assert.True(Article);
            Article = repository.Any(5);
            Assert.False(Article);
        }
        [Fact]
        public async void TestFindFirst()
        {
            var Article = await repository.FirstOrDefaultAsync(4);
            Assert.NotNull(Article);
            Assert.Equal("Test4", Article.Text);
            Assert.IsType<Article>(Article);
            Article = await repository.FirstOrDefaultAsync(5);
            Assert.Null(Article);
        }
        [Fact]
        public async void TestRemove()
        {
            var Article = await repository.FindAll();
            Assert.NotNull(Article);
            Assert.Equal(4, Article.Count);
            await repository.Remove(await repository.FirstOrDefaultAsync(2));
            Article = await repository.FindAll();
            Assert.NotNull(Article);
            Assert.Equal(3, Article.Count);
        }
        [Fact]
        public async void TestCreate()
        {
            var Article = await repository.FindAll();
            Assert.NotNull(Article);
            Assert.Equal(4, Article.Count);
            await repository.Create(new Article { DateTime = DateTime.Now, Title = "First", Text = "Test3", TagID = 2 });
            Article = await repository.FindAll();
            Assert.NotNull(Article);
            Assert.Equal(5, Article.Count);
        }
        [Fact]
        public async void TestUpdate()
        {
            var Article = await repository.FirstOrDefaultAsync(2);
            Article.Text = "Updated";
            await repository.Update(Article);
            Article = await repository.FirstOrDefaultAsync(2);
            Assert.NotNull(Article);
            Assert.Equal("Updated", Article.Text);
        }
    }
}
