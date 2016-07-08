using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;

namespace ACG.Common.Data
{
  public class SearchDataSourceDataSources : DataAccessBase, ISearchDataSource
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public bool IncludeOnlyAnalysis { get; set; }
    public SearchDataSourceDataSources()
    {
      SQL = "Select DataSource, Description from DataSources";
      OrderByClause = " order by DataSource";
      IDName = "DataSource";
      NameName = "Description";
      IncludeOnlyAnalysis = false;
    }
    public string[] Search(string criteria, bool useExactID)
    {
      string sql;
      if (IncludeOnlyAnalysis)
        sql = SQL + " WHERE Isnull(IncludeInAnalysis,0) = 1";
      else
        sql = SQL;
      return getSearchList(sql, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID, false);
    }
    public string GetIDFromDescription(string desc)
    {
      if (string.IsNullOrEmpty(desc))
        return string.Empty;
      string sql = string.Format("{0} WHERE Description = '{1}'", SQL, desc);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return string.Empty;
      string id = string.Empty;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        id = CommonFunctions.CString(ds.Tables[0].Rows[0]["DataSource"]);
      ds.Clear();
      ds = null;
      return id;
    }
  }
}
