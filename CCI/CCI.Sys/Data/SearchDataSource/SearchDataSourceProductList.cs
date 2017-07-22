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
    public string PrimaryCarrier { get; set; }
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
      // create defaults for missing Carrier or Primary Carrier
      if (string.IsNullOrEmpty(Carrier))
      {
        if (string.IsNullOrEmpty(PrimaryCarrier))
        {
          Carrier = "CityHosted";
          PrimaryCarrier = "Saddleback";
        }
        else
        {
          Carrier = PrimaryCarrier;
        }
      }
      else
      {
        if (string.IsNullOrEmpty(PrimaryCarrier))
        {
          if (Carrier.Equals("CityHosted"))
          {
            PrimaryCarrier = "Saddleback";
          }
        }
      }

      SQL = string.Format(@"SELECT * FROM (SELECT dbo.MasterProductList.ItemID, dbo.MasterProductList.Name
FROM         dbo.MasterProductList INNER JOIN
             dbo.ProductList AS p ON dbo.MasterProductList.ItemID = p.ItemID
WHERE        p.Carrier = '{0}'
AND          p.PrimaryCarrier = '{1}') p", Carrier, PrimaryCarrier);

        return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID, true);

    }
  }
}
