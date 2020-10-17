using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using RestSharp;

namespace BibleVerses.Common
{
  public static class ServiceClient
  {

    public static T CallService<T>(string host, string command, Dictionary<string, object> data, Method verb, Dictionary<string, string> headers = null)
    {

      RestClient client = new RestClient(host);
      RestRequest req = new RestRequest("/" + command, verb);
      if (headers != null && headers.Count > 0)
        foreach (KeyValuePair<string, string> header in headers)
          req.AddHeader(header.Key, header.Value);
      req.RequestFormat = DataFormat.Json;
      if (data != null)
        foreach (KeyValuePair<string, object> parm in data)
          if (parm.Key == "postbody")
            req.AddJsonBody(parm.Value);
          else
            req.AddParameter(parm.Key, parm.Value, ParameterType.QueryString);
      IRestResponse res = client.Execute(req);
      string jsonResult = res.Content;
      if (string.IsNullOrWhiteSpace(jsonResult))
      {
        return default(T);
      }
      Type t = typeof(T);
      T returnList;
      if (t.Name.Equals("String")) // if type tyle is string, then we dont convert. And the T cast works cause it was string to begin with
        return returnList = (T)((object)jsonResult);
      else
        returnList = JsonConvert.DeserializeObject<T>(jsonResult);
      return returnList;
    }

  }


}
