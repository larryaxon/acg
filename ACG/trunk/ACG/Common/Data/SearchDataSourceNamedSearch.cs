using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;

namespace ACG.Common.Data
{
  public class SearchDataSourceNamedSearch : DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public string User { get; set; }
    public string NameType { get; set; }
    public SearchDataSourceNamedSearch()
    {
      SQL = @"    select * from (select substring(OptionType, 13,99) SearchName, Description 
  from useroptions 
  where userid = '{0}' and OptionName = '{1}' 
  and OptionType like 'NamedSearch:%') a ";
      OrderByClause = " order by SearchName ";
      IDName = "SearchName";
      NameName = "Description";
      NameType = "None";
      User = string.Empty;
    }
    public string[] Search(string criteria, bool useExactID)
    {
      if (NameType == "None")
        return new string[0];
      else
        return getSearchList(string.Format(SQL, User, NameType.ToString()), OrderByClause, criteria, IDName, new string[] { NameName }, useExactID, true);
    }
  }
}
