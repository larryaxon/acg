﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;


using ACG.Common;


namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    #region Login
    public CCI.Common.SecurityContext Login(CCI.Common.SecurityContext s)
    {
      s.Security = SecurityFactory.GetInstance().getSecurity("citycare.com", s.Login, s.Password);
      EntityAttributesCollection user = _ea.getAttributes(s.Security.User, "Entity", "User", null, DateTime.Today);
      if (user != null && user.Entities.Contains(s.Security.User))
        s.EntityUserInfo = (Item)user.getValue(string.Format("{0}.Entity.User", s.Security.User));
      _securityContext = s;  // now save the context so we know who we are
      logSecurity(s);
      return s;
    }
    public void SavePassword(CCI.Common.SecurityContext s)
    {
      CCI.Common.EncryptDecryptString encrypt = new CCI.Common.EncryptDecryptString();
      string sql = string.Format("Update SecurityUsers set Password = '{0}' Where login = '{1}'",
        encrypt.encryptString(ACG.Common.CommonFunctions.CString(s.NewPassword)), s.Login);
      updateDataFromSQL(sql);
      s.Password = s.NewPassword;
      s.NewPassword = null;
    }
    public string[] getUserList()
    {
      string sql = "select Login from SecurityUsers order by Login";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0)
        return new string[0];
      else
      {
        string[] userList = new string[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          userList[i] = ACG.Common.CommonFunctions.CString(ds.Tables[0].Rows[i]["Login"]);
        return userList;
      }
    }
    private void logSecurity(CCI.Common.SecurityContext s)
    {
      string dt = DateTime.Now.ToString(ACG.Common.CommonData.FORMATLONGDATETIME);
      string[] fields = new string[] { "SecurityID", "Account", "Timekeeper", "LoginDateTime", "LastAccessedDateTime", "SecurityAccess" };
      string[] values = new string[] { "0", s.Account, s.User, dt, dt, s.ToString() };
      long securityID = createRecordReturnID(fields, values, "createSecurityLogReturnID", "SecurityID");
      s.SecurityID = Convert.ToInt32(securityID);

      //After we create the record we need to UPDATE the record so as to UPDATE the string value for the SecurityID!!
      string[] keys = new string[] { "SecurityID" };
      fields = new string[] { "SecurityID", "SecurityAccess" };
      values = new string[] { s.SecurityID.ToString(), s.ToString() };
      updateRecord(fields, values, keys, "securityLog");
    }
    public CCI.Common.SecurityContext getSecurity(int securityID)
    {
      string sql = string.Format("Select SecurityAccess, LastAccessedDateTime from SecurityLog where SecurityId = {0}", securityID);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return new CCI.Common.SecurityContext();
      string securityString = ACG.Common.CommonFunctions.CString(ds.Tables[0].Rows[0]["SecurityAccess"]);

      CCI.Common.SecurityContext s = new CCI.Common.SecurityContext(securityString);
      s.LastAccessedDateTime = ACG.Common.CommonFunctions.CDateTime(ds.Tables[0].Rows[0]["LastAccessedDateTime"]);
      s.SecurityID = securityID;
      return s;
    }
    #endregion
  }
}
