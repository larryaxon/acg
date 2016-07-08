﻿using System;
namespace CCI.Common
{
  public interface ISortedItemBase
  {
    string ID { get; set; }
    object Object { get; set; }
    string OriginalID { get; set; }
    int SortOrder { get; set; }
  }
}
