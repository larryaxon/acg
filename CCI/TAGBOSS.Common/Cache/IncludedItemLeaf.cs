using System;
using System.Collections.Generic;
using System.Collections;

namespace TAGBOSS.Common
{
  [Serializable]
  public class IncludedItemLeaf
  {
    string itemTypeID;
    string itemID;
    bool validInclude;
    List<IncludedItemAddress> embeddedIncludesList;

    public string ItemTypeID
    {
      get { return itemTypeID; }
      set { itemTypeID = value; }
    }

    public string ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }

    public bool ValidInclude
    {
      get { return validInclude; }
      set { validInclude = value; }
    }

    public List<IncludedItemAddress> EmbeddedIncludesList
    {
      get { return embeddedIncludesList; }
      set { embeddedIncludesList = value; }
    }
  }
}
