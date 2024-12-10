namespace AspNetMvcCacheRedis.Services.Interfaces;

public interface IRedisCacheService
{
    Task SetCacheValueAsync(string key, string val, TimeSpan expireDate);
    Task<string> GetCacheValueAsync(string key);
    Task DeleteCacheValueAsync(string key);
    Task AppendCacheValueAsync(string key, string val);
}
