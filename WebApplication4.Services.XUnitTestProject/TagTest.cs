
using System.Collections.Generic;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace WebApplication4.Services.XUnitTestProject
{
    public class TagTest
    {
        private TagRepository repository;
        public static DbContextOptions<ApplicationContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext121-bc;Trusted_Connection=True;MultipleActiveResultSets=true";

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
            Tag = await repository.FirstOrDefaultAsync(5);
            Assert.Null(Tag);
        }
       

    }
}
