using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace ACG.Common.Data
{
  public class DataAdapterContainer
  {
    private DataSet _ds = null;
    public DataSet DataSet 
    {
      get { return _ds; }
      set
      {
        _ds = value;
      }
    }
    public SqlDataAdapter DataAdapter { get; set; }
    public string EncryptedFieldName { get; set; }
    public DataAdapterContainer()
    {
      DataSet = null;
      DataAdapter = null;
      EncryptedFieldName = null;
    }
    public void Update()
    {
      if (DataSet != null && DataSet.Tables.Count > 0)
        Update(DataSet.Tables[0].TableName);
      else
        DataAdapter.Update(DataSet);
    }
    public void Update(string tableName)
    {
      if (!string.IsNullOrEmpty(EncryptedFieldName))
      {
        DataSet ds = CommonFunctions.encryptDataset(DataSet, EncryptedFieldName);
        DataSet.Clear();
        DataSet = CommonFunctions.decryptDataset(ds, EncryptedFieldName);
        ds.Clear();
        ds = null;
      }
      DataAdapter.Update(DataSet, tableName);
    }
  }
}
