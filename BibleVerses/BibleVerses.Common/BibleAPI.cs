using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace BibleVerses.Common
{
  public static class BibleAPI
  {
    private const string APIKEY = "dcb386670156df8bd44aa0bf54d97c44";
    private const string AUTHHEADER = "api-key";
    private const string HOSTKEY = "BibleServiceHost";
    private const string LANGENGLISH = "eng";
    private const string VERSON = "v1";
    private const string PARMLANGUAG = "language";
    private const string PARMABBREVATION = "abbreviation";
    private static string _host = ConfigurationManager.AppSettings[HOSTKEY];
    private static Dictionary<string, string> _authHeaders = new Dictionary<string, string>()
    { { AUTHHEADER, APIKEY } };

    public static dynamic GetBibles(string language = LANGENGLISH)
    {
      return CallBibleService("bibles", new Dictionary<string, object>() { { PARMLANGUAG, language } }, Method.GET );
    }

    public static dynamic CallBibleService(string command, Dictionary<string, object> data, Method verb)
    {
      return ServiceClient.CallService<dynamic>(_host, makeCommand(command), data, verb, _authHeaders);
    }
    private static string makeCommand(string command)
    {
      return VERSON + "/" + command;
    }
  }
}
