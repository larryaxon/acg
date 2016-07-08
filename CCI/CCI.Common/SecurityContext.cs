using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Model;


namespace CCI.Common
{
  public class SecurityContext : ACG.Common.SecurityContext, ACG.Common.ISecurityContext
  {
    // we use an Item instead of UserInfo for this in this project
    private  Item _uInfo = new Item();

    public Item EntityUserInfo
    {
      get { return _uInfo; }
      set { _uInfo = (Item)value; }
    }
    public string UserType = string.Empty;
    public SecurityContext() { }
    public SecurityContext(string securityString)
      : base(securityString)
    {
      UserType = CommonFunctions.CString(EntityUserInfo.getValue("UserType"));
    }
    public override void loadUserInfo(string json)
    {
      _uInfo.loadJson(json);
    }
    public override string getUserInfoJson()
    {
      return _uInfo.ToJson();
    }
    public override string ToJson()
    {
      string securityID = SecurityID.ToString();
      string account = CommonFunctions.CString(Account);
      string login = CommonFunctions.CString(Login);
      string domain = CommonFunctions.CString(Domain);
      string password = CommonFunctions.CString(Password);
      string usertype = CommonFunctions.CString(UserType);
      string security = string.Empty;
      string userinfo = string.Empty;
      string timekeeper = string.Empty;
      if (UseDelegate && TimeKeeper != null)
        timekeeper = string.Format(", \"timekeeper\"=\"{0}\"", CommonFunctions.CString(TimeKeeper));
      if (EntityUserInfo != null)
        userinfo = string.Format(", {0}", getUserInfoJson());

      if (Security == null)
        security = "\"Error\":\"Security Object is NULL\"";
      else
        security = Security.ToJson();

      return string.Format("\"securitycontext\": {{ \"securityid\": \"{0}\", \"account\": \"{1}\", \"login\": \"{2}\", \"domain\": \"{3}\", \"password\": \"{4}\", \"usertype\": \"{8}\", {5} {6}{7} }}",
            securityID, account, login, domain, password, security, userinfo, timekeeper, usertype);

    }
  }
}
