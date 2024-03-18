using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCI.Common
{
  public class CreatioEDICarrierMatchingModel
  {
    public int? ID { get; set; }
    [DisplayName("Audit Carrier")]
    public string CreatioCarrier { get; set; }
    [DisplayName("EDI Carrier")]
    public string EDICarrier { get; set; }
  }
}
