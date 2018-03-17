using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public DataSet getWholesaleExceptions()
    {
      string sql = @"Select WholesaleUSOCToDelete, RetailUSOCToCopy, WholesaleCost,StartDate,EndDate, CustomerID, LastModifiedDateTime, LastModifiedBy from ImportUSOCMatchingExceptions ";
      return getDataFromSQL(sql);
    }
    public int? saveWholesaleExceptions(int? id, string wholesaleUsoc, string retailUsoc, string wholesaleReplaceUsoc, decimal cost, DateTime? startDate, 
      DateTime? endDate, string customerId, string user)
    {
      if (string.IsNullOrEmpty(wholesaleUsoc) || string.IsNullOrEmpty(retailUsoc) || string.IsNullOrEmpty(user))
        throw new Exception("saveWholesaleExceptions wholesale and retail usocs, and user are required");
      string sqlformat;
      if (id == null)
        sqlformat = @"INSERT INTO ImportUSOCMatchingExceptions (WholesaleUSOCToDelete, RetailUSOCToCopy,	WholesaleUSOCToReplace,	WholesaleCost,	StartDate,	EndDate,	CustomerID,	LastModifiedDateTime,	LastModifiedBy)
Values ('{1}' ,'{2}', '{3}', {4}, {5}, {6}, {7}, getdate(), '{8}')";
      else
        sqlformat = @"UPDATE ImportUSOCMatchingExceptions
SET WholesaleUSOCToDelete = '{1}', 
RetailUSOCToCopy = '{2}',	
WholesaleUSOCToReplace = '{3}',	
WholesaleCost = {4},	
StartDate = {5},	
EndDate = {6},	
CustomerID = {7},	
LastModifiedDateTime = getdate(),	
LastModifiedBy = '{8}'
WHERE ID = {0}";
      string sql = string.Format(sqlformat, 
          id, 
          wholesaleUsoc, 
          retailUsoc, 
          string.IsNullOrEmpty(wholesaleReplaceUsoc) ? wholesaleUsoc : "'" + wholesaleReplaceUsoc + "'" , 
          cost.ToString(), 
          startDate == null ? "null" : "'" + ((DateTime)startDate).ToShortDateString() + "'", 
          endDate == null ? "null" : "'" + ((DateTime)endDate).ToShortDateString() + "'", 
          string.IsNullOrEmpty(customerId) ? "null" : customerId,
          user);
      //WholesaleUSOCToDelete	RetailUSOCToCopy	WholesaleUSOCToReplace	WholesaleCost	StartDate	EndDate	CustomerID	LastModifiedDateTime	LastModifiedBy
      return updateDataFromSQL(sql);
    }
  }
}
