using ACG.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCI.Common
{
  public class ServerResponse
  {
    public ArrayList Results = new ArrayList();
    public CCI.Common.SecurityContext SecurityContext = null;
    public Hashtable Options = new Hashtable();
    public ArrayList Errors = new ArrayList();
  }
}
