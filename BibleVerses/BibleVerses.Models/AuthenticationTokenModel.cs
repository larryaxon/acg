using System;
using System.Collections.Generic;
using System.Text;

namespace BibleVerses.Models
{
  public class AuthenticationTokenModel
  {
    public string UserName { get; set; }
    public string Role { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public bool IsAuthenticated { get; set; }
  }
}
