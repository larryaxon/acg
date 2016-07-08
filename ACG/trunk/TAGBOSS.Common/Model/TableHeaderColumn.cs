using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// Defines a Column for the TableHeader
  /// </summary>
  [SerializableAttribute]
  public class TableHeaderColumn : FieldBase, IDataClassItem
  {
    private string caption = string.Empty;
    private int sortKey = 0;
    public bool Enabled { get; set; }
    public bool Required { get; set; }
    public new bool ReadOnly
    {
      get { return !Enabled; }
      set
      {
        base.ReadOnly = value;
        Enabled = !value;
      }
    }
    public PickList PickList { get; set; }
    public string PickListAttribute { get; set; }
    public string Mask { get; set; }
    public string Caption
    {
      get
      {
        if (caption != null && caption.Length > 0)
          return caption;
        if (description != null && description.Length > 0)
          return description;
        return origID;
      }
      set { description = value; caption = value; }
    }

    public int SortKey
    {
      get { return sortKey; }
      set { sortKey = value; }
    }

    public TableHeaderColumn()
    {
      Required = false;
      DataType = TAGFunctions.DATATYPESTRING;
      ReadOnly = false;
      PickListAttribute = string.Empty;
      PickList = new PickList(0);

    }
    public TableHeaderColumn(string columnName)
      : base()
    {
      ID = columnName;
    }
    public object Clone()
    {
      TableHeaderColumn c = new TableHeaderColumn();
      c.ID = origID;
      c.DataType = dataType;
      c.Deleted = isDeleted;
      c.Dirty = isDirty;
      c.Virtual = isVirtual;
      c.Value = myvalue;
      c.Required = Required;
      c.Enabled = Enabled;
      c.Caption = Caption;
      c.ReadOnly = isReadOnly;
      c.PickList = PickList;
      c.PickListAttribute = PickListAttribute;
      c.SortKey = SortKey;
      return c;
    }
  }
}
