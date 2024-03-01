using ACG.Common;
using CCI.Sys.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OfficeOpenXml;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Net.WebRequestMethods;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.Web.Mvc;
using OfficeOpenXml.Table;
using OfficeOpenXml.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Status;
using System.Windows.Media;
using OfficeOpenXml.Style;
using System.Drawing;
using CCI.Common;
using System.Diagnostics;

namespace CCI.Sys.Processors
{
  public class InvoiceCreationProcessor : ImportFileProcessorBase
  {
    private const string APPSETTINGCREATIOIMPORTFOLDER = "CreatioImportFolder";
    private const string FILETYPECREATIONETWORKINVENTORY = "CreatioNetworkInventory";
    private const string PROCESSSTEPCYCLE = "CreatioBillAudit";
    private string _creatioImportFolder = "\\InvoiceIQ\\Creatio\\";
    private const string CREATIOBILLAUDITUPLOADTABLE = "CreatioAuditUpload";

    public const string FILETYPENETWORKINVENTORY = "NI";
    public const string FILETYPEIMPORTFROMCREATIO = "CreatioAudit";
    public const string FILETYPEEDITEDUPLOAD = CREATIOBILLAUDITUPLOADTABLE;
    public const string FILESPROCESSEDFILETYPEINVOICEIQ = "InvoiceIQ";
    public const string FILESPROCESSEDFILETYPEUNIBILL = "Unibill";

    public InvoiceCreationProcessor() : base()
    {
      _creatioImportFolder = getAppSetting(APPSETTINGCREATIOIMPORTFOLDER, _creatioImportFolder);
      _importFileSpecs = new List<ImportFileSpecs>()
      {
        new ImportFileSpecs()
        {
          FileType = FILETYPENETWORKINVENTORY,
          TableName = "[dbo].[CreatioNetworkInventory]",
          HeaderLine =  "Number,Carrier,Service Location,Parent Account No.,Child Account No.,Product Nickname,MRC ($),Stage,Status" ,
          RepaceAllRecords = true,
          IsActive = true
        },
        new ImportFileSpecs()
        {
          FileType = FILETYPEIMPORTFROMCREATIO,
          TableName = "[dbo].[CreatioBillAudit]",
          HeaderLine =   "Number,Bill cycle date,Carrier invoice date,Order,Carrier,Building Type,Location Nickname,Location,Product,BAN,Parent,Child,Carrier charges to audit,Order.MRC,Variance (needs to be a calculated field),Total bill,First invoice1,Multi-Site Invoice,Ancillary charges,Comments,Dispute Pending,Dispute Notes,Stage,Status",
          //HeaderLine =   "Number,Bill cycle date,Carrier invoice date,Order,Carrier,Location,Product,BAN,Parent,Child,Carrier charges to audit,Order.MRC,Variance (needs to be a calculated field),Total bill,First invoice1,Multi-Site Invoice,Ancillary charges,Comments,Dispute Pending,Dispute Notes,Stage,Status,Install Date,Building Type",
          RepaceAllRecords = false,
          CheckForDups = true,
          UniqueKeys = new List<string>() { "[Number]" },
          IsActive = true,
          FixupHeaderNames = true
        },
        new ImportFileSpecs()
        {
          FileType = FILETYPEEDITEDUPLOAD,
          TableName = "[dbo].[CreatioBillAuditUpload]",
          HeaderLine =   "Number,Invoice Date,Creatio ID,Carrier,Building Type,Location Nickname,Location,Product,Parent,Child,Carrier Charges to Audit,R P M Control Charges,Variance,Total Bill,First Invoice,Multi- Site Invoice,Ancillary Charges?,Comment,Dispute Pending,Dispute Notes,Has E D IData",
          RepaceAllRecords = false,
          CheckForDups = true,
          IsActive = true,
          ForcePreprocess = true,
          FixupHeaderNames = true
        }
      };
      _localDirectory = _localDirectory + _creatioImportFolder;
    }
    public List<string> ImportCreatioFiles(string fileType = null, DateTime? billCycleDate = null, string filename = null)
    {
      return ProcessFiles(fileType, billCycleDate, filename);
    }
    public List<ACGFileInfo> GetFileList(string directory = null)
    {
      if (directory == null)
        directory = LocalFolder;
      if (_fileList == null)
        _fileList = new List<ACGFileInfo>();
      else
        _fileList.Clear();

      foreach (string filepath in Directory.GetFiles(directory))
      {
        _fileList.Add(new ACGFileInfo()
        {
          Name = Path.GetFileName(filepath),
          FullName = filepath,
          IsDirectory = false

        });
      }
      return _fileList;
    }
    public List<string> ListUnprocessedImportFiles()
    {
      GetFileList(LocalFolder);
      List<ACGFileInfo> filesToProcess = getFilesToProcess();
      List<string> filelist = filesToProcess.Select(f => f.Name).ToList();
      return filelist;
    }
    public List<string> ProcessFiles(string fileType = null, DateTime? billCycleDate = null, string filename = null)
    {
      GetFileList(LocalFolder);
      List<ACGFileInfo> filesToProcess = getFilesToProcess(fileType, true, filename);
      foreach (ACGFileInfo file in filesToProcess)
        ProcessFile(file, fileType, billCycleDate);
      return filesToProcess.Select(f => f.Name).ToList();
    }
    public MemoryStream GetCreatioInvoice(DateTime billCycleDate)
    {
      //fromDate = fromDate.AddMonths(-1);
      //toDate = toDate.AddMonths(-1);
      Dictionary<string, object> data = new Dictionary<string, object>() { {"@BillCycleDate", billCycleDate } };
      DataSet ds = GetProcReportDataSetFromQuery("Exec CreatioAuditReport", data);
      List<string> tabnames = new List<string>() { "Audit", "Understanding your Audit" };
      Dictionary<int, List<int>> tabSelectMap = new Dictionary<int, List<int>>()
      {
        { 1, new List<int>() { 1 } },
        { 2, new List<int>() { 2 } }
      };
      /*
       * Total columns for the spreadsheet. 
       * Tab 0, Table 0, columns 7-10 (zero based)
       */
      Dictionary<int, Dictionary<int, List<int>>> totalcolumns = 
        new Dictionary<int, Dictionary<int, List<int>>>() 
        {
          { 0, new Dictionary<int, List<int>>() { { 0, new List<int>() { 7,8,9,10 } } } }
        };
      // explicitly format the first column of the first table as date
      Dictionary<int, Dictionary<string, List<int>>> formats = new Dictionary<int, Dictionary<string, List<int>>>()
      {
        { 1, new Dictionary<string, List<int>>()
             {
               { "Date", new List<int>() { 2 }  },
               { "Decimal", new List<int>() { 9,10,11,12}  }
              }
        }
      };
      using (ExcelProcessor excel = ExcelProcessor.CreateWorkbookFromDataset(ds, tabnames, tabSelectMap, formats, totalcolumns))
      {
        // now that we have the spreadsheet, we need to add some data and formatting
        string logopath = ConfigurationManager.AppSettings["CityCareLogoPath"];
        excel.AddImage("CityCareLogo", logopath, 0, "A1");
        /*
            Customer:	        Carvana LLC
            Invoice Audit:	5/16/23 - 06/15/23
        */
        // calc the from/to dates from the cycle date
        DateTime fromDate = billCycleDate.AddMonths(-1); // go back a month
        int month = fromDate.Month;
        int year = fromDate.Year;
        fromDate = new DateTime(year, month, 16); // 16th of the month prior
        DateTime toDate = fromDate.AddMonths(1).AddDays(-1); // 15th of the bcd month

        string daterange = fromDate.ToShortDateString() + " - " + toDate.ToShortDateString();
        int maintab = 0;
        int startingrow = 2;
        int startingcol = 3;
        excel.SetCellValue(maintab, startingrow, startingcol, "Customer:");
        excel.SetCellFormat(maintab, startingrow, startingcol, "bold");
        excel.SetCellValue(maintab, startingrow, startingcol + 1, "Carvana LLC");
        excel.SetCellFormat(maintab, startingrow, startingcol + 1, "bold");
        excel.SetCellFormat(maintab, startingrow, startingcol + 1, "center");
        excel.SetCellValue(maintab, startingrow + 1, startingcol, "Invoice Audit:");
        excel.SetCellFormat(maintab, startingrow + 1, startingcol, "bold");
        excel.SetCellValue(maintab, startingrow + 1, startingcol + 1, daterange);
        excel.SetCellFormat(maintab, startingrow + 1, startingcol + 1, "bold");
        excel.SetCellFormat(maintab, startingrow + 1, startingcol + 1, "center");
        // change Variance to calcualted column
        ExcelWorksheet ws = excel.Workbook.Worksheets[0];
        ExcelTable exceltable = ws.Tables[0];
        int varianceCol = 13;
        // get the range of the column of data for the variance. Not4 we add 1 to the row cause we don't want the header
        ExcelRange excelRange = ws.Cells[exceltable.Range.Start.Row+1, varianceCol, exceltable.Range.End.Row-1, varianceCol];
        excelRange.Formula = string.Format("{0}-{1}", ws.Cells[exceltable.Range.Start.Row + 1, varianceCol-2].Address, ws.Cells[exceltable.Range.Start.Row + 1, varianceCol-1].Address);

        //calculate the formulas
        //ws.Calculate();
        MemoryStream stream = excel.ToStream();
        return stream;
      }


    }
    public MemoryStream GetEDIData(DateTime billCycleDate)
    {
      DataSet ds = GetEDIDataSet(billCycleDate);
      Dictionary<int, Dictionary<string, List<int>>> formats = new Dictionary<int, Dictionary<string, List<int>>>()
      {
        { 1, new Dictionary<string, List<int>>()
             {
               { "Date", new List<int>() { 8,9,10,11,12,15 }  },
               { "Decimal", new List<int>() { 13,14,16,17,18,19,20,21,22,23,24,25,26 }  }
              }
        }
      };
      using (ExcelProcessor excel = ExcelProcessor.CreateWorkbookFromDataset(ds, null, null, formats))
      {
        return excel.ToStream();
      }

    }
    public MemoryStream getCreationAuditExport(DateTime billCycleDate)
    {
      using (DataAccess da = new DataAccess())
      {
        DataSet ds = da.getCreationAuditExport(billCycleDate);
        //ds = adjustFieldsForDataType(ds);
        List<string> tabnames = new List<string>() { "Bill Audit " };
        Dictionary<int, Dictionary<string, List<int>>> formats = new Dictionary<int, Dictionary<string, List<int>>>()
        {
          { 
            1, new Dictionary<string, List<int>>()
            {
              {  "Date", new List<int>() { 4,5 } }
            }
          }
        };

        using (ExcelProcessor excel = ExcelProcessor.CreateWorkbookFromDataset(ds, tabnames, null, formats))
        {
          ExcelWorksheet ws = excel.Workbook.Worksheets[0];
          ExcelRange headerline = ws.Cells[1, 1, 1, ws.Dimension.Columns];
          headerline.Style.Fill.PatternType = ExcelFillStyle.Solid;
          headerline.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#5B9BD5"));
          headerline.Style.Font.Color.SetColor(System.Drawing.Color.White);
          headerline.Style.Font.Bold = true;
          headerline.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
          ws.Row(1).Height = 40;
          int nbrcols = ws.Dimension.Columns;
          int nbrrows = ws.Dimension.Rows;
          ExcelRange table = ws.Cells[1, 1, nbrrows, nbrcols];
          table.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
          for (int irow = 2; irow <= nbrrows; irow++)
            ws.Row(irow).Height = 30;
          MemoryStream stream = excel.ToStream();
          return stream;
        }
      }
    }

    public DataSet GetProcReportDataSetFromQuery(string procname, Dictionary<string, object> parms, int maxrecords = -1)
    {
      StringBuilder sql = new StringBuilder();
      if (!procname.TrimStart().StartsWith("exec ", StringComparison.CurrentCultureIgnoreCase))
        sql.Append("EXEC ");
      sql.Append(procname);
      if (parms != null)
      {
        sql.Append(' ');
        foreach (KeyValuePair<string, object> parm in parms)
        {
          if (!parm.Key.StartsWith("@"))
            sql.Append("@"); // flag the parm as a proc parm
          sql.Append(parm.Key);
          sql.Append(" = ");
          string val;
          if (parm.Value == null)
            val = "null";
          else
          {
            Type t = parm.Value.GetType();
            // if this is a string or a date, enclose in quotes (but only of there are not already enclosing quotes)
            if ((t == typeof(string) || t == typeof(DateTime)) && !parm.Value.ToString().StartsWith("'"))
              val = "'" + parm.Value.ToString() + "'";
            else if (t == typeof(bool))
              val = ((bool)parm.Value) ? "1" : "0";
            else
              val = parm.Value.ToString();
          }
          sql.Append(val);
          sql.Append(",");
        }
        sql.Length--; // strip last comma
      }
      using (DataAccess da = new DataAccess())
      {
        return da.GetDataFromSQL(sql.ToString());
      }
    }
    private DataSet GetEDIDataSet(DateTime billCycleDate)
    {
      int year = billCycleDate.Year;
      int month = billCycleDate.Month;

      int day = billCycleDate.Day;
      DateTime endDate = new DateTime(year, month, 15);
      DateTime startDate = new DateTime(year, month, 16).AddMonths(-1);

      string sql = "SELECT * from unibillfacepage where INV_Date between '" + startDate.ToShortDateString() + 
        "' AND '" + endDate.ToShortDateString() + "'";
      using (DataAccess da = new DataAccess())
      {
        return da.GetDataFromSQL(sql);
      }
    }
    public string AddCodes()
    {
      using (DataAccess da = new DataAccess())
      {
        using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
        {
          da.AddCodeMasterValues("DataDictionary", processor._dataTypes);

        }
      }
      return "Success";


    }
    private void ProcessFile(ACGFileInfo file, string fileType = null, DateTime? billCycleDate = null)
    {
      List<string> textExtensions = new List<string>() { ".txt", ".csv" };
      List<string> excelExtensions = new List<string>() { ".xlsx" };
      ImportFileInfo fileinfo;
      string thisFilesType;
      string extension = Path.GetExtension(file.Name);
      if (textExtensions.Contains(extension, StringComparer.CurrentCultureIgnoreCase))
        fileinfo = ImportFile(file, out thisFilesType, fileType);
      else if (excelExtensions.Contains(extension,StringComparer.CurrentCultureIgnoreCase))
        fileinfo = ImportExcelFile(file.FullName, out thisFilesType, fileType);
      else return; // we can't import it if we don't know what kind it is
      if (fileinfo == null) // we didn't process this file, probably cause it was not the one we asked for
        return;
      ImportFileSpecs spec = _importFileSpecs.Where(s => s.FileType == thisFilesType).FirstOrDefault();
      SaveImportFile(fileinfo, spec.TableName, spec.RepaceAllRecords, spec.CheckForDups, spec.UniqueKeys, spec.ForcePreprocess, billCycleDate);
      if (spec.FileType == CREATIOBILLAUDITUPLOADTABLE)
        SaveUploadToCreatioBillAudit(fileinfo);
    }
    public void SaveUploadToCreatioBillAudit(ImportFileInfo fileinfo)
    {
      // first we need the bill cycle date
      if (fileinfo.Records == null || fileinfo.Records.Count == 0 || !fileinfo.Records.ContainsKey(CREATIOBILLAUDITUPLOADTABLE))
        return;
      object[] records = fileinfo.Records[CREATIOBILLAUDITUPLOADTABLE].First().ToArray();
      DateTime invoiceDate = ACG.Common.CommonFunctions.CDateTime(records[2]); // 3rd column is invoice date
      DateTime billCycleDate = CalculateBillCycleDate(invoiceDate);
      // and finally exec the proc to copy the data to the main table
      using (DataAccess da = new DataAccess())
      {
        da.SaveUploadToCreatioBillAudit(billCycleDate);
      }
    }
    public static DateTime CalculateBillCycleDate(DateTime dt)
    {
      // now find the bill cycle date
      int day = dt.Day;
      int month = dt.Month;
      int year = dt.Year;
      if (day >= 16) // prior month
        month = month + 1;
      if (month > 12)
      {
        month = 1;
        year++;
      }
      DateTime billCycleDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
      return billCycleDate;
    }
    public List<ProcessStepsModel> getProcessStepsList()
    {
      using (DataSource da = new DataSource())
      {
        return da.getProcessStepsList(PROCESSSTEPCYCLE);
      }
    }
    public List<BillCycleModel> getThisBillCycle(DateTime billCycleDate)
    {
      using (DataSource da = new DataSource())
      {
        return da.getThisBillCycle( billCycleDate);
      }
    }
    public static int? processStep(string step, DateTime billDate, bool processed, string user)
    {
      using (DataSource da = new DataSource())
      {
        return da.processStep(step, billDate, processed, user);
      }
    }
    public List<CreatioEDIMatchingModel> getEDIMatching()
    {
      string sql = "Select * from CreatioEDIMatching";
      using (DataSource da = new DataSource())
      {
        DataSet ds = da.getDataFromSQL(sql);
        List<CreatioEDIMatchingModel> list = DataSource.getListFromDataset<CreatioEDIMatchingModel>(ds);
        return list;
      }

    }
    public CreatioEDIMatchingModel getEDIMatchingFromID(int id)
    {
      string sql = "Select * from CreatioEDIMatching where ID = " + id.ToString();
      using (DataSource da = new DataSource())
      {
        DataSet ds = da.getDataFromSQL(sql);
        CreatioEDIMatchingModel model = DataSource.getListFromDataset<CreatioEDIMatchingModel>(ds).FirstOrDefault();
        return model;
      }
    }
    public void DeleteEDIMatching(int id)
    {
      string sql = "DELETE from CreatioEDIMatching where ID = " + id.ToString();
      using (DataSource da = new DataSource())
      {
        da.updateDataFromSQL(sql);
      }
    }
    public int? UpdateEDIMatching(CreatioEDIMatchingModel model)
    {
      const string INSERTSQL = @"INSERT INTO CreatioEDIMatching ([Carrier]
,[AuditParent]
,[AuditChild]
,[EDIParent]
,[EDIChild]
,[ConsolidateToParent])
VALUES ('{0}','{1}','{2}','{3}','{4}',{5})";

      const string UPDATESQL = @"UPDATE CreatioEDIMatching
SET Carrier = '{0}',
AuditParent = '{1}',
AuditChild = '{2}',
EDIParent = '{3}',
EDIChild = '{4}',
ConsolidateToParent = {5}
WHERE ID = {6}";
      if (model == null)
        return null;
      string sql;
      if (model.ID == null) // it is a new record
        sql = string.Format(INSERTSQL, model.Carrier, model.AuditParent, model.AuditChild, model.EDIParent, model.EDIChild,
          model.ConsolidateToParent == null || !(bool)model.ConsolidateToParent ? "0" : "1");
      else
        sql = string.Format(UPDATESQL, model.Carrier, model.AuditParent, model.AuditChild, model.EDIParent, model.EDIChild,
          model.ConsolidateToParent == null || !(bool)model.ConsolidateToParent ? "0" : "1", model.ID.ToString());
      using (DataSource da = new DataSource())
      {
        return da.updateDataFromSQL(sql);
      }
    }
  }
}
