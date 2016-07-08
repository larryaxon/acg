using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ACG.Common;
using CCI.Sys.Data;

namespace CCI.Sys.Data
{
  public class SearchDataSourceEntity : DataAccessBase, ISearchDataSource
  {
    protected string _entityType = string.Empty;
    protected string _entityOwner = string.Empty; 
    protected string _sql = "Select Entity, LegalName from Entity WHERE EntityType = '{0}'";
    protected const string _orderByClause = " order by LegalName ";
    protected const string _iDName = "Entity";
    protected const string _nameName = "LegalName";
    public string IDName { get; set; }
    public string NameName { get; set; }
    public string SQL { get; set; }
    public string OrderByClause { get; set; }
    public string EntityType { get { return _entityType; } set { _entityType = value; SQL = setSQL(); }  }
    public string EntityOwner { get { return _entityOwner; } set { _entityOwner = value; if (!string.IsNullOrEmpty(_entityOwner)) SQL = setSQL(); } }
    protected string alternateName = "AlternateName";
    public SearchDataSourceEntity(string entityType, string entityOwner)
    {
      EntityType = entityType;
      EntityOwner = entityOwner;
      setProperties();
    }
    public SearchDataSourceEntity(string entityType)
    {
      EntityType = entityType;
      setProperties();
    }
    public SearchDataSourceEntity()
    {
      setProperties();
    }
    private void setProperties()
    {
      SQL = _sql;
      OrderByClause = _orderByClause;
      IDName = _iDName;
      NameName = _nameName;
    }
    public string[] Search(string criteria, bool useExactID)
    {
      if (string.IsNullOrEmpty(EntityType))
        return new string[0];
      string sql = (string)this.GetType().GetMethod("setSQL").Invoke(this, new object[0]); // so inherited classes can override
      return getSearchList(sql, OrderByClause, criteria, IDName, new string[] { NameName, alternateName }, useExactID);
    }
    public string setSQL()
    {
      if (string.IsNullOrEmpty(_entityType))
        return _sql;
      else
        if (string.IsNullOrEmpty(_entityOwner))
          return string.Format(_sql, _entityType);
        else
          return string.Format("{0} AND EntityOwner = '{1}'", string.Format(_sql, _entityType), _entityOwner);
    }
  }
}
