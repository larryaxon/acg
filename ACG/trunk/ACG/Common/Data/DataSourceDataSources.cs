﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;

namespace ACG.Common.Data
{
  public class DataSourceDataSources : DataAccessBase
  {
    private string _tableName = "DataSources";
    private string[] _keyFields = new string[] { "DataSource" };
    public DataSet getDataSourceRecord(string dataSource, bool analysisOnly)
    {
      if (existsTable(_tableName))
      {
        string analysisClause = string.Empty;
        if (analysisOnly)
          analysisClause = " and Isnull(IncludeInAnalysis, 0) = 1";
        string sql = string.Format("select * from {1} where DataSource = '{0}' {2}", dataSource, _tableName, analysisClause);
        return getDataFromSQL(sql);
      }
      else
        return null;
    }
    public int? updateDataSourceRecord(string dataSource, string description, string fromClause, string orderByClause, string parameterList, int maxCount, bool overrideWhere, 
      bool includeInAnalysis, string user)
    {
      string sql;
      if (existsRecord(_tableName, _keyFields, new string[] { dataSource }))
        sql = @"update {0} set Description = '{2}', FromClause = '{3}', OrderByClause = '{4}', ParameterList = '{5}', MaxCount = {6}, OverrideWhere = {7},
            LastModifiedBy = '{8}', LastModifiedDateTime = '{9}', IncludeInAnalysis = {10} where DataSource = '{1}'";        
      else
        sql = @"insert into {0} (DataSource, Description, FromClause, OrderByClause, ParameterList, MaxCount, OverrideWhere, LastModifiedBy, LastModifiedDateTime, IncludeInAnalysis)
            Values ('{1}','{2}','{3}','{4}','{5}',{6},{7},'{8}','{9}', {10})";

      sql = string.Format(sql, _tableName, dataSource, description, fromClause, orderByClause, parameterList, maxCount.ToString(), overrideWhere ? "1" : "0", 
        user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), includeInAnalysis ? "1" : "0");
      return updateDataFromSQL(sql);
    }
    public int? deleteDataSourceRecord(string dataSource)
    {
      if (existsRecord(_tableName, _keyFields, new string[] { dataSource }))
      {
        string sql = string.Format("Delete from {0} where DataSource = '{1}'", _tableName, dataSource);
        return updateDataFromSQL(sql);
      }
      else
        return null;
    }
  }
}
