using System;
using System.Collections;
//using System.Collections.Generic;

namespace TAGBOSS.Common.Model
{
  public class TAttributeTHMerge
  {
    private ArrayList attributeMergeItemHashList = new ArrayList();
    private Hashtable attributeMergeItemHashRAList = new Hashtable();

    public string AttributeId { get; set; }

    public ArrayList AttributeMergeItemHashList
    {
      get { return attributeMergeItemHashList; }
    }

    public Hashtable AttributeMergeItemHashRAList
    {
      get { return attributeMergeItemHashRAList; }
    }
  }
}
