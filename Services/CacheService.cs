using Microsoft.Extensions.Caching.Memory;

namespace Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> fetchData, TimeSpan cacheDuration)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T cachedData))
            {
                cachedData = fetchData();
                _memoryCache.Set(cacheKey, cachedData, cacheDuration);
            }
            return cachedData;
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }

}
