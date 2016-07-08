using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Script.Serialization;
using CCIWebClient.Common;

namespace CCIWebClient
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
           
        }

        /// <summary>
        /// Used to retreive the User from the cookie stored on the user browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            //HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (authCookie != null )
            //{
            //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();
            //    CustomPrincipalSerializeModel serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);
            //    CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
            //    newUser.UserId = serializeModel.UserId;
            //    newUser.UserType = serializeModel.UserType;
            //    newUser.FirstName = serializeModel.FirstName;
            //    newUser.LastName = serializeModel.LastName;
            //    newUser.SecurityId = serializeModel.SecurityId;
            //    if (!Proxy.isValidSecurityID(newUser.SecurityId))
            //    {
            //        authCookie.Expires = DateTime.Now.AddDays(-1);
            //        Request.Cookies.Add(authCookie);
            //        Request.Cookies.Clear();
            //    }
            //    HttpContext.Current.User = newUser;
            //}
        } 
   }
}
