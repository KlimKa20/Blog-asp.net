using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using blog_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                List<Article> articles = await _context.Articles.Where(e => e.Tag.TagID == id).ToListAsync();
                ViewData["Tags"] = _context.Tags.FindAsync(id).Result.TagName;
                return View(articles);
            }
            ViewData["Tags"] = "Все статьи";
            return View(await _context.Articles.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> UserArticle(string? UserName)
        {
            List<Article> articles;
            if (UserName == null)
            {
                articles = await _context.Articles.Where(e => e.Profile.UserName == User.Identity.Name).ToListAsync();
            }
            else
            {
                articles = await _context.Articles.Where(e => e.Profile.UserName == UserName).ToListAsync();
            }
            if (articles.Count == 0)
            {
                ViewData["Tags"] = "Нет статей";
            }
            else
            {
                ViewData["Tags"] = "Статьи";
            }
            return View("Index",articles);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UserBlocked(string? UserName)
        {
            var profile = await _context.Profiles.Where(e => e.UserName == UserName).ToListAsync();
            if (profile[0].isBlocked)
            {
                profile[0].isBlocked = false;
            }
            else
            {
                profile[0].isBlocked = true;
            }
            _context.Update(profile[0]);
            await _context.SaveChangesAsync();
            return View("AdminPage", await _context.Profiles.Where(e => e.UserName != "admin").ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminPage(string? UserName)
        {
            return View(await _context.Profiles.Where(e => e.UserName != "admin").ToListAsync());
        }
    }
}
