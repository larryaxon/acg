using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmMaintenanceBase : ScreenBase
  {
    public const string colLASTMODIFIEDBY = "LastModifiedBy";
    public const string colLASTMODIFIEDDATETIME = "LastModifiedDateTime";
    DataSource _dSource = null;
    private DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    private DataAdapterContainer _da = new DataAdapterContainer();
    public Dictionary<string, object> DefaultValues = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, string> defaultParameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, string> HiddenColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, string> ReadOnlyColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public string TableName = "";

    public frmMaintenanceBase()
    {
      InitializeComponent();
      if (!ReadOnlyColumns.ContainsKey(colLASTMODIFIEDBY))
        ReadOnlyColumns.Add(colLASTMODIFIEDBY, null);
      if (!ReadOnlyColumns.ContainsKey(colLASTMODIFIEDDATETIME))
        ReadOnlyColumns.Add(colLASTMODIFIEDDATETIME, null);      
    }

    protected void load(Dictionary<string, string> parameters)
    {
      _da = _dataSource.getMaintenanceAdapter(TableName, parameters);
      CommonFormFunctions.displayDataSetGrid(grdMaintenance, _da.DataSet);
      foreach (KeyValuePair<string, string> hCols in HiddenColumns)
        if (grdMaintenance.Columns.Contains(hCols.Key))
          grdMaintenance.Columns[hCols.Key].Visible = false;
      foreach (KeyValuePair<string, string> roCols in ReadOnlyColumns)
        if (grdMaintenance.Columns.Contains(roCols.Key))
          grdMaintenance.Columns[roCols.Key].ReadOnly = true;
    }

    private void grdMaintenance_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (_da.DataSet != null && _da.DataSet.Tables.Count > 0 && e.RowIndex >= 0 && e.RowIndex < _da.DataSet.Tables[0].Rows.Count)
      {
        switch (_da.DataSet.Tables[0].Rows[e.RowIndex].RowState)
        {
          case DataRowState.Added:
          case DataRowState.Modified:
            if (_da.DataSet.Tables[0].Columns.Contains(colLASTMODIFIEDBY))
              _da.DataSet.Tables[0].Rows[e.RowIndex][colLASTMODIFIEDBY] = SecurityContext.User;
            if (_da.DataSet.Tables[0].Columns.Contains(colLASTMODIFIEDDATETIME))
              _da.DataSet.Tables[0].Rows[e.RowIndex][colLASTMODIFIEDDATETIME] = DateTime.Now;
            foreach (KeyValuePair<string, object> defCols in DefaultValues)
            {
              object val = _da.DataSet.Tables[0].Rows[e.RowIndex][defCols.Key];
              if (val == null || val == System.DBNull.Value)
                _da.DataSet.Tables[0].Rows[e.RowIndex][defCols.Key] = defCols.Value;
            }
            _da.DataAdapter.Update(_da.DataSet, TableName);
            break;
          case DataRowState.Deleted:
            _da.DataAdapter.Update(_da.DataSet, TableName);
            break;
        }
      }
    }

  }
}
