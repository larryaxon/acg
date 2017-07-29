using System;
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

    public Exception executestoredproc(string StoredProc, string FilePath, string criteria)
    {
      Exception returnmsg;
      string sql = string.Format("{0} '{1}' {2}",StoredProc, FilePath, criteria);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    public bool checkfortollimporterror(string type,string billingyear,string billingmonth)
    {
      bool error;   
      string sql = string.Format("Select Top 1 * from {0} where billingyear = '{1}' and billingmonth = '{2}'", type,billingyear,billingmonth);
      DataSet ds = getDataFromSQL(sql);
      if (ds.Tables[0].Rows.Count > 0)
        error = true;
      else
        error = false;
      return error;
    }
    public bool invoicesArePosted(DateTime billdate)
    {
      string sql = string.Format("select count(*) Count from ARTransactions where TransactionType = 'Invoice' and Reference <> 'Service Activation' and BillDate = '{0}'", billdate.ToShortDateString());
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return false;
      bool hasInvoices = false;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        hasInvoices = ( CommonFunctions.CInt(ds.Tables[0].Rows[0]["Count"]) > 0);
      ds.Clear();
      ds = null;
      return hasInvoices;
    }
    public Exception execUnpostInvoices(DateTime billdate)
    {
      Exception returnmsg;
      string sql = string.Format("delete from ARTransactions where TransactionType = 'Invoice' and Reference <> 'Service Activation' and BillDate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    public DataSet getPostedInvoicesForChargify(DateTime billdate)
    {
      string sql = string.Format(@" Select t.CustomerID, t.TransactionDate, t.BillDate, t.Amount, case when isnull(bal.AmountDue,0) <= 0 then 0 else bal.AmountDue end AmountDue, 
 t.Reference, e.LegalName CustomerName
,p.value('(./valuehistory/@value)[1]', 'VARCHAR(255)') AS ChargifySubscription 
from ARTransactions t 
inner join Entity e on t.CustomerID = e.Entity
inner join Attribute c on t.CustomerID = c.Entity and c.Item = 'Customer' and c.ItemType = 'Entity'
left join ( select customerID, sum(amount) AmountDue from artransactions group by customerid ) bal on bal.CustomerID = e.Entity
CROSS APPLY c.attributes.nodes('/attributes/attribute[@name=""ChargifySubscription""]') a(p)
where t.TransactionType = 'Invoice' and t.Reference <> 'Service Activation' and t.BillDate = '{0}'", billdate);
      return getDataFromSQL(sql);
    }
    public DataSet getChargifyCustomerInfo()
    {
      string sql = @"Select e.Entity CustomerID, e.LegalName CustomerName,p.value('(./valuehistory/@value)[1]', 'VARCHAR(255)') AS ChargifySubscription 
from Entity e 
inner join Attribute c on e.Entity = c.Entity and c.Item = 'Customer' and c.ItemType = 'Entity'
CROSS APPLY c.attributes.nodes('/attributes/attribute[@name=""ChargifySubscription""]') a(p)
where e.entitytype = 'customer'";
      return getDataFromSQL(sql);
    }
    public Exception execPostInvoices(DateTime billdate)
    {
      Exception returnmsg;
      string sql = string.Format("exec postedCityHostedInvoices '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }


    public DateTime selectBillingDatefromTemp(string Table)
    {
      DateTime nodate = new DateTime(1980, 1, 1);
      string sql = string.Format("Select Top 1 [BillDate] from {0}", Table);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return nodate;
      return CommonFunctions.CDateTime(ds.Tables[0].Rows[0][0]);
    }
    public string selectBillingDatefromCallTemp(string Table)
    {
      string nodate = "";
      string sql = string.Format("Select Top 1 [BillingYear] + '/' + [BillingMonth] from {0}", Table);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return nodate;
      return CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
    }
    public Exception processMRCDetails(DateTime billdate, string user)
    {
      Exception returnmsg;
      string sql = string.Format("ProcessMRCDetails '{0}','{1}'", user, billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(sql);
      return returnmsg;
    }
    public Exception processOCCDetails(DateTime billdate, string user)
    {
      Exception returnmsg;
      string sql = string.Format("ProcessOCCDetails '{0}','{1}'", user, billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    public Exception processTOLLDetails(DateTime billdate, string user)
    {
      Exception returnmsg;
      string billyear = Convert.ToString(DateTime.Parse(Convert.ToString(billdate)).Year);
      string billmonth = Convert.ToString(DateTime.Parse(Convert.ToString(billdate)).Month);
      string sql = string.Format("ProcessTollDetails '{0}','{1}','{2}'", user, billyear, billmonth);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    public string processUndoImport(DateTime billdate, string user)
    {
      Exception returnmsg;
      string errmsg;
      string sql;
      sql = string.Format("Delete from HostedImportMRCWholesale where billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo MRC Wholesale Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-MRC Wholesale");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      sql = string.Format("Delete from HostedImportMRCRetail where billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo MRC Retail Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-MRC Retail");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      sql = string.Format("Delete from HostedImportOCCWholesale where billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo OCC Wholesale Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-OCC Wholesale");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      sql = string.Format("Delete from HostedImportOCCRetail where billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo OCC Retail Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-OCC Retail");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }

      sql = string.Format("Delete from HostedImportTollWholesale where billingyear = year('{0}') and billingmonth = month('{0}')", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo Call Wholesale Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-Call Wholesale");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      sql = string.Format("Delete from HostedImportTollRetail where billingyear = year('{0}') and billingmonth = month('{0}')", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo Call Retail Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-Call Retail");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      sql = string.Format("Delete from HostedImportTaxes where billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Undo Taxes Import.";
        return errmsg;
      }
      returnmsg = updateAcctImportsLog(user, billdate, "X-Taxes");
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      returnmsg = unProcessStep("Import", billdate);
      if (returnmsg != null)
      {
        errmsg = "Error on updating process cycle.";
        return errmsg;
      }
      return "";
    }

    public string processUndoPost(DateTime billdate, string user, string source)
    {
      Exception returnmsg;
      string errmsg;
      string sql;
      sql = string.Format("ProcessUndoMRCPost '{0}','{1}'", user, billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on MRC Undo Post.";
        return errmsg;
      }
      sql = string.Format("ProcessUndoOCCPost '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on OCC Undo Post.";
        return errmsg;
      }
      sql = string.Format("ProcessUndoTOLLPost '{0}','{1}'", user, billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error on Toll Undo Post.";
        return errmsg;
      }
      sql = string.Format(@"delete from hostedtaxtransactions where billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error removing tax transactions.";
        return errmsg;
      }
      sql = string.Format("Delete from artransactions where transactiontype = 'invoice' and billdate = '{0}'", billdate);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      if (returnmsg != null)
      {
        errmsg = "Error removing invoice transactions.";
        return errmsg;
      }
      returnmsg = insertAcctImportLog(user, "UnPost", null, billdate, source);
      if (returnmsg != null)
      {
        errmsg = "Error on updating log file.";
        return errmsg;
      }
      returnmsg = unProcessStep("Posted", billdate);
      if (returnmsg != null)
      {
        errmsg = "Error on updating process cycle.";
        return errmsg;
      }
      return "";
    }
    public Exception insertTaxDetail(DateTime billdate, string user)
    {
      Exception returnmsg;
     // string sql = string.Format(@"Insert into HostedTaxTransactions ([Customer],[MasterBTN],[Level],[TaxType],[Sign],[TaxAmount], 
     //                     [BillDate],[LastModifiedBy],[LastModifiedDateTime])
     //                 (Select [Customer],[MasterBTN],[Level],[TaxType],[Sign],[TaxAmount],'{0}','{1}','{2}' 
     //                     FROM [HostedImportTaxes] where BillDate = '{0}')", billdate, user, DateTime.Now);
      string sql = string.Format(@"Insert into HostedTaxTransactions ([Customer],[MasterBTN],[Level],[leveltype],[jurisdiction],[rate],[TaxType],[title],[TaxAmount], 
                          [BillDate],[sign],[LastModifiedBy],[LastModifiedDateTime])
                      (Select [Customer],[MasterBTN],[Level],[leveltype],[jurisdiction],[rate],[TaxType],[title],[TaxAmount],
                          '{0}',[sign],'{1}','{2}' 
                          FROM [HostedImportTaxes] where BillDate = '{0}')", billdate, user, DateTime.Now);
      returnmsg = updateDataFromSQLReturnErrorDescription(sql);
      return returnmsg;
    }
    public Exception processLedgerTransactions(DateTime billdate, string user)
    {
      string matchedBy = "PostedFromImport";
      string matchedon = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
      // first get rid of dups from last import
      // step one: find dups on transactiondate and customer and number of records and get rid of them from the "new" records. 
      string sqlelimdups = @"delete from hostedimportledger
                               FROM hostedimportledger a 
                                inner join (select distinct posted.customer, posted.transactiondate, posted.Count PostedCount, unposted.Count UnPostedCount from 
                                              (select customer, transactiondate, count(*) Count 
                                                 from hostedimportledger 
                                                 where matchedby is null
                                                 group by customer, transactiondate) unposted
                                              left join (select customer, transactiondate, count(*) Count 
                                                           from hostedimportledger 
                                                           where matchedby is not null
                                                           group by customer, transactiondate) posted 
                                              on unposted.customer = posted.customer and unposted.transactiondate = posted.transactiondate 
                                          where posted.customer is not null and posted.count = unposted.count ) b 
                                on a.customer = b.customer and a.transactiondate = b.transactiondate";
      string sql = sqlelimdups + " where a.matchedby is null";
      Exception ex = updateDataFromSQLReturnErrorDescription(sql);
      if (ex != null)
        return ex;
      // now get rid of dups on customer and transaction date from the old one where the counts don't match (new has more transactions than old)
      sql = sqlelimdups + " where a.matchedby is not null and b.PostedCount <> b.UnPostedCount";
      ex = updateDataFromSQLReturnErrorDescription(sql);
      if (ex != null)
        return ex;
      // now update the new records with matchedby
      sql = string.Format("update HostedImportLedger set MatchedBy = '{0}', MatchedDateTime = '{1}' where MatchedBy is null", matchedBy, matchedon);
      ex = updateDataFromSQLReturnErrorDescription(sql);
      if (ex != null)
        return ex;
      // finally, update the HostedLedgerTransactions table
      // first, clear out the ones for this bill date
      sql = string.Format("delete from HostedLedgerTransactions where BillDate = '{0}'", billdate.ToShortDateString());
      ex = updateDataFromSQLReturnErrorDescription(sql);
      if (ex != null)
        return ex;
      // then add the ones from the import
      sql = string.Format(@"  insert into HostedLedgerTransactions (CustomerID, BillDate, TransactionDate, TransactionType, Amount, AlternateID, LastModifiedBy, LastModifiedDateTime)
select e.Entity, l.BillDate, l.transactionDate, l.Title, l.Amount, l.Customer, '{0}', getdate() from HostedImportLedger l
left join Entity e on l.Customer = e.AlternateID and e.entityType = 'Customer'
where l.Billdate = '{2}'", user, matchedon, billdate.ToShortDateString());
      ex = updateDataFromSQLReturnErrorDescription(sql);
      return ex;
    }
    public DataSet getHostedMRCRetailStructure()
    {
      return getDataFromSQL("select * from HostedImportMRCRetail where 1 = 0");
    }
    public DataSet getTableStructure(string tablename)
    {
      return getDataFromSQL(string.Format("select * from {0} were 1=0", tablename));
    }
    public DataSet getBillingDataset(DateTime billingdate)
    {
      string billingmonth = Convert.ToString(billingdate.Month);
      string billingyear = Convert.ToString(billingdate.Year);
      string sql = string.Format(@"Select '{0}' as BillDate, 'NWHS-' + right(c.CustomerID,3) as CustomerID, 
          (select top 1 externalid as CustomerName from externalidmapping x where c.CustomerID = x.AlternateID and entitytype = 'QB') as CustomerName,
            isnull(m.[MRC Total],0) as [MRC Total], isnull(o.[OCC Total],0) as [OCC Total],
            isnull(t.[Toll Total],0) as [Toll Total], isnull(x.[Tax Total],0) as [Tax Total]
            from
          --Customer List
            (Select distinct x.AlternateID as CustomerID
                  from entity e
                   inner join externalidmapping x on e.entity = x.internalid
                  group by x.AlternateID) c
            left join
          --MRC Total
            (select mrc.AlternateID as CustomerID, sum(retailamount) as [MRC Total]
                  from hostedmatchedmrc mrc 
                 where retailbilldate = '{0}'
                 group by mrc.alternateid) m
             on c.customerID = m.customerID
            left join     
          --OCC Total  
            (select ot.customerid as CustomerID, sum(ot.retailamount) as [OCC Total]
              from 
	            (select alternateid as customerid, occ.retailamount as retailamount
			          from hostedmatchedocc occ 
		             where occ.retailbilldate = '{0}') ot
              group by ot.CustomerID) o
             on c.CustomerID = o.CustomerID
            left join
          --Toll Total  
            (select tt.customerid as CustomerID, sum(tt.retailcharge) as [Toll Total]
              from 
	            (select toll.alternateid as customerid, toll.retailcharge as retailcharge 
			          from hostedmatchedtoll toll
		             where toll.retailbillingmonth = '{1}' and toll.retailbillingyear = '{2}' and toll.retailcharge <> 0) tt
              group by tt.CustomerID) t
             on c.CustomerID = t.CustomerID
            left join
          --Tax Total
            (select tx.customer as CustomerID, sum(tx.taxamount) as [Tax Total]
              from 
                (select customer,case when [sign] = '-' then taxamount * -1 else taxamount end as taxamount
                  from hostedtaxtransactions tax
                  where tax.billdate = '{0}') tx
              group by tx.customer) x
             on c.CustomerID = x.CustomerID 
             where c.CustomerID is not null and ([mrc total] <> 0 or [occ total] <> 0 or [toll total] <> 0 or [tax total] <> 0)
             order by c.CustomerID", billingdate, billingmonth, billingyear);
      DataSet ds = getDataFromSQL(sql);
      return ds;
    }
    public string exportToExcel(DataSet ds)
    {
      string workbookPath = getexcelfile();
      if (workbookPath != null)
      {
        if (workbookPath.Substring(workbookPath.Length - 4, 4) != ".csv")
        {
          workbookPath = workbookPath + ".csv";
        }
        StreamWriter wr = new StreamWriter(workbookPath);

        string customerid = "";
        string outstring;
        foreach (DataTable dt in ds.Tables)
        {
          foreach (DataRow myRow in dt.Rows)
          {
            foreach (DataColumn myCol in dt.Columns)
            {
              if (myCol.ColumnName == "BillDate")
              {
                DateTime outdate = Convert.ToDateTime(myRow[myCol]);
                wr.Write(outdate.ToString("d") + ",");
              }
              else
              {
                if (myCol.ColumnName == "MRC Total" || myCol.ColumnName == "OCC Total" || myCol.ColumnName == "Toll Total")
                {
                  wr.Write(myRow[myCol] + ",");
                }
                else
                {
                  if (myCol.ColumnName == "Tax Total")
                  {
                    outstring = myRow[myCol] + "," + string.Format("\"{0}\"", customerid);
                    wr.Write(outstring);
                  }
                  else
                  {
                    outstring = string.Format("\"{0}\",",myRow[myCol]);
               //     string leadquote = "\"";
               //     string postpipequote = "\",";
               //     outstring = leadquote + outstring + postpipequote;
                    if (myCol.ColumnName == "CustomerName")
                    {
                      wr.Write(outstring);
                    }
                    else // CustomerID
                    {
                      customerid = Convert.ToString(myRow[myCol]);
                    }
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
      return workbookPath;
    }
    public Exception processFreezeDealerInvoice(string dealer, DateTime billdate)
    {
      string dealerwhere = null;
      if (string.IsNullOrEmpty(dealer) || dealer.Equals("All", StringComparison.CurrentCultureIgnoreCase))
        dealerwhere = string.Empty;
      else
        dealerwhere = string.Format("dealer = '{0}' AND", dealer);
      string sql = string.Format(@"insert into HostedDealerMarginHistory
select * from vw_DealerMargin where {0} billdate = '{1}'", dealerwhere, billdate);
      return updateDataFromSQLReturnErrorDescription(sql);
    }
    public Exception unprocessFreezeDealerInvoice(string dealer, DateTime billdate)
    {
      string dealerwhere = null;
      if (string.IsNullOrEmpty(dealer) || dealer.Equals("All",StringComparison.CurrentCultureIgnoreCase))
        dealerwhere = string.Empty;
      else
        dealerwhere = string.Format("dealer = '{0}' AND", dealer);
      string sql = string.Format(@"delete from HostedDealerMarginHistory where {0} billdate = '{1}'", dealerwhere, billdate);
      return updateDataFromSQLReturnErrorDescription(sql);
    }
    public bool hasDealerFrozenInvoice(string dealer, DateTime billdate)
    {
      string dealerwhere = null;
      if (string.IsNullOrEmpty(dealer) || dealer.Equals("All", StringComparison.CurrentCultureIgnoreCase))
        dealerwhere = string.Empty;
      else
        dealerwhere = string.Format("dealer = '{0}' AND", dealer);
      string sql = string.Format("select count(*) from hosteddealermarginhistory where {0} billdate = '{1}'", dealerwhere, billdate);
      using (DataSet ds = getDataFromSQL(sql))
      {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
          return false;
        return CommonFunctions.CInt(ds.Tables[0].Rows[0][0]) > 0;
      }
    }
    public string getDealerFrozenInvoiceState(DateTime billDate)
    {
      string sql = @"select h.BillDate, 
(select count(*) from entity where entitytype = 'dealer' and h.BillDate between isnull(startdate, '1/1/1900') and isnull(enddate, '12/31/2100')) as DealerCount, 
HistoryCount
--, case when HistoryCount = 0 then 'Unposted' when HistoryCount = DealerCount then 'Posted' else 'No' end DealerPosted 
from (select BillDate, count(*) historyCount from (select distinct dealer, BillDate from hosteddealermarginhistory group by Dealer, BillDate ) a
group by BillDate) h";
      using (DataSet ds = getDataFromSQL(string.Format("{0} WHERE BillDate = '{1}'", sql, billDate.ToShortDateString())))
      {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
          return "UnPosted";
        DataRow row = ds.Tables[0].Rows[0];
        int dlrCount = CommonFunctions.CInt(row["DealerCount"]);
        int histCount = CommonFunctions.CInt(row["HistoryCount"]);
        if (histCount == 0)
          return "UnPosted";
        if (histCount == dlrCount)
          return "Posted";
        return "Partial";
      }
    }
    public DataSet getImportSources()
    {
      const string sql = "SELECT * from ImportSources";
      return getDataFromSQL(sql);
    }
    public DataSet getImportFileTypes(string source)
    {
      string sql = string.Format("SELECT FileType, Prefix, Suffix, StoredProcedure, SkipLines from ImportFileTypes where Source = '{0}'", source);
      return getDataFromSQL(sql);
    }
    private static string getexcelfile()
    {
      Stream myStream;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();

      saveFileDialog1.Filter = "ALL files (*.csv)|*.csv";
      saveFileDialog1.FilterIndex = 2;
      saveFileDialog1.RestoreDirectory = true;
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        if ((myStream = saveFileDialog1.OpenFile()) != null)
        {
          myStream.Close();
          return saveFileDialog1.FileName;
        }
        return null;
      }
      return null;
    }

  }
}

