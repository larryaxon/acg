using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using ACG.App.Common;
using ACG.Sys.Data;
using ACG.DesktopClient.Screens;

namespace ACG.DesktopClient.Common
{
  public partial class ctlSearchGrid : UserControl
  {
    private string _selectedID = string.Empty;
    private const string SEARCHLABEL = "Search ({0} Records Found)";
    private string _uniqueIdentifier = "ID";
    private int _selectedRowIndex = -1;
    private bool _exportCriteria = false;
    private DataSource _dSource = null;
    private DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    private DataSet _dsRaw = null;
    private DataSet _dsResearch = null;
    private string _name = string.Empty;
    private string _fieldName = "CustomerName";
    private bool _canSearchLockedColumns = false;
    public const string opSTARTSWITH = "Starts With";
    public const string opCONTAINS = "Contains";
    public const string opENDSWITH = "Ends With";
    public const string opEQUALS = "Equals";
    public const string opNOTEQUALS = "Does Not Equal";
    public const string opNOTCONTAINS = "Does Not Contain";
    public const string opISNULL = "Is Null";
    public const string opISNOTNULL = "Is Not Null";
    public const string opGREATERTHAN = "Greater Than";
    public const string opLESSTHAN = "Less Than";
    public const string opGREATEROREQUAL = "Greater or Equal";
    public const string opLESSOREQUAL = "Less or Equal";
    private const string colField = "Field";
    private const string colOperator = "Operator";
    private const string colValue = "Value";
    private bool _searchColumnsLoaded = false;
    private CommonData.NameTypes _nameType;
    private SortedDictionary<string, string> _fields = new SortedDictionary<string,string>(StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, string[]> _searchCriteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
    private string[] operatorList = new string[] { opSTARTSWITH, 
                                                    opCONTAINS,
                                                    opENDSWITH,
                                                    opEQUALS,
                                                    opNOTEQUALS,
                                                    opNOTCONTAINS,
                                                    opISNULL,
                                                    opISNOTNULL,
                                                    opGREATERTHAN,
                                                    opGREATEROREQUAL,
                                                    opLESSTHAN,
                                                    opLESSOREQUAL};
    private Dictionary<string, string> normalOperators = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) { { opEQUALS, " = " }, 
                                                                                                                      { opNOTEQUALS, " <> "},
                                                                                                                      { opGREATERTHAN, " > "},
                                                                                                                      { opGREATEROREQUAL, " >= "},
                                                                                                                      { opLESSTHAN, " < "},
                                                                                                                      { opLESSOREQUAL, " <= "} };
    public int LastRowSelected { get { return _selectedRowIndex; } }
    public CommonData.NameTypes NameType 
    { 
      get { return _nameType; }
      set
      {
        _nameType = value;
      }
    }
    public Dictionary<string, string> HiddenColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> LockedColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public bool CanSearchLockedColumns { get { return _canSearchLockedColumns; } set { _canSearchLockedColumns = value; } }
    public string ColumnName { get { return _fieldName; } set { _fieldName = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Title { get { return lblTitle.Text; } set { lblTitle.Text = value; } }
    public string SelectedID { get { return _selectedID; } }
    public string UniqueIdentifier { get { return _uniqueIdentifier; } set { _uniqueIdentifier = value; } }
    public string InnerWhere { get; set; }
    public DataGridViewRow SelectedRow { get { if (_selectedRowIndex < 0) return null; else return grdResearch.Rows[_selectedRowIndex]; } }
    public bool DisplaySearchCriteria { get { return !splitMain.Panel1Collapsed; } set { splitMain.Panel1Collapsed = !value; } }
    public string CheckBoxColumn = string.Empty;

    public bool ExportCriteria
    {
      get { return _exportCriteria; }
      set { _exportCriteria = value; }
    }
    public Dictionary<string, string[]> SearchCriteria 
    { 
      get { return _searchCriteria; } 
      set
      {
        Dictionary<string, string[]> tmpList = value; 
        _searchCriteria.Clear();
        foreach (KeyValuePair<string, string[]> entry in tmpList)
        {
          string key = entry.Key;
          switch (key.ToLower())
          {
            case "customername":
              key = CommonData.CUSTOMER;
              break;
          }
          // now fixup so it doesn't crash the pick list
          foreach (KeyValuePair<string, string> f in _fields)
            if (key.Equals(f.Key, StringComparison.CurrentCultureIgnoreCase))
            {
              key = f.Key;
              break;
            }
          if (_fields.ContainsKey(key))
            if (!_searchCriteria.ContainsKey(key))
              _searchCriteria.Add(key, entry.Value);
            else
              _searchCriteria[key] = entry.Value;
        }
        LoadSearchGrid(_searchCriteria);
      }
    }
    public ctlSearchGrid()
    {
      InitializeComponent();
      InnerWhere = string.Empty;
      grdResearch.ContextMenuStrip = mnuGridContext;
    }
    public void Load(CommonData.NameTypes nameType, string name)
    {
      _name = name;
      NameType = nameType;
      loadSearch();
    }
    public void ReLoad()
    {
      search();
    }
    public void Clear()
    {
      //foreach (DataGridViewRow row in grdResearch.Rows)
      //  grdResearch.Rows.Remove(row);
      grdResearch.ClearSelection();
      grdResearch.DataSource = null;
      grdResearch.Rows.Clear();
    }
    public List<string> getIDList()
    {
      List<string> idList = new List<string>();
      if (grdResearch.SelectedRows != null && grdResearch.SelectedRows.Count > 0 && grdResearch.Columns.Contains("ID"))
      {
        foreach (DataGridViewRow row in grdResearch.SelectedRows)
        {
          string id = CommonFunctions.CString(row.Cells["ID"].Value);
          idList.Add(id);
        }
      }
      return idList;
    }
    private void btnSearch_Click(object sender, EventArgs e)
    {
      search();
    }
    private void grdResearch_SelectionChanged(object sender, EventArgs e)
    {
      if (grdResearch.SelectedRows != null && grdResearch.SelectedRows.Count > 0)
      {
        //if (grdResearch.SelectedRows[0].Index != 0 || _selectedRowIndex == -1)
          _selectedRowIndex = grdResearch.SelectedRows[0].Index;  // only select the first row selected
        if (grdResearch.Columns.Contains(UniqueIdentifier))
          _selectedID = CommonFunctions.CString(grdResearch.Rows[_selectedRowIndex].Cells[UniqueIdentifier].Value);
        raiseRowChangedEvent(grdResearch.SelectedRows[0]);
      }
    }
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
    public void LoadSearchGrid(Dictionary<string, string[]> criteria)
    {
      grdSearch.Rows.Clear();
      foreach (KeyValuePair<string, string[]> entry in criteria)
      {
        string[] values = new string[grdSearch.Columns.Count];
        values[0] = entry.Key;
        values[1] = entry.Value[0];
        values[2] = entry.Value[1];
        grdSearch.Rows.Add(values);
        if (HiddenColumns.ContainsKey(entry.Key) && !CanSearchLockedColumns)
          grdSearch.Rows[grdSearch.Rows.Count - 1].ReadOnly = true;
      }
      grdSearch.Refresh();
    }
    private void search()
    {
      Cursor.Current = Cursors.WaitCursor;
      string where = getSearchWhere();
      if (string.IsNullOrEmpty(InnerWhere))
        _dsResearch = _dataSource.getResearchData(NameType, where, -1);
      else // if there is an inner where, use the alternate use of the where clause with an array of outer and inner where clauses
        _dsResearch = _dataSource.getResearchData(NameType, new string[] { where, InnerWhere }, -1);
      CommonFormFunctions.displayDataSetGrid(grdResearch, _dsResearch);
      lblTitle.Text = string.Format(SEARCHLABEL, ((DataView)grdResearch.DataSource).Count.ToString());
      // Hide the hidden columns
      foreach (KeyValuePair<string, string> entry in HiddenColumns)
      {
        if (grdResearch.Columns.Contains(entry.Key))
          grdResearch.Columns[entry.Key].Visible = false;
        else
          grdResearch.Columns[entry.Key].Visible = true;
      }
      setMRCTotal();
      //if (ExportCriteria)
      //  ((ACG.DesktopClient.Commissions.frmUnmatchedNetworkInventory)this.Parent.Parent.Parent).setGridCriteria(_searchCriteria); // ok, i KNOW this is ugly but I was in a hurry
      grdResearch.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
      if (_selectedRowIndex >= 0)
      {
        if (_selectedRowIndex < grdResearch.Rows.Count)
        {
          grdResearch.Rows[_selectedRowIndex].Selected = true;
          grdResearch.FirstDisplayedScrollingRowIndex = _selectedRowIndex;
        }
        else
          _selectedRowIndex = -1;
      }
      Cursor.Current = Cursors.Default;
    }
    private void loadSearch()
    {
      if (!_searchColumnsLoaded)
      {
        _searchColumnsLoaded = true;
        _fields = _dataSource.getResearchFields(NameType);
        grdSearch.Rows.Clear();
        DataGridViewComboBoxCell op = new DataGridViewComboBoxCell();
        DataGridViewComboBoxCell fld = new DataGridViewComboBoxCell();
        //op.DefaultNewRowValue = opCONTAINS;

        foreach (KeyValuePair<string, string> entry in _fields)
          fld.Items.Add(entry.Key);
        op.Items.AddRange(operatorList);
        DataGridViewColumn col = new DataGridViewColumn(fld);
        col.Name = colField;
        grdSearch.Columns.Add(col);
        col = new DataGridViewColumn(op);
        col.Name = colOperator;
        grdSearch.Columns.Add(col);
        grdSearch.Columns.Add(colValue, colValue);
      }
    }
    private string getSearchWhere()
    {

                                                                                                                       
      StringBuilder sbWhere = new StringBuilder();
      bool firstTime = true;
      _searchCriteria.Clear();
      foreach (DataGridViewRow row in grdSearch.Rows)
      {
        string op = CommonFunctions.CString(row.Cells[1].Value);
        string val = CommonFunctions.CString(row.Cells[2].Value);
        string fld = CommonFunctions.CString(row.Cells[0].Value);
        if (!string.IsNullOrEmpty(op) && !string.IsNullOrEmpty(fld) && !string.IsNullOrEmpty(val))
        {
          
          if (firstTime)
            firstTime = false;
          else
            sbWhere.Append(" AND ");
          //if (fld.Equals("Customer", StringComparison.CurrentCultureIgnoreCase) &&
          //    grdResearch.Columns.Contains("CustomerName"))
          //  fld = "CustomerName";
          _searchCriteria.Add(fld, new string[] { op, val });         
          sbWhere.Append(fld);
          switch (op)
          {
            case opEQUALS:
            case opNOTEQUALS:
            case opGREATERTHAN:
            case opGREATEROREQUAL:
            case opLESSTHAN:
            case opLESSOREQUAL:
              sbWhere.Append(normalOperators[op]);
              sbWhere.Append("'");
              sbWhere.Append(row.Cells[colValue].Value.ToString());
              sbWhere.Append("'");
              break;
            case opCONTAINS:
              sbWhere.Append(string.Format(" LIKE '%{0}%'", val));
              break;
            case opSTARTSWITH:
              sbWhere.Append(string.Format(" LIKE '{0}%'", val));
              break;
            case opENDSWITH:
              sbWhere.Append(string.Format(" LIKE '%{0}'", val));
              break;
            case opNOTCONTAINS:
              sbWhere.Append(string.Format(" NOT LIKE '%{0}%'", val));
              break;
            case opISNULL:
              sbWhere.Append(" IS NULL");
              break;
            case opISNOTNULL:
              sbWhere.Append(" IS NOT NULL");
              break;

          }
        }
      }
      return sbWhere.ToString();
    }

    private void grdSearch_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
    {
      e.Row.Cells[colOperator].Value = opCONTAINS;
    }

    private void setMRCTotal()
    {
      if (grdResearch.Columns.Contains("MRC") || grdResearch.Columns.Contains("MRCRaw") || grdResearch.Columns.Contains("CommissionMRC") || grdResearch.Columns.Contains("mthlyrecrev"))
      {
        lblMRCTotal.Visible = true;
        lblMRCTotal.Text = string.Format("Total MRC = {0}", getMRCTotal().ToString("c"));
      }
      else
        lblMRCTotal.Visible = false;
    }
    private decimal getMRCTotal()
    {
      decimal total = 0;
      string colName = "MRC";
      if (grdResearch.Columns.Contains("CommissionMRC"))
        colName = "CommissionMRC";
      else
        if (grdResearch.Columns.Contains("MRCRaw"))
          colName = "MRCRaw";
        else
          if (grdResearch.Columns.Contains("mthlyrecrev"))
            colName = "mthlyrecrev";
      foreach (DataGridViewRow row in grdResearch.Rows)
        total += CommonFunctions.CDecimal(row.Cells[colName].Value, 0);
      return total;
    }

    private void grdResearch_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      // this is only for checked/scrubbed fields
      if (grdResearch.Columns[e.ColumnIndex].Name.Equals(CheckBoxColumn, StringComparison.CurrentCultureIgnoreCase) &&
        grdResearch.Columns.Contains(UniqueIdentifier))
      {
        try
        {
          DataGridViewRow row = grdResearch.Rows[e.RowIndex];
          string id = CommonFunctions.CString(row.Cells[UniqueIdentifier].Value);
          bool isChecked = CommonFunctions.CBoolean(row.Cells[e.ColumnIndex].EditedFormattedValue);
          //_dataSource.checkResearchRecord(NameType, id, isChecked);
        }
        catch (Exception ex)
        {
          CommonFormFunctions.showException(ex);
        }
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      CommonFormFunctions.exportToExcel(grdResearch);
    }

    private void exportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CommonFormFunctions.exportToExcel(grdResearch);
    }

    private void copyCellToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CommonFormFunctions.copyCellContents(grdResearch);
    }

    private void grdSearch_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {

    }

    private void grdSearch_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (e.RowIndex >= 0 && grdSearch.Columns[e.ColumnIndex].Name.Equals(colValue, StringComparison.CurrentCultureIgnoreCase))
      {
        DataGridViewRow row = grdSearch.Rows[e.RowIndex];
        string fldName = CommonFunctions.CString(row.Cells[colField].Value);
        if (!CanSearchLockedColumns && LockedColumns.ContainsKey(fldName)) // this is a locked column and the user is not allowed to change its value
        {
          string fldVal = CommonFunctions.CString(row.Cells[colValue].Value);
 
          if (row.Cells[colValue].IsInEditMode)
          {
            e.Cancel = true;
            MessageBox.Show(string.Format("You cannot edit the search criteria for {0}", fldName));
          }
        }
      }
    }

  }
}
