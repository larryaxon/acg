using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BibleVerses.Manager;
using BibleVerses.Models;
using BibleVerses.Common;

namespace BibleVerses.Api.Controllers
{
    public class APILoginController : ApiController
    {
        [HttpPost]
        [Route("api/token")]
        public string GetAuthenticationToken(LoginModel login)
        {
            return LoginProcessor.CreateAuthenticationToken(login);
        }
    }
}
