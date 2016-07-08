using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceMasterDealer : DataAccessBase, ISearchDataSource 
  {
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public SearchDataSourceMasterDealer()
    {
      SQL = @"Select * from 
(  Select top 1 'None' Dealer, 'No Master Dealer' DealerName from HostedDealerCosts dc1
UNION
select distinct Dealer, legalname DealerName 
from HostedDealerCosts dc
inner join Entity e on dc.Dealer = e.entity 
where itemid like 'Master%'
) a";
      OrderByClause = " order by DealerName ";
      IDName = "Dealer";
      NameName = "DealerName";

    }
    public string[] Search(string criteria, bool useExactID)
    {
      return getSearchList(SQL, OrderByClause, criteria, IDName, new string[] { NameName }, useExactID, true);
    }
  }
}
