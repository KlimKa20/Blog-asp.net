using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;

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
            int x = 0;
            if (id != null)
            {
                List<Article> articles;
                try
                {
                    articles = await _context.Articles.Where(e => e.Tag.TagID == id).ToListAsync();
                }
                catch (Exception)
                {
                    _logger.LogError("Doesn't exist id. Controller:Home. Action:Index");
                    return RedirectPermanent("~/Error/Index?statusCode=404");
                }
                
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
