using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG.Common
{
  public class ImportFileInfo
  {
    public string filepath { get; set; }
    public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();
    public Dictionary<string, List<List<object>>> Records { get; set; } = new Dictionary<string, List<List<object>>>();
  }
}
