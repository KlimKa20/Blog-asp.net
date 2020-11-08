using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Domain.Core;

namespace WebApplication4.Domain.Interfaces
{
    public interface IRepository<T, Y> : IDisposable
        where T : class
    {
        Task Create(T article);
        Task Update(T article);
        bool Any(Y id);
        Task Remove(T article);
        Task<T> FirstOrDefaultAsync(Y id);
    }
}
