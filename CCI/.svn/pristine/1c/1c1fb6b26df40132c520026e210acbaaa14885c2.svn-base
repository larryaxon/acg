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
    private ServerResponse getVersion(ServerRequest request, ServerResponse response)
    {
      response.Results.Add(CommonFunctions.ProductVersion());
      return response;
    }
  }
}
