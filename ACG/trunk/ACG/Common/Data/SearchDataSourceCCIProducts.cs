using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;

namespace ACG.Common.Data
{
  public class SearchDataSourceCCIProducts: DataAccessBase, ISearchDataSource
  {
    const string ID_SPLIT_CHAR = ".";

    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public string ItemCategory { get; set; }
    public bool DontRetreiveSize { get; set; }
    public SearchDataSourceCCIProducts()
    {
      OrderByClause = " ORDER BY ProductID";
      IDName = "ProductID";
      NameName = "ProductID";
      DontRetreiveSize = false;
    }
    public string[] Search(string criteria, bool useExactID)
    {
      string sql = getSQL();
      char[] delimiterChars = { ':' };
      string[] cac = getSearchList(sql, OrderByClause, criteria, IDName, new string[] { NameName }, false, false, CommonFunctions.SearchIn.Prefix);
      return cac;
    }
    public string getSQL()
    {
      return string.Format(@"SELECT * FROM (SELECT Carrier + '.' + ItemID ProductID FROM ProductList) a");
    }
    public string getItemID(string id)
    {
      if (string.IsNullOrEmpty(id))
        return string.Empty;
      int pos1 = id.IndexOf(ID_SPLIT_CHAR);
      if (pos1 > 0)
        return id.Substring(0, pos1);

      return id;
    }
    public string getSize(string id)
    {
      if (string.IsNullOrEmpty(id))
        return string.Empty;
      int pos1 = id.IndexOf(ID_SPLIT_CHAR);
      if (pos1 > 0)
      {
        string remainder = id.Substring(pos1 + 1);
        if (string.IsNullOrEmpty(remainder))
          return string.Empty;
        pos1 = remainder.IndexOf(ID_SPLIT_CHAR);
        if (pos1 > 0)
          return remainder.Substring(0, pos1);
      }

      return string.Empty;
    }
    public string getItemType(string id)
    {
      if (string.IsNullOrEmpty(id))
        return string.Empty;
      int pos1 = id.IndexOf(ID_SPLIT_CHAR);
      if (pos1 > 0)
      {
        string remainder = id.Substring(pos1 + 1);
        if (string.IsNullOrEmpty(remainder))
          return string.Empty;
        pos1 = remainder.IndexOf(ID_SPLIT_CHAR);
        if (pos1 > 0)
          return remainder.Substring(pos1 + 1);
      }

      return string.Empty;
    }
  }
}
