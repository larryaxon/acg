using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public int? updateCreditMemo(string ID, string customerid, DateTime billdate, decimal memoAmount, string description, string user, string comment)
    {
      string sql;
      if (ID == null || ID == "")
        sql = string.Format(@"insert into ARTransactions 
                                  (CustomerID,TransactionType,TransactionDate,BillDate,Amount,Reference,LastModifiedBy,LastModifiedDateTime,Comment)
                           values ('{0}','Credit',getdate(),'{1}','{2}','{3}','{4}',getdate(),'{5}')",
                              customerid, billdate.ToShortDateString(), memoAmount.ToString(), description, user, comment);
      else
        sql = string.Format(@"update ARTransactions set customerid = '{0}',billdate = '{1}',amount = '{2}',reference = '{3}',lastmodifiedby = '{4}',
                                    lastmodifieddatetime = getdate()
                              where ID = {5}",
                              customerid, billdate.ToShortDateString(), memoAmount.ToString(), description, user, ID);
      return updateDataFromSQL(sql);
    }
    public int? deleteCreditMemo(string id)
    {
      if (string.IsNullOrEmpty(id))
        return -1;
      string sql = string.Format("Delete from ARTransactions where ID = {0}", id);
      return updateDataFromSQL(sql);
    }

  }
}
