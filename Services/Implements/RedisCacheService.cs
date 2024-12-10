using AspNetMvcCacheRedis.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace AspNetMvcCacheRedis.Services.Implements;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<string> GetCacheValueAsync(string key)
    {
        var val = await _db.StringGetAsync(key);
        if (!val.HasValue) throw new Exception($"No result for the key {key}");

        return val.ToString();
    }

    public async Task SetCacheValueAsync(string key, string val, TimeSpan expireDate)
    {
        bool res = await _db.StringSetAsync(key, val, expireDate);
        if (!res) throw new Exception("Error when setting value to key");
    }

    public async Task DeleteCacheValueAsync(string key)
    { 
        bool res = await _db.KeyDeleteAsync(key);
        if (!res) throw new Exception($"No result for the key {key}");
    }

    public async Task AppendCacheValueAsync(string key, string val)
    {
        string str = await GetCacheValueAsync(key);
        await _db.StringAppendAsync(key, val);
    }
}
