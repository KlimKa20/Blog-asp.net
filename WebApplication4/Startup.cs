using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication4.Domain.Core;
using WebApplication4.Domain.Interfaces;
using WebApplication4.Infrastructure.Data;
using WebApplication4.Services.BusinessLogic;

namespace WebApplication4
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("WebApplication4")));

            services.AddIdentity<Profile, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            services.AddScoped<ArticleRepository>();
            services.AddScoped<TagRepository>();
            services.AddScoped<CommentRepository>();
            services.AddTransient<ISender, EmailService>();
            services.AddSingleton<ImageService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/ErrorPro");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "MethodWithId",
                   pattern: "{controller}/{action}/{id}");
                endpoints.MapControllerRoute(
                   name: "MainPage",
                   pattern: "Home/{action}/{id?}",
                   null,
                   new { action = new PositionConstraint() });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapHub<HabrDotNetHub>("/chathub");
            });
        }

    }
    public class PositionConstraint : IRouteConstraint
    {
        string[] positions = new[] { "Index", "UserArticle" };
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            return positions.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }
    }

}
