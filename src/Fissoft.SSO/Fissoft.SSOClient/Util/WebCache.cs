using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Fissoft.SSOClient.Util
{
    //Client Project Cache
    internal class WebCache 
    {
        public HttpContextBase Context { get; set; }
        public WebCache(HttpContextBase context)
        {
            Context = context;
        }

        protected Cache Cache
        {
            get { return Context.Cache; }
        }

        ///<summary>
        ///</summary>
        ///<param name="key"></param>
        public object this[string key]
        {
            get { return Get(key); }
        }

        /// <summary>
        /// 得到某一特定缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// 得到某一特定缓存，并转为特定类型
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return Cache[key] as T;
        }

        /// <summary>
        /// 缓存中是否包含某一键值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Cache[key] != null;
        }

        /// <summary>
        /// 将对象添加到缓存中
        /// </summary>
        /// <param name="key">为对象对应的键值</param>
        /// <param name="obj">对象</param>
        public void Add(string key, object obj)
        {
            Cache.Add(key, obj, null, DateTime.MaxValue
                      , Timespan,
                      CacheItemPriority.Normal, null);
        }

 
        public void Add(string key, object obj, TimeSpan timeSpan)
        {
            Cache.Add(key, obj, null, DateTime.MaxValue
                      , timeSpan,
                      CacheItemPriority.Normal, null);
        }

 

        private TimeSpan? _timespan;

        public void RemoveAll()
        {
            foreach (DictionaryEntry de in Cache)
            {
                Cache.Remove(de.Key.ToString());
            }
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

     
        /// <summary>
        /// 获取或设置缓存的存储时间,默认值为1天
        /// </summary>
        public TimeSpan Timespan
        {
            get
            {
                if (_timespan == null)
                {
                    return new TimeSpan(0, 10, 0);
                }
                return _timespan.Value;
            }
            set { _timespan = value; }
        }
 
    }
}