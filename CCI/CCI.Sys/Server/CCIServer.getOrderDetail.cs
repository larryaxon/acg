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
    public ServerResponse getOrderDetail(ServerRequest request, ServerResponse response)
    {
      string orderID, orderName, user, screenSection;
      user = response.SecurityContext.User;
      orderID = orderName = string.Empty;
      if (request.Parameters.ContainsKey(CommonData.fieldID))
        orderID = request.Parameters[CommonData.fieldID];
      else
        if (request.Parameters.ContainsKey(CommonData.fieldORDERID))
          orderID = request.Parameters[CommonData.fieldORDERID];
      if (request.Parameters.ContainsKey(CommonData.fieldSCREENSECTION))
        screenSection = request.Parameters[CommonData.fieldSCREENSECTION];
      else
        screenSection = string.Empty;
      bool isRecommended = false;
      if (request.Parameters.ContainsKey("isrecommended"))
        isRecommended = CommonFunctions.CBoolean(request.Parameters["isrecommended"]);
      CCITable result = _dataSource.getOrderDetail(orderID, screenSection); // first get the raw order data
      CCITable screen = _dataSource.getOrderScreen(user, isRecommended, orderID, screenSection); // then get the screen data
      // now merge the two
      Dictionary<string, int> screenRows = new Dictionary<string,int>(StringComparer.CurrentCultureIgnoreCase);
      String theKey; // so we don't have to keep calling CommonFunctions.CString(....)
      for (int iScreenRow = 0; iScreenRow < screen.NumberRows; iScreenRow++)
      {
          theKey = CommonFunctions.CString(screen[iScreenRow, "ItemID"]); 
          // TODO: handle a duplicate key gracefully
          if (true == screenRows.ContainsKey(theKey))
          {
              // do nothing
              screenRows.Add(theKey, iScreenRow);   // for now, duplicate current behavior and cause a runtime error
          }
          else
          {
              // do something
              screenRows.Add(theKey, iScreenRow);
          }
      }
      for (int iDataRow = 0; iDataRow < result.NumberRows; iDataRow++)
      {
        int iScreenRow = -1;
        // first, find the matching row (by item id) in the screen data
        string item = CommonFunctions.CString(result[iDataRow, "ItemID"]);
        if (string.IsNullOrEmpty(item)) // this is a "manual entry" line item
        {
          screen.AddRow(new object[screen.NumberColumns]);
          iScreenRow = screen.NumberRows - 1;
        }
        else
        {
          if (screenRows.ContainsKey(item))
            iScreenRow = screenRows[item];
          else
            iScreenRow = -1;
        }
        if (iScreenRow >= 0)
        {
          // if we found one or added one, then  replace all of the values with the values from the order detail
          for (int iDataCol = 0; iDataCol < result.NumberColumns; iDataCol++)
          {
            if (screen.ContainsColumn(result.Column(iDataCol)) && !result.Column(iDataCol).Equals("dealercost", StringComparison.CurrentCultureIgnoreCase))
            {
              string col = result.Column(iDataCol);
              if (col.Equals("Description", StringComparison.CurrentCultureIgnoreCase))
              {
                string val = CommonFunctions.CString(result[iDataRow, iDataCol]);
                if (!string.IsNullOrEmpty(val))
                  screen[iScreenRow, result.Column(iDataCol)] = result[iDataRow, iDataCol];
              }
              else
                screen[iScreenRow, result.Column(iDataCol)] = result[iDataRow, iDataCol];
            }
          }
        }
      }
      response.Results.Add(screen); // we return the "enriched" screen detail 
      return response;
    }
  }
}