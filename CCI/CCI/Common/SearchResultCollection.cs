using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CCI.Common
{
  [Serializable]
  public class SearchResultCollection : ACG.Common.SearchResultCollection
  {
    public new SearchResult this[int index] { get { return (SearchResult)base[index]; } }
    public new SearchResult this[string key] { get { return (SearchResult)base[key]; } }

    public SortedDictionary<string, SearchResult> SortedBy(string memberName) 
    {
      SortedDictionary<string, ACG.Common.SearchResult> temp = base.SortedBy(memberName);
      SortedDictionary<string, SearchResult> result = new SortedDictionary<string, SearchResult>(StringComparer.CurrentCultureIgnoreCase);
      foreach (KeyValuePair<string, ACG.Common.SearchResult> entry in temp)
        result.Add(entry.Key, (SearchResult)entry.Value);
      return result;
    }
  }
}
