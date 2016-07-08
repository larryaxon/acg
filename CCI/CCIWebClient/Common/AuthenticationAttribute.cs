using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Script.Serialization;

namespace CCIWebClient.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ACGAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthenticated = base.AuthorizeCore(httpContext);
            if (isAuthenticated)
            {
                //if (httpContext.Session["securityid"] == null)
                //{
                //    HttpContext.Current.Response.Redirect("/");
                //    return false;
                //}


                string cookieName = FormsAuthentication.FormsCookieName;

                if (!httpContext.User.Identity.IsAuthenticated ||
                    httpContext.Request.Cookies == null ||
                    httpContext.Request.Cookies[cookieName] == null)
                {
                    //HttpContext.Current.Response.Redirect("/");
                    return false;
                }

                var authCookie = httpContext.Request.Cookies[cookieName];
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                CustomPrincipalSerializeModel serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.UserId = serializeModel.UserId;
                newUser.UserType = serializeModel.UserType;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.SecurityId = serializeModel.SecurityId;



                if (!Proxy.isValidSecurityID(newUser.SecurityId))
                {
                    return false;
                }
                return true;
            }
            //HttpContext.Current.Response.Redirect("/");
            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        message = "Logged out"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }



}