using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class ReportFormatModel
  {
    public const string FORMATTYPENUMERIC = "Number";
    public const string FORMATTYPEALIGN = "Alignment";
    public const string FORMATALIGHLEFT = "Left";
    public const string FORMATALIGNRIGHT = "Right";
    public const string FORMATALIGHCENTER = "Center";
    public int ID { get; set; }
    public string FormatName { get; set; }
    public string FormatType { get; set; }
    public string FormatString { get; set; }
    public string FormatOutput { get; set; }
  }
}
