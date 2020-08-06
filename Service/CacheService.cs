using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisClient.Service.Interface;
using StackExchange.Redis;
using System;

namespace RedisClient.Service
{
    public class CacheService: ICacheService
    {
        const uint DEFAULT_TIME_MINUTES = 1;

        private uint CacheMinutesExpire { get; set; } = DEFAULT_TIME_MINUTES;

        private DistributedCacheEntryOptions CacheOptions { get; set; }


        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
            SetOptionsCache();
        }

        public T Set<T>(string key, T data)
        {
            try
            {

                _cache.SetString(key, JsonConvert.SerializeObject(data), CacheOptions);

                return data;
            }
            catch(RedisConnectionException e)
            {
                Console.WriteLine("Redis Cluster is running?");

                throw e;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                var response = _cache.GetString(key);

                return response != null ? JsonConvert.DeserializeObject<T>(_cache.GetString(key)) : default(T);
            }
            catch(RedisConnectionException e)
            {
                Console.WriteLine("Redis Cluster is running?");

                throw e;
            }

        }

        public T GetOrSet<T>(string key, Func<T> func)
        {
            try
            {
                var objCache = Get<T>(key);

                if (objCache != null)
                {
                    return objCache;
                }

                return Set<T>(key, func.Invoke());
            }
            catch (RedisConnectionException e)
            {
                Console.WriteLine("Redis Cluster is running?");

                throw e;
            }
        }

        private void SetCacheMinutes(uint minutes)
        {
            if (minutes == 0)
                return;

            CacheMinutesExpire = minutes;

            SetOptionsCache();
        }

        private void SetOptionsCache()
        {
            CacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheMinutesExpire));
        }
    }
}
