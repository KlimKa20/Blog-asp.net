using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication4.Domain.Core;
using WebApplication4.Domain.Interfaces;

namespace WebApplication4.Infrastructure.Data
{
    public class TagRepository : IRepository<Tag, int>
    {

        private readonly ApplicationContext _context;

        public TagRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool Any(int id)
        {
            throw new NotImplementedException();
        }

        public Task Create(Tag article)
        {
            throw new NotImplementedException();
        }


        public async Task<Tag> FirstOrDefaultAsync(int id)
        {
            return  await _context.Tags.FindAsync(id);
        }

        public async Task<List<Tag>> FindAll()
        {
            return await _context.Tags.ToListAsync();
        }
        public Task Remove(Tag article)
        {
            throw new NotImplementedException();
        }

        public Task Update(Tag article)
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

        public  void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
