﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using blog_project.Models;
using Microsoft.EntityFrameworkCore.Internal;
using WebApplication4.Models;
using WebApplication4.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Blog_project_.Controllers
{
    public class ArticlesController : Controller
    {

        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<ArticlesController> _logger;
        public ArticlesController(UserManager<Profile> userManager, ApplicationContext context, IWebHostEnvironment appEnvironment, ILogger<ArticlesController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Article. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                _logger.LogError("Doesn't exist article. Controller:Article. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var user = await _context.Profiles.FindAsync(article.ProfileID);
            if (user==null)
            {
                _logger.LogError("Doesn't exist user. Controller:Article. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            ViewData["UserName"] = user.UserName;
            List<Comment> comments = new List<Comment>();
            foreach (var item in _context.Comments.Include(s => s.Article).Where(s => s.ArticleID == article.ArticleID).ToList())
            {
                item.Profile = await _context.Profiles.FindAsync(item.ProfileID);
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
                SelectList tags = new SelectList(_context.Tags, "TagID", "TagName");
                ViewBag.Tags = tags;
                ViewData["tags"] = _context.Tags.ToList();
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
                    //byte[] imageData = null;
                    //// считываем переданный файл в массив байтов
                    //using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                    //{
                    //    imageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                    //}
                    //article.Image = imageData;
                    //_context.SaveChanges();
                    string path = "/Files/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    article.Image = path;
                    _context.SaveChanges();
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                article.Profile= user;
                article.DateTime = DateTime.Now;
                article.Tag = await _context.Tags.FindAsync(article.TagID);
                _context.Add(article);
                await _context.SaveChangesAsync();
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
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                _logger.LogError("Doesn't exist article. Controller:Article. Action:Edit");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            var user = await _userManager.FindByIdAsync(article.ProfileID);
            if (User.Identity.Name.ToString() == user.UserName || User.IsInRole("admin"))
            {
                _logger.LogError("Doesn't exist user. Controller:Article. Action:Edit");
                return RedirectPermanent("~/Error/Index?statusCode=404");
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
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            Article article1 = await _context.Articles.FindAsync(article.ArticleID);
            if (ModelState.IsValid)
            {
                try
                {
                    if (article.Text != article1.Text && article1 != null)
                        article1.Text = article.Text;
                    if (article.Title != article1.Title && article1 != null)
                        article1.Title = article.Title;
                    if (article.Image != article1.Image && article != null)
                    {
                        string path = "/Files/" + uploadedFile.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                        article1.Image = path;
                    }
                    _context.Update(article1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleID))
                    {
                        _logger.LogError("Doesn't exist db. Controller:Article. Action:Edit");
                        return RedirectPermanent("~/Error/Index?statusCode=404");
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
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                _logger.LogError("Doesn't exist areticle. Controller:Article. Action:Delete");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            return View(article);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectPermanent("~/Home/Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
