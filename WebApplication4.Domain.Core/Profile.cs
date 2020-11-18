using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplication4.Domain.Core
{
    public class Profile : IdentityUser
    {
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public bool isBlocked { get; set; }
    }
}
