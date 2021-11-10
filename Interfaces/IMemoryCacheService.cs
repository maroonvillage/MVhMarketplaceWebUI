using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

public interface IMemoryCacheService
{

    T Get<T>(string key);

    Task<T> GetAsync<T>(string key);

    void Set<T>(string key, object entry);


    void Remove<T>(string key);
    
}