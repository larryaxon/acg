using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;


namespace ACG.Common
{
  public class SecurityContext
  {
    public int SecurityID = -1;
    public string Account = null;
    public string Login = null;
    public string Domain = null;
    public string User { get { if (Security == null) return string.Empty; else return Security.User; } }
    public string Password = null;
    public string NewPassword = null;
    public string TimeKeeper = null;
    public DateTime LastAccessedDateTime = CommonData.PastDateTime;
    private  UserInfo _userInfo = new UserInfo();

    public virtual UserInfo UserInfo
    {
      get { return _userInfo; }
      set { _userInfo = value; }
    }

    public bool UseDelegate = false;

    public bool IsLoggedIn 
    { 
      get { if (Security == null) return false; else return Security.IsLoggedIn; }
      set { if (Security != null) Security.IsLoggedIn = value; }
    }
    public bool Cancelled = false;
    public Security Security = null;

    public SecurityContext() { }

    public SecurityContext(string securityString)
    {
      bool isJson = false;
      if (string.IsNullOrEmpty(securityString))
        throw new Exception("SecurityContext constructor cannot accept empty string");
      if (!securityString.StartsWith("SecurityContext:("))
        if (securityString.StartsWith("\"securitycontext\":"))
          isJson = true;
        else
          throw new Exception("SecurityContext constructor: invalid security string");
      string tokenList = CommonFunctions.stripDelims(securityString, isJson ? CommonData.cLEFTCURLY : CommonData.cLEFT);
      string[] tokens = CommonFunctions.parseString(tokenList, new string[] { "," });
      foreach (string token in tokens)
      {
        string[] entry = CommonFunctions.parseString(token, new string[] { ":" });
        string key = CommonFunctions.stripDelims(entry[0], CommonData.cDOUBLEQUOTE).ToLower();
        string val = CommonFunctions.stripDelims(entry[1], CommonData.cDOUBLEQUOTE);
        switch (key)
        {
          case "account":
            Account = val;
            break;
          case "securityid":
            SecurityID = CommonFunctions.CInt(val);
            break;
          case "login":
            Login = val;
            break;
          case "domain":
            Domain = val;
            break;
          case "password":
            Password = val;
            break;
          case "security":
            Security = new Security(CommonFunctions.stripDelims(entry[1], CommonData.cLEFTCURLY));
            break;
          case "item":  // in some inherited forms, userinfo is called "item" in the jscon
          case "userinfo":
            // UserInfo in some inheritances has a different type, but they all have a loadJson() method
            this.GetType().GetMethod("loadUserInfo").Invoke(this, new object [] { token });
            //this.GetType().GetProperty("UserInfo").GetType().GetMethod("loadJson").Invoke(UserInfo, new object[] { token });
            break;
          case "timekeeper":
            TimeKeeper = val;
            break;
        }
      }
    }

    public virtual void loadUserInfo(string json)
    {
      UserInfo.loadJson(json);
    }

    public new string ToString()
    {
      string securityID = SecurityID.ToString();
      string account = CommonFunctions.CString(Account);
      string login = CommonFunctions.CString(Login);
      string domain = CommonFunctions.CString(Domain);
      string password = CommonFunctions.CString(Password);
      string security = string.Empty;
      string userinfo = string.Empty;
      string timeKeeper = string.Empty;
      if (UseDelegate && TimeKeeper != null)
        timeKeeper = string.Format(",Timekeeper:{0}", CommonFunctions.CString(TimeKeeper));
      if (Security == null)
        security = "\"Error\":\"Security Object is NULL\"";
      else
        security = Security.ToJson();
      if (UserInfo != null)
        userinfo = UserInfo.ToJson();

      return string.Format("SecurityContext:(SecurityID:{0},Account:{1},Login:{2},Domain:{3},Password:{4},{5},{6}{7})",
            securityID, account, login, domain, password, security,userinfo,timeKeeper);
    }
    public virtual string ToJson()
    {
      string securityID = SecurityID.ToString();
      string account = CommonFunctions.CString(Account);
      string login =  CommonFunctions.CString(Login);
      string domain = CommonFunctions.CString(Domain);
      string  password = CommonFunctions.CString(Password);
      string security = string.Empty;
      string userinfo = string.Empty;
      string timekeeper = string.Empty;
      if (UseDelegate && TimeKeeper != null)
        timekeeper = string.Format(", \"timekeeper\":\"{0}\"", CommonFunctions.CString(TimeKeeper));
      if (UserInfo != null)
        userinfo = string.Format(", {0}", getUserInfoJson()); 

      if (Security == null)
        security = "\"Error\":\"Security Object is NULL\"";
      else
        security = Security.ToJson();

      return string.Format("\"securitycontext\": {{ \"securityid\": \"{0}\", \"account\": \"{1}\", \"login\": \"{2}\", \"domain\": \"{3}\", \"password\": \"{4}\", {5} {6}{7} }}",
            securityID, account, login, domain, password, security, userinfo, timekeeper);

    }
    public virtual string getUserInfoJson()
    {
      return _userInfo.ToJson();
    }
  }
}
