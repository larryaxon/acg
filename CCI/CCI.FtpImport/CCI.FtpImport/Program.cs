using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACG;
using ACG.Common;

namespace CCI.FtpImport
{

  internal class Program
  {
    const string APPSETTINGFTPURL = "FTPUrl";
    const string APPSETTINGFTPUSERNAME = "FtpUsername";
    const string APPSETTINGFTPPASSWORD = "FtpPassword";
    const string APPSETTINGLOCALFOLDER = "FTPLocalFolder";
    private static bool _pause = true;
    private static string _ftpUrl = null;
    private static string _ftpUsername = null;
    private static string _ftpPassword = null;
    private static string _localFolder = null;
    static void Main(string[] args)
    {
      _ftpUrl = getAppSetting(APPSETTINGFTPURL, null);
      _ftpUsername = getAppSetting(APPSETTINGFTPUSERNAME, null);
      _ftpPassword = getAppSetting(APPSETTINGFTPPASSWORD, null);
      _localFolder = getAppSetting(APPSETTINGLOCALFOLDER, null);
      if (_ftpUrl == null || _ftpUsername == null || _ftpPassword == null)
      {
        Console.WriteLine("FTP settings are invalid");
        return;
      }
      using (ACGSftp ftp = new ACGSftp(_ftpUrl, _ftpUsername, _ftpPassword))
      {
        string directory = "FromIQ";
        List<ACGFtpFileInfo> filelist = ftp.ListFiles(directory);
        bool firsttime = true;
        foreach (ACGFtpFileInfo file in filelist)
        {
          if (firsttime)
          {
            Console.WriteLine("Extension is " + file.Extension);
            string newfilepath = ftp.DownloadFile(file, _localFolder);
            Console.WriteLine("Downloaded " + newfilepath);
            firsttime = false;
          }
          else
          Console.WriteLine(file.Name);
        }
      }
      if (_pause)
      {
        Console.Write("Press enter to continue...");
        Console.ReadLine();
      }

    }
    private static string getAppSetting(string name, string defaultValue = "")
    {
      string val = ConfigurationManager.AppSettings[name];
      if (string.IsNullOrWhiteSpace(val))
        val = defaultValue;
      return val;
    }
  }
}
