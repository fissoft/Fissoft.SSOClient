using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using Newtonsoft.Json;

namespace Fissoft.SSOClient.Util
{
    public class RemoteCache
    {
        internal static T Get<T>(HttpContextBase context, string method, string cacheKeyFormat) where T : class
        {
            var cookie = new WebCookies(context);
            var cache = new WebCache(context);
            var userId = cookie.UserAuthCookie;
            var cacheKey = string.Format(cacheKeyFormat, userId);
            var obj = cache.Get<T>(cacheKey);
            if (obj == null)
            {
                obj = GetRemote<T>(context, method, userId);
                cache.Add(cacheKey, obj);
            }
            return obj;
        }

        internal static ReadOnlyCollection<T> GetList<T>(HttpContextBase context, string method, string cacheKeyFormat)
        {
            var cookie = new WebCookies(context);
            var cache = new WebCache(context);
            var userId = cookie.UserAuthCookie;
            var cacheKey = string.Format(cacheKeyFormat, userId);
            var list = cache.Get<ReadOnlyCollection<T>>(cacheKey);
            if (list == null)
            {
                list = GetRemote<List<T>>(context, method, userId).AsReadOnly();
                cache.Add(cacheKey, list);
            }
            return list;
        }

        private static T GetRemote<T>(HttpContextBase context, string method, string userId) where T : class
        {
            string json = null;
            try
            {
                json = HttpProcUtil.Post(
                    new Uri(
                        string.Format("{0}/AuthApi/{1}", AuthConfig.ServerUrl, method)
                        ),
                    string.Format("uid={0}&appkey={1}",
                        context.Server.UrlEncode(userId),
                        AuthConfig.AppKey)
                    );
                var list = JsonConvert.DeserializeObject<T>(json);
                return list;
            }
            catch
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    context.Response.Redirect(AuthClient.GetLogOnUrl());
                    //    throw new Exception(method + " 无法验证您的登录信息，请重新登录");
                }
                throw new Exception(method + "时发生错误：" + json);
            }
        }
    }
}