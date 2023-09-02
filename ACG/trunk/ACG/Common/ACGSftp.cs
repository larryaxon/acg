using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG.Common
{
  public class ACGSftp : IDisposable
  {
    private readonly SftpClient _sftpClient;

    public ACGSftp(string host, string username, string password )
    {
      _sftpClient = new SftpClient(host, username, password);
      _sftpClient.Connect();
    }
    public void Dispose()
    {
      _sftpClient.Disconnect();
      _sftpClient.Dispose();
    }
    public List<ACGFtpFileInfo> ListFiles(string directory)
    {
      List<ACGFtpFileInfo> fileList = new List<ACGFtpFileInfo>();
      var files =  _sftpClient.ListDirectory(directory);
      foreach (var file in files)
      {
        fileList.Add(new ACGFtpFileInfo() { Name = file.Name, FullName = file.FullName, IsDirectory = file.IsDirectory });
      }
      return fileList;
    }
    public string DownloadFile(ACGFtpFileInfo file, string localFolder)
    {
      string pathLocalFile = Path.Combine(localFolder,file.Name);

      using (Stream fileStream = File.OpenWrite(pathLocalFile))
      {
        _sftpClient.DownloadFile(file.FullName, fileStream);
      }

      return pathLocalFile;
    }
  }
}
