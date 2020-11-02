
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Domain.Interfaces;
using WebApplication4.Domain.Core;

namespace WebApplication4.Infrastructure.Data
{
    public class ArticleRepository : IRepository
    {
        private readonly ApplicationContext _context;
        public ArticleRepository()
        {
        }
        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task Create(Article article)
        {
            _context.Add(article);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Update(Article article)
        {
            _context.Update(article);
            await _context.SaveChangesAsync();
        }

        public virtual bool Any(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id); ;
        }
        public virtual async Task Remove(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<Article>> FindAll()
        {
            return await _context.Articles.ToListAsync();
        }

        public virtual async Task<Article> FirstOrDefaultAsync(int? id)
        {
            return await _context.Articles.FirstOrDefaultAsync(m => m.ArticleID == id);
        }
        public virtual async Task<List<Article>> FindAllbyTag(int? id)
        {
            return await _context.Articles.Where(e => e.Tag.TagID == id).ToListAsync();
        }

        public virtual async Task<List<Article>> FindAllbyName(string? UserName)
        {
            return await _context.Articles.Where(e => e.Profile.UserName == UserName).ToListAsync();
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

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
