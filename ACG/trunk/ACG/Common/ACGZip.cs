using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Reflection.Emit;

namespace ACG.Common
{
  public class ACGZip : IDisposable
  {
    public void Dispose() { }
    public string Unzip(string zipFile, string destination)
    {
      if (!Directory.Exists(destination)) 
      { 
        Directory.CreateDirectory(destination);
      }
      string newFolder = Path.GetFileName(zipFile); // get the directory to unzip this into from the zip file name (includes the zip extension)
      newFolder = newFolder.Substring(0, newFolder.IndexOf('.')); // without the zip extension
      string targetdirectory = Path.Combine(destination, newFolder);
      List<string> filelist = Directory.GetFiles(destination).ToList();
      while (filelist.Contains(targetdirectory, StringComparer.CurrentCultureIgnoreCase))
      {
        targetdirectory = zipFile + " Copy";
      }
      Directory.CreateDirectory((string)targetdirectory);
      string finaldestination = Path.Combine(destination, targetdirectory);
      using (var zipArchive = ZipFile.OpenRead(zipFile))
      {
        foreach (ZipArchiveEntry entry in zipArchive.Entries)
        {
          string path = Path.Combine(finaldestination, entry.Name);
          if (IsDirectory(entry))
          {
            Directory.CreateDirectory(path);
          }
          else
          {
            using (var stream = entry.Open())
            {
              using (var fileStream = File.Create(path))
              {
                stream.CopyTo(fileStream);
              }
            }
          }
        }
      }
      return finaldestination;
    }
    private bool IsDirectory(ZipArchiveEntry entry)
    {

      var lowerByte = (byte)(entry.ExternalAttributes & 0x00FF);
      var attributes = (FileAttributes)lowerByte;
      return attributes.HasFlag(FileAttributes.Directory);
    }
  }
}
