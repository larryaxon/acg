using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ACG.Common
{
  public class ACGZip
  {
    public static void Unzip(string zipFile, string destination)
    {
      using (var zipArchive = ZipFile.OpenRead(zipFile))
      {
        foreach (ZipArchiveEntry entry in zipArchive.Entries)
        {
          string path = Path.Combine(destination, entry.Name);
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
    }
    private static bool IsDirectory(ZipArchiveEntry entry)
    {

      var lowerByte = (byte)(entry.ExternalAttributes & 0x00FF);
      var attributes = (FileAttributes)lowerByte;
      return attributes.HasFlag(FileAttributes.Directory);
    }
  }
}
