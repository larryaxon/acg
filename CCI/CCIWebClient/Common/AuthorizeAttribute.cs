using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Web.Mvc.ExpressionUtil;

namespace CCIWebClient.Common
{


    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CCIAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthenticated = base.AuthorizeCore(httpContext);
            if (isAuthenticated)
            {
                string cookieName = FormsAuthentication.FormsCookieName;
                if (!httpContext.User.Identity.IsAuthenticated ||
                    httpContext.Request.Cookies == null ||
                    httpContext.Request.Cookies[cookieName] == null)
                {

                    return false;
                }

                //var authCookie = httpContext.Request.Cookies[cookieName];
                //var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //string webServiceToken = authTicket.UserData;
                //string decript = FormsAuthentication.Decrypt(webServiceToken).ToString(); ;
                //CustomPrincipal userPrincipal = serializer.Deserialize<CustomPrincipal>(decript);
                //httpContext.User = userPrincipal;

                var redirectOnSuccess = httpContext.Request.Url.AbsolutePath;

                //send them off to the login page 
                //var redirectUrl = string.Format("?RedirectUrl={0}", redirectOnSuccess); 
                
              //  var loginUrl = LinkBuilder.BuildUrlFromExpression<HomeController>(filterContext.RequestContext, RouteTable.Routes,
                                                                                //  x => x.Login(redirectOnSuccess));
               // httpContext.Response.Redirect(loginUrl, true); 

            }
            else
                httpContext.Response.Redirect("/", true);

            return isAuthenticated;
        }
    }

}