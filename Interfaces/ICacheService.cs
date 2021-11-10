using System;
using System.Threading.Tasks;

namespace webui.Interfaces
{
    public interface ICacheService
    {
        object Get(string cacheKey);
        object Delete(string cacheKey);
        object Add(string cacheKey, object item);
        Task<object> AddAsync(string cacheKey, object item);
        bool Contains(string cacheKey);

    }
}
