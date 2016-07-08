using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace CCIWebClient.Common
{
    /// <summary>
    ///  Responsible for managing the cookies
    /// </summary>
    /// 
    public enum CookiesNames
    {
        CustomPrincipal
    }
    public static class CookieHelper
    {

        public static CustomPrincipal getUser(HttpContextBase httpContext)
        {
            return getUser(httpContext.Request.Cookies);
        }

        private static CustomPrincipal getUser(HttpCookieCollection cookies)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = cookies[cookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CustomPrincipalSerializeModel serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);
            CustomPrincipal User = new CustomPrincipal(authTicket.Name);
            User.UserId = serializeModel.UserId;
            User.UserType = serializeModel.UserType;
            User.FirstName = serializeModel.FirstName;
            User.LastName = serializeModel.LastName;
            User.SecurityId = serializeModel.SecurityId;
          
            return User;
        }

        public static CustomPrincipal getUser(HttpRequestBase httpContext)
        {
            return getUser(httpContext.Cookies);
        }

        public static void AddCookie(HttpContextBase httpContext, CustomPrincipal user)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(user);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.UserId, DateTime.Now, DateTime.Now.AddHours(24), false, data);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.Expires = DateTime.Now.AddHours(24); //the cookie will expire in 24 hours.
            httpContext.Response.Cookies.Add(cookie);
            HttpCookie usercookie = new HttpCookie("atkuser", encTicket);
            usercookie.Expires = DateTime.Now.AddHours(24);
            httpContext.Response.Cookies.Add(usercookie);
        }

        public static void RemoveCookie(HttpResponse httpContext)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            var authCookie = httpContext.Cookies[cookieName];
            authCookie.Expires.AddDays(-1);
            httpContext.Cookies.Add(authCookie);
        }

        public static void RemoveCookie(HttpRequestBase httpContext)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = httpContext.Cookies[cookieName];
            authCookie.Expires.AddDays(-1);
            httpContext.Cookies.Add(authCookie);
        }

        public static void RemoveCookie(HttpContextBase httpContext)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = httpContext.Response.Cookies[cookieName];
            FormsAuthentication.SignOut();
            if (authCookie.Expires <= DateTime.MinValue)
                return;
            authCookie.Expires.AddDays(-1);
            httpContext.Response.Cookies.Add(authCookie);
        }

    }
}