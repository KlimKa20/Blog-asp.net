using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WebApplication4.Data;

namespace WebApplication4.HubS
{
    public class HabrDotNetHub : Hub
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationContext _context;

        public HabrDotNetHub(UserManager<Profile> userManager,ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(string ArticleID, string Text)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            Comment comment = new Comment();
            comment.Profile = user;
            comment.DateTime = DateTime.Now;
            comment.ArticleID = Int32.Parse(ArticleID);
            comment.Text = Text;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user.UserName, comment.Text, comment.DateTime);
        }
    }
}
