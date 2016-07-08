using System;
using System.Net;
using System.IO;

namespace CCIWebClient
{
   public static class HTTPHelper
    {
       /// <summary>
       /// Used to get Json data from the web server.
       /// </summary>
       /// <param name="url"></param>
       /// <returns></returns>
       public static string GetJSON(string url)
       {
        WebClient client = new WebClient ();
       // client.Headers.Add ("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
      //  client.Headers.Add("Content-type", "text/json");

        Stream data = client.OpenRead (url);
        StreamReader reader = new StreamReader (data);
        string s = reader.ReadToEnd ();
        data.Close ();
        reader.Close ();
        return s;
       }

/// <summary>
/// Used to post form info.
/// </summary>
/// <param name="url"></param>
/// <param name="data"></param>
/// <returns></returns>
       public static string PostForm(string url, string data)
       {
           WebClient client = new WebClient();
           client.Headers.Add("Content-type", "application/x-www-form-urlencoded");
           
           
           byte[] bret = client.UploadData(url, "POST",
                System.Text.Encoding.ASCII.GetBytes(data));
           
           string sret = System.Text.Encoding.ASCII.GetString(bret);
           return sret;
       }

 
   }
}
