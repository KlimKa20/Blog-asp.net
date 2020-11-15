﻿
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Domain.Interfaces;
using WebApplication4.Domain.Core;

namespace WebApplication4.Infrastructure.Data
{
    public class ArticleRepository : IRepository<Article, int>
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

        public Task<List<Article>> FindAll()
        {
            return  _context.Articles.ToListAsync();
        }

        public List<Article> FindAll1()
        {
            return _context.Articles.ToList();
        }
        public async Task<Article> FirstOrDefaultAsync(int id)
        {
            return await _context.Articles.FirstOrDefaultAsync(m => m.ArticleID == id);
        }
        public async Task<List<Article>> FindAllbyTag(int id)
        {
            return await _context.Articles.Where(e => e.Tag.TagID == id).ToListAsync();
        }
        public Article jj(int id)
        {
            return (Article)_context.Find( typeof(Article));
        }
        public List<Article> FindAllbyTag1(int id)
        {
            return  _context.Articles.Where(e => e.Tag.TagID == id).ToList();
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
