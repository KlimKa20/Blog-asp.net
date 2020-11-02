using Microsoft.AspNetCore.Mvc;
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

namespace WebApplication4.Services.XUnitTestProject
{
    public class UnitTest
    {
        private Mock<ILogger<HomeController>> logging;
        private Mock<ApplicationContext> mock;
        private Mock<ArticleRepository> mock2;
        private HomeController controller;
        public UnitTest()
        {
            logging = new Mock<ILogger<HomeController>>();
            mock = new Mock<ApplicationContext>();
            mock2 = new Mock<ArticleRepository>(MockBehavior.Strict);

            mock2.Setup(repo => repo.FindAll()).Returns(GetTestArticles);
            mock2.Setup(repo => repo.FindAllbyTag(1)).Returns(GetTestArticlesByTag);
            mock2.Setup(repo => repo.FindAllbyName("user")).Returns(Task.FromResult(new List<Article> { new Article { ArticleID = 1, TagID = 1, ProfileID = "1" } }));
            mock2.Setup(repo => repo.FindAllbyName("user0")).Returns( Task.FromResult( new List<Article>()));
            mock.Setup(repo => repo.Articles.Find()).Returns(new Article { ArticleID = 1, TagID = 1, ProfileID = "1" });
            mock.Setup(repo => repo.Tags.Find(1)).Returns( new Tag { TagID = 1, TagName = "style" });
            controller = new HomeController(logging.Object, mock.Object, mock2.Object);
        }
        [Fact]
        public void TestHomeIndexNull()
        {

            var result = controller.Index(null) ;

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<Article>>(viewResult.Model);
            Assert.Equal(4, model.Count());
            Assert.Equal("Все статьи", viewResult.ViewData["Tags"]);
        }
        [Fact]
        public void TestHomeIndexNotNull()
        {

            var result = controller.Index(1);

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<Article>>(viewResult.Model);
            Assert.Equal(2, model.Count());
            Assert.Equal("style", viewResult.ViewData["Tags"]);
        }
        [Fact]
        public void TestHomeUserArticleEmpty()
        {

            var result = controller.UserArticle("user0");

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<Article>>(viewResult.Model);
            Assert.Empty(model);
            Assert.Equal("Нет статей", viewResult.ViewData["Tags"]);
        }

        [Fact]
        public void TestHomeUserArticleNotEmpty()
        {
            var result = controller.UserArticle("user");

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<Article>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("Статьи", viewResult.ViewData["Tags"]);
        }




        private async Task<List<Article>> GetTestArticles()
        {
            var articles = new List<Article>
            {
                new Article { ArticleID = 1, TagID =1 , ProfileID = "1" },
                new Article { ArticleID = 2, TagID =1 , ProfileID = "2" },
                new Article { ArticleID = 3, TagID =2 , ProfileID = "2" },
                new Article { ArticleID = 4, TagID =3 , ProfileID = "1" },
            };
            return await Task.FromResult(articles);
        }
        private async Task<List<Article>> GetTestArticlesByTag()
        {
            var articles = new List<Article>
            {
                new Article { ArticleID = 1, TagID =1 , ProfileID = "1" },
                new Article { ArticleID = 2, TagID =1 , ProfileID = "2" },
            };
            return await Task.FromResult(articles);
        }
    }
}
