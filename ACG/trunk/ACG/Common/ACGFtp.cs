using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using System.Net;

namespace ACG.Common
{
  public class ACGFtp
  {
    /// <summary>
    /// uses ftp to get a file and returns the local file location path.
    /// </summary>
    /// <param name="configKeySite"></param>
    /// <param name="ftpUser"></param>
    /// <param name="ftpPassword"></param>
    /// <param name="remoteFilePath"></param>
    /// <param name="remoteFileName"></param>
    /// <returns></returns>
    public string ftpGetFile(string ftpUri, string ftpUser, string ftpPassword, string fileName, string localFileFolder, int skipLines = 0, string firstLine = "")
    {
      Uri baseUri = new Uri(ftpUri);
      Uri serverUri = new Uri(baseUri, fileName);
      string lastSlash = localFileFolder.EndsWith("\\") ? string.Empty : "\\";
      string localFilePath = string.Format("{0}{1}{2}",localFileFolder, lastSlash, fileName);
      // The serverUri parameter should start with the ftp:// scheme. 
      if (serverUri.Scheme != Uri.UriSchemeFtp)
      {
        return null;
      }
      // Get the object used to communicate with the server.
      FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverUri);
      ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

      ftpRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);

      using (Stream stream = ftpRequest.GetResponse().GetResponseStream())
      {
        using (StreamReader reader = new StreamReader(stream))
        {
          using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
          {
            int nbrLines = 0;
            string line = null;
            if (!string.IsNullOrEmpty(firstLine))
            {
              line = firstLine + "\r\n";
              fileStream.Write(line.Select(c => (byte)c).ToArray(), 0, line.Length);
            }

            while ((line = reader.ReadLine()) != null)
            {
              if (++nbrLines > skipLines)
              {
                line += "\r\n";
                fileStream.Write(line.Select(c => (byte)c).ToArray(), 0, line.Length);
              }
            }
            fileStream.Close();
          }
          reader.Close();
        }
        stream.Close();
      }
      return localFilePath;
    }
  }
}
