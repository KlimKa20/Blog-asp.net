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
using Microsoft.AspNetCore.Identity;

namespace WebApplication4.Services.XUnitTestProject
{
    public class ArticleTest
    {
        private Mock<ILogger<ArticlesController>> logging;
        private Mock<UserManager<Profile>> mock1;
        private Mock<ApplicationContext> mock;
        private Mock<ArticleRepository> mock2;
        private ArticlesController controller;
        public ArticleTest()
        {
            logging = new Mock<ILogger<ArticlesController>>();
            mock = new Mock<ApplicationContext>();
            mock2 = new Mock<ArticleRepository>(MockBehavior.Strict);
            mock2.Setup(repo => repo.FirstOrDefaultAsync(2)).Returns(Task.FromResult(new Article() { ArticleID = 1, ProfileID = "1", TagID = 1}));
            mock.Setup(repo => repo.Profiles.FindAsync(1)).Returns(ProfileAsync);
            controller = new ArticlesController(null,mock.Object,null,mock2.Object,logging.Object);
        }

        [Fact]
        public void TestArticlesDetailsNull()
        {

            var result = controller.Details(null);

            Assert.IsType<RedirectResult>(result.Result);
        }
        [Fact]
        public void TestArticlesDetailsArticleNotEmpty()
        {

            var result = controller.Details(2);

            Assert.IsType<RedirectResult>(result.Result);
        }

        [Fact]
        public void TestArticlesEdit()
        {

            var result = controller.Edit(null);

            Assert.IsType<RedirectResult>(result.Result);
        }
        [Fact]
        public void TestArticlesDeleteNull()
        {

            var result = controller.Delete(null);

            Assert.IsType<RedirectResult>(result.Result);
        }
        [Fact]
        public void TestArticlesDeleteArticleNotEmpty()
        {

            var result = controller.Delete(2);

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.IsAssignableFrom<Article>(viewResult.Model);
        }
        [Fact]
        public void TestArticlesEditNull()
        {

            var result = controller.Edit(null);

            Assert.IsType<RedirectResult>(result.Result);
        }
        [Fact]
        public void TestArticlesEditArticleNotEmpty()
        {

            var result = controller.Edit(2);

            var viewResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        static async ValueTask<Profile> ProfileAsync()
        {
            return await Task.Run(() => new Profile() { Id = "1" , UserName ="aa"});
        }
    }
}
