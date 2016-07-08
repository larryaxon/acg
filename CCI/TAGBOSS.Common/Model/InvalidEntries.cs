using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common;

namespace TAGBOSS.Common.Model
{
  [SerializableAttribute]
  /// <summary>
  /// Collection of entries that failed validatio, returned by "page" Entity version of IsValid.
  /// </summary>
  public class InvalidEntries : DataClass<InvalidEntry> 
  {
    public new object Clone()
    {
      InvalidEntries ie = new InvalidEntries();
      ie = (InvalidEntries)base.Clone<InvalidEntries>(ie);
      return ie;
    }
  } // class which defines a collection of invalid entries to pass to calling program for IsValid()
}
