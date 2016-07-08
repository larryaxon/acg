using System;
using System.Collections.Generic;
using System.Text;

using ACG.Common;

namespace ACG.App.Common
{
  public class ACGForm : SortedCollectionBase, IComparable, IEquatable<ACGForm>, IComparable<ACGForm>
  {
    public string Name { get; set; }
    public new ACGFormItem this[string key]
    {
      get { return (ACGFormItem)base[key]; }
      set { base[key] = value; }
    }   
    public void Add(ACGFormItem item)
    {
      base.Add(item);
    }
    public int CompareTo(object o)
    {
      if (o == null)
        return 1;
      if (o.GetType() == typeof(ACGForm))
        return CompareTo((ACGForm)o);
      throw new Exception("Must compare to type ATKForm");
    }
    public int CompareTo(ACGForm form)
    {
      if (form == null)
        return 1;
      if (Count < form.Count)
        return -1;
      if (Count > form.Count)
        return 1;
      for (int iRow = 0; iRow < Count; iRow++)
      {
        ACGFormItem item = (ACGFormItem)this[iRow];
        ACGFormItem newItem = (ACGFormItem)form[iRow];
        int cmp = item.CompareTo(newItem);
        if (cmp != 0)
          return cmp;
      }
      return 0;
    }
    public bool Equals(ACGForm form)
    {
      if (form == null)
        return false;
      return (CompareTo(form) == 0);
    }
  }
}
