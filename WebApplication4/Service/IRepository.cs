using blog_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Service
{
    interface IRepository : IDisposable
    {
        Task Create(Article article);
        Task Update(Article article);
        bool Any(int id);
        Task Remove(Article article);
        Task<Article> FirstOrDefaultAsync(int? id);
    }
}
