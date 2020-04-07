using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace siccar_cache.Caches
{
    public class AbstractCollectionCache<T>
    {
        protected MemoryCacheEntryOptions _cacheOptions;
        protected IMemoryCache _cache;

        protected AbstractCollectionCache(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
        }

        protected bool ContainsAll(string key, HashSet<T> values)
        {
            HashSet<T> valuesInCache;

            if (!_cache.TryGetValue(key, out valuesInCache))
            {
                return false;
            }
            return valuesInCache.SequenceEqual(values);
        }


        protected HashSet<T> GetValues(string key)
        {
            HashSet<T> schemasInCache;
            if (!_cache.TryGetValue(key, out schemasInCache))
            {
                return new HashSet<T>();
            }
            return schemasInCache;
        }

        protected void AddValue(string key, T element)
        {
            HashSet<T> valuesInCache;
            if (!_cache.TryGetValue(key, out valuesInCache))
            {
                _cache.Set(key, new HashSet<T>() { element }, _cacheOptions);
            }
            else
            {
                valuesInCache.Add(element);
                _cache.Set(key, valuesInCache, _cacheOptions);
            }
        }

        protected void AddValues(string key, HashSet<T> elements)
        {
            HashSet<T> valuesInCache;
            if (!_cache.TryGetValue(key, out valuesInCache))
            {
                _cache.Set(key, elements, _cacheOptions);
            }
            else
            {
                // Cache isnt replacing newer with old, so this should do it 
                // Need to find a better answer to this 
                foreach(var element in elements)
                {
                    if (valuesInCache.ToList<T>().Contains(element))
                    {
                        valuesInCache.Remove(element);
                        valuesInCache.Add(element);
                    }
                }
                
                _cache.Set(key, valuesInCache, _cacheOptions);
            }
        }
    }
}
