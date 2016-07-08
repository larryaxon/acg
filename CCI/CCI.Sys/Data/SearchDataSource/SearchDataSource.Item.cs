using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceItem: DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public SearchDataSourceItem()
    {
      SQL = "Select * from MasterProductList ";
      OrderByClause = " order by Name ";
      IDName = "ItemID";
      NameName = "Name";

    }
    public string[] Search(string criteria, bool useExactID)
    {
      return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
    }
  }
}
