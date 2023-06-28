﻿using System;
using System.Collections.Generic;
using System.Data;
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
        sql += "WHERE FileType = '" + filetype + "'";
      DataSet ds = getDataFromSQL(sql);
      if (ds.Tables.Count > 0)
      {
        ds.Tables[0].TableName = "CodeMaster";
      }
      return ds;
    }
    public void addFileProcessed(string FileType
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

      updateDataFromSQL(sql);
    }


    #endregion
    public DataSet getDatasetFromDictionaryData(string tablename, List<string> headers, List<List<string>> records)
    {
      DataSet ds = new DataSet();
      DataTable dt = new DataTable();
      dt.TableName = tablename;
      ds.Tables.Add(dt);
      foreach (string headername in headers)
        dt.Columns.Add(headername);
      foreach (List<string> record in records)
      {
        DataRow row = dt.NewRow();
        row.ItemArray = records.ToArray();
        dt.Rows.Add(row);
      }
      return ds;
    }
  }
}
