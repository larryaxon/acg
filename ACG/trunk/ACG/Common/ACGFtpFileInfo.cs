using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG.Common
{
  public class ACGFtpFileInfo : ACGFileInfo
  {
    public ACGFileInfo ToFtpFileInfo()
    {
      return this as ACGFileInfo;
    }
  }
}
