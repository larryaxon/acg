using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TIndex :TAEL
  {
    public IEnumerator GetEnumerator() 
    {
      return this.List.GetEnumerator();
    }

  }
}
