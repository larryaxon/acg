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

      //string client = null;
      //if (request.Parameters.ContainsKey(CommonData.parmCLIENT))
      //  client = CommonFunctions.CString(request.Parameters[CommonData.parmCLIENT]);

      //string matter = null;
      //if (request.Parameters.ContainsKey(CommonData.parmMATTER))
      //  matter = CommonFunctions.CString(request.Parameters[CommonData.parmMATTER]);

      response.Options = new Hashtable();
        //_dataSource.getTimeOptions(
        //  response.SecurityContext.Account,
        //  response.SecurityContext.User,
        //  client, 
        //  matter);

      switch (command)
      {
        //GET Commands BEGIN: This are the GET equivalent web requests
        case ServerCommands.Login: return Login(request, response);
        case ServerCommands.GetPickLists: return getPickLists(request, response);
        case ServerCommands.GetVersion: return getVersion(request, response);
        //GET Commands BEGIN: This are the PUT and POST equivalent web requests
        default: throw new Exception("Illegal CCI Command");
      }
    }

  }
}
