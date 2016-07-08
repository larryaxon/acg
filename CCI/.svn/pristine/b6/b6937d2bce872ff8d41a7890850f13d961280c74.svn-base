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
    public int? deleteNameMatch(string customerID,string saddlebackName)
    {
      if (string.IsNullOrEmpty(customerID) || string.IsNullOrEmpty(saddlebackName))
        return -1;
      string sql = string.Format("Delete from externalidmapping where externalid = '{0}' and internalid = '{1}'", saddlebackName,customerID);
      return updateDataFromSQL(sql);
    }
    public int? saveNameMatch(string oldcustomer, string newcustomer, string saddlebackName)
    {
      if (string.IsNullOrEmpty(oldcustomer) || string.IsNullOrEmpty(saddlebackName) || string.IsNullOrEmpty(newcustomer))
        return -1;
      string sql = string.Format(@"Update externalidmapping set internalid = '{2}' where externalid = '{0}' and internalid = '{1}'", 
                                     saddlebackName, oldcustomer, newcustomer);
      return updateDataFromSQL(sql);
    }

  }
}
