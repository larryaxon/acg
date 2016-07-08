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
    public ServerResponse getOrderHeader(ServerRequest request, ServerResponse response)
    {
      string orderID, orderName;
      orderID = orderName = string.Empty;
      
      if (request.Parameters.ContainsKey(CommonData.fieldID))
        orderID = request.Parameters[CommonData.fieldID];
      else if (request.Parameters.ContainsKey(CommonData.fieldORDERID))
        orderID = request.Parameters[CommonData.fieldORDERID];

      if (request.Parameters.ContainsKey(CommonData.fieldSHORTNAME))
        orderName = request.Parameters[CommonData.fieldSHORTNAME];
      else if (request.Parameters.ContainsKey(CommonData.fieldORDERNAME))
        orderName = request.Parameters[CommonData.fieldORDERNAME];
      string user = response.SecurityContext.User;
      if (string.IsNullOrEmpty(user))
        user = "Dealer-Dealer";
      string dealer = _dataSource.getEntityOwner(user);
      string dealerName = CommonFunctions.CString(_dataSource.getEntityValue(dealer, CommonData.fieldLEGALNAME)).ToUpper().Replace(" ", "").Replace("'", "").Replace("/", "");
      orderName = dealerName + orderName;
      //_dataSource.checkIfRecordExists(CommonData.tableORDERS, new string[] { CommonData.fieldSHORTNAME }, new string[] { orderName })
      CCITable result = _dataSource.getOrderHeader(orderID, orderName);
      
      response.Results.Add(result);
      return response;
    }
  }
}
