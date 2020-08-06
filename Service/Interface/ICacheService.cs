using System;
using System.Collections.Generic;
using System.Text;

namespace RedisClient.Service.Interface
{
    public interface ICacheService
    {
        T Get<T>(string key);

        T Set<T>(string key, T data);

        T GetOrSet<T>(string key, Func<T> func);
    }
}
