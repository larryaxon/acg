using System;
using System.Collections.Generic;
using System.Collections;

namespace TAGBOSS.Common
{
  //Comparer for elements in the embeddedIncludeList
  [Serializable]
  public class IncludedItemAddress : IComparable<IncludedItemAddress>, IComparer<IncludedItemAddress>
  {
    int depth;
    int orderIndex;

    public int Depth
    {
      get { return depth; }
      set { depth = value; }
    }

    public int OrderIndex
    {
      get { return orderIndex; }
      set { orderIndex = value; }
    }

    public IncludedItemAddress()
    {
      depth = 0;
      orderIndex = 0;
    }

    public IncludedItemAddress(int MyDepth, int MyOrderIndex)
    {
      depth = MyDepth;
      orderIndex = MyOrderIndex;
    }

    public void setItemAddress(int NewDepth, int NewOrderIndex)
    {
      if (NewDepth > -1 && NewDepth < 256)
        depth = NewDepth;
      if (NewOrderIndex > -1)
        orderIndex = NewOrderIndex;
    }

    #region IComparer<includeItemAddress> Members
    int IComparer<IncludedItemAddress>.Compare(IncludedItemAddress eIAFirst, IncludedItemAddress eIASecond)
    {
      if (eIAFirst == null && eIASecond == null)
        return 0;
      else if (eIAFirst == null)
        return -1;
      else if (eIASecond == null)
        return 1;
      else
        return compareAddresses(eIAFirst.depth, eIAFirst.orderIndex, eIASecond.depth, eIASecond.orderIndex);
    }
    #endregion

    #region IComparable<includeItemAddress> Members
    int IComparable<IncludedItemAddress>.CompareTo(IncludedItemAddress other)
    {
      if (this == null && other == null)
        return 0;
      else if (this == null)
        return -1;
      else if (other == null)
        return 1;
      else
        return compareAddresses(depth, orderIndex, other.depth, other.orderIndex);
    }
    #endregion

    private int compareAddresses(int x1, int y1, int x2, int y2)
    {
      if (x1 > x2)
        return 1;
      else if (x1 < x2)
        return -1;
      else
      {
        if (y1 > y2)
          return 1;
        else if (y1 < y2)
          return -1;
        else
          return 0;
      }
    }
  }
}
