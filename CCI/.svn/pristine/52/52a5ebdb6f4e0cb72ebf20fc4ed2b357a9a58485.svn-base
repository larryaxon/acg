using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// A ItemHistory element from the ItemHistory XML column in TAGAttribute
  /// </summary>
  [SerializableAttribute]
  public class ItemHistory : DateHistory, IDataClassItem
  {
    new public object Clone()
    {
      ItemHistory ih = new ItemHistory();
      ih.Deleted = Deleted;
      ih.Dirty = Dirty;
      ih.EndDate = EndDate;
      ih.LastModifiedBy = LastModifiedBy;
      ih.LastModifiedDateTime = LastModifiedDateTime;
      ih.StartDate = StartDate;
      return ih;
    }
  }
}
