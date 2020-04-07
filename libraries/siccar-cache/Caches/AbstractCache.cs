using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace siccar_cache.Caches
{
    public class AbstractCache<Key, Value>
    {
        protected MemoryCacheEntryOptions _cacheOptions;
        protected IMemoryCache _cache;

        protected AbstractCache(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
        }

        protected void RemoveEntry(string key)
        {
            _cache.Remove(key);
        }

        protected bool KeyExistsInCache(Key key)
        {
            return _cache.TryGetValue(key, out _);
        }

        protected List<Value> GetAll()
        {
            //////////
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_cache) as ICollection;
            var items = new List<Value>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var obj = methodInfo.GetValue(item);
                    if (obj.GetType() == typeof(Value))
                    {
                        items.Add((Value)obj);
                    }
                }
            }
            return items;

        }
        protected Value GetValue(Key key)
        {
            Value element;
            if (_cache.TryGetValue(key, out element))
            {
                return element;
            }
            return default;
        }

        protected void AddValue(Key key, Value value)
        {
            _cache.Set(key, value, _cacheOptions);
        }
    }
}
