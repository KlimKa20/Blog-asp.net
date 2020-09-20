using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    //[Authorize(Roles = "admin")]
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
                ViewData["Tags"] = _context.Tags.FindAsync(id).Result.TagName;
                return View(await _context.Articles.Where(e => e.Tag.TagID == id).ToListAsync());
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
    }
}
