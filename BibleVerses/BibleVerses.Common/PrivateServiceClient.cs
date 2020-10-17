using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

using RestSharp;

namespace BibleVerses.Common
{
  public static class PrivateServiceClient
  {
    private const string APPSETTINGSERVICEURL = "ApiUrl";
    private static string _host = ConfigurationManager.AppSettings[APPSETTINGSERVICEURL];
    public static T CallService<T>(string command, Dictionary<string, object> data, Method verb, Dictionary<string, string> headers = null)
    {
      return ServiceClient.CallService<T>(_host, command, data, verb, headers);
    }
  }
}
