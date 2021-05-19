using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace EMPMGMT.Utility
{
    public static class CookieProtectionHelperWrapper
    {
        private static MethodInfo _encode;
        private static MethodInfo _decode;

        static CookieProtectionHelperWrapper()
        {
            // obtaining a reference to System.Web assembly
            Assembly systemWeb = typeof(HttpContext).Assembly;
            if (systemWeb == null)
            {
                throw new InvalidOperationException(
                    "Unable to load System.Web.");
            }
            // obtaining a reference to the internal class CookieProtectionHelper
            Type cookieProtectionHelper = systemWeb.GetType(
                    "System.Web.Security.CookieProtectionHelper");
            if (cookieProtectionHelper == null)
            {
                throw new InvalidOperationException(
                    "Unable to get the internal class CookieProtectionHelper.");
            }
            // obtaining references to the methods of CookieProtectionHelper class
            _encode = cookieProtectionHelper.GetMethod(
                    "Encode", BindingFlags.NonPublic | BindingFlags.Static);
            _decode = cookieProtectionHelper.GetMethod(
                    "Decode", BindingFlags.NonPublic | BindingFlags.Static);

            if (_encode == null || _decode == null)
            {
                throw new InvalidOperationException(
                    "Unable to get the methods to invoke.");
            }
        }

        public static string Encode(CookieProtection cookieProtection,
                                    byte[] buf, int count)
        {
            return (string)_encode.Invoke(null,
                    new object[] { cookieProtection, buf, count });
        }

        public static byte[] Decode(CookieProtection cookieProtection,
                                    string data)
        {
            return (byte[])_decode.Invoke(null,
                    new object[] { cookieProtection, data });
        }

    }
    public static class MachineKeyCryptography {
        public static string Encode(string text, CookieProtection cookieProtection)
        {
            if (string.IsNullOrEmpty(text) || cookieProtection == CookieProtection.None)
            {
                return text;
            }
            byte[] buf = Encoding.UTF8.GetBytes(text);
            return CookieProtectionHelperWrapper.Encode(cookieProtection, buf, buf.Length);
        }

        public static string Decode(string text, CookieProtection cookieProtection)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            byte[] buf;
            try
            {
                buf = CookieProtectionHelperWrapper.Decode(cookieProtection, text);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                   "Unable to decode the text");
                //throw new InvalidCypherTextException(
                //    "Unable to decode the text", ex.InnerException);
            }
            if (buf == null || buf.Length == 0)
            {
                throw new InvalidOperationException(
                   "Unable to decode the text");
                //throw new InvalidCypherTextException(
                //    "Unable to decode the text");
            }
            return Encoding.UTF8.GetString(buf, 0, buf.Length);
        }
    }

    public static class HttpSecureCookie
    {

        public static HttpCookie Encode(HttpCookie cookie)
        {
            return Encode(cookie, CookieProtection.All);
        }

        public static HttpCookie Encode(HttpCookie cookie,
                      CookieProtection cookieProtection)
        {
            HttpCookie encodedCookie = CloneCookie(cookie);
            encodedCookie.Value =
              MachineKeyCryptography.Encode(cookie.Value, cookieProtection);
            return encodedCookie;
        }

        public static HttpCookie Decode(HttpCookie cookie)
        {
            return Decode(cookie, CookieProtection.All);
        }

        public static HttpCookie Decode(HttpCookie cookie,
                      CookieProtection cookieProtection)
        {
            HttpCookie decodedCookie = CloneCookie(cookie);
            decodedCookie.Value =
              MachineKeyCryptography.Decode(cookie.Value, cookieProtection);
            return decodedCookie;
        }

        public static HttpCookie CloneCookie(HttpCookie cookie)
        {
            HttpCookie clonedCookie = new HttpCookie(cookie.Name, cookie.Value);
            clonedCookie.Domain = cookie.Domain;
            clonedCookie.Expires = cookie.Expires;
            clonedCookie.HttpOnly = cookie.HttpOnly;
            clonedCookie.Path = cookie.Path;
            clonedCookie.Secure = cookie.Secure;

            return clonedCookie;
        }
    }
    public class CookieStore
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            HttpCookie encodedCookie = HttpSecureCookie.Encode(new HttpCookie(key, value));

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }
        public static string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                // For security purpose, we need to encrypt the value.
                HttpCookie decodedCookie = HttpSecureCookie.Decode(cookie);
                value = decodedCookie.Value;
            }
            return value;
        }

    }
}
