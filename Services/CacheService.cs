using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using webui.Interfaces;

namespace webui.Services
{
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }


        public T Get(string cacheKey)
        {
            if (!_cache.TryGetValue(cacheKey, out _))
            {
                // Key not in cache, so get data.
                object cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache.
                _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
            }

            return default;
        }


        public T Delete(string cacheKey)
        {

            return default;
        }


        public T Add(string cacheKey, T item)
        {
            var cacheEntry = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return DateTime.Now;
            });

            return default;
        }

        public async Task<T> AddAsync(string cacheKey, T item)
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync(cacheKey, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                    return Task.FromResult(DateTime.Now);
                });

            return default;
        }

        public bool Contains(string cacheKey)
        {
            return _ = _cache.TryGetValue<T>(cacheKey, out _);
        }

    }
}
