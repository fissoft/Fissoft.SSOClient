using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Fissoft.SSOClient.Util;

namespace Fissoft.SSOClient
{
    public class AuthClient
    {
        private static List<string> _ignorePaths = new List<string>
        {
            "/images/", "/scripts/", "/style/", "/styles/",
            "/content/",
            "/js/", "/css/","/account/logon","/home/quicklogon"
        };

        static public string AppKey { get { return AuthConfig.AppKey; } }
        static public string Secret { get { return AuthConfig.Secret; } }

        public static string AuthServerUrl { get { return AuthConfig.ServerUrl; } }

        #region For views

        public static string GetLogOnUrl(string returnUrl = null)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            return string.Format("{0}/Account/LogOn?appKey={1}&returnUrl={2}",
                                 AuthConfig.ServerUrl,
                                 context.Server.UrlEncode(AuthConfig.AppKey),
                                 context.Server.UrlEncode(returnUrl ?? context.Request.Url.ToString())
                );
        }

        public static string GetLogOutUrl(string returnUrl = null)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            return string.Format("{0}/Account/LogOff?id={1}&returnUrl={2}",
                                 AuthConfig.ServerUrl,
                                 AuthConfig.AppKey,
                                 context.Server.UrlEncode(returnUrl ?? context.Request.Url.ToString())
                );
        }

        /// <summary>
        /// 获取管理的Url
        /// </summary>
        /// <returns></returns>
        public static string GetAssignUrl(string returnUrl = null)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            return string.Format("{0}/Assign/Index?id={1}&returnUrl={2}",
                                 AuthConfig.ServerUrl,
                                 AuthConfig.AppKey,
                                 context.Server.UrlEncode(returnUrl ?? context.Request.Url.ToString())
                );
        }

        #endregion

        #region for global

        static public void RegisterCookieDomain(string domain)
        {
            AuthConfig.Domain = domain;
        }

        static public void RegisterCookieNameDelegate(Func<string> func)
        {
            AuthConfig.GetCookieNameFunc = func;
        }
        static public void RegisterIgnorePaths(params string[] paths)
        {
            _ignorePaths = _ignorePaths.Union(paths).Distinct().ToList();
        }

        static public void RemoveIgnorePaths(params string[] paths)
        {
            foreach (var path in paths)
            {
                _ignorePaths.Remove(path);
            }
        }
        /// <summary>
        /// 授权事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AuthenticateRequestEvent<TPrincipal, TIdentity>(object sender, EventArgs e)
            where TIdentity :
            class
            where TPrincipal : IPrincipal
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            var webCookies = new WebCookies(context);
            if (context.Request.Url != null)
            {
                var pathAndQuery = context.Request.Url.PathAndQuery.ToLower();
                if (_ignorePaths.Any(pathAndQuery.StartsWith))
                {
                    return;
                }
            }
            if (webCookies.HasLogOn)
            {
                string message = "";
                try
                {
                    var identity = RemoteCache.Get<TIdentity>(context, "GetIdentity", "GanjiNETSSOIdentityKey_{0}");
                    if (identity != null)
                    {
                        var principal = (TPrincipal)Activator.CreateInstance(typeof(TPrincipal), identity);

                        context.User = principal;
                        return;
                    }
                }
                catch (Exception ep)
                {
                    message = ep.Message;
                }
                webCookies.ClearInternal();
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.HeaderEncoding = Encoding.UTF8;
                context.Response.Write(
                    string.Format(
                        @"
<!DOCTYPE html><html><head>
<title>请重新登录</title>
<meta charset='utf-8' />
</head>
<body>
用户认证失败，请重新<a href='" +
                        AuthClient.GetLogOnUrl() +
                        @"'>登录</a>！<br />
{0}
</body></html>
", message));
                context.Response.End();
            }
        }

        #endregion
    }
}