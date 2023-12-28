using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCI.Common
{
  public class CreatioEDIMatchingModel
  {
    public int? ID { get; set; }
    public string Carrier { get; set; }
    public string AuditParent { get; set; }
    public string AuditChild { get; set; }
    public string EDIParent { get; set; }
    public string EDIChild { get; set; }
    public bool? ConsolidateToParent { get; set; }
  }
}
