﻿using DevelopmentFast.Repository.Domain.Interfaces.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace DevelopmentFast.Repository.Repository
{
    public class BaseRedisRepositoryDF : IBaseRedisRepositoryDF
    {
        private readonly IDistributedCache _rd_context;

        public BaseRedisRepositoryDF(IDistributedCache rd_context)
        {
            this._rd_context = rd_context;            
        }

        public void CreateOrUpdate<T>(string key, T obj, TimeSpan timeSpan)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(timeSpan);            
            _rd_context.SetString(key, JsonSerializer.Serialize<T>(obj), options);
        }

        public async Task CreateOrUpdateAsync<T>(string key, T obj, TimeSpan timeSpan)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(timeSpan);            
            await _rd_context.SetStringAsync(key, JsonSerializer.Serialize(obj), options);
        }

        public T? Get<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(_rd_context.GetString(key));
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var obj = await _rd_context.GetStringAsync(key);
            if(obj != null)
            {
               return JsonSerializer.Deserialize<T>(obj);
            }
            return default(T);
        }
    }
}
