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
    public ServerResponse getCustomerSuggestions(ServerRequest request, ServerResponse response)
    {
      string name, address, city, state, zip, dealer;
      name = address = city = state = zip = dealer= string.Empty;
      //dealer = CommonFunctions.CString(_dataSource.getEntityOwner(response.SecurityContext.User));
      if (dealer.Equals("CCI", StringComparison.CurrentCultureIgnoreCase))
        dealer = string.Empty;
      if (request.Parameters.ContainsKey("name"))
        name = request.Parameters["name"];
      if (request.Parameters.ContainsKey("address1"))
        address = request.Parameters["address1"];
      if (request.Parameters.ContainsKey("city"))
        city = request.Parameters["city"];
      if (request.Parameters.ContainsKey("zip"))
        zip = request.Parameters["zip"];
      response.Results.Add(_dataSource.getCustomerSuggestions(name, address, city, state, zip, dealer));

      return response;
    }
  }
}
