using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;

namespace ACG.Common.Data
{
  public class SearchDataSourceScreen : DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public string User { get; set; }
    public SearchDataSourceScreen()
    {
      SQL = @"select DataSource, Description from dbo.MaintenanceDataSources";
      OrderByClause = " order by Description ";
      IDName = "DataSource";
      NameName = "Description";
      User = string.Empty;
    }
    public string[] Search(string criteria, bool useExactID)
    {
        return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName });
    }
  }
}
