using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceEntityList : DataAccessBase, ISearchDataSource
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public ArrayList MemberList { get; set; }

    public SearchDataSourceEntityList(ArrayList memberList)
    {
      MemberList = memberList;
      SQL = "";
      OrderByClause = "";
      IDName = "";
      NameName = "";

    }

    public string[] Search(string criteria, bool useExactID)
    {
      ArrayList returnList = new ArrayList();
      foreach (string member in MemberList)
      {
        if (useExactID)
        {
          if (getID(member).Equals(criteria, StringComparison.CurrentCultureIgnoreCase))
          {
            returnList.Add(member);
            break;
          }
        }
        else
        {
          int pos = member.IndexOf(criteria, StringComparison.CurrentCultureIgnoreCase);
          if (pos >= 0)
            returnList.Add(member);
        }
      }
      return (string[])returnList.ToArray(typeof(string));
    }
    private string getID(string member)
    {
      int pos = member.IndexOf(":");
      if (pos < 0)
        return member;
      else
        return member.Substring(0, pos).Trim();
    }
  }
}
