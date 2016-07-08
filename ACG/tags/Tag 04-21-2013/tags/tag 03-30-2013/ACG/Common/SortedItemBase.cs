using System;
using System.Text;

namespace ACG.Common
{
  [Serializable]
  public class SortedItemBase : ISortedItemBase
  {
    private string origID = string.Empty;
    private int sortOrder = 0;
    public int SortOrder { get { return sortOrder; } set { sortOrder = value; } }
    public string ID 
    { 
      get { return origID.ToLower(); }
      set { if (value == null) origID = string.Empty; else origID = value; } 
    }
    public string OriginalID
    {
      get { return origID; }
      set { if (value == null) origID = string.Empty; else origID = value; } 
    }
    public object Object { get; set; }
    public ISortedItemBase Clone()
    {
      SortedItemBase s = new SortedItemBase();
      s.ID = origID;
      s.SortOrder = sortOrder;
      s.Object = Object;
      return s;
    }
  }
}
