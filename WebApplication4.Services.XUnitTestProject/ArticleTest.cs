﻿//using Microsoft.AspNetCore.Mvc;
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
    public class ArticleTest
    {

        private ArticleRepository repository;
        public static DbContextOptions<ApplicationContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext12-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

        static ArticleTest()
        {

            dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        Mock<ArticleRepository> mockArticleRepo;
        public readonly ArticleRepository MockArticleRepository;
        public ArticleTest()
        {
            var context = new ApplicationContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);

            repository = new ArticleRepository(context);
            //List<Article> articles = new List<Article>
            //{
            //    new Article {ProfileID = "1", ArticleID = 1, DateTime = DateTime.Now , Title = "First", Text = "Test1",TagID = 1},
            //    new Article {ProfileID = "2", ArticleID = 2, DateTime = DateTime.Now , Title = "First", Text = "Test2",TagID = 2},
            //    new Article {ProfileID = "3", ArticleID = 3, DateTime = DateTime.Now , Title = "First", Text = "Test1",TagID = 3},
            //    new Article {ProfileID = "1", ArticleID = 4, DateTime = DateTime.Now , Title = "First", Text = "Test4",TagID = 1}
            //};
            //mockArticleRepo = new Mock<ArticleRepository>();
            //mockArticleRepo.Setup(mr => mr.FindAll().Result).Returns(articles);
            ////mockArticleRepo.Setup(mr => mr.FindAllbyTag(It.IsAny<int>())).Returns((int s) => Task.FromResult(articles.Where(x => x.TagID == s).ToList()));
            //MockArticleRepository = mockArticleRepo.Object;
        }

        //[Fact]
        //public void TestArticlesDetailsNull()
        //{

        //    List<Article> testArticle = MockArticleRepository.FindAll1();

        //    Assert.NotNull(testArticle); // Test if null
        //    Assert.Single(testArticle);
        //}
        [Fact]
        public async void TestFindAll()
        {
            //Setup DbContext and DbSet mock  
            //var dbContextMock = new Mock<ApplicationContext>();
            //var dbSetMock = new Mock<DbSet<Article>>();
            //dbSetMock.Setup(s => s.Find(It.IsAny<int>())).Returns(new Article()) ;
            //dbContextMock.Setup(s => s.Set<Article>()).Returns(dbSetMock.Object);

            ////Execute method of SUT (ProductsRepository)  
            //var productRepository = new ArticleRepository(dbContextMock.Object);
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
        public async void TestFindFirst()
        {
            var Article = await repository.FirstOrDefaultAsync(3);
            Assert.NotNull(Article);
            Assert.Equal("Test3", Article.Text);
            Assert.IsType<Article>(Article);
            Article = await repository.FirstOrDefaultAsync(5);
            Assert.Null(Article);
        }
        [Fact]
        public async void TestRemove()
        {
            var Article = await repository.FindAll();
            Assert.NotNull(Article);
            Assert.Equal(4,Article.Count);
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

        //        private Mock<ILogger<ArticlesController>> logging;
        //        private Mock<UserManager<Profile>> mock1;
        //        private Mock<ApplicationContext> mock;
        //        private Mock<ArticleRepository> mock2;
        //        private ArticlesController controller;
        //        public ArticleTest()
        //        {
        //            logging = new Mock<ILogger<ArticlesController>>();
        //            mock = new Mock<ApplicationContext>();
        //            mock2 = new Mock<ArticleRepository>(MockBehavior.Strict);
        //            mock2.Setup(repo => repo.FirstOrDefaultAsync(2)).Returns(Task.FromResult(new Article() { ArticleID = 1, ProfileID = "1", TagID = 1}));
        //            mock.Setup(repo => repo.Profiles.FindAsync(1)).Returns(ProfileAsync);
        //            controller = new ArticlesController(null,mock.Object,null,mock2.Object,logging.Object);
        //        }

        //        [Fact]
        //        public void TestArticlesDetailsNull()
        //        {

        //            var result = controller.Details(null);

        //            Assert.IsType<RedirectResult>(result.Result);
        //        }
        //        [Fact]
        //        public void TestArticlesDetailsArticleNotEmpty()
        //        {

        //            var result = controller.Details(2);

        //            Assert.IsType<RedirectResult>(result.Result);
        //        }

        //        [Fact]
        //        public void TestArticlesEdit()
        //        {

        //            var result = controller.Edit(null);

        //            Assert.IsType<RedirectResult>(result.Result);
        //        }
        //        [Fact]
        //        public void TestArticlesDeleteNull()
        //        {

        //            var result = controller.Delete(null);

        //            Assert.IsType<RedirectResult>(result.Result);
        //        }
        //        [Fact]
        //        public void TestArticlesDeleteArticleNotEmpty()
        //        {

        //            var result = controller.Delete(2);

        //            var viewResult = Assert.IsType<ViewResult>(result.Result);
        //            Assert.IsAssignableFrom<Article>(viewResult.Model);
        //        }
        //        [Fact]
        //        public void TestArticlesEditNull()
        //        {

        //            var result = controller.Edit(null);

        //            Assert.IsType<RedirectResult>(result.Result);
        //        }
        //        [Fact]
        //        public void TestArticlesEditArticleNotEmpty()
        //        {

        //            var result = controller.Edit(2);

        //            var viewResult = Assert.IsType<NotFoundResult>(result.Result);
        //        }

        //        static async ValueTask<Profile> ProfileAsync()
        //        {
        //            return await Task.Run(() => new Profile() { Id = "1" , UserName ="aa"});
        //        }
    }
}
