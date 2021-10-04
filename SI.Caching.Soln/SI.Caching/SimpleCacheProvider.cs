using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SI.Caching
{
    public abstract class SimpleCacheProvider : ISimpleCacheProvider
    {
        public int CacheDuration
        { get; private set; }

        public virtual CacheProviderTypes CacheProviderType
        { get { return CacheProviderTypes.None; } }

        public Func<string, object> BuildCacheKey(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key, object defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public int Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string key)
        {
            throw new NotImplementedException();
        }

        public object GetOrAdd(string key, Func<string, object> valueFactory)
        {
            throw new NotImplementedException();
        }
    }
}
