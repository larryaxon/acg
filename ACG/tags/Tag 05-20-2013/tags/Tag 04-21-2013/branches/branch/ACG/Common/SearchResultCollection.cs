using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ACG.Common
{
  [Serializable]
  public class SearchResultCollection : PickListEntries
  {
    public new SearchResult this[int index] { get { return (SearchResult)base[index]; } }
    public new SearchResult this[string key] { get { return (SearchResult)base[key]; } }

    public new void Add(string key, SearchResult searchResult) { base.Add(key, searchResult); }

    public SortedDictionary<string, SearchResult> SortedBy(string memberName)
    {
      return (SortedDictionary<string, SearchResult>)base.SortedBy(memberName);
    }
  }
}
