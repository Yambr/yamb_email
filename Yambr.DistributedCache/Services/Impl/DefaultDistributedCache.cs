using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yambr.SDK.ComponentModel;

namespace Yambr.DistributedCache.Services.Impl
{
    [Service]
    class DefaultDistributedCache : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger;

        public DefaultDistributedCache(IDistributedCache distributedCache, ILogger<ICacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        private const string DefaultRegion = "DefaultRegion";




        public void Insert<T>(string key, T value, string region, TimeSpan cacheDuration)
        {

            _distributedCache.Set(GetKey(key, region), Encode(value), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            });
        }



        public void Insert<T>(string key, T value, TimeSpan cacheDuration)
        {
            Insert(key, value, null, cacheDuration);
        }

        public void Insert<T>(string key, T value, string region)
        {
            _distributedCache.Set(GetKey(key, region), Encode(value));
        }

        public void Insert<T>(string key, T value)
        {
            _distributedCache.Set(GetKey(key, null), Encode(value));
        }

        public T Get<T>(string key)
        {
            return Get<T>(key, null);
        }



        public T Get<T>(string key, string region)
        {
            byte[] value = _distributedCache.Get(GetKey(key, region));
            return Decode<T>(value);
        }

        public void Remove(string key)
        {
            Remove(key, null);
        }

        public void Remove(string key, string region)
        {
            _distributedCache.Remove(GetKey(key, region));
        }

        private static string ProcessRegion(string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                return $"{DefaultRegion}";
            }
            var processedRegion = string.Concat(region.Select(delegate (char c)
            {
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9'))
                {
                    return '_';
                }
                return c;
            }));
            return $"R_{processedRegion}";
        }

        private byte[] Encode<T>(T value)
        {
            //TODO может использовать bynaryserializer?
            var json = JsonConvert.SerializeObject(value);
            return Encoding.UTF8.GetBytes(json);
        }
        private T Decode<T>(byte[] value)
        {
            var json = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<T>(json);
        }
        private string GetKey(string key, string region)
        {
            return $":{ProcessRegion(region)}:{key}";
        }
    }
}
