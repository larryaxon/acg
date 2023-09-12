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

namespace CCI.Sys.Processors
{
  public class InvoiceCreationProcessor : ImportFileProcessorBase
  {
    private const string APPSETTINGCREATIOIMPORTFOLDER = "CreatioImportFolder";
    private const string FILETYPECREATIONETWORKINVENTORY = "CreatioNetworkInventory";
    private string _creatioImportFolder = "\\InvoiceIQ\\Creatio\\";




    public InvoiceCreationProcessor() : base()
    {
      _creatioImportFolder = getAppSetting(APPSETTINGCREATIOIMPORTFOLDER, _creatioImportFolder);
      _importFileSpecs = new List<ImportFileSpecs>()
      {
        new ImportFileSpecs()
        {
          FileType = "NI",
          TableName = "[dbo].[CreatioNetworkInventory]",
          HeaderLine = "Number,Carrier,Service Location,Parent Account No.,Child Account No.,Product Nickname,MRC ($),Stage,Status",
          RepaceAllRecords = true,
          IsActive = true
        },
        new ImportFileSpecs()
        {
          FileType = "CreatioAudit",
          TableName = "[dbo].[CreatioBillAudit]",
          HeaderLine = "Number,Bill cycle date,Carrier invoice date,Order,Carrier,Location,Product,BAN,Parent,Child,Carrier charges to audit,Order.MRC,Variance (needs to be a calculated field),Total bill,First invoice1,Multi-Site Invoice,Ancillary charges,Comments,Dispute Pending,Dispute Notes,Stage,Status,Install Date,Building Type",
          RepaceAllRecords = false,
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

      using (ExcelProcessor excel = ExcelProcessor.CreateWorkbookFromDataset(ds, tabnames, tabSelectMap))
      {
        MemoryStream stream = excel.ToStream();
        return stream;
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

    private void ProcessFile(ACGFileInfo file)
    {
      List<string> textExtensions = new List<string>() { "txt", "csv" };
      List<string> excelExtensions = new List<string>() { "xlsx" };
      string fileType;
      ImportFileInfo fileinfo;

      string extension = Path.GetExtension(file.Name);
      if (textExtensions.Contains(extension, StringComparer.CurrentCultureIgnoreCase))
        fileinfo = ImportFile(file, out fileType);
      else if (excelExtensions.Contains(extension, StringComparer.CurrentCultureIgnoreCase))
        fileinfo = ImportExcelFile(file.FullName, out fileType);
      else return; // we can't import it if we don't know what kind it is
      ImportFileSpecs spec = _importFileSpecs.Where(s => s.FileType == fileType).FirstOrDefault();
      SaveImportFile(fileinfo, spec.TableName, spec.RepaceAllRecords);
    }
  }
}
