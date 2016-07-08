using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys.Server
{
  public partial class CCIServer
  {
    /// <summary>
    /// TODO: Build NUNIT for this, and test it, add description here
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    private ServerResponse isValidSecurityID(ServerRequest request, ServerResponse response)
    {
      int securityID = response.SecurityContext.SecurityID;
      Hashtable results = new Hashtable();
      bool isValid = true;
      if (!response.SecurityContext.IsLoggedIn)
        isValid = false;
      if (response.SecurityContext.LastAccessedDateTime.AddMinutes(CommonData.MAXIDLEMINUTES) < DateTime.Now) // security has expired
        isValid = false;
      results.Add("isvalid", isValid);
      response.Results.Add(results);
      return response;
    }
  }
}
