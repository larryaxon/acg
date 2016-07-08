using System;
using System.Collections;
using System.Text;

namespace TAGBOSS.Common
{
  [Serializable]
  public class SortedHashTable : SortedCollectionBase
  {
    private const string SORTORDERCOLNAME = "sortorder";
    /// <summary>
    /// create a SortedCollection version of the data in a Hash table. 
    /// </summary>
    /// <param name="h"></param>
    public SortedHashTable(Hashtable h)
    {
      if (h == null)
        return;
      SortByID = true; // sort by ID, not sort order
      int sortOrder = 0;
      foreach (DictionaryEntry entry in h)
      {
        SortedItemBase item = new SortedItemBase();
        item.ID = entry.Key.ToString();
        item.Object = entry.Value;
        if (entry.Value.GetType() == typeof(Hashtable)) // no point in addig this entry if the value is not in the right format
        {
          Hashtable fields = (Hashtable)entry.Value;
          if (fields.ContainsKey(SORTORDERCOLNAME))
            item.SortOrder = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, fields["sortorder"]);
          else
            item.SortOrder = sortOrder++;
        }
        else
          item.SortOrder = sortOrder++;
        base.Add(item);
      }
    }
  }
}
