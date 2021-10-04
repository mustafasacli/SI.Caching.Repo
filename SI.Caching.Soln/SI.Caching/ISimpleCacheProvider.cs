using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SI.Caching
{

    public interface ISimpleCacheProvider
    {
        CacheProviderTypes CacheProviderType { get; }

        int CacheDuration { get; }

        object Get(string key, object defaultValue = null);

        int Set(string key, object value);

        bool IsExist(string key);

        Func<string, object> BuildCacheKey(string key);

        object GetOrAdd(string key, Func<string, object> valueFactory);
    }
}
