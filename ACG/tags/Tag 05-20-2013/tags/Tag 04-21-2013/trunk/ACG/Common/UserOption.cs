using System;
using System.Collections.Generic;
using System.Text;

namespace ACG.Common
{
  public class UserOption
  {
    public string User { get; set; }
    public string OptionType { get; set; }
    public string OptionName { get; set; }
    public string Option { get; set; }
    public string Description { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
  }
}
