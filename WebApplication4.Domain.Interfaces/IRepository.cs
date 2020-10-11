using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Domain.Core;

namespace WebApplication4.Domain.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task Create(Article article);
        Task Update(Article article);
        bool Any(int id);
        Task Remove(Article article);
        Task<Article> FirstOrDefaultAsync(int? id);
    }
}
