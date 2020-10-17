using System;
using System.Collections.Generic;
using System.Text;

namespace BibleVerses.Models
{
  public class LoginModel
  {
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool IsAuthenticated { get; set; }

  }
}
