using StackExchange.Redis;

namespace AspNetMvcCacheRedis.Services.Interfaces;

public interface IRedisCacheService
{
    Task SetCacheValueAsync(string key, string val, TimeSpan expireDate);
    Task<string> GetCacheValueAsync(string key);
    Task DeleteCacheValueAsync(string key);
    Task AppendCacheValueAsync(string key, string val);
    Task SetHashValueAsync(string hashName, string field, string value);
    Task<HashEntry[]> GetHashValueAsync(string hashName);
    Task<int> GetHashEntryAsync(string hashName, string field);
}
