using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ACG.App.Common
{
  public class ServerResponse
  {
    public ArrayList Results = new ArrayList();
    public SecurityContext SecurityContext = null;
    public Hashtable Options = new Hashtable();
    public ArrayList Errors = new ArrayList();
  }
}
