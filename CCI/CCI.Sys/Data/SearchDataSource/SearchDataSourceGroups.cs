using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceGroups : DataAccessBase, ISearchDataSource
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }

    public SearchDataSourceGroups()
    {
      IDName = "Entity";
      NameName = "LegalName";
      SQL = "select e.Entity, e.LegalName from Entity e inner join (select distinct GroupID from GroupMembers) g on g.GroupID = e.entity";
      OrderByClause = " order by LegalName";
    }
    public string[] Search(string criteria, bool useExactID)
    {
      return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
    }
  }
}
