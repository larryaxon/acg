using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCIWebClient.Models;
using CCI.Common;
//using ACG.Common;
using CCIWebClient.Common;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CCIWebClient.Controllers
{
   
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            ServerResponse response = Proxy.Login(model.UserName, model.Password);
            if (response.SecurityContext.IsLoggedIn)
            {

                this.Session["securityid"] = response.SecurityContext.SecurityID;
                CustomPrincipal cp = new CustomPrincipal(response.SecurityContext.SecurityID.ToString());
                cp.UserId = model.UserName;
                cp.SecurityId = response.SecurityContext.SecurityID;
                cp.UserType = response.SecurityContext.UserType;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string data = jss.Serialize(cp);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddMinutes(15), false, data);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(cookie);
                switch (cp.UserType.ToLower())
                {
                    case "dealer":
                        return RedirectToAction("Index", "Dealer");
                      
                    case "agent":
                        return RedirectToAction("Index", "Agent");
                }
                return RedirectToAction("Index", "Dealer");
            }
            return View(model);
        }

    }
}
