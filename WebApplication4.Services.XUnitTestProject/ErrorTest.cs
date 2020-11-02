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
    public class ErrorTest
    {
        private Mock<ILogger<Error>> logging;
        private Error controller;
        public ErrorTest()
        {
            logging = new Mock<ILogger<Error>>();
            controller = new Error(logging.Object);
        }
        [Fact]
        public void TestErrorIndexNotNull()
        {

            var result = controller.Index(2);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(2, viewResult.ViewData["Error"]);
        }
        [Fact]
        public void TestErrorIndexNull()
        {

            var result = controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData["Error"]);
        }
    }
}

