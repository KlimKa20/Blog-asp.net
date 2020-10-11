using blog_project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Data;

namespace WebApplication4.Service
{
    public class ArticleRepository : IRepository
    {
        private readonly ApplicationContext _context;
        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Create(Article article)
        {
            _context.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Article article)
        {
            _context.Update(article);
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id); ;
        }
        public async Task Remove(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        public async Task<Article> FirstOrDefaultAsync(int? id)
        {
            return await _context.Articles.FirstOrDefaultAsync(m => m.ArticleID == id);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
