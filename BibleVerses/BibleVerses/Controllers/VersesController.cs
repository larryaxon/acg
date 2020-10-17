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
  public class VersesController : BaseController
  {
    // GET: Verses
    public ActionResult Index(string authToken)
    {
      AuthenticationTokenModel auth = getAuthToken(authToken, out authToken);
      if (!auth.IsAuthenticated)
        ReturnToLogin();
      string command = "api/verses/next/" + auth.UserName; // we ignore group for now
                                                           //string command = string.IsNullOrWhiteSpace(criteria) ? "clients" : "clients?criteria=" + criteria;
      BibleVerseModel verse = PrivateServiceClient.CallService<BibleVerseModel>(
        command,
        null,
        Method.GET);
      BibleVerseGUIModel model = new BibleVerseGUIModel()
      {
        AuthenticationToken = auth,
        model = verse,
        Verse = CommonFunctions.MakeFormattedVerse(verse)
      };
      
      return View(model);
    }
    
  }
}