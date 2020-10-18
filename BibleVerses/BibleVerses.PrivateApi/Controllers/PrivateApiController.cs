
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BibleVerses.Manager;
using BibleVerses.Models;
using BibleVerses.Common;
using System.Drawing.Printing;

namespace BibleVerses.Api.Controllers
{
  public class PrivateApiController : ApiController
  {
    [HttpGet]
    [Route("api/verses/{username}")]
    public IEnumerable<BibleVerseModel> GetVerses(string username, string group = "default")
    {
      using (VerseProcessor processor = new VerseProcessor())
      {
        return processor.GetBibleVerses(username, group);
      }
    }
    [HttpGet]
    [Route("api/verses/next/{username}")]
    public BibleVerseModel GetNextVerse(string username, string group = "default")
    {
      using (VerseProcessor processor = new VerseProcessor())
      {
        return processor.GetNextVerse(username, group);
      }
    }
    [HttpGet]
    [Route("api/bibles")]
    public Dictionary<string, string> GetBibleList(string language="eng")
    {
      using (VerseProcessor processor = new VerseProcessor())
      {
        return processor.GetBibleList(language);
      }
    }
    [HttpPost]
    [Route("api/verses")]
    public int SaveVerse(BibleVerseModel verse)
    {
      using (VerseProcessor processor = new VerseProcessor())
      {
        return processor.SaveVerse(verse);
      }
    }
    [HttpDelete]
    [Route("api/verses/{id}")]
    public void DeleteVerse(int id)
    {
      using (VerseProcessor processor = new VerseProcessor())
      {
        processor.DeleteVerse(id);
      }
    }
  }
    
}
