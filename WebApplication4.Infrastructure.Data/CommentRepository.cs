using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.Domain.Core;
using WebApplication4.Domain.Interfaces;

namespace WebApplication4.Infrastructure.Data
{
    public class CommentRepository:IRepository<Comment,int>
    {
        private readonly ApplicationContext _context;
       
        public CommentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool Any(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Create(Comment article)
        {
            _context.Add(article);
            await _context.SaveChangesAsync();
        }

        public List<Comment> FindAllByArticle(int ArticleID)
        {
            return _context.Comments.Include(s => s.Article).Where(s => s.ArticleID == ArticleID).ToList();
        }
       

        public Task<Comment> FirstOrDefaultAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Comment article)
        {
            throw new NotImplementedException();
        }

        public Task Update(Comment article)
        {
            throw new NotImplementedException();
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
