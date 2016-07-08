using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

 
using System.Xml;
using System.Xml.Linq;

using CCI.Common;
using CCI.Sys.Server;

namespace ATKBusinessServer.Services
{
    public partial class Default : System.Web.UI.Page
    {

        private CCIServer _server = new CCIServer();

        protected void Page_Load(object sender, EventArgs e)
        {
          ServerRequest req = getRequest(Request);
          Response.Clear();
          Response.Expires = 0;
          if (req.Command == ServerCommands.None)
            Response.Write(this.GetType().Assembly.GetName().Version.ToString());
          else
          {
            ServerResponse res = _server.Execute(req.Command, req);
            string response = ServerResponseSerializer.ToJson(res);
            //ServerResponse resTest = ServerResponseSerializer.FromJson(response);
            Response.Write(response);
          }
          Response.Cache.SetExpires(DateTime.Now);

        }
        private ServerCommands getCommand(string command)
        {
          //foreach (var cmd in Enum.GetValues(typeof(ServerCommands)))
          //{
          //  if (command.Equals(cmd.ToString(), StringComparison.CurrentCultureIgnoreCase))
          //    return (ServerCommands)cmd;
          //}
          //return ServerCommands.None;
          try
          {
            return (ServerCommands)Enum.Parse(typeof(ServerCommands), command, true);
          }
          catch
          {
            return ServerCommands.None;
          }
        }
        private ServerRequest getRequest(HttpRequest httpRequest)
        {
          ServerRequest req = new ServerRequest();
          foreach (string key in httpRequest.QueryString.Keys)
          {
            if (key != null)
            {
              if (key.Equals("securityid", StringComparison.CurrentCultureIgnoreCase))
                req.SecurityID = CommonFunctions.CInt(httpRequest.Params[key]);
              else
                if (key.Equals("command", StringComparison.CurrentCultureIgnoreCase))
                  req.Command = getCommand(httpRequest.Params[key]);
                else
                  req.Parameters.Add(key, httpRequest.Params[key]);
            }
          }
          foreach (string key in httpRequest.Form.Keys)
            req.Form.Add(key, httpRequest.Form[key]);
          return req;
        }
    }
}

