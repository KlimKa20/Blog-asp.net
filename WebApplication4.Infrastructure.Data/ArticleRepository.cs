
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Domain.Interfaces;
using WebApplication4.Domain.Core;

namespace WebApplication4.Infrastructure.Data
{
    public class ArticleRepository : IRepository<Article>
    {
        private readonly ApplicationContext _context;
        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Create(Article item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Article item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id); ;
        }

        public async Task Remove(Article item)
        {
            _context.Articles.Remove(item);
            await _context.SaveChangesAsync();
        }

        public Task<List<Article>> FindAll()
        {
            return _context.Articles.ToListAsync();
        }

        public async Task<Article> FirstOrDefaultAsync(int id)
        {
            return await _context.Articles.FirstOrDefaultAsync(m => m.ArticleID == id);
        }

        public async Task<List<Article>> FindAllbyTag(int id)
        {
            return await _context.Articles.Where(e => e.Tag.TagID == id).ToListAsync();
        }

        public async Task<List<Article>> FindAllbyName(string UserName)
        {
            return await _context.Articles.Where(e => e.Profile.UserName == UserName).ToListAsync();
        }

        private bool disposed = false;

        public void Dispose(bool disposing)
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
