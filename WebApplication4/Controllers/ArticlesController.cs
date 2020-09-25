using System;
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

namespace Blog_project_.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public ArticlesController(UserManager<Profile> userManager, ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(article.ProfileID);
            if (user==null)
            {
                return NotFound();
            }
            ViewData["UserName"] = user.UserName;
            ViewData["Comment"] = _context.Comments.Include(s=>s.Article).Where(s => s.ArticleID == article.ArticleID).ToList() ;
            return View(article);
        }

        [Authorize]
        public IActionResult Create()
        {
            //_context.Articles
            //    .Where(s => s.TagID==2)
            //    .Include("Profile")
            SelectList tags = new SelectList(_context.Tags, "TagID", "TagName");
            ViewBag.Tags = tags;
            ViewData["tags"] = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,TagID")] Article article, IFormFile uploadedFile)
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
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("ArticleID,Text")] Comment comment)
        {
            if (ModelState.IsValid)
            {
               
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                comment.Profile = user;
                comment.DateTime = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(article.ProfileID);
            if (User.Identity.Name.ToString() == user.UserName || User.IsInRole("admin"))
            {
                return View(article);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,Image")] Article article)
        {
            if (id != article.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
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
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
