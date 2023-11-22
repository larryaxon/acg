using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCI.Common
{
  public class ProcessStepsModel
  {
    public string Cycle { get; set; }
    public int Sequence { get; set; }
    public string Step { get; set; }
    public string Description { get; set; }
    public string Form { get; set; }
    public string FormParameters { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDateTime { get; set; }
    public bool? IsRequired { get; set; } 
  }
}
