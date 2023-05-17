using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG.Common
{
  public class ACGFtpFileInfo
  {
    public string Name { get; set; }
    public string FullName { get; set; }
    public bool IsDirectory { get; set; }
    public string Extension
    {
      get
      {
        if (!Name.Contains('.'))
          return null;
        return Name.Substring(Name.LastIndexOf('.') + 1, Name.Length - Name.LastIndexOf('.') - 1);
      }
    }
    public bool IsZip
    {
      get { return (Extension ?? string.Empty) == "zip"; } 
    }
    public bool IsPdf
    {
      get { return (Extension ?? string.Empty) == "pdf"; }
    }
  }
}
