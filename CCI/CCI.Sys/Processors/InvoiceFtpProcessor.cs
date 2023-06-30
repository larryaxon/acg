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


namespace CCI.Sys.Processors
{
  public class InvoiceFtpProcessor : IDisposable
  {
    #region private properties
    const string APPSETTINGFTPURL = "FTPUrl";
    const string APPSETTINGFTPUSERNAME = "FtpUsername";
    const string APPSETTINGFTPPASSWORD = "FtpPassword";
    const string APPSETTINGLOCALFOLDER = "FTPLocalFolder";
    const string APPSETTINGMAXDAYSTOPROCESS = "InvoiceIQUnibillMaxDaysToProcess";

    const string FILESPROCESSEDFILETYPEINVOICEIQ = "InvoiceIQ";
    const string FILESPROCESSEDFILETYPEUNIBILL = "Unibill";
    private List<ACGFtpFileInfo> _fileList = null;
    private string _ftpUrl = null;
    private string _ftpUsername = null;
    private string _ftpPassword = null;
    private string _ftpDirectory = "FromIQ";
    private string _localDirectory = "U:\\Data\\InvoiceIQ\\downloads\\";
    private string _localTextDirecory = "U:\\Data\\InvoiceIQ\\texts\\";
    private int _maxDaysToProcess = 45; 
    private ACGSftp _sftp = null;
    private Dictionary<string, string> _unibillTableNames = new Dictionary<string, string>()
    {
      { "1", "UnibillFileInfo"},
      { "2", "UnibillFacePage"},
      { "3", "UnibillCharge"},
      { "4", "UnibillUsageCDR"},
      { "5", "UnibillNotes"},
      { "6", "UnibillContacts"},
      { "7", "UnibillOther"}

    };
    private Dictionary<string, string> _dataTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
    {
      { "ACCT_LEVEL", "int" },
      { "ACTIVITY_COMP_DATE", "date" },
      { "AIR_CHG_AMT", "decimal(8,2)" },
      { "BAL_FWD", "decimal(8,2)" },
      { "BAL_FWD_ADJ", "decimal(8,2)" },
      { "BEG_CHG_DATE", "date" },
      { "BILL_PERIOD_END", "date" },
      { "BILL_PERIOD_START", "date" },
      { "CHG_AMT", "decimal(8,2)" },
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
      { "DUE_DATE" , "date" },
      { "END_CHG_DATE", "date" },
      { "FEAT_CHG_AMT", "decimal(8,2)" },
      { "INV_DATE", "date" },
      { "LD_CHG_AMT", "decimal(8,2)" },
      { "MSG_CHG_AMT", "decimal(8,2)" },
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
      { "FilesProcessedID", "int" }

    };

    #endregion

    public class UnibillFile
    {
      public string filepath { get; set; }
      public Dictionary<string, List<string>> Headers { get; set; }  = new Dictionary<string, List<string>>();
      public Dictionary<string, List<List<object>>> Records { get; set; } = new Dictionary<string, List<List<object>>>();
    }

    #region public properties
    public List<ACGFtpFileInfo> FileList 
    { 
      get 
      {
        if (_fileList == null)
          _fileList = GetFileList();
        return _fileList;
      } 
    }
    public List<string> FileNameList 
    {  
      get 
      {
        if (_fileList == null)
          _fileList = GetFileList();
        return _fileList.Select(f => f.Name).ToList();
      } 
    }
    public string LocalFolder
    {
      get { return _localDirectory; }
      set { _localDirectory = value; }
    }
    public string FtpFolder
    {
      get { return _ftpDirectory; }
      set { _ftpDirectory = value; }
    }

    #endregion
    #region constructors & dispose
    public InvoiceFtpProcessor()
    {
      _ftpUrl = getAppSetting(APPSETTINGFTPURL, null);
      _ftpUsername = getAppSetting(APPSETTINGFTPUSERNAME, null);
      _ftpPassword = getAppSetting(APPSETTINGFTPPASSWORD, null);
      _localDirectory = getAppSetting(APPSETTINGLOCALFOLDER, null);
      if (_ftpUrl == null || _ftpUsername == null || _ftpPassword == null)
      {
        throw new Exception("FTP settings are invalid");
      }
      commonConstructor();

    }
    public InvoiceFtpProcessor(string url, string username, string password, string ftpDirectory = null, string localDirectory = null)
    {
      _ftpUrl = url;
      _ftpUsername = username;
      _ftpPassword = password;
      if (_ftpUrl == null || _ftpUsername == null || _ftpPassword == null)
      {
        throw new Exception("FTP settings are invalid");
      }
      if (ftpDirectory != null)
        _ftpDirectory = ftpDirectory;
      if (localDirectory != null)
        _localDirectory = localDirectory;
      commonConstructor();

    }
    private void commonConstructor()
    {
      _sftp = new ACGSftp(_ftpUrl, _ftpUsername, _ftpPassword);
      CommonData.SERVERCONFIGFILENAME = CommonData.SERVERCONFIGFILEDEFAULT; // db.config for now
      _maxDaysToProcess = CommonFunctions.CInt(getAppSetting(APPSETTINGMAXDAYSTOPROCESS), 45);
    }
    public void Dispose()
    {
      _sftp.Dispose();
    }
    #endregion
    #region download zips
    #region public methods
    public List<string> ProcessFiles()
    {
      List<ACGFtpFileInfo> filesToProcess = getFilesToProcess();
      foreach (ACGFtpFileInfo file in filesToProcess)
        ProcessFile(file);
      return filesToProcess.Select(f => f.Name).ToList();
    }
    public List<ACGFtpFileInfo> getFilesToProcess()
    {
      List<string> processedFiles = getFilesProcessed(FILESPROCESSEDFILETYPEINVOICEIQ);
      List<ACGFtpFileInfo> ftpFiles = FileList.ToList();
      List<ACGFtpFileInfo> filesToProcess = ftpFiles.Where(ftp => !processedFiles.Contains(ftp.FullName)).ToList();
      return filesToProcess;
    }
    private void ProcessFile(ACGFtpFileInfo file)
    {
      string filename = DownloadFile(file);
      if (file.IsPdf)
      {
        moveToHome(filename, "pdfs");
      }
      else if (file.IsZip)
      {
        string zipHomePath = moveToHome(filename, "zips"); // first move the zip file to the zips folders
        string newZipPath = Directory.GetParent(changeFolder(zipHomePath, "unzips")).FullName;
        using (ACGZip zip = new ACGZip())
        {
          string newzipfolder = zip.Unzip(zipHomePath, newZipPath);
          List<string> unzippedfiles = Directory.GetFiles(newzipfolder).ToList();
          foreach (string unzippedfile in unzippedfiles)
          {
            string extension = Path.GetExtension(unzippedfile).ToLower();
            string newfolder;
            switch (extension)
            {
              case ".pdf":
                newfolder = "pdfs";
                break;
              case ".txt":
                newfolder = "texts";
                break;
              case ".xlsx":
                newfolder = "excels";
                break;
              default:
                newfolder = "Other";
                break;
            }
            moveUnziped(unzippedfile, newfolder);

          }
        }
      }
      else
      {
        moveToHome(filename, "Other");
      }
      using (DataAccess db = new DataAccess())
      {
        db.addFileProcessed(FILESPROCESSEDFILETYPEINVOICEIQ, file.FullName, DateTime.Now, -1, false, "Download Successful");
      }

    }
    private string moveUnziped(string unzippedfile, string newFolder)
    {
      string rootFolder = Directory.GetParent(Directory.GetParent(Directory.GetParent(unzippedfile).FullName).FullName).FullName;
      string filename = Path.GetFileName(unzippedfile);
      string targetfolder = Path.Combine(rootFolder, newFolder);
      string targetpath = Path.Combine(targetfolder, filename);
      if (!File.Exists(targetpath))
        File.Move(unzippedfile, targetpath);
      return targetpath;
    }
    private string moveToHome(string oldPath, string homefolder)
    {
      string newPath = changeFolder(oldPath, homefolder);
      if (!File.Exists(newPath))
        File.Move(oldPath, newPath);
      return newPath;
    }
    private string changeFolder(string oldpath, string newfolder)
    {
      string filename = Path.GetFileName(oldpath);
      string rootpath = Directory.GetParent(Directory.GetParent(oldpath).FullName).FullName;
      string newPath = Path.Combine(Path.Combine(rootpath, newfolder), filename);
      return newPath;
    }
    public void InitalFilesProcessedLoad()
    {
      List<ACGFtpFileInfo> ftpFiles = FileList;

      using (DataAccess db = new DataAccess())
      {
        foreach (ACGFtpFileInfo fileInfo in ftpFiles)
        {
          db.addFileProcessed(FILESPROCESSEDFILETYPEINVOICEIQ, fileInfo.FullName, DateTime.Now, -1, false, "Initial Load");
        }
      }

    }
    public string DownloadFile(ACGFtpFileInfo file, string localfolder = null)
    {
      if (localfolder == null)
        localfolder = Path.Combine(_localDirectory, "downloads");
      string filepath = _sftp.DownloadFile(file, localfolder);
      return filepath;
    }
    public string UnzipFile(string filename, string zipfolder = null)
    {
      using (ACGZip zip = new ACGZip())
      {
        return zip.Unzip(filename, zipfolder);
      }
    }

    #endregion
    public List<ACGFtpFileInfo> GetFileList(string directory = null)
    {
      if (directory == null)
        directory = _ftpDirectory;
      _fileList = _sftp.ListFiles(directory);
      return _fileList;
    }
    #endregion
    #region import unibill
    #region public methods
    public List<string> ImportUnibills(int maxFilesToProcess = -1)
    {
      List<string> filesToProcess = getUnibillFilesToProcess(maxFilesToProcess);
      foreach (string filename in filesToProcess)
      {
        UnibillFile file = getUnibillFileData(filename);
        saveUnibillFile(file);
        // save files processed record here
      }
      return filesToProcess;
    }
    public List<string> ListUnprocessedUnibills()
    {
      List<string> filesToProcess = getUnibillFilesToProcess(-1);
      return filesToProcess;
    }
    #endregion
    #region private methods
    private UnibillFile getUnibillFileData(string filename)
    {
      UnibillFile file = new UnibillFile();
      file.filepath = Path.Combine(_localTextDirecory, filename);
      int fileProssedID = 0;
      using (DataAccess da = new DataAccess())
      {
        fileProssedID = da.addFileProcessed(FILESPROCESSEDFILETYPEUNIBILL, file.filepath, DateTime.Now, -1, false, "Import Unibill");
      }
      // Read the text file line by line
      using (StreamReader sr = new StreamReader(file.filepath))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          string[] values = line.Split('|');
          string linetype = values[0];
          switch (linetype)
          {
            case "901":
            case "903":
            case "904":
            case "905":
              // marker records (begin/end sections, etc.)
              // we ignore these
              break;
            case "902":
              // get all the header names and then add the FilesPrcessedID column at the end
              string rectype = values[1];
              string[] headernames = new string[values.Length - 1];
              Array.Copy(values, 2, headernames, 0, values.Length - 2);
              headernames[headernames.Length - 1] = "FilesProcessedID";
              List<string> names = new List<string>();
              names.AddRange(headernames);
              file.Headers.Add(rectype, names);
              break;
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
              // data records
              // first get the record type
              string datarectype = linetype;
              // next get an ordered list of the values
              object[] fieldvalues = new string[values.Length];
              Array.Copy(values, 1, fieldvalues, 0, values.Length - 1);
              // take any values that cannot be converted to the correct datatype and make them null
              fieldvalues = adjustFieldsForDataType(fieldvalues, file.Headers[datarectype].ToArray());
              int fileprocessedidsub = fieldvalues.Length - 1;

              fieldvalues[fileprocessedidsub] = fileProssedID; // so use the file processed it to uniquely identify this batch
              List<object> fields = new List<object>();
              fields.AddRange(fieldvalues);

              // now see if this record type is already out there
              List<List<object>> theserecords;
              if (file.Records.ContainsKey(datarectype))
                theserecords = file.Records[datarectype]; // yes, get the current list of records for that type
              else
              {
                theserecords = new List<List<object>>(); // no create a new list of records 
                file.Records.Add(datarectype, theserecords); // and associate with that type
              }
              theserecords.Add(fields); // now add the new record (which is itself a list of fields) to the collection for this record type
              // Note: since this is a reference we can add it to the variable and it adds it to the collection
              break;
          }

        }
      }
      return file;
    }
    private object[] adjustFieldsForDataType(object[] fields, string[] columnNames)
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
        }
      }
      return fldarray;
    }
    private void saveUnibillFile(UnibillFile file)
    {
      foreach (string rectype in file.Headers.Keys)
      {
        List<string> headers = file.Headers[rectype]; // now get the list of headers
        if (file.Records.ContainsKey(rectype)) // does the records collection have this record type?
        {
          // yes, so add the records to the db
          List<List<object>> records = file.Records[rectype]; // next get all the records with matching values
          string tablename = _unibillTableNames[rectype]; // now get the table name
          using (DataAccess da = new DataAccess())
          {
            // all of these UniBill files have an ID so we set the last parm to true
            DataSet ds = da.getDatasetFromDictionaryData(tablename, headers, records, _dataTypes, true);
            if (ds != null && ds.Tables.Count > 0)
            {
              DataTable dt = ds.Tables[0];
              string fileProcessedID = dt.Rows[0]["UNIQ_ID"].ToString();
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
        // else
        //     No: do nothing
      }
    }

    private List<string> getUnibillFilesToProcess(int maxFilesToProcess = -1)
    {
      DirectoryInfo dir = new DirectoryInfo(_localTextDirecory);
      DateTime toDate = DateTime.Now;
      DateTime fromDate = DateTime.Now.Date.AddDays(-_maxDaysToProcess); // dont go back more than we want to
      List<string> textfiles = dir.GetFiles()
        .Where(file => file.LastWriteTime >= fromDate && file.LastWriteTime <= toDate)
        .OrderByDescending(file => file.LastWriteTime)
        .Select(f => f.Name).ToList();
      List<string> unibillFiles = textfiles.Where(f => f.StartsWith("unibill", StringComparison.CurrentCultureIgnoreCase)).ToList();
      List<string> unibillFilesProcessed = getFilesProcessed(FILESPROCESSEDFILETYPEUNIBILL);
      unibillFilesProcessed = getFileNamesfromPaths(unibillFilesProcessed);
      List<string> filesToProcess = unibillFiles.Where(f => !unibillFilesProcessed.Contains(f)).ToList();
      if (maxFilesToProcess != -1)
        filesToProcess = filesToProcess.Take(maxFilesToProcess).ToList();
      return filesToProcess;
    }
    public List<string> getFileNamesfromPaths(List<string> paths)
    {
      List<string> filenames = new List<string>();
      filenames = paths.Select(p => Path.GetFileName(p)).ToList();
      return filenames;
    }

    #endregion
    #endregion
    #region excel invoice
    public MemoryStream GetInvoice(int fileProcessedId)
    {
      using (ExcelProcessor excel = new ExcelProcessor())
      {
        using (DataAccess da = new DataAccess())
        {
          DataSet ds = da.GetDataFromSQL("Select * from UnibillCharge where FilesProcessedID = " + fileProcessedId.ToString());
          ExcelWorksheet w = excel.CreateWorksheetFromDataset(ds, "Invoice");
          w.InsertRow(1, 2);
          w.Cells["A1"].Value = "Invoice for File Processed " + fileProcessedId.ToString();
          MemoryStream stream = excel.ToStream();
          return stream;
        }
      }
    }
    #endregion

    #region other private methods

    private string getAppSetting(string name, string defaultValue = "")
    {
      string val = ConfigurationManager.AppSettings[name];
      if (string.IsNullOrWhiteSpace(val))
        val = defaultValue;
      return val;
    }
    private List<string> getFilesProcessed(string filetype)
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
    #endregion
  }

}

