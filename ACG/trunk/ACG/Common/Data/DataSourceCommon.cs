using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ACG.Common.Data
{
  public  class DataSourceCommon : DataAccessBase, IDisposable
  {
    public void Dispose() { }
    public  DataSet GetDataFromSQL(string sql)
    {
      return base.getDataFromSQL(sql);
    }
    //public new DataSet getDataFromSQL(string sql)
    //{
    //  return base.getDataFromSQL(sql);
    //}
  }
}
