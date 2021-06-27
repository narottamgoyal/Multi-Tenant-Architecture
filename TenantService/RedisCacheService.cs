using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TenantService
{
    ///<inheritdoc/>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        ///<inheritdoc/>
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
            _connectionMultiplexer = connectionMultiplexer;
        }

        ///<inheritdoc/>
        public async Task<string> GetCacheValueAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        ///<inheritdoc/>
        public Task InValidateKey(string keyPrefix)
        {
            EndPoint endPoint = _connectionMultiplexer.GetEndPoints().First();
            RedisKey[] keys = _connectionMultiplexer.GetServer(endPoint).Keys(pattern: $"{ keyPrefix }*").ToArray();
            _db.KeyDelete(keys);
            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public async Task SetCacheValueAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        ///<inheritdoc/>
        public async Task SetCacheValueAsync(string key, string value, TimeSpan timeSpan)
        {
            await _db.StringSetAsync(key, value, timeSpan);
        }

        ///<inheritdoc/>
        public async Task SetCacheValueAsync(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null) return;
            await SetCacheValueAsync(key, JsonConvert.SerializeObject(value));
        }

        ///<inheritdoc/>
        public async Task SetCacheValueAsync(string key, object value, TimeSpan timeSpan)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null) return;
            await SetCacheValueAsync(key, JsonConvert.SerializeObject(value), timeSpan);
        }
    }
}
