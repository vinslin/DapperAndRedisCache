using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NPOI.SS.Formula.Functions;
namespace DapperAndRedisCache.Service
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cacheData = await _cache.GetStringAsync(key);
            if (cacheData == null) {
                return default;
            }
            return JsonSerializer.Deserialize<T>(cacheData);
        }

        public async Task SetAsync<T>(string key, T data, TimeSpan? expiry = null) 
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
            };

            var jsonData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(key,jsonData,options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

    }
}
