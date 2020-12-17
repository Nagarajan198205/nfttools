using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;

        public LocalStorageService()
        {
        }

        public async Task<T> GetItem<T>(string key)
        {
            object catchItem = null;

            await Task.Run(() =>
            {
                if (_cache.Contains(key))
                {
                    catchItem = _cache.Get(key);
                }
            });

            return (T)catchItem;
        }

        public async Task SetItem<T>(string key, T value)
        {
            await Task.Run(() =>
            {
                var cacheItemPolicy = new CacheItemPolicy()
                {
                    //Set your Cache expiration.
                    AbsoluteExpiration = DateTime.Now.AddDays(1)
                };
                _cache.Add(key, value, cacheItemPolicy);
            });
        }

        public async Task RemoveItem(string key)
        {
            await Task.Run(() => _cache.Remove(key));
        }
    }
}