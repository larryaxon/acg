using System;

namespace TAGBOSS.Common.Model
{
  public class TIncludeItemObject
  {
    public TItem IncludeSourceObj { get; set; }
    public TItem IncludeObj { get; set; }
    public string IncludeSource { get; set; }
    public string IncludeHash { get; set; }
    public int IncludeDepth { get; set; }
    public bool BrokenInclude { get; set; }
  }
}
