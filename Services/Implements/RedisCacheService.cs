using AspNetMvcCacheRedis.Services.Interfaces;
using StackExchange.Redis;

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

    public async Task SetHashValueAsync(string hashName, string field, string value)
    {
        HashEntry[] hashEntries = new HashEntry[]
        {
            new HashEntry(field, value)
        };
        await _db.HashSetAsync(hashName, hashEntries);
        await _db.KeyExpireAsync(hashName, TimeSpan.FromSeconds(30));
    }

    public async Task<HashEntry[]> GetHashValueAsync(string hashName)
    {
        var data = await _db.HashGetAllAsync(hashName);
        return data;
    }

    public async Task<int> GetHashEntryAsync(string hashKey, string fieldName)
    {
        RedisValue? value = await _db.HashGetAsync(hashKey, fieldName);
        if (value == RedisValue.Null) throw new Exception($"Value not found with key {fieldName}!");

        return Convert.ToInt32(value.Value);
    }
}
