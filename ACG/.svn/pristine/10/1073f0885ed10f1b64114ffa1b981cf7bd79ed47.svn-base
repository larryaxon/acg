using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;

namespace ACG.Common.Data
{
  public class SearchDataSourceCodeMaster : DataAccessBase, ISearchDataSource 
  {
    //public string CodeType { get; set; }
    private string codeType;
    public string CodeType
    {
      get { return codeType; }
      set { codeType = value; }
    }
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public SearchDataSourceCodeMaster()
    {
      SQL = @"Select * from CodeMaster";
      OrderByClause = " order by Description ";
      IDName = "CodeValue";
      NameName = "Description";

    }
    public string[] Search(string criteria, bool useExactID)
    {
      string sql = SQL;
      if (!string.IsNullOrEmpty(CodeType))
        sql = SQL + string.Format(" Where CodeType = '{0}'", CodeType);
      return getSearchList(sql, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
    }
  }
}
