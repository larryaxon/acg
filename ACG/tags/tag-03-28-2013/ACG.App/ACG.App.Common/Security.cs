using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using ACG.Common.Logging;
using ACG.Common;

namespace ACG.App.Common
{
  [Serializable]
  public class Security : ACG.Common.Security
  {
    private Dictionary<string, string> entityRightsList = new Dictionary<string, string>();      // hash lookup of rightys by entity

    private Dictionary<string, bool> entityList = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);  // actual list of Entities, since HashTable does not actually store the string key

  }
}
