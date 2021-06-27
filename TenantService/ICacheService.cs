using System;
using System.Threading.Tasks;

namespace TenantService
{
    /// <summary>
    /// Cache Service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Delete or remove cached data with key prefix
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        Task InValidateKey(string keyPrefix);
        /// <summary>
        /// Return cached data by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetCacheValueAsync(String key);
        /// <summary>
        /// Cache data with key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetCacheValueAsync(String key, String value);
        /// <summary>
        /// Cache data with key and expiry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetCacheValueAsync(String key, object value);
        /// <summary>
        /// Cache data with key and expiry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        Task SetCacheValueAsync(string key, object value, TimeSpan timeSpan);
        /// <summary>
        /// Cache data with key and expiry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        Task SetCacheValueAsync(string key, string value, TimeSpan timeSpan);
    }
}
