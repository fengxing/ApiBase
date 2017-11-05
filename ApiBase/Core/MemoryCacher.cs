using System;
using System.Runtime.Caching;

namespace ApiBase.Core
{
    public class MemoryCacher
    {
        private static MemoryCache cache = new MemoryCache("ApiBase");

        public static CacheItem Get(string key)
        {
            return cache.GetCacheItem(key);
        }

        public static bool Exist(string key)
        {
            return cache.Contains(key);
        }

        public static void Set(string key, object value, int seconds)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                cache.Set(key, value, DateTimeOffset.UtcNow.AddSeconds(seconds));
            }
        }
    }
}
