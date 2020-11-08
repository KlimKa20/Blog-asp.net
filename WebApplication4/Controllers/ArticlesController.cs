using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;
using WebApplication4.Services.BusinessLogic;

namespace WebApplication4.Controllers
{
    public class ArticlesController : Controller
    {

        private readonly UserManager<Profile> _userManager;
        private readonly TagRepository _tagRepository;
        private readonly ImageService _imageService;
        private readonly ProfileRepository _profileRepository;
        private readonly ArticleRepository _articleRepository;
        private readonly CommentRepository _commentRepository;

        private readonly ILogger<ArticlesController> _logger;
        public ArticlesController(UserManager<Profile> userManager, TagRepository tagRepository, ImageService imageService, CommentRepository commentRepository, ProfileRepository profileRepository, ArticleRepository articleRepository, ILogger<ArticlesController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _tagRepository = tagRepository;
            _imageService = imageService;
            _profileRepository = profileRepository;
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Article. Action:Details");
                return NotFound();
            }

            var article = await _articleRepository.FirstOrDefaultAsync(id.Value);
            if (article == null)
            {
                _logger.LogError("Doesn't exist article. Controller:Article. Action:Details");
                return NotFound();
            }

            var user = await _profileRepository.FindAsync(article.ProfileID);
            if (user == null)
            {
                _logger.LogError("Doesn't exist user. Controller:Article. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            ViewData["UserName"] = user.UserName;
            List<Comment> comments = new List<Comment>();
            foreach (var item in _commentRepository.FindAllByArticle(article.ArticleID))
            {
                item.Profile = await _profileRepository.FindAsync(item.ProfileID);
                comments.Add(item);
            }

            ViewData["Comment"] = comments;

            return View(article);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var profile = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!profile.isBlocked)
            {

                ViewData["tags"] = await _tagRepository.FindAll();
                return View();
            }
            else
            {
                ViewData["Text"] = "Ваша страница заблокирована администратором";
                return View("~/Views/Shared/TextPage.cshtml");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleID,Title,Text,TagID")] Article article, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    article.Image = await _imageService.SaveImageAsync(uploadedFile);
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                article.Profile = user;
                article.DateTime = DateTime.Now;
                article.Tag = await _tagRepository.FirstOrDefaultAsync(article.TagID);
                await _articleRepository.Create(article);
                return RedirectPermanent("~/Home/Index");
            }
            return View(article);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Article. Action:Edit");
                return NotFound();
            }

            var article = await _articleRepository.FirstOrDefaultAsync(id.Value);
            if (article == null)
            {
                _logger.LogError("Doesn't exist article. Controller:Article. Action:Edit");
                return NotFound();
            }
            var user = await _userManager?.FindByIdAsync(article.ProfileID);
            if (User?.Identity.Name.ToString() == user?.UserName || User.IsInRole("admin"))
            {
                return View(article);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,Title,Text")] Article article, IFormFile uploadedFile)
        {

            if (id != article.ArticleID)
            {
                _logger.LogError("Doesn't exist id. Controller:Article. Action:Edit");
                return NotFound();
            }
            Article article1 = await _articleRepository.FirstOrDefaultAsync(article.ArticleID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (article.Text != article1.Text && article1 != null)
                        article1.Text = article.Text;
                    if (article.Title != article1.Title && article1 != null)
                        article1.Title = article.Title;
                    if (article.Image != article1.Image && article.Image != null)
                    {
                        article1.Image = await _imageService.SaveImageAsync(uploadedFile);
                    }
                    await _articleRepository.Update(article1);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleID))
                    {
                        _logger.LogError("Doesn't exist db. Controller:Article. Action:Edit");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectPermanent("~/Home/Index");
            }
            return View(article);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Article. Action:Delete");
                return NotFound();
            }

            var article = await _articleRepository.FirstOrDefaultAsync(id.Value);
            if (article == null)
            {
                _logger.LogError("Doesn't exist areticle. Controller:Article. Action:Delete");
                return NotFound();
            }

            return View(article);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _articleRepository.FirstOrDefaultAsync(id);
            await _articleRepository.Remove(article);
            return RedirectPermanent("~/Home/Index");
        }

        private bool ArticleExists(int id)
        {
            return _articleRepository.Any(id);
        }
    }
}
