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
        private readonly ArticleRepository _articleRepository;
        private readonly TagRepository _tagRepository;
        private readonly ProfileRepository _profileRepository;
        public HomeController(ILogger<HomeController> logger, TagRepository tagRepository, ArticleRepository articleRepository, ProfileRepository profileRepository)
        {
            _logger = logger;
            _tagRepository = tagRepository;
            _articleRepository = articleRepository;
            _profileRepository = profileRepository;
        }


        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                List<Article> articles;
                try
                {
                    articles = await _articleRepository.FindAllbyTag(id.Value);
                }
                catch (Exception)
                {
                    _logger.LogError("Doesn't exist id. Controller:Home. Action:Index");
                    return NotFound();
                }
                ViewData["Tags"] = (await _tagRepository.FirstOrDefaultAsync(id.Value)).TagName;
                return View(articles);
            }
            ViewData["Tags"] = "Все статьи";
            return View(await _articleRepository.FindAll());
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
                articles = await _articleRepository.FindAllbyName(User.Identity.Name); 
            }
            else
            {
                articles = await _articleRepository.FindAllbyName(UserName);
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
            var profile = await _profileRepository.FirstOrDefaultAsync(UserName);
            if (profile.isBlocked)
            {
                profile.isBlocked = false;
            }
            else
            {
                profile.isBlocked = true;
            }
            await _profileRepository.Update(profile);
            return View("AdminPage", await _profileRepository.FindAllAsyncByUserName());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminPage(string? UserName)
        {
            return View(await _profileRepository.FindAllAsyncByUserName());
        }
    }
}
