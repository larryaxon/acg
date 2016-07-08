using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCIWebClient.Reports;
using CrystalDecisions.Shared;
using System.IO;

namespace CCIWebClient.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/

        public ActionResult QuoteReport(string quoteId)
        {
            if (string.IsNullOrWhiteSpace(quoteId))
                quoteId = "-1";
            int quoteid = int.Parse(quoteId);
            QuoteReportHandler quoteReport = new QuoteReportHandler(quoteid);
            return File(quoteReport.QuoteReportStream(quoteid), "application/pdf");
        }

    }
}
