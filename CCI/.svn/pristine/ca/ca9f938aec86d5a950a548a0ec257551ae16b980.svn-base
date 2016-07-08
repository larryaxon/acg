using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace CCIWebClient.Common
{
    /// <summary>
    /// Interface used to store the user info in the cookie
    /// </summary>
    interface ICustomPrincipal : IPrincipal
    {
        string UserId { get; set; }
        int SecurityId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string UserType { get; set; }
    }
    /// <summary>
    /// Used to store the userinfo info in the cookie
    /// </summary>
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public CustomPrincipal(string username)
        {
          //  this.Identity = new GenericIdentity(username);
        }
        public int SecurityId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
    }
    /// <summary>
    /// Used just to deserialize the cookie
    /// </summary>
    public class CustomPrincipalSerializeModel
    {
        public string UserId { get; set; }
        public int SecurityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
    } 

}