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
    public class BaseController : Controller
    {
        /// <summary>
        /// Checks whether an auth token is authorized or expired. 
        /// Then it refreshes the expiration date/time so each time the user initiates a request the expiration date/time is updated.
        /// THat way it won't expire unless they are inactive for the expiration period.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newToken"></param>
        /// <returns></returns>
        internal bool IsAuthorized(string token, out string newToken)
        {
            token = token.Replace(' ', '+'); // passing this around somehow sometimes replaces the plus with spaces so we change it back just in case
            AuthenticationTokenModel auth = EncryptDecrypt.DecryptModel<AuthenticationTokenModel>(token);
            if (auth.ExpirationDateTime < DateTime.Now || !auth.IsAuthenticated)
            {
                newToken = token; // dont change this, we are going to make them log in again anyway
                return false;
            }
            auth.ExpirationDateTime = DateTime.Now.AddMinutes(CommonData.TokenExpirationMinutes);
            newToken = EncryptDecrypt.EncryptModel(auth);
            return true;
        }
    /// <summary>
    /// Checks whether an auth token is authorized or expired. 
    /// Then it refreshes the expiration date/time so each time the user initiates a request the expiration date/time is updated.
    /// THat way it won't expire unless they are inactive for the expiration period.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="newToken"></param>
    /// <returns></returns>
    internal AuthenticationTokenModel getAuthToken(string token, out string newToken)
    {
      token = token.Replace(' ', '+'); // passing this around somehow sometimes replaces the plus with spaces so we change it back just in case
      AuthenticationTokenModel auth = EncryptDecrypt.DecryptModel<AuthenticationTokenModel>(token);
      if (auth.ExpirationDateTime < DateTime.Now || !auth.IsAuthenticated)
      {
        newToken = token; // dont change this, we are going to make them log in again anyway
        auth.IsAuthenticated = false;
        return auth;
      }
      auth.ExpirationDateTime = DateTime.Now.AddMinutes(CommonData.TokenExpirationMinutes);
      newToken = EncryptDecrypt.EncryptModel(auth);
      return auth;
    }

    internal ActionResult ReturnToLogin()
        {
            return RedirectToAction("Index", "Login", new LoginGUIModel() { ErrorMessage = "Login has Expired" });
        }
        internal string getAuthToken(string username, string password)
        {
            string token = PrivateServiceClient.CallService<string>("api/token",
                new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
                {
                    { "postbody", new LoginModel() { UserName = username, Password = password } }
                },
                Method.POST);
            if (!string.IsNullOrWhiteSpace(token) && token.StartsWith("\""))
                token = token.Substring(1, token.Length - 2);
            return token;
        }
    }
}