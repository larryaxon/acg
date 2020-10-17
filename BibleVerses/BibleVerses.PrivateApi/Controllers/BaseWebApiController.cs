using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BibleVerses.Common;
using BibleVerses.Models;

namespace BibleVerses.Api.Controllers
{
    public class BaseWebApiController : ApiController
    {
        internal string getFormBody()
        {
            try
            {
                if (!Request.Content.Headers.ContentType.MediaType.Contains("xml"))
                {
                    string stream = Request.Content.ReadAsStringAsync().Result;
                    return stream;
                }
                else
                {
                    throw new Exception("Form body must be json");
                    //XmlSerializer serializer = new XmlSerializer(typeof(T));
                    //using (Stream stream = Request.Content.ReadAsStreamAsync().Result)
                    //{
                    //    return (T)serializer.Deserialize(stream);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot Deserialize form body to string", ex);
            }
        }

    }
}
