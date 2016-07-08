using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

//using ACG.App.Common;
using ACG.Common;
using ACG.Sys.SecurityEngine;

namespace ACG.Sys.Data
{
  public partial class DataSource
  {
    #region Login
    public SecurityContext Login(SecurityContext s)
    {
      s.Security = SecurityFactory.GetInstance().getSecurity("axonconsultinggroup.com", s.Login, s.Password);
      //EntityAttributesCollection user = _ea.getAttributes(s.Security.User, "Entity", "User", null, DateTime.Today);
      //if (user != null && user.Entities.Contains(s.Security.User))
      //  s.UserInfo = (Item)user.getValue(string.Format("{0}.Entity.User", s.Security.User));
      _securityContext = s;  // now save the context so we know who we are
      logSecurity(s);
      return s;
    }
    public void SavePassword(SecurityContext s)
    {
      EncryptDecryptString encrypt = new EncryptDecryptString();
      string sql = string.Format("Update SecurityUsers set Password = '{0}' Where login = '{1}'",
        encrypt.encryptString(CommonFunctions.CString(s.NewPassword)), s.Login);
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
          userList[i] = CommonFunctions.CString(ds.Tables[0].Rows[i]["Login"]);
        return userList;
      }
    }
    private void logSecurity(SecurityContext s)
    {
      string dt = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
      string[] fields = new string[] { "SecurityID", "Account", "Timekeeper", "LoginDateTime", "LastAccessedDateTime", "SecurityAccess" };
      string[] values = new string[] { "0", s.Account, s.User, dt, dt, s.ToString() };
      long securityID = createRecordReturnID(fields, values, "createSecurityLogReturnID", "SecurityID");
      s.SecurityID = Convert.ToInt32(securityID);

      //After we create the record we need to UPDATE the record so as to UPDATE the string value for the SecurityID!!
      string[] keys = new string[] { "SecurityID" };
      fields = new string[] { "SecurityID", "SecurityAccess" };
      values = new string[] { s.SecurityID.ToString(), s.ToString() };
      updateRecord(fields, values, keys, "securityLog");
      updateUserState(s, dt, null);
    }
    public SecurityContext getSecurity(int securityID)
    {
      string sql = string.Format("Select SecurityAccess, LastAccessedDateTime from SecurityLog where SecurityId = {0}", securityID);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return new SecurityContext();
      string securityString = CommonFunctions.CString(ds.Tables[0].Rows[0]["SecurityAccess"]);

      SecurityContext s = new SecurityContext(securityString);
      s.LastAccessedDateTime = CommonFunctions.CDateTime(ds.Tables[0].Rows[0]["LastAccessedDateTime"]);
      s.SecurityID = securityID;
      return s;
    }
    public void updateUserState(SecurityContext s, string loggedindatetime, string timeid)
    {
      if (s.IsLoggedIn)
      {
        string[] loginparts = s.Security.UserLogin.Split(new char[] { '@' });
        string login = loginparts[0];
        string domain = loginparts[1];
        string sql = string.Empty;
        if (timeid == null)
        {
          if (existsRecord("UserState", new string[] { "Login", "Domain" }, new string[] { login, domain }))
            sql = "update UserState set LastLoggedIn = '{0}' WHERE login = '{1}' and domain = '{2}'";
          else
            sql = "insert INTO UserState (Login, Domain, LastLoggedIn) VALUES ('{1}', '{2}', '{0}')";
          sql = string.Format(sql, loggedindatetime, login, domain);
        }
        else
        {
          if (existsRecord("UserState", new string[] { "Login", "Domain" }, new string[] { login, domain }))
            sql = "update UserState set LastTimeID = {0} WHERE login = '{1}' and domain = '{2}'";
          else
            sql = "insert INTO UserState (Login, Domain, LastTimeID) VALUES ('{1}', '{2}', {0})";
          sql = string.Format(sql, timeid, login, domain);
        }
        updateDataFromSQL(sql);
      }
    }
    public int getLastTimeID(SecurityContext s)
    {
      string[] loginparts = s.Security.UserLogin.Split(new char[] { '@' });
      string login = loginparts[0];
      string domain = loginparts[1];
      string sql = string.Empty;
      int timeID = -1;
      if (existsRecord("UserState", new string[] { "Login", "Domain" }, new string[] { login, domain }))
      {
        sql = string.Format("Select LastTimeID from UserState WHERE Login = '{0}' and domain = '{1}'", login, domain);
        DataSet ds = getDataFromSQL(sql);
        if (ds != null)
        {
          if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            timeID = CommonFunctions.CInt(ds.Tables[0].Rows[0][0], -1);
          ds.Clear();
          ds = null;
        }
      }
      return timeID;

    }
    public int getMostRecentTimeID(SecurityContext s)
    {
      int timeID = -1;
      string sql = string.Format("Select Max(ID) from TimeEntry where ResourceID = '{0}'", s.User);
      DataSet ds = getDataFromSQL(sql);
      if (ds != null)
      {
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
          timeID = CommonFunctions.CInt(ds.Tables[0].Rows[0][0], -1);
        ds.Clear();
        ds = null;
      }
      return timeID;
    }
    #endregion
  }
}
