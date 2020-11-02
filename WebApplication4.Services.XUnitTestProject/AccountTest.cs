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
using System.Security.Claims;
using WebApplication4.ViewModels;

namespace WebApplication4.Services.XUnitTestProject
{
    public class AccountTest
    {
        private Mock<ILogger<AccountController>> logging;
        private Mock<UserManager<Profile>> mock1;
        private Mock<SignInManager<Profile>> mock;
        private AccountController controller;
        public AccountTest()
        {
            logging = new Mock<ILogger<AccountController>>();
            mock = new Mock<SignInManager<Profile>>();
            mock
    .Setup(_ => _.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
    .Returns(true);
            mock1 = new Mock<UserManager<Profile>>(MockBehavior.Strict);
            controller = new AccountController(null, null, null, logging.Object);
        }

        [Fact]
        public void TestAccountIndexNull()
        {

            var result = controller.Register();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void TestAccountConfirmEmailNull()
        {

            var result = controller.ConfirmEmail(null,null);

            var viewResult = Assert.IsType<Task<IActionResult>>(result);
        }
        [Fact]
        public void TestAccountLoginNull()
        {

            var result = controller.Login();

             Assert.IsType<Task<IActionResult>>(result);
        }
        [Fact]
        public void TestAccountResetPasswordNull()
        {

            var result = controller.ResetPassword("12");

            Assert.IsType<ViewResult>(result);

            result = controller.ResetPassword();

            Assert.IsType<ViewResult>(result);
        }
    }
}
