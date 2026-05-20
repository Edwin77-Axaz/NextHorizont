using System.Text.Json;
using StackExchange.Redis;

namespace NextHorizont.Infraestructura.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
}

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _connection;
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(15);

    public RedisCacheService(IConnectionMultiplexer connection)
    {
        _connection = connection;
        _db = connection.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>((string)value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry ?? DefaultExpiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        var endpoints = _connection.GetEndPoints();
        var server = _connection.GetServer(endpoints[0]);
        var keys = server.Keys(pattern: pattern);
        foreach (var key in keys)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}
