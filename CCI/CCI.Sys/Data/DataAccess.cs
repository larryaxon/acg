using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCI.Sys.Data
{
  public class DataAccess : DataAccessBase, IDisposable
  {
    public void Dispose() { }
    #region InvoiceIQ
    public DataSet getFilesProcessed(string filetype = null)
    {
      string sql = "Select * from FilesProcessed";
      if (!string.IsNullOrWhiteSpace(filetype))
        sql += " WHERE FileType = '" + filetype + "'";
      DataSet ds = getDataFromSQL(sql);
      if (ds.Tables.Count > 0)
      {
        ds.Tables[0].TableName = "CodeMaster";
      }
      return ds;
    }
    public int addFileProcessed(string FileType
      , string FileName
      , DateTime DateTImeProcessed
      , int NbrRecords = -1
      , bool HadError = false
      , string ErrorMessage = null
      , string StackTrace = null
      , DateTime? FileDateTime = null
      , DateTime? FirstRecordDateTime = null
      , DateTime? LastRecordDateTime = null)
    {
      string sql = string.Format(@"
Insert Into FilesProcessed ([FileType]
      ,[FileName]
      ,[DateTImeProcessed]
      ,[NbrRecords]
      ,[HadError]
      ,[ErrorMessage]
      ,[StackTrace]
      ,[FileDateTime]
      ,[FirstRecordDateTime]
      ,[LastRecordDateTime])
      VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, {8}, {9})",
      FileType, FileName, DateTImeProcessed, NbrRecords, HadError, "'" + LastRecordDateTime.ToString() + "'", 
      (ErrorMessage == null ? "null" : "'" + ErrorMessage + "'"),
      (StackTrace == null ? "null" : "'" + StackTrace + "'"),
      (FileDateTime == null ? "'" + FileDateTime.ToString() + "'" : "null"),
      (FirstRecordDateTime == null ? "'" + FirstRecordDateTime.ToString() + "'" : "null"),
      (LastRecordDateTime == null ? "'" + LastRecordDateTime.ToString() + "'" : "null") ) ;

      int? id = updateDataFromSQL(sql);
      return id ?? 0;
    }


    #endregion
    public DataSet getDatasetFromDictionaryData(string tablename, List<string> headers, List<List<object>> records, Dictionary<string, string> datatypes = null,  bool hasID = false)
    {
      DataSet ds = new DataSet();
      DataTable dt = new DataTable();
      dt.TableName = tablename;
      ds.Tables.Add(dt);
      dt.Columns.Add("ID");
      foreach (string headername in headers)
        dt.Columns.Add(headername);
      foreach (List<object> record in records)
      {
        DataRow row = dt.NewRow();
        object[] rowToAdd;
        if (hasID)
        {
          // this record has a unique id PK so we put an xtra null in the first column
          rowToAdd = new object[record.Count + 1];
          try
          {
            Array.Copy(record.ToArray(), 0, rowToAdd, 1, record.Count);
          }
          catch (Exception ex)
          {

            for (int i = 0; i < record.Count; i++)
            {
              try
              {
                rowToAdd[i] = record[i + 1];
              }
              catch (Exception e)
              {
                rowToAdd[i] = null;
              }



            }
          }
          rowToAdd[0] = null;
        }
        else
          rowToAdd = record.ToArray(); // just add the record as is
        row.ItemArray = rowToAdd;
        dt.Rows.Add(row);
      }
      return ds;
    }
    public DataSet GetDataFromSQL(string sql)
    {
      return getDataFromSQL(sql);
    }
  }
}
