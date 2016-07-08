using System;
namespace ACG.Common
{
  public interface ISecurityContext
  {
    string getUserInfoJson();
    bool IsLoggedIn { get; set; }
    void loadUserInfo(string json);
    string ToJson();
    string ToString();
    string User { get; }
    UserInfo UserInfo { get; set; }
    Security Security { get; set; }
  }
}
