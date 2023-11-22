using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCI.Common
{
  public class BillCycleModel
  {
    public int ID { get; set; }
    public DateTime BillingDate { get; set; }
    public string Step { get; set; }
    public string SubStep { get; set; }
    public string ProcessedBy { get; set; }
    public DateTime ProcessedDateTime { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
    public string LastModifiedBy { get; set; }
    public string FileType { get; set; }
    public string FileName { get; set; }
    public DateTime UnProcessedDateTime { get; set; }

  }
}
