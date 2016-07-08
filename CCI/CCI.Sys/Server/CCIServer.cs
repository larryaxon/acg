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

    private DataSource _dataSource = new DataSource();

    /// <summary>
    /// TODO: Build NUNIT for this, and test it, add description here
    /// </summary>
    /// <param name="command"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public ServerResponse Execute(ServerCommands command, ServerRequest request)
    {
      ServerResponse response = new ServerResponse();
      response.SecurityContext = _dataSource.getSecurity(request.SecurityID);
      response = loadOptions(request, response);
      if (!response.Options.ContainsKey(CommonData.NEWTABLEFORMAT))
        response.Options.Add(CommonData.NEWTABLEFORMAT, "Yes");

      switch (command)
      {
        //GET Commands BEGIN: This are the GET equivalent web requests
        case ServerCommands.Login: return Login(request, response);
        case ServerCommands.GetCustomerInfo: return getCustomerInfo(request, response);
        case ServerCommands.GetCustomerSuggestions: return getCustomerSuggestions(request, response);
        case ServerCommands.GetFieldValidationInfo: return GetFieldValidationInfo(request, response);
        case ServerCommands.GetOrderDetail: return getOrderDetail(request, response);
        case ServerCommands.GetOrderHeader: return getOrderHeader(request, response);
        case ServerCommands.GetOrderScreen: return getOrderScreen(request, response);
        case ServerCommands.GetOrderTotals: return getOrderTotals(request, response);
        case ServerCommands.GetPickLists: return getPickLists(request, response);
        case ServerCommands.GetVersion: return getVersion(request, response);
        case ServerCommands.IsValidSecurityID: return isValidSecurityID(request, response);
        //PUT and POST Commands BEGIN: This are the PUT and POST equivalent web requests
        case ServerCommands.UpdateOrderDetail: return updateOrderDetails(request, response);
        //case ServerCommands.UpdateOrderHeader: return updateOrderHeader(request, response);
        default: throw new Exception("Illegal CCI Command");
      }
    }

    private ServerResponse loadOptions(ServerRequest request, ServerResponse response)
    {
      response.Options = new Hashtable();

      // load options into the response
      bool newTableFormat = true;
      if (request.Parameters.ContainsKey(CommonData.NEWTABLEFORMAT))
        newTableFormat = CommonFunctions.CBoolean(request.Parameters[CommonData.NEWTABLEFORMAT]);
      response.Options.Add(CommonData.NEWTABLEFORMAT, newTableFormat);
      return response;    
    }

  }
}
