﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCIWebClient.Common;

namespace CCIWebClient.Controllers
{
     [ACGAuthorize]
    public class ServicesController : Controller
    {
        //
        // GET: /Services/

        public ActionResult Index()
        {
            return View();
        }

    }
}
