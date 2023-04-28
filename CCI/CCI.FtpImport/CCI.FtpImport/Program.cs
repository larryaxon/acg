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
    private static bool _pause = true;
    private static string _ftpUrl = null;
    private static string _ftpUsername = null;
    private static string _ftpPassword = null;
    static void Main(string[] args)
    {
      _ftpUrl = getAppSetting(APPSETTINGFTPURL, null);
      _ftpUsername = getAppSetting(APPSETTINGFTPUSERNAME, null);
      _ftpPassword = getAppSetting(APPSETTINGFTPPASSWORD, null);
      if (_ftpUrl == null || _ftpUsername == null || _ftpPassword == null)
      {
        Console.WriteLine("FTP settings are invalid");
        return;
      }
      ACGFtp ftp = new ACGFtp(_ftpUrl, _ftpUsername, _ftpPassword);
      List<string> filelist = ftp.getDirectoryListing();
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
