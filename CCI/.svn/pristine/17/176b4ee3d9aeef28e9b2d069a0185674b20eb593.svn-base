using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using CCI.Common;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public void addExceptionLog(string exceptionType, string reasonCode, string source, string destination, string customer, string from, string to, string comments, string user)
    {
      string sql = string.Format(
        @"insert into ExceptionLog (ExceptionType, ReasonCode, Source, Destination, CustomerID, FromItem, ToItem, Comments, LastModifiedBy, LastModifiedDateTime)
          VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
          exceptionType, reasonCode, source, destination, customer, from, to, comments, user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
      updateDataFromSQL(sql);
    }
  }
}
