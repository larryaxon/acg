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
    #endregion

    public class UnibillFile
    {
      public string filepath { get; set; }
      public Dictionary<string, List<string>> Headers { get; set; }  = new Dictionary<string, List<string>>();
      public Dictionary<string, List<List<string>>> Records { get; set; } = new Dictionary<string, List<List<string>>>();
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
    #region contructors & disposse
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
      string newPath = changeFolder (oldPath, homefolder);
      if (!File.Exists(newPath))
        File.Move(oldPath, newPath);
      return newPath;
    }
    private string changeFolder(string oldpath,  string newfolder)
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
    public void ImportUnibills(string localfolder = null)
    {
      List<string> filesToProcess = getUnibillFilesToProcess();
      foreach (string filename in filesToProcess)
      {
        UnibillFile file = getUnibillFileData(filename);
        saveUnibillFile(file);
        // save files processed record here
      }
    }
    #endregion
    #region private methods
    private UnibillFile getUnibillFileData(string filename)
    {
      UnibillFile file = new UnibillFile();
      file.filepath = Path.Combine(_localTextDirecory, filename);
      int recordnumber = 0;
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
              // get all the header names
              string rectype = values[1];
              string[] headernames = new string[values.Length - 2];
              Array.Copy(values, 2, headernames, 0, values.Length - 2);
              List<string> names = new List<string>();
              names.AddRange(headernames);
              file.Headers.Add(rectype, names);
              break;
            case "!":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
              // data records
              // first get the record type
              string datarectype = values[1];
              // next get an ordered list of the values
              string[] fieldvalues = new string[values.Length - 2];
              Array.Copy(values, 2, fieldvalues, 0, values.Length - 2);
              List<string> fields = new List<string>();
              fields.AddRange(fieldvalues);
              // now see if this record type is already out there
              List<List<string>> theserecords;
              if (file.Records.ContainsKey(datarectype))
                theserecords = file.Records[datarectype]; // yes, get the current list of records for that type
              else
              {
                theserecords = new List<List<string>>(); // no create a new list of records 
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
        List<List<string>> records = file.Records[rectype]; // next get all the records with matching values
        string tablename = _unibillTableNames[rectype]; // now get the table name
        using (DataAccess da = new DataAccess())
        {
          DataSet ds = da.getDatasetFromDictionaryData(tablename, headers, records);
          // now save the dataset here
        }
      }
    }
    private string getAppSetting(string name, string defaultValue = "")
    {
      string val = ConfigurationManager.AppSettings[name];
      if (string.IsNullOrWhiteSpace(val))
        val = defaultValue;
      return val;
    }
    public List<ACGFtpFileInfo> GetFileList(string directory = null)
    {
      if (directory == null)
        directory = _ftpDirectory;
      _fileList = _sftp.ListFiles(directory);
      return _fileList;
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
    private List<string> getUnibillFilesToProcess()
    {
      DirectoryInfo dir = new DirectoryInfo(_localTextDirecory);
      DateTime toDate = DateTime.Now;
      DateTime fromDate =  DateTime.Now.Date.AddDays(-_maxDaysToProcess); // dont go back more than we want to
      List<string> textfiles = dir.GetFiles().Where(file => file.LastWriteTime >= fromDate && file.LastWriteTime <= toDate).Select(f => f.Name).ToList();
      List<string> unibillFiles = textfiles.Where(f => f.StartsWith("unibill", StringComparison.CurrentCultureIgnoreCase)).ToList();
      List<string> unibillFilesProcessed = getFilesProcessed(FILESPROCESSEDFILETYPEUNIBILL);
      List<string> filesToProcess = unibillFiles.Where(f => !unibillFilesProcessed.Contains(f)).ToList();
      return filesToProcess;
    }
    #endregion
  }

}

