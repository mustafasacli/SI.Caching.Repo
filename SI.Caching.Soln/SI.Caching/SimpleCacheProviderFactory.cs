using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SI.Caching
{
    public static class SimpleCacheProviderFactory
    {
        private static readonly ConcurrentDictionary<CacheProviderTypes, object> cacheProviderInstances =
            new ConcurrentDictionary<CacheProviderTypes, object>();

        public static ISimpleCacheProvider GetCacheProvider(CacheProviderTypes cacheProviderType, CacheDurationTypes cacheDurationType, uint? duration)
        {
            var cache =
            cacheProviderInstances.GetOrAdd(cacheProviderType, x =>
            {
                var instance = GetCacheProviderInternal(x);
                return instance;
            });

            return cache as ISimpleCacheProvider;
        }

        private static ISimpleCacheProvider GetCacheProviderInternal(CacheProviderTypes cacheProviderType)
        {
            throw new NotImplementedException();
        }
    }
}
