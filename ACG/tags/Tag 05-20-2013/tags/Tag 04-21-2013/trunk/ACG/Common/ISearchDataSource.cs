using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ACG.Common
{
  public interface ISearchDataSource
  {
    string IDName { get; set; }
    string NameName { get; set; }
    string SQL { get; set; }
    string OrderByClause { get; set; }
    string[] Search(string criteria);
  }
}
