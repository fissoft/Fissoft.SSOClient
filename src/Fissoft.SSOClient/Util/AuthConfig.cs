using System;
using System.Configuration;

namespace Fissoft.SSOClient.Util
{
    //read config
    internal class AuthConfig
    {
        public static string Domain { get; set; }
        public static string CookieName
        {
            get
            {
                return GetCookieNameFunc == null ? ConfigurationManager.AppSettings["sso.application"] : GetCookieNameFunc();
            }
        }

        public static Func<string> GetCookieNameFunc;
        public static string UserAuthCookieName
        {
            get { return "userm"; }
        }
        internal static string AppKey
        {
            get { return ConfigurationManager.AppSettings["sso.appKey"]; }
        }

        internal static string ServerUrl
        {
            get { return ConfigurationManager.AppSettings["sso.serverUrl"]; }
        }

        internal static string Secret
        {
            get { return ConfigurationManager.AppSettings["sso.secret"]; }
        }
    }
}