using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceProductList: DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public string Carrier { get; set; }
    public SearchDataSourceProductList()
    {
//      SQL = "Select * from (select m.ItemID, m.Name from ProductList p inner join MasterProductList m on p.ItemId = m.ItemId {0}) p ";
      SQL = @"Select ItemID, Name
                from MasterProductList
                where IsSaddlebackUSOC = 0 ";
      OrderByClause = " order by Name ";
      IDName = "ItemID";
      NameName = "Name";

    }
    public string[] Search(string criteria, bool useExactID)
    {
      string carrierClause = string.Empty;
      if (string.IsNullOrEmpty(Carrier))
          Carrier = "CityHosted";

      SQL = string.Format(@"SELECT * FROM (SELECT dbo.MasterProductList.ItemID, dbo.MasterProductList.Name
FROM         dbo.MasterProductList INNER JOIN
             dbo.ProductList AS p ON dbo.MasterProductList.ItemID = p.ItemID
WHERE        p.Carrier = '{0}') p",Carrier);
//      return getSearchList(string.Format(SQL, carrierClause), OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
      //return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID);
        return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID, true);

    }
  }
}
