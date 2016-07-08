using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;

namespace CCI.Sys.Server
{
  public partial class CCIServer
  {
    public ServerResponse getCustomerInfo(ServerRequest request, ServerResponse response)
    {
      string customerID = string.Empty;
      if (request.Parameters.ContainsKey("customerid"))
        customerID = CommonFunctions.CString(request.Parameters["customerid"]);
      CCITable cust = _dataSource.getCustomerTable(customerID);
      response.Results.Add(cust);
      return response;
    }
  }
}
