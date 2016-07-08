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
    public ServerResponse getOrderScreen(ServerRequest request, ServerResponse response)
    {
      string user = response.SecurityContext.User;
      string screenSection;
      if (request.Parameters.ContainsKey(CommonData.fieldSCREENSECTION))
        screenSection = request.Parameters[CommonData.fieldSCREENSECTION];
      else
        screenSection = string.Empty;
      bool isRecommended = false;
      if (request.Parameters.ContainsKey("isrecommended"))
        isRecommended = CommonFunctions.CBoolean(request.Parameters["isrecommended"]);
      string orderId = string.Empty;
      if (request.Parameters.ContainsKey("orderid"))
        orderId = CommonFunctions.CString(request.Parameters["orderid"]);
      else
        if (request.Parameters.ContainsKey("id"))
          orderId = CommonFunctions.CString(request.Parameters["id"]);
      // first get the raw data
      CCITable  screen = _dataSource.getOrderScreen(user, isRecommended, orderId, screenSection);
      response.Results.Add(screen);
      return response;
    }
  }
}
