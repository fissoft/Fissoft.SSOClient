using System;
using System.Web;

namespace Fissoft.SSOClient.Util
{
    /// <summary>
    /// 不要持久化存储本类的对象
    /// </summary>
    class WebCookies
    {
        public WebCookies(HttpContextBase context)
        {
            Context = context;
        }

        public bool ExistsCookies
        {
            get { return Context.Request.Cookies[AuthConfig.CookieName] != null; }
        }

        public bool HasLogOn
        {
            get { return ExistsCookies && !string.IsNullOrEmpty(UserAuthCookie); }
        }

        public HttpContextBase Context { get; set; }

        #region private Method

        private HttpCookie HttpCookieRequest
        {
            get { return Context.Request.Cookies[AuthConfig.CookieName]; }
        }
        private HttpCookie HttpCookieResponse
        {
            get
            {
                var cookie = Context.Response.Cookies[AuthConfig.CookieName];
                if (!string.IsNullOrWhiteSpace(AuthConfig.Domain) && 
                    cookie != null &&
                    cookie.Domain != AuthConfig.Domain)
                {
                    cookie.Domain = AuthConfig.Domain;
                }
                return cookie;
            }
        }
        private string GetCookieItem(string field)
        {
            if (Context.Request.Cookies[AuthConfig.CookieName] == null)
                return "";
            return HttpCookieRequest[field];
        }

        private void SetCookieItem(string field, string value)
        {
            HttpCookieResponse[field] = value;
        }

        #endregion


        internal void ClearInternal() {
            Context.Response.Cookies.Remove(AuthConfig.CookieName);
            Context.Response.Cookies.Clear();
            if (Context.Request.Cookies[AuthConfig.CookieName] != null)
            {
                var myCookie = new HttpCookie(AuthConfig.CookieName) { Expires = DateTime.Now.AddDays(-1d), Domain = AuthConfig.Domain };
                Context.Response.Cookies.Add(myCookie);
            }
        }
        #region public 属性
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserAuthCookie
        {
            get
            {
                string userm = GetCookieItem(AuthConfig.UserAuthCookieName).Contains("%")
                                   ? Context.Server.UrlDecode(GetCookieItem(AuthConfig.UserAuthCookieName))
                                   : GetCookieItem(AuthConfig.UserAuthCookieName);
                if (string.IsNullOrWhiteSpace(userm)) return null;
                return userm;
            }
            set { SetCookieItem(AuthConfig.UserAuthCookieName, value); }
        }

        public DateTime Expires
        {
            set { HttpCookieResponse.Expires = value; }
        }

        #endregion
    }
}