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

using ACG.App.Common;
using ACG.Sys.Data;
using ACG.DesktopClient.Screens;

namespace ACG.DesktopClient.Common
{
  public partial class ctlMaintenanceBase : UserControl
  {
    #region public constants
    public const string colNOTE = "Note";
    public const string colLASTMODIFIEDBY = "LastModifiedBy";
    public const string colLASTMODIFIEDDATETIME = "LastModifiedDateTime";
    #endregion
    #region private module data

    private string _tableName = string.Empty;
    private string _encryptedFieldName = null;
    private bool _dirty = false;
    private bool _fireRowSelectedEvent = true;
    private int _rowIndex = 0;
    private DataSource _dSource = null;
    private bool _readOnly = false;
    private bool _leavingTxtValue = false;
    private string _noteLabel = "Note for";
    private string _extendedFieldCaptionColumn = null;
    private bool _updating = false;
    private DataGridViewRow _selectedRow = null;
    private int _nextColumnAfterExtendedField = -1;
    private bool _hasExtendedField
    {
      get { return !string.IsNullOrEmpty(ExtendedFieldName) && grdMaintenance.Columns.Contains(ExtendedFieldName); }
    }

    #endregion
    #region protected module data

    protected bool _suspendRowValidation = false;
    protected bool _refreshing = false;
    protected DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    protected DataAdapterContainer _da = new DataAdapterContainer();
    protected Dictionary<string, string> defaultParameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, DataGridViewComboBoxColumn> PickLists = new Dictionary<string, DataGridViewComboBoxColumn>(StringComparer.CurrentCultureIgnoreCase);

    #endregion
    #region public data and properties

    public int WideColumnWidth = 200;
    public Dictionary<string, string> ReadOnlyColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> WideColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, object> DefaultValues = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> HiddenColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public string TableName { get { return _tableName; } set { _tableName = value; } }
    public string EncryptedFieldName { get { return _encryptedFieldName; } set { _encryptedFieldName = value; if (_da != null) _da.EncryptedFieldName = _encryptedFieldName; } }
    public SecurityContext SecurityContext { get; set; }
    public string UniqueIdentifier { get; set; }
    public DataGridViewRow SelectedRow { get { return _selectedRow; } }
    public bool ReadOnly { get { return _readOnly; } set { _readOnly = value; } }
    public bool ShowExtendedField
    {
      get { return !splitMain.Panel2Collapsed; }
      set { splitMain.Panel2Collapsed = !value; }
    }
    public string ExtendedFieldName { get; set; }
    public string ExtendedFieldCaptionColumn { get { return _extendedFieldCaptionColumn; } set { _extendedFieldCaptionColumn = value; } }
    public string NoteLabel
    {
      get { return _noteLabel; }
      set { _noteLabel = value; }
    }
    public bool Dirty { get { return _dirty; } }
    public int RowCount { get { return grdMaintenance.RowCount; } }
    // do we fire the row selected event when we are processing the search button?
    public bool FireRowSelectedOnSearch { get { return _fireRowSelectedEvent; } set { _fireRowSelectedEvent = value; } }
    #endregion

    public ctlMaintenanceBase()
    {
      InitializeComponent();
      UniqueIdentifier = null;
      if (!ReadOnlyColumns.ContainsKey(colLASTMODIFIEDBY))
        ReadOnlyColumns.Add(colLASTMODIFIEDBY, null);
      if (!ReadOnlyColumns.ContainsKey(colLASTMODIFIEDDATETIME))
        ReadOnlyColumns.Add(colLASTMODIFIEDDATETIME, null);
      if (!WideColumns.ContainsKey(colNOTE))
        WideColumns.Add(colNOTE, null);
      txtValue.Text = string.Empty;
      grdMaintenance.ContextMenuStrip = mnuGrid;
    }

    #region public methods
    public void Clear()
    {
      if (_da.DataSet != null)
        _da.DataSet.Clear();
      if (grdMaintenance.DataSource != null)
        ((DataView)grdMaintenance.DataSource).Table.Rows.Clear();

    }
    public void updateFromDataAdapter(object sender, DataGridViewCellEventArgs e)
    {
      if (!_updating && _da.DataSet != null && _da.DataSet.Tables.Count > 0 && e.RowIndex >= 0 && e.RowIndex < _da.DataSet.Tables[0].Rows.Count)
      {
        DataTable dt = _da.DataSet.Tables[0];
        DataRow row = dt.Rows[e.RowIndex];
        switch (row.RowState)
        {
          case DataRowState.Added:
          case DataRowState.Modified:
            bool isAdded = (row.RowState == DataRowState.Added);
            DateTime dtUpdated = DateTime.Now;
            try
            {
              if (dt.Columns.Contains(colLASTMODIFIEDBY))
                row[colLASTMODIFIEDBY] = SecurityContext.User;
              if (dt.Columns.Contains(colLASTMODIFIEDDATETIME))
                row[colLASTMODIFIEDDATETIME] = dtUpdated;
            }
            catch { }
            foreach (KeyValuePair<string, object> defCols in DefaultValues)
            {
              if (dt.Columns.Contains(defCols.Key))
              {
                object val = row[defCols.Key];
                if (val == null || val == System.DBNull.Value)
                  try
                  {
                    row[defCols.Key] = defCols.Value;
                  }
                  catch { }
              }
            }
            try
            {
              _updating = true;
              //row.AcceptChanges();
              _da.Update(TableName);
              //_da.DataSet.AcceptChanges();
              _updating = false;
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message);
            }
            if (isAdded && !string.IsNullOrEmpty(UniqueIdentifier))
            {
              int id = _dataSource.getIDFromRowAdded(TableName, UniqueIdentifier, SecurityContext.User, dtUpdated);
              _refreshing = true;
              row[UniqueIdentifier] = id;
              dt.AcceptChanges();
            }
            break;
          case DataRowState.Deleted:
            try
            {
              _updating = true;
              _da.Update(TableName);
              //row.AcceptChanges();
              _updating = false;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message) ; }
            break;
        }
        _dirty = false;
      }
    }
    public bool ContainsColumn(string columnName)
    {
      return grdMaintenance.Columns.Contains(columnName);
    }
    #endregion
    #region private and protected methods
    public void load(Dictionary<string, string> parameters)
    {
      if (!parameters.ContainsKey(CommonData.parmENCRYPTEDFIELDNAME) && !string.IsNullOrEmpty(_encryptedFieldName))
        parameters.Add(CommonData.parmENCRYPTEDFIELDNAME, _encryptedFieldName);
      _da = _dataSource.getMaintenanceAdapter(TableName, parameters);
      CommonFormFunctions.displayDataSetGrid(grdMaintenance, _da.DataSet);
      if (!_hasExtendedField)
        ShowExtendedField = false;
      foreach (KeyValuePair<string, string> hCols in HiddenColumns)
        if (grdMaintenance.Columns.Contains(hCols.Key))
          grdMaintenance.Columns[hCols.Key].Visible = false;
      foreach (KeyValuePair<string, string> roCols in ReadOnlyColumns)
        if (grdMaintenance.Columns.Contains(roCols.Key))
          grdMaintenance.Columns[roCols.Key].ReadOnly = true;
      if (_readOnly)
      {
        grdMaintenance.AllowUserToAddRows = false;
        foreach (DataGridViewColumn col in grdMaintenance.Columns)
          col.ReadOnly = true;
      }
      foreach (KeyValuePair<string, string> wideColumn in WideColumns)
        if (grdMaintenance.Columns.Contains(wideColumn.Key))
          grdMaintenance.Columns[wideColumn.Key].Width = WideColumnWidth;
      grdMaintenance.AllowUserToResizeColumns = true;
      grdMaintenance.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
    }
    private void launchMaintenance(string entity)
    {
      //string entityType = _dataSource.getEntityType(entity);
      //if (!string.IsNullOrEmpty(entityType))
      //{
      //  ScreenBase frm;
      //  switch (entityType.ToLower())
      //  {
      //    case "prospect":
      //    case "ibpprospect":
      //      frm = new frmProspects();
      //      ((frmProspects)frm).EntityType = entityType;
      //      frm = (ScreenBase)CommonFormFunctions.FindMatchingChild(CommonFormFunctions.getMainForm(this), frm);
      //      frm.SecurityContext = SecurityContext;
      //      object[] pList = new object[] { entity };
      //      frm.GetType().GetMethod("Init").Invoke(frm, pList); // this syntax is to make sure the inherited form method is the one we invoke

      //      //((frmProspects)frm).Init(entity);
      //      break;
      //    default:
      //      frm = new frmEntityMaintenance();
      //      frm.SecurityContext = SecurityContext;
      //      ((frmEntityMaintenance)frm).Init(entity);
      //      break;
      //  }
      //  MainForm m = CommonFormFunctions.getMainForm(this);
      //  m.ShowForm(frm, true);
      //}
     
    }
    protected DataGridViewComboBoxColumn loadColumn(DataGridViewComboBoxColumn col, string colName, object colList)
    {
      return loadColumn(col, colName, colList, false);
    }
    protected DataGridViewComboBoxColumn loadColumn(DataGridViewComboBoxColumn col, string colName, object colList, bool forceReload)
    {
      //_suspendRowValidation = true;
      int colIndex = 0;
      if (col != null && (col.Items.Count == 0 || forceReload))
      {

        col.DataPropertyName = colName;
        col.Name = colName;
        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        col.Resizable = DataGridViewTriState.True;
        string type = colList.GetType().Name.ToLower();
        switch (type)
        {
          case "picklistentries":
            col.ValueMember = "IDupper";
            col.DisplayMember = "Description";
            col.Items.Clear();
            foreach (PickListEntry item in (PickListEntries)colList)
              col.Items.Add(item);
            break;
          case "array":
            col.Items.Clear();
            col.Items.AddRange(colList);
            break;
          case "searchresultcollection":
            col.ValueMember = "IDupper";
            col.DisplayMember = "LegalName";
            col.Items.Clear();
            foreach (SearchResult s in (SearchResultCollection)colList)
              col.Items.Add(s);
            break;
          case "itemlistcollection":
            col.ValueMember = "IDupper";
            col.DisplayMember = "ItemIDandDescription";
            col.Items.Clear();
            foreach (ItemListEntry i in (ItemListCollection)colList)
              col.Items.Add(i);
            break;
        }
      }
      if (grdMaintenance.Columns.Contains(colName))
      {
        colIndex = grdMaintenance.Columns[colName].Index;
        grdMaintenance.Columns.Remove(colName);
      }
      grdMaintenance.Columns.Insert(colIndex, col);
      if (!PickLists.ContainsKey(colName))
        PickLists.Add(colName, col);
      //_suspendRowValidation = false;
      return col;
    }
    #endregion
    #region form events
    #region custom events
    public delegate void RowSelectedHandler(object sender, MaintenanceGridRowSelectedArgs e);
    public event RowSelectedHandler RowSelected;
    protected void OnRowSelected(MaintenanceGridRowSelectedArgs e)
    {
      if (RowSelected != null)
      {
        RowSelected(this, e);
      }
    }
    private void raiseRowChangedEvent(DataGridViewRow row)
    {
      MaintenanceGridRowSelectedArgs e = new MaintenanceGridRowSelectedArgs();
      e.SelectedRow = row;
      OnRowSelected(e);
    }
    #endregion
    #region public events
    public void grdMaintenance_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
      //_suspendRowValidation = false;
      int rowIndex = _rowIndex = e.RowIndex;
      _selectedRow = grdMaintenance.Rows[rowIndex];
      if (grdMaintenance.CurrentCell == null)
        _selectedRow.Cells[0].Selected = true;
      if (ShowExtendedField)
      {
        if (_hasExtendedField && (grdMaintenance.CurrentCell != null &&
          grdMaintenance.CurrentCell.ColumnIndex < grdMaintenance.Columns[ExtendedFieldName].Index))
        {
          txtValue.Text = CommonFunctions.CString(_selectedRow.Cells[ExtendedFieldName].Value);
          if (_nextColumnAfterExtendedField < 0)
            _nextColumnAfterExtendedField = grdMaintenance.Columns[ExtendedFieldName].Index + 1;
          if (_nextColumnAfterExtendedField >= grdMaintenance.Columns.Count)
            _nextColumnAfterExtendedField = -1;
          if (_extendedFieldCaptionColumn == null || !grdMaintenance.Columns.Contains(_extendedFieldCaptionColumn))
            txtNoteLabel.Text = _noteLabel;
          else
            txtNoteLabel.Text = string.Format("{0} {1}", _noteLabel, CommonFunctions.CString(_selectedRow.Cells[ExtendedFieldCaptionColumn].Value));
        }
      }
      raiseRowChangedEvent(_selectedRow);
    }
    public void grdMaintenance_CellLeave(object sender, DataGridViewCellEventArgs e)
    {
      // doesn't do anything but exposes this for inherited forms
    }
    public void grdMaintenance_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      // doesn't do anything but exposes this for inherited forms
    }    
    public void grdMaintenance_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      // doesn't do anything but exposes this for inherited forms
    }
    public void grdMaintenance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      
    }
    #endregion
    #region private events
    private void grdMaintenance_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (!_refreshing)
      {
        object[] pLIst = new object[] { sender, e };
        string saveTableName = TableName;
        if (TableName.Equals("SearchNetworkInventory"))
          TableName = "NetworkInventory";
        this.GetType().GetMethod("updateFromDataAdapter").Invoke(this, pLIst);
        TableName = saveTableName;
      }
      _refreshing = false;
    }
    /// <summary>
    ///  stubbed out version that always returns true, required by rowValidated event
    /// </summary>
    /// <returns>true if row passes validation, false if not</returns>
    public bool isValidRow()
    {
      return true;
    }
    private void grdMaintenance_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      Exception ex = e.Exception;
      //while (ex.InnerException != null)
      //{
      //  ex = ex.InnerException;
      //}
      //MessageBox.Show(string.Format("Error {0} in cell[{1},{2}]", ex.Message, e.RowIndex.ToString(), e.ColumnIndex.ToString()));
    }
    private void grdMaintenance_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      //DataGridView.HitTestInfo hit = grdMaintenance.HitTest(e.X, e.Y);
      //if (hit.RowIndex >= 0 && hit.RowIndex < grdMaintenance.Rows.Count)
      //{
      //  DataGridViewRow row = grdMaintenance.Rows[hit.RowIndex];
      //  if (grdMaintenance.Columns.Contains("Entity"))
      //  {
      //    string entity = CommonFunctions.CString(row.Cells["Entity"].Value);
      //    launchMaintenance(entity);
      //  }
      //}
    }
    private void grdMaintenance_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
    {
      _suspendRowValidation = false;
    }
    private void txtValue_TextChanged(object sender, EventArgs e)
    {
      if (_hasExtendedField)
      {
        if (_rowIndex >= 0 && _rowIndex < grdMaintenance.Rows.Count)
        {
          DataGridViewRow row = grdMaintenance.Rows[_rowIndex];
          row.Cells[ExtendedFieldName].Value = txtValue.Text;
        }
      }
    }
    private void grdMaintenance_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        if (_hasExtendedField && grdMaintenance.Columns[e.ColumnIndex].Name.Equals(ExtendedFieldName))
          txtValue.Focus();
      }
      catch (Exception ex) 
      { ; }
    }
    private void txtValue_Leave(object sender, EventArgs e)
    {
      if (!_leavingTxtValue)
      {
        if (_nextColumnAfterExtendedField >= 0)
        {
          int colIdx = _selectedRow.Cells[_nextColumnAfterExtendedField].ColumnIndex;
          grdMaintenance.CurrentCell = _selectedRow.Cells[_nextColumnAfterExtendedField];
          _selectedRow.Cells[colIdx].Selected = true;
          _leavingTxtValue = true;
          grdMaintenance.Focus();
          _leavingTxtValue = false;
        }
      }
    }
    private void grdMaintenance_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      DataGridViewRow newRow = e.Row;
      int index = newRow.Index - 1;
      if (index < 0)
        index = 0;
      DataGridViewRow row = grdMaintenance.Rows[index];
      // then set default values
      foreach (KeyValuePair<string, object> defaultValue in DefaultValues)
      {
        if (grdMaintenance.Columns.Contains(defaultValue.Key))
        {
          index = grdMaintenance.Columns[defaultValue.Key].Index;
          row.Cells[index].Value = defaultValue.Value;
        }
      }      
      // set up pick lists for new row
      //foreach (KeyValuePair<string, DataGridViewComboBoxColumn> pickList in PickLists)
      //{
      //  if (grdMaintenance.Columns.Contains(pickList.Key))
      //  {
      //    index = grdMaintenance.Columns[pickList.Key].Index;
      //    row.Cells[index] = (DataGridViewCell)((DataGridViewCell)pickList.Value).Clone();
      //    newRow.Cells[index] = (DataGridViewCell)((DataGridViewCell)pickList.Value).Clone();
      //  }
      //}

    }
    private void exportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CommonFormFunctions.exportToExcel(grdMaintenance);
    }
    #endregion

    protected void grdMaintenance_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (!_suspendRowValidation)
        e.Cancel = !((bool)this.GetType().GetMethod("isValidRow").Invoke(this, new object[0])); //only do the update if the row is valid
      return;
               
    }


    #endregion

    private void grdMaintenance_CancelRowEdit(object sender, QuestionEventArgs e)
    {
      _suspendRowValidation = true;
    }
  }
}
