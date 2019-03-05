using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceCustomer : SearchDataSourceEntity
  {
    private string _dealer = null;
    public string Dealer { get { return _dealer; } set { _dealer = value; } }
    public SearchDataSourceCustomer() : base("Customer", null)
    {
      _sql =
@"Select isnull(dlr.SalesOrDealer, 'CCIDealer') Dealer, Entity, LegalName from Entity e 
left join SalesOrDealerCustomers dlr on e.Entity = dlr.Customer and dlr.SalesType = 'Dealer'
WHERE EntityType = 'Customer'";
    }
    public new string[] Search(string criteria, bool useExactID)
    {
      if (string.IsNullOrEmpty(EntityType))
        return new string[0];
      string sql = (string)this.GetType().GetMethod("setSQL").Invoke(this, new object[0]); // so inherited classes can override
      return getSearchList(sql, OrderByClause, criteria, IDName, new string[] { NameName, alternateName }, useExactID);
    }
    public new string setSQL()
    {
      string sql = base.setSQL();
      if (!string.IsNullOrEmpty(_dealer))
        sql = string.Format("{0} AND isnull(dlr.SalesOrDealer, 'CCIDealer') = '{1}'", sql, Dealer);
      return sql;
    }
  }
}
