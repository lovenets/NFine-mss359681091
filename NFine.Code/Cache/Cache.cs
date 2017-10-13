﻿/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace NFine.Code
{
    public class Cache : ICache
    {
        private static System.Web.Caching.Cache cache = HttpRuntime.Cache;

        public T GetCache<T>(string cacheKey) where T : class
        {
            if (cache[cacheKey] != null)
            {
                return (T)cache[cacheKey];
            }
            return default(T);
        }
        public void WriteCache<T>(T value, string cacheKey) where T : class
        {
            cache.Insert(cacheKey, value, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        public void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            cache.Insert(cacheKey, value, null, expireTime, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        public void RemoveCache(string cacheKey)
        {
            cache.Remove(cacheKey);
        }
        public void RemoveCache()
        {
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 数据库缓存依赖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">存储值</param>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="database">数据库名称</param>
        /// <param name="table">表名称</param>
        public void WriteCache<T>(T value, string cacheKey, string database, string table) where T : class
        {
            //制定缓存策略
            SqlCacheDependency scd = new SqlCacheDependency(database, table);

            //插入缓存
            cache.Insert(cacheKey, value, scd);

            //cache.Insert(cacheKey, value, new System.Web.Caching.SqlCacheDependency(database, table));
        }
    }
}
