using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceCarrier : DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public SearchDataSourceCarrier()
    {
      SQL = "Select Entity, LegalName from Entity where EntityType = 'Carrier' ";
      OrderByClause = " order by LegalName ";
      IDName = "Entity";
      NameName = "LegalName";

    }
    public string[] Search(string criteria, bool useExactID)
    {
      return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
    }
  }
}
