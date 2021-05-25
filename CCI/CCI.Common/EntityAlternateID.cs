using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCI.Common
{
  public class EntityAlternateID
  {
    public string Entity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ExternalServiceName { get; set; }
    public string ExternalID { get; set; }
  }
}
