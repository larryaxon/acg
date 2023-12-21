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
    public const string CREATIOBILLAUDITIMPORTTABLE = "CreatioBillAuditImport";
    public const string CREATIOBILLAUDITIMPORTTABLEUPLOAD = "CreatioBillAuditUploadImport";
    internal List<ACGFileInfo> _fileList = null;
    internal Dictionary<string, string> _dataTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    // changed to get from codemaster
    //{
    //  { "ACCT_LEVEL", "int" },
    //  { "ACTIVITY_COMP_DATE", "date" },
    //  { "AIR_CHG_AMT", "decimal(8,2)" },
    //  { "Ancillary_charges","bit" },
    //  { "Ancillary_Charges?","bit" },
    //  { "BAL_FWD", "decimal(8,2)" },
    //  { "BAL_FWD_ADJ", "decimal(8,2)" },
    //  { "BEG_CHG_DATE", "date" },
    //  { "Bill_cycle_date", "date" },
    //  { "BILL_PERIOD_END", "date" },
    //  { "BILL_PERIOD_START", "date" },
    //  { "Carrier_invoice_date", "date" },
    //  { "CHG_AMT", "decimal(8,2)" },
    //  { "Carrier_charges_to_audit", "decimal(10,2)" },
    //  { "CHG_BASIS", "decimal(8,2)" },
    //  { "CHG_QTY1_BILLED", "decimal(8,2)" },
    //  { "CHG_QTY1_USED", "decimal(8,2)" },
    //  { "CHG_QTY2_BILLED", "decimal(8,2)" },
    //  { "CHG_QTY2_USED", "decimal(8,2)" },
    //  { "CHG_RATE", "decimal(8,2)" },
    //  { "CONTRACT_EFF_DATE", "date" },
    //  { "CONTRACT_END_DATE", "date" },
    //  { "DATA_CHG_AMT", "decimal(8,2)" },
    //  { "DATE_ISSUED", "date" },
    //  { "DATE_RECEIVED_FROM_SP", "date" },
    //  { "DISC_CHG_AMT", "decimal(8,2)" },
    //  { "DISC_PCT", "decimal(8,2)" },
    //  { "Dispute_Pending", "bit" },
    //  { "DUE_DATE" , "date" },
    //  { "END_CHG_DATE", "date" },
    //  { "FEAT_CHG_AMT", "decimal(8,2)" },
    //  { "First_invoice1", "bit" },
    //  { "First_invoice", "bit" },
    //  { "Has_E_D_IData","bit"},
    //  { "Install_Date", "date" },
    //  { "INV_DATE", "date" },
    //  { "Invoice_Date", "exceldate" },
    //  { "LD_CHG_AMT", "decimal(8,2)" },
    //  { "MRC ($)", "decimal(8,2)" },
    //  { "MSG_CHG_AMT", "decimal(8,2)" },
    //  { "Multi__Site_Invoice", "bit" },
    //  { "Order_MRC", "decimal(10,2)" },
    //  { "ORIG_INV_DATE", "date" },
    //  { "PMTS_APP_THRU_DATE", "date" },
    //  { "PMTS_RCVD", "decimal(8,2)" },
    //  { "PREV_BILL_AMT", "decimal(8,2)" },
    //  { "PRORATE_FACTOR", "decimal(8,2)" },
    //  { "ROAM_CHG_AMT", "decimal(8,2)" },
    //  { "ROAM_TAX_CHG_AMT", "decimal(8,2)" },
    //  { "R_P_M_Control_Charges","decimal(10,2)"},
    //  { "SP_BAL_FWD", "decimal(8,2)" },
    //  { "SP_INV_LINE_NUM", "int" },
    //  { "SP_TOT_AMT_DUE", "decimal(8,2)" },
    //  { "SP_TOT_NEW_CHGS", "decimal(8,2)" },
    //  { "SVC_ESTABLISH_DATE", "date" },
    //  { "TAX_SUR_CHG_AMT", "decimal(8,2)" },
    //  { "Total_bill", "decimal(10,2)" },
    //  { "TOT_AMT_DUE", "decimal(8,2)" },
    //  { "TOT_AMT_DUE_ADJ", "decimal(8,2)" },
    //  { "TOT_DISC_AMT", "decimal(8,2)" },
    //  { "TOT_MRC_CHGS", "decimal(8,2)" },
    //  { "TOT_NEW_CHG_ADJ", "decimal(8,2)" },
    //  { "TOT_NEW_CHGS", "decimal(8,2)" },
    //  { "TOT_OCC_CHGS", "decimal(8,2)" },
    //  { "TOT_TAXSUR", "decimal(8,2)" },
    //  { "TOT_USAGE_CHGS", "decimal(8,2)" },
    //  { "USG_BAND", "decimal(8,2)" },
    //  { "Variance_needs_to_be_a_calculated_field", "decimal(10,2)" },
    //  { "Variance", "decimal(10,2)" },
    //  { "FilesProcessedID", "int" }

    //};
    internal static string _localDirectory = "U:\\Data";
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
    public static string LocalFolder
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
      public int HeaderRow { get; set; } = 1;
      public bool RepaceAllRecords { get; set; } = false;
      public bool CheckForDups { get; set; } = false;
      public List<string> UniqueKeys { get; set; } = null;
      public bool IsActive { get; set; } = true;
      public bool ForcePreprocess {  get; set; } = false;
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
      using (DataAccess da = new DataAccess())
      {
        _dataTypes = da.GetDataDictionary();
      }
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
    internal DataSet adjustFieldsForDataType(DataSet ds)
    {
      if (ds == null || ds.Tables.Count == 0)
        return ds;
      DataTable dt = ds.Tables[0];
      dt = adjustFieldsForDataType(dt);
      return ds;
    }
    internal DataTable adjustFieldsForDataType(DataTable dt)
    {
      if (dt == null)
        return dt;
      if (dt.Rows.Count == 0)
        return dt;
      List<string> cols = new List<string>();
      foreach (DataColumn c in dt.Columns)
        cols.Add(c.ColumnName);
      string[] colsarray = cols.ToArray();
      foreach (DataRow row in dt.Rows)
      {
        object[] vals = new object[dt.Columns.Count];
        for (int icol = 0; icol < vals.Length; icol++)
          vals[icol] = row[icol];
        vals = adjustFieldsForDataType(vals, colsarray);
        for (int icol = 0; icol < vals.Length; icol++)
          row[icol] = vals[icol];
      }
      return dt;
    }
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
          case "exceldate":
            fldarray[i] = CommonFunctions.CDateTimeFromExcel(fldarray[i], DateTime.Now);
            break;
          case "date":
            if (!CommonFunctions.IsDateTime(fldarray[i]))
            {
              if (CommonFunctions.IsNumeric(fldarray[i])) // its an excel date
                fldarray[i] = CommonFunctions.CDateTimeFromExcel(fldarray[i], DateTime.Now);
              else
                fldarray[i] = null;
            }
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
    internal ImportFileInfo ImportFile(ACGFileInfo file, out string fileType, string selectedFileType = null)
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
        if (selectedFileType != null && !fileType.Equals(selectedFileType, StringComparison.CurrentCultureIgnoreCase))
          return null;
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
    internal  ImportFileInfo ImportExcelFile(string path, out string fileType, string selectedFileType = null)
    {
      using (ExcelProcessor excel = new ExcelProcessor())
      {
        int fileProcessedID = -1;
        ExcelPackage pck = excel.LoadSpreadsheetFromFile(path);
        var ws = pck.Workbook.Worksheets.First();
        DataTable tbl = new DataTable();
        List<string> headerlist = new List<string>();
        StringBuilder headers = new StringBuilder();
        List<List<object>> theserecords = new List<List<object>>();
        // find header row
        int headerrow = 1;
        for (int irow = 1; irow < 20; irow++ ) // Surely the header rown can't be more than 20 rows down
        {
          object val = ws.Cells[irow, 1].Value;
          if (val != null && !string.IsNullOrWhiteSpace(val.ToString()))
          {
            headerrow = irow;
            break;
          }
        }
        foreach (var firstRowCell in ws.Cells[headerrow, 1, headerrow, ws.Dimension.End.Column])
        {
          string header = firstRowCell.Text;
          headers.Append(header);
          headers.Append(",");
          headerlist.Add(header);
        }
        headers.Length--; //strip the last comman
        headerlist.Add("FilesProcessedID");

        string headerline = headers.ToString();
        string headercompare = headerline.Substring(0, 35).Replace("_"," ");
        if (!_importFileSpecs.Where(s => s.HeaderLine.StartsWith(headercompare, StringComparison.CurrentCultureIgnoreCase)).Any())
          throw new Exception("Import File with Header Line " + headerline + " does not exist");
        ImportFileSpecs spec = _importFileSpecs.Where(s => s.HeaderLine.StartsWith(headercompare, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        fileType = spec.FileType;
        if (selectedFileType != null && !fileType.Equals(selectedFileType, StringComparison.CurrentCultureIgnoreCase))
          return null;
        if (spec.FixupHeaderNames)
        {
          headerline = headerline.Replace(" ", "_").Replace("-", "_");
          List<string> newheaders = new List<string>();
          foreach (string header in headerlist)
          {
            newheaders.Add(header.Replace(" ", "_").Replace("-", "_"));
          }
          headerlist = newheaders;
        }
        //List<string> commentsToExclude = new List<string>()
        //{
        //  "Final invoice, charged for $100.00 non returned equipment",
        //  "RPM 5365 and 5575 are same invoice but separated by product type.",
        //  "RPM 5365 and 5575 are same invoice but separated by product type.",
        //  "Final invoice, charged $100 non returned equipment.  Credit to be mailed for -$47.49.",
        //  "RPM 5531 and 6317 are same invoice but separated by product type",
        //  "RPM 5531 and 6317 are same invoice but separated by product type",
        //  "Final invoice, charged $100 non returned equipment.","Charges include RPM 07095","Includes credit of $835 for free month",
        //  "This is split into 2 charges on the invoice $3400 and $726.18"
        //};
        string[] allheaders = new string[headerlist.Count]; // add one for fileprocessedid
        Array.Copy(headerlist.ToArray(), 0, allheaders, 0, headerlist.Count);
        using (DataAccess da = new DataAccess())
        {
          fileProcessedID = da.addFileProcessed(fileType, path, DateTime.Now, -1, false, "Import " + fileType);
        }
        int startRow = headerrow + 1;
        for (int rowNum = startRow; rowNum < ws.Dimension.End.Row; rowNum++)
        {
          var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
          object[] values = new object[ws.Dimension.End.Column];
          if (ws.Cells[rowNum, 2] != null) // sometimes the spreadsheet has a TON of empty rows. if the third column is null, then we assume empty
          {
            for (int icol = 0; icol < wsRow.Columns; icol++)
              values[icol] = ws.Cells[rowNum, icol + 1].Value;
              //{
              //  if (icol == 16 || icol == 18)
              //  {
              //    string comment = ws.Cells[rowNum, icol + 1].Text;
              //    //if (!commentsToExclude.Contains(comment))
              //    //{
              //      if (comment.Length < 1)
              //        comment = string.Empty;
              //      else
              //        values[icol] = comment.Substring(0, Math.Min(comment.Length, 128)).Trim();
              //    //}
              //    //else
              //    //values[icol] = "Comment Excluded";
              //  }
              //  else
            object[] fieldvalues = new object[values.Length + 1];
            Array.Copy(values, 0, fieldvalues, 0, values.Length);
            fieldvalues = adjustFieldsForDataType(fieldvalues, allheaders);
            fieldvalues[fieldvalues.Length - 1] = fileProcessedID;
            theserecords.Add(fieldvalues.ToList());
          }
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

    internal void SaveImportFile(ImportFileInfo file, string tablename, bool replaceall = false,
      bool checkfordups = false, List<string> uniquekeys = null, bool forcePreprocess = false, DateTime? billCycleDate = null)
    {
      {
        foreach (string rectype in file.Headers.Keys)
        {
          List<string> headers = file.Headers[rectype]; // now get the list of headers
          if (file.Records.ContainsKey(rectype)) // does the records collection have this record type?
          {
            // yes, so add the records to the db
            List<List<object>> records = file.Records[rectype]; // next get all the records with matching values
            saveTableFromFileData(tablename, headers, records, _dataTypes, true, replaceall, checkfordups, uniquekeys, forcePreprocess, billCycleDate);
          }
          // else
          //     No: do nothing
        }
      }
    }

    internal void saveTableFromFileData(string tablename, List<string> headers, 
      List<List<object>> records, Dictionary<string, string> datatypes, bool hasID, bool replaceall = false,
      bool checkfordups = false, List<string> uniquekeys = null, bool forcePreprocess = false, DateTime? billCycleDate = null)
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
        Exception ex = null;
        if (ds != null && ds.Tables.Count > 0)
        {
          DataTable dt = ds.Tables[0];
          string fileProcessedID = dt.Rows[0]["FilesProcessedID"].ToString();
          if (!CommonFunctions.IsInteger(fileProcessedID))
            fileProcessedID = "-1";
          if (forcePreprocess || (!replaceall && checkfordups && uniquekeys != null && uniquekeys.Count > 0))
          {
            /*
             * in this case, we do a bulk copy into an import table.
             * Then we insert records from there into the original table
             * where the unique key does not exist.
             *
             * Then we empty out the import table to be ready for next time
             */
            StringBuilder colssb = new StringBuilder();
            StringBuilder selectlist = new StringBuilder();
            foreach (DataColumn col in dt.Columns)
            {
              if (!col.ColumnName.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
              {
                // we "escape" all column names with "[]" cause some of them are funky and sql won't like them otherwise
                string colname = col.ColumnName.Replace(".","_").Replace("(","").Replace(")","");
                
                colssb.Append("[");
                colssb.Append(colname);
                colssb.Append("]");
                colssb.Append(",");
                // column list for select must reference the import table
                selectlist.Append("i.");
                selectlist.Append("[");
                selectlist.Append(colname);
                selectlist.Append("]");
                selectlist.Append(",");
              }
            }
            colssb.Length--; // strip last comma
            selectlist.Length--;
            string columnslist = " (" + colssb.ToString() + ") ";
            string importtable;
            if (tablename.EndsWith("]"))
            {
              // we have to insert the Import suffix INSIDE the square brackets
              importtable = tablename.Substring(0, tablename.Length - 1) + "Import" + "]";
            }
            else
              importtable = tablename + "Import";
            if (da.existsTable(importtable)) // if this table has not been built, then we can't do anything
            {
              da.updateDataFromSQL("Truncate table " + importtable); // first, get rid of existing records in the import table
              ex = da.insertDataTabletoSQL(importtable, dt);

              if (ex == null) //if no error 
              {
                // now update the import table with the files processed id
                string sql = "Update " + importtable + " SET FilesProcessedID = " + fileProcessedID.ToString();
                da.updateDataFromSQL(sql);
                if (importtable.Equals(CREATIOBILLAUDITIMPORTTABLEUPLOAD, StringComparison.CurrentCultureIgnoreCase))
                {
                  if (billCycleDate == null)
                    return; // we can't process if we don't have the date
                  else
                    sql = "EXEC UpdateCreatioAuditFromCreatio @billCycleDate='" + ((DateTime)billCycleDate).ToShortDateString() + "'";
                }
                else
                {
                  // now build the insert to copy the records to the "real" table
                  sql = "INSERT INTO " + tablename + columnslist +
                    " SELECT " + selectlist.ToString() + " from " + importtable + " i " +
                    " LEFT JOIN " + tablename + " t on ";
                  // left join on the target table based on the unique keys
                  bool firsttime = true;
                  foreach (string key in uniquekeys)
                  {
                    if (firsttime)
                      firsttime = false;
                    else
                      sql += " AND ";
                    sql += "i." + key + " = t." + key;
                  }
                  sql += " WHERE t." + uniquekeys.First() + " IS NULL"; // and select records with no match
                }
                da.updateDataFromSQL(sql); // insert non dup records
                sql = "TRUNCATE TABLE " + importtable;
                da.updateDataFromSQL(sql); // empty out the import table
              }
            }
            else
              ex = new Exception("Import Table Does Not Exist");
          }
          else
            ex  = da.insertDataTabletoSQL(tablename, dt);
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
