using System;
using System.Collections.Generic;
using System.Text;

namespace CCI.Common
{
  [Serializable]
  public class PickListEqualityComparer
  {
    public bool Equals(PickListEntry result1, PickListEntry result2)
    {
      return result1.HashCode() == result2.HashCode();
    }
    public int GetHashCode(SearchResult result)
    {
      return result.HashCode();
    }
  }
}
