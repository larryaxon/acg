using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TableHeaderUniqueKeyEqualityComparer : IEqualityComparer<TableHeaderUniqueKey>
  {
    public bool Equals(TableHeaderUniqueKey key1, TableHeaderUniqueKey key2)
    {
      return key1.HashCode() == key2.HashCode();
      //return key1.Equals(key2);
    }
    public int GetHashCode(TableHeaderUniqueKey key)
    {
      return key.HashCode();
    }
  }
}
