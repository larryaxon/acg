using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACG.App.Common
{
  public class ServerRequest
  {
    public int SecurityID = -1;
    public ServerCommands Command = ServerCommands.None;
    public Dictionary<string, string> Parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> Form = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
  }
}
