using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using ACG.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Screens;

namespace CCI.DesktopClient.Common
{
  public partial class ctlSearchGrid : ACG.CommonForms.ctlSearchGrid
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    public new CCI.Common.CommonData.UnmatchedNameTypes NameType
    {
      get { return getType(base.NameType); }
      set { base.NameType = value.ToString(); }
    }
    public ctlSearchGrid() : base()
    {
      this.grdResearch.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdResearch_CellValidating);  
    }

    public void Init(CCI.Common.CommonData.UnmatchedNameTypes nameType, string name)
    {
      base.Init(nameType.ToString(), name);
    }

    private void grdResearch_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      // this is only for checked/scrubbed fields
      string columnName = grdResearch.Columns[e.ColumnIndex].Name;
      if (columnName.Equals(CheckBoxColumn, StringComparison.CurrentCultureIgnoreCase) &&
        grdResearch.Columns.Contains(UniqueIdentifier))
      {
        try
        {
          DataGridViewRow row = grdResearch.Rows[e.RowIndex];
          string id = CommonFunctions.CString(row.Cells[UniqueIdentifier].Value);
          bool isChecked = CommonFunctions.CBoolean(row.Cells[e.ColumnIndex].EditedFormattedValue);
          _dataSource.checkResearchRecord(NameType, id, isChecked);
        }
        catch (Exception ex)
        {
          CommonFormFunctions.showException(ex);
        }
      }

    }
    private CCI.Common.CommonData.UnmatchedNameTypes getType(string name)
    {
      try
      {
        return (CCI.Common.CommonData.UnmatchedNameTypes)Enum.Parse(typeof(CCI.Common.CommonData.UnmatchedNameTypes), name, true);
      }
      catch
      {
        return CCI.Common.CommonData.UnmatchedNameTypes.None;
      }
    }
  }
}