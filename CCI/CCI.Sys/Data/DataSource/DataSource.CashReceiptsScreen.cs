﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

using System.Data.SqlClient;
using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;

namespace CCI.Sys.Data
{
  public partial class DataSource : DataAccessBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }

    public string automaticChargifyCashPayment(DateTime billDate, string customerID, decimal amount, DateTime transactionDateTime, string User, string comment)
    {
      int? ret = insertCashReceipt(customerID, transactionDateTime, billDate, "ChargifyAutoPayment", string.Empty, Convert.ToDouble(amount), 0, User, comment);
      if (ret < 0) // error
        return string.Format("Error Posting Chargify Cash Receipt (Customer={0}, BillDate={1}, Amount={3}, DateTime={4}", customerID, billDate.ToShortDateString(), amount.ToString(), transactionDateTime.ToShortDateString());
      else
        return "Ready";
    }
    public DataSet getLastChargifyCashReceipt(string customerID)
    {
      string sql = string.Format(@"select * from artransactions ar
inner join (
select customerid, max(ID) LastID
from artransactions 
where customerID = '{0}' and TransactionType = 'Cash' and TransactionSubType = 'ChargifyAutoPayment'
group by customerID) m on ar.ID = m.lastID", customerID);
      return getDataFromSQL(sql);
    }

    public int? insertCashReceipt(string customerid, DateTime datereceived, DateTime billdate, string paymenttype, string checknumber, 
      double amount, double nonSBamount, string User, string comment)
    {
      string TransDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"insert into ARTransactions (CustomerID, TransactionType, TransactionDate, BillDate, TransactionDateTime,
	TransactionSubType, Amount, NonSaddlebackAmount, Reference, LastModifiedBy, LastModifiedDateTime, Comment)
	values ('{0}','Cash', '{1}','{2}','{3}','{4}','{5}','{6}','{7}', '{8}', '{9}', '{10}')",
      customerid, datereceived.ToShortDateString(), billdate.ToShortDateString(), datereceived.ToString(CommonData.FORMATLONGDATETIME), 
      paymenttype, (-amount).ToString(), (-nonSBamount).ToString(), checknumber, User, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), comment);
      return updateDataFromSQL(sql);
    }
    public int? updateCashReceipt(string ID, string customerid, DateTime datereceived, string paymenttype, string checknumber, 
      double amount, double nonSBamount, string User, string comment)
    {
      string TransDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"update ARTransactions set customerid = '{0}', TransactionDate = '{1}', TransactionSubType = '{2}', Reference = '{3}',
                                    amount = {4}, NonSaddlebackAmount = {5}, lastmodifiedby = '{6}', lastmodifieddatetime = '{7}', comment = '{9}'
                                    where ID = '{8}'",
                                     customerid, datereceived, paymenttype, checknumber, (-amount).ToString(),
                                     (-nonSBamount).ToString(), User, TransDateTime, ID, comment);
      return updateDataFromSQL(string.Format(sql));
    }
    public int? insertDebitMemo(string customerid, DateTime datereceived, DateTime billdate, double amount, string User, string comment)
    {
      string TransDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"insert into ARTransactions (CustomerID, TransactionType, TransactionDate, BillDate, TransactionDateTime,
	TransactionSubType, Amount, NonSaddlebackAmount, Reference, LastModifiedBy, LastModifiedDateTime, Comment)
	values ('{0}','Invoice', '{1}','{2}','{3}','Invoice','{4}',0,'Service Activation', '{5}', '{6}', '{7}')",
      customerid, datereceived.ToShortDateString(), billdate.ToShortDateString(), datereceived.ToString(CommonData.FORMATLONGDATETIME),
      (amount).ToString(), User, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), comment);
      return updateDataFromSQL(string.Format(sql));
    }
    public void deleteCashReceipt(string rowid)
    {
      string sql = string.Format("delete from artransactions where id = {0}", rowid);
      getDataFromSQL(sql);
    }
    public void deleteDebitMemo()
    {
    }
    public DataSet GetQBCustomerList()
    {
      string sql = "select InternalID as CustomerID, ExternalID as Name from externalidmapping where entitytype = 'QB' order by ExternalID";
      return getDataFromSQL(sql);
    }
    public DataSet getTaxesDataset(DateTime billdate)
    {
      string sql = string.Format(@"select 'Account' as Account,sum(taxamount) as Amount from hostedtaxtransactions where billdate = '{0}'", billdate);
      DataSet ds = getDataFromSQL(sql);
      return ds;
    }
    public void exportTaxestoCSV(DataSet ds)
    {
      string workbookPath = getexcelfile();
      if (workbookPath != null)
      {
        if (workbookPath.Substring(workbookPath.Length - 4, 4) != ".csv")
        {
          workbookPath = workbookPath + ".csv";
        }
        StreamWriter wr = new StreamWriter(workbookPath);

        string outstring;
        foreach (DataTable dt in ds.Tables)
        {
          foreach (DataRow myRow in dt.Rows)
          {
            foreach (DataColumn myCol in dt.Columns)
            {
              outstring = string.Format("\"{0}\",", myRow[myCol]);
              if (myCol.ColumnName == "Account")
              {
                wr.Write(outstring);
              }
              else // Amount
              {
                outstring = string.Format("\"{0}\"", Convert.ToString(myRow[myCol]));
                wr.Write(outstring);
              }
            }
            wr.WriteLine();
          }
        }
        //close file
        wr.Close();
        //        Excel.Application excelApp = new Excel.Application();
        //       excelApp.Visible = true;
        //        Excel.Workbook newworkbook = excelApp.Workbooks.Open(workbookPath);
      }
    }
    public void updateTaxesExported(DateTime billdate)
    { // Not used right now
      string postDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"", postDateTime, billdate);
      updateDataFromSQL(sql);
    }
    public DataSet getNewCustomersDataset()
    {
      string sql = @"select e.LegalName,e.Entity,'NWHS-' + right(e.AlternateID,3) as Account
                        from entity e
                           left join vw_attributenonxml a on e.entity = a.entity and a.item = 'Customer' and Name = 'Exported' 
                        where e.entitytype = 'Customer' and e.alternateid is not null and a.Entity is null
                        order by e.AlternateID";
      DataSet ds = getDataFromSQL(sql);
      return ds;
    }
    public void exportNewCustomerstoCSV(DataSet ds)
    {
      string workbookPath = getexcelfile();
      if (workbookPath != null)
      {
        if (workbookPath.Substring(workbookPath.Length - 4, 4) != ".csv")
        {
          workbookPath = workbookPath + ".csv";
        }
        StreamWriter wr = new StreamWriter(workbookPath);

        string outstring;
        foreach (DataTable dt in ds.Tables)
        {
          foreach (DataRow myRow in dt.Rows)
          {
            foreach (DataColumn myCol in dt.Columns)
            {
              outstring = string.Format("\"{0}\",", myRow[myCol]);
              if (myCol.ColumnName == "LegalName" || myCol.ColumnName == "Entity")
              {
                wr.Write(outstring);
              }
              else // Account
              {
                outstring = string.Format("\"{0}\"", Convert.ToString(myRow[myCol]));
                wr.Write(outstring);
              }
            }
            wr.WriteLine();
          }
        }
        //close file
        wr.Close();
        //        Excel.Application excelApp = new Excel.Application();
        //       excelApp.Visible = true;
        //        Excel.Workbook newworkbook = excelApp.Workbooks.Open(workbookPath);
      }
    }
    public void updateNewCustomersExported(DataSet ds)
    {
      foreach (DataTable dt in ds.Tables)
      {
        foreach (DataRow myRow in dt.Rows)
        {
          foreach (DataColumn myCol in dt.Columns)
          {
            if (myCol.ColumnName == "Entity")
            _dataSource.updateEntityAttribute(Convert.ToString(myRow[myCol]), "Exported", DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
          }
        }
      }
    }
    public DataSet getCashReceiptsDataset(DateTime billdate)
    {
      string sql = string.Format(@"select e.LegalName as Customer, cr.TransactionDate as DateReceived, cr.TransactionSubType as PaymentType, 
                           cr.Reference as CheckNumber, -cr.amount as Amount, 'NWCS-' + right(e.alternateid,3) as CustomerID
                       from ARTransactions cr 
                          inner join entity e on cr.customerid = e.entity
                       where ExportDateTime is null and cr.TransactionType = 'Cash' and cr.BillDate = '{0}'
                       order by cr.TransactionDate, e.AlternateID",billdate);
      DataSet ds = getDataFromSQL(sql);
      return ds;
    }
    public void updateCashReceiptsPosted()
    {
      string postDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"update artransactions set ExportDateTime = '{0}' where TransactionType = 'Cash' and ExportDateTime is null", postDateTime);
      updateDataFromSQL(sql);
    }
    public void updateCashReceiptsExported(DateTime billdate)
    {
      string postDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"update artransactions set ExportDateTime = '{0}' 
                                    where TransactionType = 'Cash' and ExportDateTime is null and BillDate = '{1}'", postDateTime,billdate);
      updateDataFromSQL(sql);
    }
    public void exportCashReceiptsToCSV(DataSet ds)
    {
      string workbookPath = getexcelfile();
      if (workbookPath != null)
      {
        if (workbookPath.Substring(workbookPath.Length - 4, 4) != ".csv")
        {
          workbookPath = workbookPath + ".csv";
        }
        StreamWriter wr = new StreamWriter(workbookPath);

        string outstring;
        foreach (DataTable dt in ds.Tables)
        {
          foreach (DataRow myRow in dt.Rows)
          {
            foreach (DataColumn myCol in dt.Columns)
            {
              if (myCol.ColumnName == "DateReceived")
              {
                DateTime outdate = Convert.ToDateTime(myRow[myCol]);
                wr.Write(outdate.ToString("d") + ",");
              }
              else
              {
                if (myCol.ColumnName == "Amount")
                {
                  wr.Write(myRow[myCol] + ",");
                }
                else
                {
                  outstring = string.Format("\"{0}\",", myRow[myCol]);
                  if (myCol.ColumnName == "Customer" || myCol.ColumnName == "PaymentType" || myCol.ColumnName == "CheckNumber")
                  {
                    wr.Write(outstring);
                  }
                  else // CustomerID
                  {
                    outstring = string.Format("\"{0}\"",Convert.ToString(myRow[myCol]));
                    wr.Write(outstring);
                  }
                }
              }
            }
            wr.WriteLine();
          }
        }


        //close file
        wr.Close();
        //        Excel.Application excelApp = new Excel.Application();
        //       excelApp.Visible = true;
        //        Excel.Workbook newworkbook = excelApp.Workbooks.Open(workbookPath);
      }
    }
    public void applyCashPost(DateTime throughDate)
    {
      //string sql = string.Format("exec CalculateCurrentARBalance '{0}'", throughDate.ToShortDateString());
      //updateDataFromSQL(sql);
    }
    public DateTime? getMostRecentCashReceiptDate()
    {
      string sql = "select Max(TransactionDate) from ARTransactions where TransactionType = 'Cash'";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return null;
      DateTime? returnDate = null;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        returnDate = CommonFunctions.CDateTime(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return returnDate;
    }
    public void undoImportPayments(DateTime billDate, DateTime postDateTime)
    {
      string sql = string.Format("DELETE FROM HostedImportPayments where BillDate = '{0}' and DateTimeImported = '{1}'", 
        billDate.ToShortDateString(),postDateTime.ToString());
      updateDataFromSQL(sql);
      sql = string.Format(@"DELETE FROM ARTransactions 
WHERE Comment = 'Import Payments' 
  AND BillDate = '{0}' 
  AND LastModifiedDateTime = '{1}'", billDate.ToShortDateString(), postDateTime.ToString());
      updateDataFromSQL(sql);
    }

  }
}
