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
    public class ProfileRepository : IRepository<Profile, string>
    {

        private readonly ApplicationContext _context;
        public ProfileRepository(ApplicationContext context)
        {
            _context = context;
        }
        public bool Any(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> FindAsync(string id)
        {
            return await _context.Profiles.FindAsync(id);
        }
        public async Task<List<Profile>> FindAllAsyncByUserName()
        {
            return await _context.Profiles.Where(e => e.UserName != "admin").ToListAsync();
        }
        public async Task<Profile> FirstOrDefaultAsync(string id)
        {
            return await _context.Profiles.Where(e => e.UserName == id).FirstAsync();
        }

        public Task Create(Profile article)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Profile article)
        {
            _context.Update(article);
            await _context.SaveChangesAsync();
        }

        public Task Remove(Profile article)
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
