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
    public ServerResponse getOrderTotals(ServerRequest request, ServerResponse response)
    {
      int orderID = 0;

      if (request.Parameters.ContainsKey("orderid"))
        orderID = CommonFunctions.CInt(request.Parameters["orderid"]);
      else
      {        
        response.Errors.Add("OrderID must not be empty");
        return response;
      }

      if(orderID < 1)
      {        
        response.Errors.Add("OrderID must be greater than 0");
        return response;
      }

      ArrayList getOrderTotals = new ArrayList();
      if (_dataSource.existsRecord(CommonData.tableORDERS, new string[] { CommonData.fieldID }, new string[] { orderID.ToString() }))
      {
        getOrderTotals = _dataSource.getOrderTotals(orderID);
        if (getOrderTotals.Count > 0)
          foreach (CCITable orderTotal in getOrderTotals) 
            response.Results.Add(orderTotal);
        //response.Results.Add(_dataSource.getOrderTotals(orderID));
      }
      else
        response.Errors.Add(string.Format("OrderID:{0} does NOT exists!!", orderID.ToString()));
      
      return response;
    }
  }
}
