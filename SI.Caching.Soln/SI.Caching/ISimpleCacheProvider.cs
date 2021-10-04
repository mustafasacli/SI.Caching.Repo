using System;

namespace SI.Caching
{
    /// <summary>
    /// Defines the <see cref="ISimpleCacheProvider"/>.
    /// </summary>
    public interface ISimpleCacheProvider
    {
        /// <summary>
        /// Gets the CacheProviderType.
        /// </summary>
        CacheProviderTypes CacheProviderType { get; }

        /// <summary>
        /// Gets the CacheDurationType.
        /// </summary>
        CacheDurationTypes CacheDurationType { get; }

        /// <summary>
        /// Gets the CacheDuration.
        /// </summary>
        int CacheDuration { get; }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <param name="defaultValue">The defaultValue <see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Get(string key, object defaultValue = null);

        /// <summary>
        /// The Set.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <param name="value">The value <see cref="object"/>.</param>
        void Set(string key, object value);

        /// <summary>
        /// The IsExist.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsExist(string key);

        /// <summary>
        /// The BuildCacheKey.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <returns>The <see cref="Func{string, object}"/>.</returns>
        Func<string, object> BuildCacheKey(string key);

        /// <summary>
        /// The GetOrAdd.
        /// </summary>
        /// <param name="key">The key <see cref="string"/>.</param>
        /// <param name="valueFactory">The valueFactory <see cref="Func{string, object}"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object GetOrAdd(string key, Func<string, object> valueFactory);
    }
}