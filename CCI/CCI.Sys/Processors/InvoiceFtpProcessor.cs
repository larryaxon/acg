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
using OfficeOpenXml.FormulaParsing.ExpressionGraph;


namespace CCI.Sys.Processors
{
  public class InvoiceFtpProcessor : ImportFileProcessorBase, IDisposable
  {
    #region private properties
    const string APPSETTINGFTPURL = "FTPUrl";
    const string APPSETTINGFTPUSERNAME = "FtpUsername";
    const string APPSETTINGFTPPASSWORD = "FtpPassword";
    const string APPSETTINGLOCALFTPFOLDER = "FTPLocalFolder";
    const string APPSETTINGMAXDAYSTOPROCESS = "InvoiceIQUnibillMaxDaysToProcess";

    const string FILESPROCESSEDFILETYPEINVOICEIQ = CCI.Common.CommonData.FILESPROCESSEDFILETYPEINVOICEIQ;
    const string FILESPROCESSEDFILETYPEUNIBILL = CCI.Common.CommonData.FILESPROCESSEDFILETYPEUNIBILL;

    const string FILEDOWNLOADFOLDER = "\\InvoiceIQ\\downloads\\";

    private string _ftpUrl = null;
    private string _ftpUsername = null;
    private string _ftpPassword = null;
    private string _ftpDirectory = "FromIQ";

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


    #endregion

    public class UnibillFile : ImportFileInfo
    {
    }

    public string FtpFolder
    {
      get { return _ftpDirectory; }
      set { _ftpDirectory = value; }
    }
    #region constructors & dispose
    public InvoiceFtpProcessor() : base()
    {
      _ftpUrl = getAppSetting(APPSETTINGFTPURL, null);
      _ftpUsername = getAppSetting(APPSETTINGFTPUSERNAME, null);
      _ftpPassword = getAppSetting(APPSETTINGFTPPASSWORD, null);
      const string APPSETTINGLOCALBASEFOLDER = "LocalBaseFolder";
      string localroot = getAppSetting(APPSETTINGLOCALBASEFOLDER, null);
      if (FILEDOWNLOADFOLDER.StartsWith(localroot, StringComparison.CurrentCultureIgnoreCase))
        _localDirectory = FILEDOWNLOADFOLDER;
      else
        _localDirectory = localroot + FILEDOWNLOADFOLDER;
      if (_ftpUrl == null || _ftpUsername == null || _ftpPassword == null)
      {
        throw new Exception("FTP settings are invalid");
      }
      _sftp = new ACGSftp(_ftpUrl, _ftpUsername, _ftpPassword);
      _maxDaysToProcess = CommonFunctions.CInt(getAppSetting(APPSETTINGMAXDAYSTOPROCESS), 45);
      _fileList = GetFileList();


    }
    public InvoiceFtpProcessor(string url, string username, string password, string ftpDirectory = null, string localDirectory = null) : base()
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
      _sftp = new ACGSftp(_ftpUrl, _ftpUsername, _ftpPassword);
      _maxDaysToProcess = CommonFunctions.CInt(getAppSetting(APPSETTINGMAXDAYSTOPROCESS), 45);
      _fileList = GetFileList();

    }

    public new void Dispose()
    {
      base.Dispose();
      _sftp.Dispose();
    }
    #endregion

    public List<ACGFileInfo> GetFileList(string directory = null)
    {
      if (directory == null)
        directory = _ftpDirectory;
      if (_fileList == null)
        _fileList = new List<ACGFileInfo>();
      else
        _fileList.Clear();
      foreach (ACGFtpFileInfo fileInfo in _sftp.ListFiles(directory))
        _fileList.Add(fileInfo.ToFtpFileInfo());
      return _fileList;
    }
    #region download zips
    #region public methods
    public List<string> ProcessFiles(string fileType)
    {
      List<ACGFileInfo> filesToProcess = getFilesToProcess(fileType);
      foreach (ACGFileInfo file in filesToProcess)
        ProcessFile(file, fileType);
      return filesToProcess.Select(f => f.Name).ToList();
    }

    private void ProcessFile(ACGFileInfo file, string fileType)
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
        db.addFileProcessed(fileType, file.FullName, DateTime.Now, -1, false, "Download Successful");
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
      if (File.Exists(oldPath) && !File.Exists(newPath))
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
    public void InitalFilesProcessedLoad(string fileType)
    {
      List<ACGFileInfo> ftpFiles = FileList;

      using (DataAccess db = new DataAccess())
      {
        foreach (ACGFileInfo fileInfo in ftpFiles)
        {
          db.addFileProcessed(fileType, fileInfo.FullName, DateTime.Now, -1, false, "Initial Load");
        }
      }

    }
    public string DownloadFile(ACGFileInfo file, string localfolder = null)
    {
      if (localfolder == null)
      {
        if (_localDirectory.Contains("downloads"))
          localfolder = _localDirectory;
        else
          localfolder = Path.Combine(_localDirectory, "downloads");
      }
      string filepath = _sftp.DownloadFile((ACGFtpFileInfo)file, localfolder);
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
      int fileProcessedID = 0;
      using (DataAccess da = new DataAccess())
      {
        fileProcessedID = da.addFileProcessed(FILESPROCESSEDFILETYPEUNIBILL, file.filepath, DateTime.Now, -1, false, "Import Unibill");
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

              fieldvalues[fileprocessedidsub] = fileProcessedID; // so use the file processed it to uniquely identify this batch
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
          saveTableFromFileData(tablename, headers, records, _dataTypes, true);
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


  }

}

