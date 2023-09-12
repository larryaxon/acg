using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG.Common
{
  public class ImportExcelFileInfo
  {
    public string filepath { get; set; }
    public string FileType { get; set; }
    public List<string> Headers { get; set; }
    public DataTable dt { get; set; }
  }
}
