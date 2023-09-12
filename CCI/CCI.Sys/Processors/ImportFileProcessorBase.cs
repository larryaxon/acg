using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACG.Common;
using CCI.Sys.Data;
using OfficeOpenXml;
using static System.Net.WebRequestMethods;


namespace CCI.Sys.Processors
{
  public class ImportFileProcessorBase : IDisposable
  {

    const string APPSETTINGLOCALBASEFOLDER = "LocalBaseFolder";
    internal List<ACGFileInfo> _fileList = null;
    internal Dictionary<string, string> _dataTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
    {
      { "ACCT_LEVEL", "int" },
      { "ACTIVITY_COMP_DATE", "date" },
      { "AIR_CHG_AMT", "decimal(8,2)" },
      { "Ancillary_charges","bit" },
      { "BAL_FWD", "decimal(8,2)" },
      { "BAL_FWD_ADJ", "decimal(8,2)" },
      { "BEG_CHG_DATE", "date" },
      { "Bill_cycle_date", "date" },
      { "BILL_PERIOD_END", "date" },
      { "BILL_PERIOD_START", "date" },
      { "Carrier_invoice_date", "date" },
      { "CHG_AMT", "decimal(8,2)" },
      { "Carrier_charges_to_audit", "decimal(10,2)" },
      { "CHG_BASIS", "decimal(8,2)" },
      { "CHG_QTY1_BILLED", "decimal(8,2)" },
      { "CHG_QTY1_USED", "decimal(8,2)" },
      { "CHG_QTY2_BILLED", "decimal(8,2)" },
      { "CHG_QTY2_USED", "decimal(8,2)" },
      { "CHG_RATE", "decimal(8,2)" },
      { "CONTRACT_EFF_DATE", "date" },
      { "CONTRACT_END_DATE", "date" },
      { "DATA_CHG_AMT", "decimal(8,2)" },
      { "DATE_ISSUED", "date" },
      { "DATE_RECEIVED_FROM_SP", "date" },
      { "DISC_CHG_AMT", "decimal(8,2)" },
      { "DISC_PCT", "decimal(8,2)" },
      { "Dispute_Pending", "bit" },
      { "DUE_DATE" , "date" },
      { "END_CHG_DATE", "date" },
      { "FEAT_CHG_AMT", "decimal(8,2)" },
      { "First_invoice1", "bit" },
      { "Install_Date", "date" },
      { "INV_DATE", "date" },
      { "LD_CHG_AMT", "decimal(8,2)" },
      { "MRC ($)", "decimal(8,2)" },
      { "MSG_CHG_AMT", "decimal(8,2)" },
      { "Multi_Site_Invoice", "bit" },
      { "Order_MRC", "decimal(10,2)" },
      { "ORIG_INV_DATE", "date" },
      { "PMTS_APP_THRU_DATE", "date" },
      { "PMTS_RCVD", "decimal(8,2)" },
      { "PREV_BILL_AMT", "decimal(8,2)" },
      { "PRORATE_FACTOR", "decimal(8,2)" },
      { "ROAM_CHG_AMT", "decimal(8,2)" },
      { "ROAM_TAX_CHG_AMT", "decimal(8,2)" },
      { "SP_BAL_FWD", "decimal(8,2)" },
      { "SP_INV_LINE_NUM", "int" },
      { "SP_TOT_AMT_DUE", "decimal(8,2)" },
      { "SP_TOT_NEW_CHGS", "decimal(8,2)" },
      { "SVC_ESTABLISH_DATE", "date" },
      { "TAX_SUR_CHG_AMT", "decimal(8,2)" },
      { "Total_bill", "decimal(10,2)" },
      { "TOT_AMT_DUE", "decimal(8,2)" },
      { "TOT_AMT_DUE_ADJ", "decimal(8,2)" },
      { "TOT_DISC_AMT", "decimal(8,2)" },
      { "TOT_MRC_CHGS", "decimal(8,2)" },
      { "TOT_NEW_CHG_ADJ", "decimal(8,2)" },
      { "TOT_NEW_CHGS", "decimal(8,2)" },
      { "TOT_OCC_CHGS", "decimal(8,2)" },
      { "TOT_TAXSUR", "decimal(8,2)" },
      { "TOT_USAGE_CHGS", "decimal(8,2)" },
      { "USG_BAND", "decimal(8,2)" },
      { "Variance_needs_to_be_a_calculated_field", "decimal(10,2)" },
      { "FilesProcessedID", "int" }

    };
    internal string _localDirectory = "U:\\Data";
    #region public properties
    public List<ACGFileInfo> FileList
    {
      get
      {
        if (_fileList == null)
          _fileList = new List<ACGFileInfo>();
        return _fileList;
      }
    }
    public List<string> FileNameList
    {
      get
      {
        return FileList.Select(f => f.Name).ToList();
      }
    }
    public string LocalFolder
    {
      get { return _localDirectory; }
      set { _localDirectory = value; }
    }


    #endregion

    public class ImportFileSpecs
    {
      public string FileType { get; set; }
      public string TableName { get; set; }
      public string HeaderLine { get; set; }
      public bool RepaceAllRecords { get; set; } = false;
      public bool IsActive { get; set; } = true;
      public bool FixupHeaderNames { get; set; } = false;
      public List<string> HeaderList
      {
        get
        {
          try
          {
            List<string> list = HeaderLine.Split(',').ToList();
            return list;
          }
          catch
          {
            return new List<string>();
          }
        }
      }
    }
    internal List<ImportFileSpecs> _importFileSpecs = new List<ImportFileSpecs>();
    public ImportFileProcessorBase()
    {
      _localDirectory = getAppSetting(APPSETTINGLOCALBASEFOLDER, null);
      CommonData.SERVERCONFIGFILENAME = CommonData.SERVERCONFIGFILEDEFAULT; // db.config for now
    }
    public void Dispose()
    {
    }
    public List<ACGFileInfo> getFilesToProcess(string fileType = null)
    {
      List<string> processedFiles = getFilesProcessed(fileType);  
      List<ACGFileInfo> files = FileList.ToList();
      List<ACGFileInfo> filesToProcess = files.Where(ftp => !processedFiles.Contains(ftp.FullName)).ToList();
      return filesToProcess;
    }

    #region other protected methods
    internal object[] adjustFieldsForDataType(object[] fields, string[] columnNames)
    {
      // fixes up fields to be correct for data types
      object[] fldarray = fields.ToArray();
      string[] colarray = columnNames.ToArray();
      for (int i = 0; i < fldarray.Length; i++)
      {
        string col = colarray[i];
        string datatype = "string";
        if (_dataTypes.ContainsKey(col))
        {
          datatype = _dataTypes[col];
          if (datatype.StartsWith("decimal"))
            datatype = "decimal";
        }
        switch (datatype)
        {
          case "string":
            break;
          case "date":
            if (!CommonFunctions.IsDateTime(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CDateTime(fldarray[i]);
            break;
          case "decimal":
            if (!CommonFunctions.IsNumeric(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CDecimal(fldarray[i]);
            break;
          case "int":
            if (!CommonFunctions.IsInteger(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CInt(fldarray[i]);
            break;
          case "bit":
            if (!CommonFunctions.IsBoolean(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CBoolean(fldarray[i]) ? "true" : "false";
            break;
        }
      }
      return fldarray;
    }
    internal ImportFileInfo ImportFile(ACGFileInfo file, out string fileType)
    {

      ImportFileInfo importFile = new ImportFileInfo();
      importFile.filepath = Path.Combine(LocalFolder, file.Name);
      int fileProcessedID = 0;

      // Read the text file line by line
      using (StreamReader sr = new StreamReader(importFile.filepath))
      {
        string headerline = sr.ReadLine();
        if (!_importFileSpecs.Where(s => s.HeaderLine.Equals(headerline, StringComparison.CurrentCultureIgnoreCase)).Any())
          throw new Exception("Import File with Header Line " + headerline + " does not exist");
        ImportFileSpecs spec = _importFileSpecs.Where(s => s.HeaderLine.Equals(headerline, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        fileType = spec.FileType;
        using (DataAccess da = new DataAccess())
        {
          fileProcessedID = da.addFileProcessed(fileType, importFile.filepath, DateTime.Now, -1, false, "Import " + fileType);
        }
        if (spec.FixupHeaderNames)
          headerline = headerline.Replace(" ", "_").Replace("-","_");
        string[] headers = headerline.Split(',');
        string[] allheaders = new string[headers.Length + 1]; // add one for fileprocessedid
        Array.Copy(headers, 0, allheaders, 0, headers.Length);
        allheaders[headers.Length] = "FilesProcessedID";

        List<List<object>> theserecords = new List<List<object>>();
        string line;
        // Load the data from the csv into headers and collections of fieldvalues
        while ((line = sr.ReadLine()) != null)
        {
          string[] values = ACG.Common.CommonFunctions.parseString(line);
          object[] fieldvalues = new string[values.Length + 1];
          Array.Copy(values, 0, fieldvalues, 0, values.Length);
          // take any values that cannot be converted to the correct datatype and make them null
          fieldvalues = adjustFieldsForDataType(fieldvalues, allheaders);
          int fileprocessedidsub = fieldvalues.Length - 1;

          fieldvalues[fileprocessedidsub] = fileProcessedID; // so use the file processed it to uniquely identify this batch
          List<object> fields = new List<object>();
          fields.AddRange(fieldvalues);
          theserecords.Add(fields); // now add the new record (which is itself a list of fields) to the collection for this record type
        }
        importFile.Records.Add(fileType, theserecords);
        importFile.Headers.Add(fileType, allheaders.ToList());
        return importFile;

      }
    }
    internal  ImportFileInfo ImportExcelFile(string path, out string fileType)
    {
      using (var pck = new OfficeOpenXml.ExcelPackage())
      {
        using (var stream = System.IO.File.OpenRead(path))
        {
          pck.Load(stream);
        }
        var ws = pck.Workbook.Worksheets.First();
        DataTable tbl = new DataTable();
        List<string> headerlist = new List<string>();
        StringBuilder headers = new StringBuilder();
        List<List<object>> theserecords = new List<List<object>>();
        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
        {
          string header = firstRowCell.Text;
          headers.Append(header);
          headers.Append(",");
          headerlist.Add(header);
        }
        headers.Length--; //strip the last comman
        headerlist.Add("FilesProcessedID");
        string[] allheaders = new string[headers.Length + 1]; // add one for fileprocessedid
        Array.Copy(headerlist.ToArray(), 0, allheaders, 0, headers.Length);
        allheaders[headers.Length] = "FilesProcessedID";
        string headerline = headers.ToString();

        if (!_importFileSpecs.Where(s => s.HeaderLine.Equals(headerline, StringComparison.CurrentCultureIgnoreCase)).Any())
          throw new Exception("Import File with Header Line " + headerline + " does not exist");
        ImportFileSpecs spec = _importFileSpecs.Where(s => s.HeaderLine.Equals(headerline, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        fileType = spec.FileType;
        int startRow = 2;
        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
        {
          var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
          object[] values = new object[ws.Dimension.End.Column];
          for (int icol = 0; icol < wsRow.Columns; icol++)
            values[icol] = ws.Cells[rowNum, icol + 1].Value;
          object[] fieldvalues = new string[values.Length + 1];
          Array.Copy(values, 0, fieldvalues, 0, values.Length);
          fieldvalues = adjustFieldsForDataType(fieldvalues, allheaders);
          theserecords.Add(fieldvalues.ToList());
        }
        ImportFileInfo importFile = new ImportFileInfo()
        {
          filepath = path
        };
        importFile.Records.Add(fileType, theserecords);
        importFile.Headers.Add(fileType, allheaders.ToList());
        return importFile;
      }
    }

    internal void SaveImportFile(ImportFileInfo file, string tablename, bool replaceall = false)
    {
      {
        foreach (string rectype in file.Headers.Keys)
        {
          List<string> headers = file.Headers[rectype]; // now get the list of headers
          if (file.Records.ContainsKey(rectype)) // does the records collection have this record type?
          {
            // yes, so add the records to the db
            List<List<object>> records = file.Records[rectype]; // next get all the records with matching values
            saveTableFromFileData(tablename, headers, records, _dataTypes, true, replaceall);
          }
          // else
          //     No: do nothing
        }
      }
    }

    internal void saveTableFromFileData(string tablename, List<string> headers, 
      List<List<object>> records, Dictionary<string, string> datatypes, bool hasID, bool replaceall = false)
    {
      using (DataAccess da = new DataAccess())
      {
        if (replaceall) // this means delete the data before we add it again
        {
          string deletesql = "Truncate Table " + tablename;
          da.updateDataFromSQL(deletesql);
        }
        // all of these  files have an ID so we set the last parm to true
        DataSet ds = da.getDatasetFromDictionaryData(tablename, headers, records, _dataTypes, true);
        if (ds != null && ds.Tables.Count > 0)
        {
          DataTable dt = ds.Tables[0];
          string fileProcessedID = dt.Rows[0]["FilesProcessedID"].ToString();
          if (!CommonFunctions.IsInteger(fileProcessedID))
            fileProcessedID = "-1";
          Exception ex = da.insertDataTabletoSQL(tablename, dt);
          if (ex != null)
          {
            // update the files processed record with the error
            string sql = "Update FilesProcessed SET ErrorMessage = 'Table:" + dt.TableName + " Error=" + ex.Message + "', StackTrace = '" + ex.StackTrace + "' WHERE ID = " + fileProcessedID;
            da.updateDataFromSQL(sql);
            throw ex;
          }
        }
      }

    }

    protected string getAppSetting(string name, string defaultValue = "")
    {
      string val = ConfigurationManager.AppSettings[name];
      if (string.IsNullOrWhiteSpace(val))
        val = defaultValue;
      return val;
    }
    protected List<string> getFilesProcessed(string filetype)
    {
      List<string> fileNames = new List<string>();
      using (DataAccess db = new DataAccess())
      {
        DataSet ds = db.getFilesProcessed(filetype);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
          fileNames.Add(row["FileName"].ToString());
        return fileNames;
      }
    }
    #endregion protected
  }
}
