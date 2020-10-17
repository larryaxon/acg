using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BibleVerses.Common;
using BibleVerses.Models;

using RestSharp;

namespace BibleVerses.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index(LoginGUIModel loginModel)
        {
            return View( loginModel == null? new  LoginGUIModel(): loginModel);
        }
        [HttpPost]
        public ActionResult Login(LoginGUIModel authInfo)
        {
            if (!(String.IsNullOrWhiteSpace(authInfo.UserName) || String.IsNullOrWhiteSpace(authInfo.Password) ))
              {           
                string token = getAuthToken(authInfo.UserName, authInfo.Password);
                if (!string.IsNullOrWhiteSpace(token))
                {

                    AuthenticationTokenModel auth = EncryptDecrypt.DecryptModel<AuthenticationTokenModel>(token);
                    if (auth.IsAuthenticated)
                        return RedirectToAction("Index", "Verses", new { authtoken = token });
                }
            }
            authInfo.ErrorMessage = "Login failed";
            return View("index",authInfo);
        }
        [HttpGet]
        public ActionResult Agent(string companyid)
        {
            string token = getAuthToken("agent", "agent");
            AuthenticationTokenModel auth = EncryptDecrypt.DecryptModel<AuthenticationTokenModel>(token);
            return RedirectToAction("Index", "Agent", new { authtoken = token, id = companyid });
        }

    }
}