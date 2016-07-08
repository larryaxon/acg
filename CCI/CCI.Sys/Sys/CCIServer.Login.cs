using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys
{
  public partial class CCIServer
  {
    /// <summary>
    /// TODO: Build NUNIT for this, and test it, add description here
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <param name="domain"></param>
    /// <returns></returns>
    private ServerResponse Login(ServerRequest request, ServerResponse response)
    {
      SecurityContext s = new SecurityContext();
      s.Login = request.Parameters["username"];
      s.Password = request.Parameters["password"];
      if (request.Parameters.ContainsKey("domain"))
        s.Domain = request.Parameters["domain"];
      else
        s.Domain = "CCI.com";
      s = _dataSource.Login(s);
      ServerResponse r = new ServerResponse();
      r.SecurityContext = s;
      //r.Options = _dataSource.getTimeOptions(s.Account, s.User, null, null);


      return r;
    }
  }
}
