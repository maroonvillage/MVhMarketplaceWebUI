using System;
using System.Threading.Tasks;

namespace webui.Interfaces
{
    public interface ICacheService<T>
    {
        T Get(string cacheKey);
        T Delete(string cacheKey);
        T Add(string cacheKey, T item);
        Task<T> AddAsync(string cacheKey, T item);
        bool Contains(string cacheKey);

    }
}
