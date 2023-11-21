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

namespace CCI.Sys.Processors
{
  public class InvoiceCreationProcessor : ImportFileProcessorBase
  {
    private const string APPSETTINGCREATIOIMPORTFOLDER = "CreatioImportFolder";
    private const string FILETYPECREATIONETWORKINVENTORY = "CreatioNetworkInventory";
    private string _creatioImportFolder = "\\InvoiceIQ\\Creatio\\";
    private const string CREATIOBILLAUDITUPLOADTABLE = "CreatioAuditUpload";

    public InvoiceCreationProcessor() : base()
    {
      _creatioImportFolder = getAppSetting(APPSETTINGCREATIOIMPORTFOLDER, _creatioImportFolder);
      _importFileSpecs = new List<ImportFileSpecs>()
      {
        new ImportFileSpecs()
        {
          FileType = "NI",
          TableName = "[dbo].[CreatioNetworkInventory]",
          HeaderLine =  "Number,Carrier,Service Location,Parent Account No.,Child Account No.,Product Nickname,MRC ($),Stage,Status" ,
          RepaceAllRecords = true,
          IsActive = true
        },
        new ImportFileSpecs()
        {
          FileType = "CreatioAudit",
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
          FileType = "CreatioAuditUpload",
          TableName = "[dbo].[CreatioBillAuditUpload]",
          HeaderLine =   "Number,Invoice Date,Creatio ID,Carrier,Building Type,Location Nickname,Location,Product,Parent,Child,Carrier Charges to Audit,R P M Control Charges,Variance,Total Bill,First Invoice,Multi- Site Invoice,Ancillary Charges?,Comment,Dispute Pending,Dispute Notes,Has E D IData",
          RepaceAllRecords = false,
          CheckForDups = false,
          IsActive = true,
          FixupHeaderNames = true
        }
      };
      _localDirectory = _localDirectory + _creatioImportFolder;
    }
    public List<string> ImportCreatioFiles()
    {
      //List<ACGFileInfo> files = GetFileList();
      return ProcessFiles();
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
    public List<string> ProcessFiles()
    {
      GetFileList(LocalFolder);
      List<ACGFileInfo> filesToProcess = getFilesToProcess();
      foreach (ACGFileInfo file in filesToProcess)
        ProcessFile(file);
      return filesToProcess.Select(f => f.Name).ToList();
    }
    public MemoryStream GetCreatioInvoice(DateTime fromDate, DateTime toDate)
    {
      Dictionary<string, object> data = new Dictionary<string, object>() { {"@FromDate", fromDate }, { "@ToDate", toDate  } };
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
    private void ProcessFile(ACGFileInfo file)
    {
      List<string> textExtensions = new List<string>() { ".txt", ".csv" };
      List<string> excelExtensions = new List<string>() { ".xlsx" };
      string fileType;
      ImportFileInfo fileinfo;

      string extension = Path.GetExtension(file.Name);
      if (textExtensions.Contains(extension, StringComparer.CurrentCultureIgnoreCase))
        fileinfo = ImportFile(file, out fileType);
      else if (excelExtensions.Contains(extension, StringComparer.CurrentCultureIgnoreCase))
        fileinfo = ImportExcelFile(file.FullName, out fileType);
      else return; // we can't import it if we don't know what kind it is
      ImportFileSpecs spec = _importFileSpecs.Where(s => s.FileType == fileType).FirstOrDefault();
      SaveImportFile(fileinfo, spec.TableName, spec.RepaceAllRecords, spec.CheckForDups, spec.UniqueKeys);
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
      // now find the bill cycle date
      int day = invoiceDate.Day;
      int month= invoiceDate.Month;
      int year= invoiceDate.Year;
      if (day >= 16) // prior month
        month = month + 1;
      if (month > 12)
      {
        month = 1;
        year++;
      }
      DateTime billCycleDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
      // and finally exec the proc to copy the data to the main table
      using (DataAccess da = new DataAccess())
      {
        da.SaveUploadToCreatioBillAudit(billCycleDate);
      }
    }
  }
}
