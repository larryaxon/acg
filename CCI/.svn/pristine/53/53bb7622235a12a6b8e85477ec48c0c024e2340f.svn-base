using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ACG.Common;

namespace CCI.Common
{
  public class CCIForm : SortedCollectionBase, IComparable, IEquatable<CCIForm>, IComparable<CCIForm>
  {
    public string Name { get; set; }
    public new CCIFormItem this[string key]
    {
      get { return (CCIFormItem)base[key]; }
      set { base[key] = value; }
    }   
    public void Add(CCIFormItem item)
    {
      base.Add(item);
    }
    public int CompareTo(object o)
    {
      if (o == null)
        return 1;
      if (o.GetType() == typeof(CCIForm))
        return CompareTo((CCIForm)o);
      throw new Exception("Must compare to type ATKForm");
    }
    public int CompareTo(CCIForm form)
    {
      if (form == null)
        return 1;
      if (Count < form.Count)
        return -1;
      if (Count > form.Count)
        return 1;
      for (int iRow = 0; iRow < Count; iRow++)
      {
        CCIFormItem item = (CCIFormItem)this[iRow];
        CCIFormItem newItem = (CCIFormItem)form[iRow];
        int cmp = item.CompareTo(newItem);
        if (cmp != 0)
          return cmp;
      }
      return 0;
    }
    public bool Equals(CCIForm form)
    {
      if (form == null)
        return false;
      return (CompareTo(form) == 0);
    }
  }
}
