using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACG.Common
{
  public class UserInfo
  {

    public string Login { get; set; }
    public string Domain { get; set; }
    public string Password { get; set; }
    public string Entity { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string OldID { get; set; }
    public string EntityType { get; set; }
    public string EntityOwner { get; set; }
    public string SecurityGroup { get; set; }
    public string UserType { get; set; }

    public UserInfo()
    {
      Login = string.Empty;
      Domain = "cci.com";
      Password = string.Empty;
      Entity = string.Empty;
      FirstName = string.Empty;
      LastName = string.Empty;
      Email = string.Empty;
      OldID = string.Empty;
      EntityType = "User";
      EntityOwner = string.Empty;
      SecurityGroup = string.Empty;
      UserType = string.Empty;
    }
    public UserInfo(string json)
    {

      loadJson(json);
    }
    public void loadJson(string json)
    {
      bool isJson = false;
      if (string.IsNullOrEmpty(json))
        throw new Exception("UserInfo constructor cannot accept empty string");
      json = json.Trim();
      if (!json.StartsWith("userinfo:("))
        if (json.StartsWith("\"userinfo\":"))
          isJson = true;
        else
          throw new Exception("UserInfo constructor: invalid json string");
      string tokenList = CommonFunctions.stripDelims(json, isJson ? CommonData.cLEFTCURLY : CommonData.cLEFT);
      string[] tokens = CommonFunctions.parseString(tokenList, new string[] { "," });
      foreach (string token in tokens)
      {
        string[] entry = CommonFunctions.parseString(token, new string[] { ":" });
        string key = CommonFunctions.stripDelims(entry[0], CommonData.cDOUBLEQUOTE).ToLower();
        string val = CommonFunctions.stripDelims(entry[1], CommonData.cDOUBLEQUOTE);
        switch (key)
        {
          case "login":
            Login = val;
            break;
          case "domain":
            Domain = val;
            break;
          case "password":
            Password = val;
            break;
          case "entity":
            Entity = val;
            break;
          case "firstname":
            FirstName = val;
            break;
          case "lastname":
            LastName = val;
            break;
          case "email":
            Email = val;
            break;
          case "usertype":
            UserType = val;
            break;
        }
      }
    }
    public string ToJson()
    {
      return string.Format("\"userinfo\": {{ \"login\": \"{0}\", \"domain\": \"{1}\", \"password\": \"{2}\", \"entity\": \"{3}\", \"firstname\": \"{4}\", \"lastname\": \"{5}\", \"email\": \"{6}\" , \"usertype\": \"{7}\" }}",
      Login, Domain, Password, Entity, FirstName, LastName, Email, UserType);
    }
  }
}
