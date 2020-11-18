using System;
using System.Threading.Tasks;

namespace WebApplication4.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        Task Create(T item);
        Task Update(T item);
        bool Any(int id);
        Task Remove(T item);
        Task<T> FirstOrDefaultAsync(int id);
    }
}
