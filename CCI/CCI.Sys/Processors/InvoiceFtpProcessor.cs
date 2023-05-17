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
    private List<ACGFtpFileInfo> _fileList = null;
    private string _ftpUrl = null;
    private string _ftpUsername = null;
    private string _ftpPassword = null;
    private string _ftpDirectory = "FromIQ";
    private string _localDirectory = "U:\\Data\\InvoiceIQ\\downloads\\";
    private ACGSftp _sftp = null;
    #endregion
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
      List<string> processedFiles = getFilesProcessed();
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
        db.addFileProcessed("InvoiceIQ", file.FullName, DateTime.Now, -1, false, "Download Successful");
      }

    }
    private string moveUnziped(string unzippedfile, string newFolder)
    {
      string rootFolder = Directory.GetParent(Directory.GetParent(Directory.GetParent(unzippedfile).FullName).FullName).FullName;
      string filename = Path.GetFileName(unzippedfile);
      string targetfolder = Path.Combine(rootFolder, newFolder);
      string targetpath = Path.Combine(targetfolder, filename);
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
          db.addFileProcessed("InvoiceIQ", fileInfo.FullName, DateTime.Now, -1, false, "Initial Load");
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
    #region private methods
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
    private List<string> getFilesProcessed()
    {
      List<string> fileNames = new List<string>();
      using (DataAccess db = new DataAccess())
      {
        DataSet ds = db.getFilesProcessed();
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
          fileNames.Add(row["FileName"].ToString());
        return fileNames;
      }
    }
    #endregion
  }

}

