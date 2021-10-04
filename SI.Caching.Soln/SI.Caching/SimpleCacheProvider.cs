using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace SI.Caching
{
    /// <summary>
    /// Defines the <see cref="SimpleCacheProvider"/>.
    /// </summary>
    public abstract class SimpleCacheProvider : ISimpleCacheProvider
    {
        /// <summary>
        /// Defines the cache.
        /// </summary>
        private readonly ObjectCache cache;

        /// <summary>
        /// Defines the policy.
        /// </summary>
        private readonly CacheItemPolicy policy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCacheProvider"/> class.
        /// </summary>
        /// <param name="cacheDurationType">The cacheDurationType <see cref="CacheDurationTypes"/>.</param>
        /// <param name="cacheDuration">The cacheDuration <see cref="uint?"/>.</param>
        protected SimpleCacheProvider(CacheDurationTypes cacheDurationType, uint? cacheDuration)
        {
            CacheDurationType = cacheDurationType;
            CacheDuration = (int)(cacheDuration ?? 0);
            CacheDuration = CacheDuration < 1 ? 1 : CacheDuration;
            Trace.WriteLine("Cache Initialized!");

            cache = MemoryCache.Default;
            policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(GetTotalCacheDuration()),
                RemovedCallback = new CacheEntryRemovedCallback(CacheRemovedCallback)
            };
        }

        /// <summary>
        /// The GetTotalCacheDuration.
        /// </summary>
        /// <returns>The <see cref="double"/>.</returns>
        private double GetTotalCacheDuration()
        {
            var duration = CacheDuration;

            switch (CacheDurationType)
            {
                case CacheDurationTypes.Seconds:
                    break;

                case CacheDurationTypes.Minutes:
                    duration *= 60;
                    break;

                case CacheDurationTypes.Hour:
                    duration *= 60;
                    break;

                case CacheDurationTypes.Day:
                    duration *= 60;
                    duration *= 60;
                    duration *= 24;
                    break;

                case CacheDurationTypes.Week:
                    duration *= 60;
                    duration *= 60;
                    duration *= 24;
                    duration *= 7;
                    break;

                case CacheDurationTypes.Month:
                    duration *= 60;
                    duration *= 60;
                    duration *= 24;
                    duration *= 30;
                    break;

                case CacheDurationTypes.Month3:
                    duration *= 60;
                    duration *= 60;
                    duration *= 24;
                    duration *= 30;
                    duration *= 3;
                    break;

                case CacheDurationTypes.Year:
                    duration *= 60;
                    duration *= 60;
                    duration *= 24;
                    duration *= 365;
                    break;

                default:
                    break;
            }

            return duration;
        }

        /// <summary>
        /// The CacheRemovedCallback.
        /// </summary>
        /// <param name="arguments">The arguments <see cref="CacheEntryRemovedArguments"/>.</param>
        private void CacheRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            Trace.WriteLine("Cache Expired.");
            Trace.WriteLine("Key : " + arguments.CacheItem.Key);
            Trace.WriteLine("Value : " + arguments.CacheItem.Value.ToString());
            Trace.WriteLine("RemovedReason : " + arguments.RemovedReason);
            Trace.WriteLine("-------------------------------------");
        }

        /// <summary>
        /// Gets the CacheDuration.
        /// </summary>
        public int CacheDuration { get; private set; }

        /// <summary>
        /// Gets the CacheDurationType.
        /// </summary>
        public CacheDurationTypes CacheDurationType { get; private set; }

        /// <summary>
        /// Gets the CacheProviderType.
        /// </summary>
        public virtual CacheProviderTypes CacheProviderType
        {
            get { return CacheProviderTypes.None; }
        }

        /// <summary>
        /// The BuildCacheKey.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <returns>The <see cref="Func{string, object}"/>.</returns>
        public abstract Func<string, object> BuildCacheKey(string key);

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <param name="defaultValue">The defaultValue <see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Get(string key, object defaultValue = null)
        {
            ThrowIsNullOrSpace(key);
            object retVal = null;
            try
            {
                retVal = cache.Get(key);
                if (retVal == null)
                    retVal = defaultValue;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Hata : GbDefaultCacheProvider.Get()\n" + e.Message);
                /// throw new Exception("Cache Get sırasında bir hata oluştu!", e);
            }

            return retVal;
        }

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <param name="value">The value <see cref="object"/>.</param>
        public void Set(string key, object value)
        {
            ThrowIsNullOrSpace(key);
            try
            {
                cache.Set(key, value, policy);
                Trace.WriteLine("Cache Set. Key : " + key);
            }
            catch (Exception e)
            {
                /// Exception handling and KykFileLogging will be added.
                Trace.WriteLine(string.Format("Hata : CacheProvider.Set()\n{0}\n{1}", e.Message, e.StackTrace));
                /// throw new Exception("Cache Set sırasında bir hata oluştu!", e);
            }
        }

        /// <summary>
        /// The IsExist.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsExist(string key)
        {
            ThrowIsNullOrSpace(key);
            var isExist = cache.Contains(key);
            return isExist;
        }

        /// <summary>
        /// The ThrowIsNullOrSpace.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        private void ThrowIsNullOrSpace(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("Parameter can not be null or emtpy space.");
        }

        /// <summary>
        /// The GetOrAdd.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <param name="valueFactory">The valueFactory <see cref="Func{string, object}"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object GetOrAdd(string key, Func<string, object> valueFactory)
        {
            ThrowIsNullOrSpace(key);
            object value = null;

            if (!IsExist(key))
            {
                value = valueFactory(key);
                Set(key, value);
                return value;
            }

            value = Get(key);
            return value;
        }
    }
}