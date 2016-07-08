using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using ACG.Common;

namespace CCI.Common
{
  public class CCIFormItem : SortedItemBase, IComparable, IEquatable<CCIFormItem>, IComparable<CCIFormItem>
  {
    public object Value { get { return base.Object; } set { base.Object = value; } }
    public FormItemTypes FormItemType { get; set; }
    public int CompareTo(object o)
    {
      if (o == null)
        return -1;
      if (o.GetType() == typeof(CCIFormItem))
        return CompareTo((CCIFormItem)o);
      throw new Exception("Must compare to an ATKFormItem object");
    }
    public int CompareTo(CCIFormItem item)
    {
      if (item == null)
        return 1;
      if (item.Value == null)
        return 1;
      if (Value == null)
        return -1;
      if (Value.GetType() == typeof(CCITable) && item.Value.GetType() == typeof(CCITable))
        return ((CCITable)Value).CompareTo((CCITable)item.Value);
      if (Equals(item))
        return 0;
      return Value.ToString().ToLower().Trim().CompareTo(item.Value.ToString().ToLower().Trim());
    }
    public bool Equals(CCIFormItem item)
    {
      if (item == null)
        return false;
      if (item.Value == null && Value == null)
        return true;
      if (item.Value == null || Value == null)
        return false;
      object[] pList = new object[] { item.Value };
      return (bool)Value.GetType().GetMethod("Equals").Invoke(this, pList);
    }
  }
}
