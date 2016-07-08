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
    private ServerResponse getPickLists(ServerRequest request, ServerResponse response)
    {
      string account, context, fieldNames, criteria;
      account = context = fieldNames = criteria = string.Empty;

      //BEGIN Parameters: Review and set the paramters to default or requested values
      //TODO: Ask Larry which account we will take! 
      //or if the request will have the account as a parameter
      if (request.Parameters.ContainsKey(CommonData.parmACCOUNT))
        account = CommonFunctions.CString(request.Parameters[CommonData.parmACCOUNT]);

      if (request.Parameters.ContainsKey(CommonData.parmCONTEXT))
        context = CommonFunctions.CString(request.Parameters[CommonData.parmCONTEXT]);

      if (request.Parameters.ContainsKey(CommonData.parmCRITERIA))
        criteria = CommonFunctions.CString(request.Parameters[CommonData.parmCRITERIA]);

      if (request.Parameters.ContainsKey(CommonData.parmFIELDNAMES))
        fieldNames = CommonFunctions.CString(request.Parameters[CommonData.parmFIELDNAMES]);

      string securityAccount = response.SecurityContext.Account;
      string securityUser = response.SecurityContext.User;

      if (string.IsNullOrEmpty(account))
        account = securityAccount;
      //END Parameters: Review and set the paramters to default or requested values

      /*
       * Now we build the required structures, supposing we receive them in the correct format
       * and this is, based on the call, which has this format:
       * ?command=getPickLists&securityid=83&context=(account:value,client:value)&fieldnames=(value1,value2,value3)
      */
      Hashtable pContext = new Hashtable();
      ArrayList pFieldNames = new ArrayList();

      string[] kvp = null;
      string[] contextLst = CommonFunctions.stripDelims(context, '(').Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
      string[] fieldNameLst = CommonFunctions.stripDelims(fieldNames, '(').Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < contextLst.GetLength(0); i++) 
      {
        kvp = contextLst[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
        if(kvp.GetLength(0) == 2)
          pContext.Add(kvp[0].Trim(), kvp[1].Trim());
      }

      for (int i = 0; i < fieldNameLst.GetLength(0); i++) 
      {
        pFieldNames.Add(fieldNameLst[i].Trim());
      }

      CCIForm pickLists =  new CCIForm();
      
      //TODO lmv66: Seems like the picklist does NOT use the ACCOUNT since it receives the account in the context
      //parameter, we will need to talk on this with Larry, or consult on the documentation, we are not using the 
      //SecurityAccount neither...
      if(//pContext.Count > 0 && // context is not required for some picklists (e.g. account, phase)
        pFieldNames.Count > 0)
        pickLists = _dataSource.getDSPickLists(pContext, pFieldNames, securityAccount, securityUser, criteria);

      response.Results.Add(pickLists);

      return response;
    }
  }
}
