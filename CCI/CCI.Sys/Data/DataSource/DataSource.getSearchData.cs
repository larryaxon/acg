using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public DataSet getSearchData(string fromClause, string orderbyClause, string where, int maxCount, bool overrideWhere)
    {
      string topClause = string.Empty;
      string whereClause = string.Empty;
      if (maxCount > 0)
        topClause = string.Format(" TOP {0} ", maxCount.ToString());
      string sql = string.Empty;
      if (!string.IsNullOrEmpty(where))
      {
        string qualifier;
        if (fromClause.ToLower().Contains(" where ") && !overrideWhere)
          qualifier = "AND";
        else
          qualifier = "WHERE";
        whereClause = string.Format(" {0} {1} ", qualifier, where);
      }
      if (!string.IsNullOrEmpty(fromClause))
        if (fromClause.StartsWith("EXEC", StringComparison.CurrentCultureIgnoreCase))
          sql = fromClause;
        else
          sql = string.Format("SELECT {0}{1}{2}{3}", topClause, fromClause, whereClause, orderbyClause);
      if (string.IsNullOrEmpty(sql))
        return null;
      else
        return getDataFromSQL(sql);
    }
    private DataSet getSearchInfo(string datasource)
    {
      if (!existsTable("DataSources"))
        return null;
      string sql = string.Format("Select * From DataSources where DataSource = '{0}'", datasource);
      return getDataFromSQL(sql);
    }
  }
}
