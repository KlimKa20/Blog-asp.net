using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;

namespace WebApplication4.Services.BusinessLogic
{
    public class HabrDotNetHub : Hub
    {
        private readonly UserManager<Profile> _userManager;
        private readonly CommentRepository _commentRepository;

        public HabrDotNetHub(UserManager<Profile> userManager, CommentRepository commentRepository)
        {
            _userManager = userManager;
            _commentRepository = commentRepository;
        }

        public async Task JoinPostGroup(string ArticleID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ArticleID);
        }

        public async Task SendMessage(string ArticleID, string Text)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            Comment comment = new Comment();
            comment.Profile = user;
            comment.DateTime = DateTime.Now;
            comment.ArticleID = Int32.Parse(ArticleID);
            comment.Text = Text;
            await _commentRepository.Create(comment);
            await Clients.Group(ArticleID).SendAsync("ReceiveMessage", user.UserName, comment.Text, comment.DateTime);
        }
    }
}
