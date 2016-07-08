using System;

namespace TAGBOSS.Common.Model
{
  public class TIncludeItem
  {
    public string IncludeSource { get; set; }
    public string IncludeHash { get; set; }
    public int IncludeDepth { get; set; }
    public bool DefaultInclude { get; set; }
  }
}
