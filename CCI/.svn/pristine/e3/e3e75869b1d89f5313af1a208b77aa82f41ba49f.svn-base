using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceOrder : DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public string OrderStatus { get; set; }
    public SearchDataSourceOrder()
    {
      SQL = @"Select * from (select 'Order-' + CONVERT(nvarchar(20), o.ID) OrderID, replace(o.ShortName,':','<')  + '> (' + o.Customer + '-' + isnull(c.LegalName, 'Not Found') + ')' Description
from orders o
inner join entity c on o.customer = c.entity) a";
      OrderByClause = " order by Description ";
      IDName = "OrderID";
      NameName = "Description";

    }
    public string[] Search(string criteria, bool useExactID)
    {
      string sql = SQL;
      if (!string.IsNullOrEmpty(OrderStatus))
        sql = SQL + string.Format(" Where OrderStatus = '{0}'", OrderStatus);
      return getSearchList(sql, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
    }
  }
}
