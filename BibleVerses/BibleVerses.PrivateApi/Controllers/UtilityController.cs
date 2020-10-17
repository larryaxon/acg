using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BibleVerses.Manager;

namespace BibleVerses.Api.Controllers
{
    public class UtilityController : ApiController
    {
        [HttpGet]
        [Route("api/heartbeat")]
        public string heartbeat()
        {
            return UtilityProcessor.heartbeat();
        }



    }
}
