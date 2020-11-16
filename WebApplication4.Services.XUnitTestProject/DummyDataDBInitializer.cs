using System;
using System.Collections.Generic;
using System.Text;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;

namespace WebApplication4.Services.XUnitTestProject
{
    class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(ApplicationContext context)
        {

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Articles.AddRange(
                new Article {  DateTime = DateTime.Now, Title = "First", Text = "Test1", TagID = 1 },
                new Article { DateTime = DateTime.Now, Title = "First", Text = "Test2", TagID = 2 },
                new Article {   DateTime = DateTime.Now, Title = "First", Text = "Test3", TagID = 3 },
                new Article {   DateTime = DateTime.Now, Title = "First", Text = "Test4", TagID = 1 }
            );

            context.SaveChanges();
        }
    }
}
