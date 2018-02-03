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
using ACG.Common.Data;

namespace ACG.CommonForms
{
  public partial class ctlSearchGrid : UserControl
  {
    #region private data
    private const int FIELDSBUTTONNORMALHEIGHT = 90;
    private const int MAINPANENORMALHEIGHT = FIELDSBUTTONNORMALHEIGHT * 2;
    private const string CELLEDITBEGIN = "Editing";
    private const string CELLNOTEDIT = "NotEditing";
    private string _selectedID = string.Empty;
    private const string SEARCHLABEL = "Search ({0} Records Found)";
    private string _uniqueIdentifier = "ID";
    private int _selectedRowIndex = -1;
    private int _lastSelectedRow = -1;
    private int _verticalButtonMargin = 2;
    private SearchDataSourceGrid _dSource = null;
    private SearchDataSourceGrid _dataSource { get { if (_dSource == null) _dSource = new SearchDataSourceGrid(); return _dSource; } }
    private DataSet _dsResearch = null;
    private string _name = string.Empty;
    private string _fieldName = "";
    private string _lastColumnSorted = string.Empty;
    private ListSortDirection _lastColumnSortDirection = ListSortDirection.Ascending;
    private bool _canChangeDisplaySearchCriteria = true;
    private bool _canChangeDisplayFields = true;
    private bool _cancelPostbackLoaded = false;
    private bool _displayFieldsLoaded = false;
    private bool _loadingDisplayFields = false;
    private bool _autoRefreshWhenFieldChecked = false;
    private bool _fieldsDefaultIsChecked = true;
    private bool _useNamedSearches = false;
    private bool _autoSaveUserOptions = false;
    private bool _firstTime = true;
    private bool _includeGroupAsCriteria = false;
    private Dictionary<string, string> _rowCheckColumns = new Dictionary<string,string>(StringComparer.CurrentCultureIgnoreCase);
    private List<string> _idList = null;
    private bool _searchColumnsLoaded = false;
    private bool _searching = false;
    private bool _searchPaneEdited = false;
    private string _nameType;
    private SortedDictionary<string, string> _fields = new SortedDictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, string[]> _searchCriteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);

    private string _fieldsCaption 
    { 
      get 
      {
        if (btnShowFields.Visible)
        {
          if (btnShowFields.Height > FIELDSBUTTONNORMALHEIGHT) return "Fields";
          else if (btnShowFields.Height > FIELDSBUTTONNORMALHEIGHT / 2) return "Fld";
          else return "F";
        }
        else return string.Empty; 
      } 
    }
    private string _filterCaption 
    { 
      get 
      {
        if (btnShowFields.Visible)
        {
          if (btnShowFields.Height > FIELDSBUTTONNORMALHEIGHT) return "Search";
          else if (btnShowFields.Height > FIELDSBUTTONNORMALHEIGHT / 2) return "Srh"; 
          else return "S";
        }
        else 
          return string.Empty; 
      } 
    }
    #endregion

    #region constants
    public const string opSTARTSWITH = "Starts With";
    public const string opCONTAINS = "Contains";
    public const string opENDSWITH = "Ends With";
    public const string opEQUALS = "Equals";
    public const string opNOTEQUALS = "Does Not Equal";
    public const string opNOTCONTAINS = "Does Not Contain";
    public const string opISNULL = "Is Null";
    public const string opISNOTNULL = "Is Not Null";
    public const string opISBLANK = "Is Blank or Null";
    public const string opISNOTBLANK = "Is Not Blank or Null";
    public const string opGREATERTHAN = "Greater Than";
    public const string opLESSTHAN = "Less Than";
    public const string opGREATEROREQUAL = "Greater or Equal";
    public const string opLESSOREQUAL = "Less or Equal";
    public const string opIN = "IN";
    public const string opNOTIN = "Not IN";
    public const string opBETWEEN = "Between";
    public const string opINGROUPEXACT = "In Group Equals";
    public const string opINGROUPLIKE = "In Group Contains";
    private const string colField = "Field";
    private const string colOperator = "Operator";
    private const string colValue = "Value";
    private string[] operatorList = new string[] { opSTARTSWITH, 
                                                    opCONTAINS,
                                                    opENDSWITH,
                                                    opEQUALS,
                                                    opNOTEQUALS,
                                                    opNOTCONTAINS,
                                                    opISNULL,
                                                    opISNOTNULL,
                                                    opISBLANK,
                                                    opISNOTBLANK,
                                                    opGREATERTHAN,
                                                    opGREATEROREQUAL,
                                                    opLESSTHAN,
                                                    opLESSOREQUAL,
                                                    opIN,
                                                    opNOTIN,
                                                    opBETWEEN};
    private Dictionary<string, string> normalOperators = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) { { opEQUALS, " = " }, 
                                                                                                                      { opNOTEQUALS, " <> "},
                                                                                                                      { opGREATERTHAN, " > "},
                                                                                                                      { opGREATEROREQUAL, " >= "},
                                                                                                                      { opLESSTHAN, " < "},
                                                                                                                      { opLESSOREQUAL, " <= "} };
    #endregion

    #region public properties
    public DataGridViewColumnCollection Columns { get { return grdResearch.Columns; } }
    public DataColumnCollection DataColumns { get { return ((DataView)grdResearch.DataSource).ToTable().Columns; } }
    public int LastRowSelected { get { return _selectedRowIndex; } }
    public string NameType 
    { 
      get { return _nameType; }
      set
      {
        _nameType = value;
      }
    }
    public Dictionary<string, string> Parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> HiddenColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> NumericFormattedColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public Dictionary<string, string> DoNotSortColumns = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public string ColumnName { get { return _fieldName; } set { _fieldName = value; } }
    public string Title { get { return lblTitle.Text; } set { lblTitle.Text = value; } }
    public string SelectedID { get { return _selectedID; } }
    public string UniqueIdentifier { get { return _uniqueIdentifier; } set { _uniqueIdentifier = value; } }
    public string InnerWhere { get; set; }
    public DataGridViewRow SelectedRow { get { if (_selectedRowIndex < 0) return null; else return grdResearch.Rows[_selectedRowIndex]; } }
    public bool DisplaySearchCriteria 
    { 
      get { return !splitMain.Panel1Collapsed; } 
      set 
      { 
        splitMain.Panel1Collapsed = !value;
        btnCollapseSearch.Text = value ? "<" : ">"; 
      } 
    }
    public bool DisplayFields 
    { 
      get { return !splitSelect.Panel2Collapsed; } 
      set { 
        if (!DisplaySearchCriteria) 
          DisplaySearchCriteria = true;
        if (value) // we are displaying the fields
          btnShowFields.Text = _filterCaption;
        else
          btnShowFields.Text = _fieldsCaption;
        splitSelect.Panel1Collapsed = false;
        splitSelect.Panel2Collapsed = false;
        if (value)
          splitSelect.Panel1Collapsed = true;
        else
          splitSelect.Panel2Collapsed = true;
      } 
    }
    public string CheckBoxColumn = string.Empty;
    public bool CanChangeDisplaySearchCriteria
    {
      get { return _canChangeDisplaySearchCriteria; }
      set
      {
        _canChangeDisplaySearchCriteria = value;
        adjustCanDisplaySearch();
      }
    }
    public bool CanChangeDisplayFields
    {
      get { return _canChangeDisplayFields; }
      set
      {
        _canChangeDisplayFields = value;
        adjustCanDisplayFields();
      }
    }
    public bool AutoRefreshWhenFieldChecked
    {
      get { return _autoRefreshWhenFieldChecked; }
      set { _autoRefreshWhenFieldChecked = value; }
    }
    public bool UseNamedSearches 
    { 
      get { return _useNamedSearches; } 
      set 
      { 
        _useNamedSearches = value;
        srchNamedSearch.Visible = _useNamedSearches;
        if (UseNamedSearches) // adjust search grid size
          grdSearch.Top = srchNamedSearch.Top + srchNamedSearch.Height + 3;
        else
          grdSearch.Top = 23;
        grdSearch.Height = toolStripSearch.Top - grdSearch.Top - 3;
        tsbtnSaveOptions.Visible = _useNamedSearches;
      } 
    }
    public bool AutoSaveUserOptions
    {
      get { return _autoSaveUserOptions; }
      set { _autoSaveUserOptions = value; }
    }
    public bool ColumnOrderSaveEnabled = true;
    public bool ForceReloadSearchColumns { get; set; }
    public bool IncludeGroupAsCriteria
    {
      get { return _includeGroupAsCriteria; }
      set { _includeGroupAsCriteria = value; }
    }
    public Dictionary<string, string[]> SearchCriteria 
    {
      get { if (_firstTime) return null; else return _searchCriteria; } 
      set
      {
        if (_firstTime || value == null)
          return;
        Dictionary<string, string[]> tmpList = value; 
        _searchCriteria.Clear();
        foreach (KeyValuePair<string, string[]> entry in tmpList)
        {
          string key = entry.Key;
          //switch (key.ToLower())
          //{
          //  case "customername":
          //    key = "customer";
          //    break;
          //}
          // now fixup so it doesn't crash the pick list
          foreach (KeyValuePair<string, string> f in _fields)
            if (key.Equals(f.Key, StringComparison.CurrentCultureIgnoreCase))
            {
              key = f.Key;
              break;
            }
          if (_fields.ContainsKey(key) || entry.Value[0].ToString().StartsWith("In Group"))
            if (!_searchCriteria.ContainsKey(key))
              _searchCriteria.Add(key, entry.Value);
            else
              _searchCriteria[key] = entry.Value;
        }
        LoadSearchGrid(_searchCriteria);
        saveDefaultUserOptions(); // save the results so search doesn't reset them
      }
    }
    public Dictionary<string, string> RowCheckColumns { get { return _rowCheckColumns; } }
    public bool AllowSortByColumn { get { return grdResearch.AllowUserToOrderColumns; }  set { grdResearch.AllowUserToOrderColumns = value; } }
    public bool FieldsDefaultIsChecked
    {
      get { return _fieldsDefaultIsChecked; }
      set { _fieldsDefaultIsChecked = value; }
    }
    public string IDListColumn = null;
    /// <summary>
    /// This is an unusual proerty, in that the get and set do NOT refer to the same data
    /// Get: get a list of IDs from the current selected set of rows. This is an Output dataset
    /// SET: set a list to be used in the next search to filter by ID. This is an Input dataset, and may be different from the input.
    /// </summary>
    public List<string> IDList { get { if (_firstTime) return null; else return getIDList(IDListColumn); } set { _idList = value; } }
    #endregion

    public ctlSearchGrid()
    {
      ForceReloadSearchColumns = false;
      InitializeComponent();
      InnerWhere = string.Empty;
      grdResearch.ContextMenuStrip = mnuGridContext;
      splitSelect.Panel2Collapsed = true;
      adjustCanDisplayFields(); // adjust Fields/Filter button according to the value of CanChangeDisplayFields
      srchNamedSearch.SearchExec = new SearchDataSourceNamedSearch();
      srchNamedSearch.AutoAddNewMode = true;
      srchNamedSearch.ClearSearchOnExpand = true;
    }

    #region public methods
    public void Init(string nameType, string name)
    {
      if (!string.IsNullOrEmpty(name))
        Name = name;    
      ((SearchDataSourceNamedSearch)srchNamedSearch.SearchExec).NameType = NameType = nameType;
      IScreenBase parent = CommonFormFunctions.getParentForm(this);
      if (UseNamedSearches)
      {
        if (parent != null && parent.SecurityContext != null)
          ((SearchDataSourceNamedSearch)srchNamedSearch.SearchExec).User = parent.SecurityContext.User;
      }
      else
      {
        UseNamedSearches = false;
      }
      if (nameType.Equals("None",StringComparison.CurrentCultureIgnoreCase)) // no data source
      {
        grdSearch.Rows.Clear();
        grdSearch.Enabled = false;
        _fields.Clear();
        _searchCriteria.Clear();
        _lastColumnSorted = null;
        lstFields.Items.Clear();
        grdResearch.DataSource = null;
      }
      else
      {
        grdSearch.Enabled = true;
        if (!_cancelPostbackLoaded)
        {
          parent.Closing += new CancelEventHandler(ParentClosing);
          _cancelPostbackLoaded = true;
        }
        if (RowCheckColumns.Count > 0)
        {
          foreach (DataGridViewColumn col in grdResearch.Columns)
          {
            string colName = col.Name;
            if (RowCheckColumns.ContainsKey(colName))
              col.ReadOnly = false;
            else
              col.ReadOnly = true;
          }
        }
        loadSearch();
        applyUserOptions();
        loadFieldList();
        _firstTime = false;
      }

    }
    public void ReLoad()
    {
      search();
    }    
    public void ReLoad(bool resetLastRowSelected)
    {
      if (resetLastRowSelected)
        _lastSelectedRow = -1;
      search();
    }
    public void ClearAll()
    {
      _fields.Clear();
      ClearSearch();
      Clear();
      lstFields.Items.Clear();
      _displayFieldsLoaded = false;
      _searchColumnsLoaded = false;
      HiddenColumns.Clear();
      RowCheckColumns.Clear();
      DoNotSortColumns.Clear();
      NumericFormattedColumns.Clear();
    }
    public void Clear()
    {
      //foreach (DataGridViewRow row in grdResearch.Rows)
      //  grdResearch.Rows.Remove(row);
      grdResearch.ClearSelection();
      grdResearch.DataSource = null;
      grdResearch.Rows.Clear();
      srchNamedSearch.Text = string.Empty;
    }
    public void ClearSearch()
    {
      _searchCriteria.Clear();
      grdSearch.Rows.Clear();
    }
    public List<string> getIDList()
    {
      return getIDList("ID");
    }
    public List<string> getIDList(string columnName)
    {
      List<string> idList = new List<string>();
      if (string.IsNullOrEmpty(columnName) || !grdResearch.Columns.Contains(columnName))
        return idList;
      foreach (DataGridViewRow row in grdResearch.Rows)
      {
        string id = CommonFunctions.CString(row.Cells[columnName].Value);
        if (!idList.Contains(id))
          idList.Add(id);
      }
      return idList;
    }
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
      }
      grdSearch.Refresh();
    }
    /// <summary>
    /// get totals for requested columns from the currently selected collection
    /// </summary>
    /// <param name="columnlist"></param>
    /// <returns></returns>
    public Dictionary<string, decimal> getTotals(string[] columnlist)
    {
      Dictionary<string, decimal> totals = new Dictionary<string, decimal>(StringComparer.CurrentCultureIgnoreCase);
      foreach (string column in columnlist)
        totals.Add(column, (decimal)0);
      foreach (DataGridViewRow row in grdResearch.Rows)
      {
        string[] keys = new string[totals.Count];
        totals.Keys.CopyTo(keys, 0);
        foreach (string key in keys)
          if (grdResearch.Columns.Contains(key))
            totals[key] += CommonFunctions.CDecimal(row.Cells[key].Value);
      }
      return totals;
    }
    public UserOption GetSavedSearch(string name, string user)
    {
      UserOptionCollection options = new UserOptionCollection();
      // ColumnOrder
      UserOption option = new UserOption();
      foreach (DataGridViewColumn col in grdResearch.Columns)
        option.Parms.Add(col.Name, col.DisplayIndex);
      option.OptionName = CommonData.USEROPTIONTYPECOLUMNORDER;
      option.Description = "Column Order";
      option.User = user;
      option.OptionType = NameType.ToString();
      options.Add(option);
      if (_canChangeDisplayFields)
      {
        // Display Fields
        option = new UserOption();
        foreach (KeyValuePair<string, string> fld in _fields) // only save the displayed fields
          if (!HiddenColumns.ContainsKey(fld.Key))
            option.Parms.Add(fld.Key, fld.Value);
        option.OptionName = CommonData.USEROPTIONDISPLAYFIELDS;
        option.Description = "Display Fields";
        option.User = user;
        option.OptionType = NameType.ToString();
        options.Add(option);
      }
      if (!string.IsNullOrEmpty(_lastColumnSorted))
      {
        // Last Column Sorted
        option = new UserOption();
        option.Parms.Add(_lastColumnSorted, _lastColumnSortDirection.ToString());
        option.OptionName = CommonData.USEROPTIONLASTCOLUMNSORTED;
        option.Description = "Display Fields";
        option.User = user;
        option.OptionType = NameType.ToString();
        options.Add(option);
      }
      // Search Criteria
      option = new UserOption();
      foreach (KeyValuePair<string, string[]> entry in SearchCriteria)
        option.Parms.Add(entry.Key, string.Format("{0}={1}", entry.Value[0], entry.Value[1]));
      option.OptionName = CommonData.USEROPTIONSEARCHCRITERI;
      option.Description = "Search Criteria";
      option.User = user;
      option.OptionType = NameType.ToString();
      options.Add(option);
      option = options.createUserOption(user, NameType.ToString(), name);
      return option;
    }
    public void LoadNamedSearch(string name)
    {
      if (!_searchPaneEdited && !string.IsNullOrEmpty(name) && !NameType.Equals("None",StringComparison.CurrentCultureIgnoreCase))
      {
        IScreenBase parent = CommonFormFunctions.getParentForm(this);
        if (parent == null || parent.SecurityContext == null)
          return;
        string user = parent.SecurityContext.User;
        UserOption option = _dataSource.getUserOption(user, string.Format("NamedSearch:{0}", name), NameType.ToString());
        if (option != null)
        {
          _searchCriteria.Clear();
          grdSearch.Rows.Clear();
          UserOptionCollection options = new UserOptionCollection(option.Option);
          foreach (KeyValuePair<string, UserOption> entry in options)
            applyUserOption(entry.Value);
          tslblCurrentSavedSearch.Text = name;
        }
      }
      _searchPaneEdited = false; // set edit flag to false since we just loaded the search pane
    }
    public bool DeleteNamedSearch(string name)
    {
      string nothingToDelete = "There is no saved search found to delete.";
      string title = "Delete Saved Search";

      if (string.IsNullOrEmpty(name))
      {
        MessageBox.Show(nothingToDelete, title);
        return false;
      }
      DialogResult ans = MessageBox.Show("Delete Save Search " + name + "?", title, MessageBoxButtons.YesNo);
      if (ans == DialogResult.Yes)
      {
        // get the current user
        IScreenBase parent = CommonFormFunctions.getParentForm(this);
        if (parent == null || parent.SecurityContext == null)
          return false;
        string user = parent.SecurityContext.User;
        string optiontype = string.Format("NamedSearch:{0}", name);
        // check to see if it is there
        UserOption option = _dataSource.getUserOption(user, optiontype, NameType.ToString());
        if (option == null) // if null then it is not
        {
          MessageBox.Show(nothingToDelete, title); // so inform the user
          return false;
        }

        if (!_searchPaneEdited && !string.IsNullOrEmpty(name) && !NameType.Equals("None", StringComparison.CurrentCultureIgnoreCase))
        {
          // delete the user option
          _dataSource.deleteUserOption(user, optiontype, NameType.ToString());
          // reset the grid
          _searchCriteria.Clear();
          grdSearch.Rows.Clear();
          // tell the user we did it
          MessageBox.Show("Saved Search Deleted.", title);
        }
        _searchPaneEdited = false; // set edit flag to false since we just loaded the search pane
        return true;
      }
      else
        return false;
    }
    public void SelectFirst()
    {
      if (grdResearch.RowCount > 0)
      {
        grdResearch.Rows[0].Selected = true;
        raiseRowChangedEvent(grdResearch.Rows[0]);
      }
      //  grdResearch.CurrentCell = grdResearch.Rows[grdResearch.RowCount - 1].Cells[0];
      //  grdResearch.SelectedRows = new DataGridViewSelectedRowCollection() { grdResearch.CurrentRow };
    }

    #endregion

    #region form events
    private void btnSearch_Click(object sender, EventArgs e)
    {
      _lastSelectedRow = -1; // we reset this when they hit search
      if(grdSearch.Rows.Count > 0)
        grdSearch.CurrentCell = grdSearch.Rows[grdSearch.Rows.Count - 1].Cells[0];
      //grdSearch.Rows[grdSearch.Rows.Count - 1].Selected = true;
      search();
    }
    private void grdResearch_SelectionChanged(object sender, EventArgs e)
    {
      // I changed the use of this event to the row header mouse click event, because sometimes this even doesn't fire if there is only one row
    }
    private void grdSearch_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
    {
      e.Row.Cells[colOperator].Value = opCONTAINS;
    }
    //private void grdResearch_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    //{
    //  // this is only for checked/scrubbed fields
    //  string columnName = grdResearch.Columns[e.ColumnIndex].Name;
    //  if (columnName.Equals(CheckBoxColumn, StringComparison.CurrentCultureIgnoreCase) &&
    //    grdResearch.Columns.Contains(UniqueIdentifier))
    //  {
    //    try
    //    {
    //      DataGridViewRow row = grdResearch.Rows[e.RowIndex];
    //      string id = CommonFunctions.CString(row.Cells[UniqueIdentifier].Value);
    //      bool isChecked = CommonFunctions.CBoolean(row.Cells[e.ColumnIndex].EditedFormattedValue);
    //      _dataSource.checkResearchRecord(NameType, id, isChecked);
    //    }
    //    catch (Exception ex)
    //    {
    //      CommonFormFunctions.showException(ex);
    //    }
    //  }

    //}
    private void btnExport_Click(object sender, EventArgs e)
    {
      exportToExcel();
    }
    private void exportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      exportToExcel();
    }
    private void copyCellToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CommonFormFunctions.copyCellContents(grdResearch);
    }
    private void grdResearch_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
    {
      //if (!_searching && ColumnOrderSaveEnabled)
      //  saveColumnOrder();
    }    
    private void btnCollapseSearch_Click(object sender, EventArgs e)
    {
      switch (btnCollapseSearch.Text)
      {
        case ">":
          DisplaySearchCriteria = true;
          break;
        case "<":
          DisplaySearchCriteria = false;
          break;
      }
    }
    private void grdResearch_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (grdResearch.SelectedRows != null && grdResearch.SelectedRows.Count > 0)
      {
        //if (grdResearch.SelectedRows[0].Index != 0 || _selectedRowIndex == -1)
        if (e.RowIndex == grdResearch.SelectedRows[0].Index)
        {
          _lastSelectedRow = _selectedRowIndex = e.RowIndex;  // only select the first row selected
          if (grdResearch.Columns.Contains(UniqueIdentifier))
            _selectedID = CommonFunctions.CString(grdResearch.Rows[_selectedRowIndex].Cells[UniqueIdentifier].Value);
          raiseRowChangedEvent(grdResearch.SelectedRows[0]);
        }
      }
      if (grdResearch.Rows.Count == 1) // selected Row deoesnt fire if there is only one row so we make sure it does
        raiseRowChangedEvent(grdResearch.Rows[e.RowIndex]);
    }
    private void grdResearch_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      // check to see if we need to fire the RowChecked event
      if (RowCheckColumns.Count > 0) // don't have any? Then do nothing
      {
        string colName = grdResearch.Columns[e.ColumnIndex].Name;
        if (RowCheckColumns.ContainsKey(colName)) // this is a column in our list
        {
          DataGridViewRow row = grdResearch.Rows[e.RowIndex];
          DataGridViewCell cell = row.Cells[e.ColumnIndex];
          string cellState = RowCheckColumns[colName];
          if (cellState != null && !cellState.Equals(CommonFunctions.CString(cell.Value))) // has the value changed?
          {
            RowCheckColumns[colName] = CELLNOTEDIT;
            raiseRowCheckedEvent(row, colName);
          }
        }
      }
    }
    private void grdResearch_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      // capture whether the call is going to be changed
      // check to see if we need to fire the RowChecked event
      if (RowCheckColumns.Count > 0) // do we have any check columns? If not, then do nothing
      {
        string colName = grdResearch.Columns[e.ColumnIndex].Name;
        if (RowCheckColumns.ContainsKey(colName)) // this is a column in our list
        {
          RowCheckColumns[colName] = CommonFunctions.CString(grdResearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value); // record the old value
        }
      }
    }
    private void grdResearch_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
      // reset the RowCheckColumns states if needed
      if (RowCheckColumns.Count > 0)
      {
        string[] keys = RowCheckColumns.Keys.ToArray<string>();
        foreach (string key in keys)
          RowCheckColumns[key] = null;
      }
    }
    private void grdResearch_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
    {

      if (NumericFormattedColumns.ContainsKey(e.Column.Name)) // if ths is a numeric column
      {
        try
        {
          e.SortResult = CommonFunctions.CDouble(e.CellValue1).CompareTo(CommonFunctions.CDouble(e.CellValue2));
        }
        catch (Exception ex)
        {
          e.SortResult = CommonFunctions.CString(e.CellValue1).CompareTo(CommonFunctions.CString(e.CellValue2));
        }
      }
      else
        e.SortResult = CommonFunctions.CString(e.CellValue1).CompareTo(CommonFunctions.CString(e.CellValue2));
      // this line prevents data type conversion errors when the grid tries to compare string and non-string values
      e.Handled = true; // now tell the grid not to try to do this itself cause i already did it

    }
    private void grdResearch_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
    {
      if (DoNotSortColumns.ContainsKey(e.Column.Name))
        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    private void grdResearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      string colName = grdResearch.Columns[e.ColumnIndex].Name;
      if (!DoNotSortColumns.ContainsKey(colName)) // don't do this if it is in the list of columns we can't sort
      {
        // remember the name and direction of the sort, so when we refresh the grid, we can do it again
        _lastColumnSorted = colName;
        if (grdResearch.SortOrder == SortOrder.Ascending)
          _lastColumnSortDirection = ListSortDirection.Ascending;
        else if (grdResearch.SortOrder == SortOrder.Descending)
          _lastColumnSortDirection = ListSortDirection.Descending;
      }
    }
    private void lstFields_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!_loadingDisplayFields)
      {
        string fieldName = CommonFunctions.CString(lstFields.Items[e.Index]);
        bool showField = (e.NewValue == CheckState.Checked);
        changeFieldList(fieldName, showField);
      }
    }
    private void btnShowFields_Click(object sender, EventArgs e)
    {
      DisplayFields = !DisplayFields;
    }
    private void ckAutoRefreshFields_CheckedChanged(object sender, EventArgs e)
    {
      _autoRefreshWhenFieldChecked = ckAutoRefreshFields.Checked;
    }
    private void ctlSearchGrid_Resize(object sender, EventArgs e)
    {
      //checkResizeFieldsButton();
    }
    private void btnClearFields_Click(object sender, EventArgs e)
    {
      clearAllFields();
    }
    private void btnSelectAllFields_Click(object sender, EventArgs e)
    {
      displayAllFields();
    }
    private void btnClear_Click(object sender, EventArgs e)
    {
      ClearSearch();
    }
    private void tsbtnSaveOptions_Click(object sender, EventArgs e)
    {
      savedSearchSave(tslblCurrentSavedSearch.Text);
    }
    private void srchNamedSearch_OnSelected(object sender, EventArgs e)
    {
      _searchPaneEdited = false; // set the edited flag to false since they are selecting a new one
      LoadNamedSearch(srchNamedSearch.Text);
    }
    private void btnRefresh_Click(object sender, EventArgs e)
    {
      search();
    }
    private void grdSearch_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      _searchPaneEdited = true; // set edited flag to true since the operator did something with a cell
    }
    private void grdSearch_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {

    }
    private void grdSearch_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      _searchPaneEdited = true; // set edited flag to true since the operator added a row
    }
    private void grdSearch_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
    }
    private void grdSearch_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
    {
      string fieldname = CommonFunctions.CString(e.Row.Cells[0].Value);
      if (SearchCriteria.ContainsKey(fieldname))
        SearchCriteria.Remove(fieldname);
      _searchPaneEdited = true; // user has deleted a row so we flag as edited
    }
    private void btnDeleteSavedSearch_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(tslblCurrentSavedSearch.Text))
        MessageBox.Show("No named search to delete");
      else
        deleteSavedSearch(tslblCurrentSavedSearch.Text);
    }
    private void tsbthNewSavedSearch_Click(object sender, EventArgs e)
    {
      tslblCurrentSavedSearch.Text = string.Empty;
      savedSearchSave(tslblCurrentSavedSearch.Text);
    }
    #endregion

    #region custom events

    public delegate void RowCheckedHandler(object sender, MaintenanceGridRowSelectedArgs e);
    public delegate void RowSelectedHandler(object sender, MaintenanceGridRowSelectedArgs e);
    public delegate void SearchPressedHandler(object sender, MaintenanceGridRowSelectedArgs e);
    public event RowSelectedHandler RowSelected;
    public event RowCheckedHandler RowChecked;
    public event SearchPressedHandler SearchPressed;

    protected void OnRowSelected(MaintenanceGridRowSelectedArgs e)
    {
      if (RowSelected != null)
      {
        RowSelected(this, e);
      }
    }
    protected void OnRowChecked(MaintenanceGridRowSelectedArgs e)
    {
      if (RowChecked != null)
      {
        RowChecked(this, e);
      }
    }
    protected void OnSearchPressed(MaintenanceGridRowSelectedArgs e)
    {
      if (SearchPressed != null)
      {
        SearchPressed(this, e);
      }
    }
    private void raiseRowChangedEvent(DataGridViewRow row)
    {
      if (!_searching)
      {
        MaintenanceGridRowSelectedArgs e = new MaintenanceGridRowSelectedArgs();
        e.SelectedRow = row;
        OnRowSelected(e);
      }
    }
    private void raiseSearchPressedEvent()
    {
      MaintenanceGridRowSelectedArgs e = new MaintenanceGridRowSelectedArgs();
      e.IDList = getIDList(IDListColumn);
      OnSearchPressed(e);
    }
    private void raiseRowCheckedEvent(DataGridViewRow row, string columnName)
    {
      if (!_searching)
      {
        MaintenanceGridRowSelectedArgs e = new MaintenanceGridRowSelectedArgs();
        e.SelectedRow = row;
        e.SelectedColumn = columnName;
        OnRowChecked(e);
      }
    }
    private void ParentClosing(object sender, CancelEventArgs e)
    {
      saveDefaultUserOptions();
    }
    //protected override void OnSizeChanged(EventArgs e)
    //{
    //  if (this.Handle != null)
    //  {
    //    this.BeginInvoke((MethodInvoker)delegate
    //    {
    //      base.OnSizeChanged(e);
    //    });
    //  }

    //} 
    #endregion

    #region private methods
    private void deleteSavedSearch(string name)
    {
      if (DeleteNamedSearch(name)) // delete the named search, returns true if successful
      {
        srchNamedSearch.Text = string.Empty; // and clear the field
        tslblCurrentSavedSearch.Text = string.Empty;
      }
    }
    private void savedSearchSave(string name)
    {
      string optionName;
      if (string.IsNullOrEmpty(name))
      {
        optionName = Interaction.InputBox("Search Save Name", "Save a Named Search");
        if (!string.IsNullOrEmpty(optionName))
        {
          saveNamedUserOptions(optionName);
          tslblCurrentSavedSearch.Text = optionName;
          srchNamedSearch.Text = string.Empty;
        }
      }
      else
      {
        optionName = name;
        DialogResult ans = MessageBox.Show(string.Format("Do you want to save Named Search '{0}'", optionName), "Save Named Search", MessageBoxButtons.YesNo);
        if (ans == DialogResult.Yes)
        {
          saveNamedUserOptions(optionName);
          tslblCurrentSavedSearch.Text = optionName;
          srchNamedSearch.Text = string.Empty;
        }
      }
    }
    private void search()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        // if this is the first time, we don't want to save the options yet cause we have't loaded or selected them

        _searching = true;
        string where = getSearchWhere();
        if (where == "ERROR")
          return;
        // user options has to be AFTER getSearchWhere cause getSearchWhere populates SearchCriteria, which it needs to save
        if (_firstTime)
          _firstTime = false;
        else
          if (_useNamedSearches)
          {
            if (_autoSaveUserOptions)
            {
              if (!string.IsNullOrEmpty(srchNamedSearch.Text))
                saveNamedUserOptions(srchNamedSearch.Text);
            }
          }
          else
            saveDefaultUserOptions(); //first save the state of the grid
        if (string.IsNullOrEmpty(InnerWhere))
          _dsResearch = _dataSource.getResearchData(NameType, where, Parameters, -1);
        else // if there is an inner where, use the alternate use of the where clause with an array of outer and inner where clauses
          _dsResearch = _dataSource.getResearchData(NameType, new string[] { where, InnerWhere }, Parameters, -1);
        //if (HiddenColumns.Count == _dsResearch.Tables[0].Columns.Count) // all of the columns are hidden
        //  return; // not sure about this
        if (RowCheckColumns.Count > 0)
          CommonFormFunctions.convertDataSetToGrid(grdResearch, _dsResearch);
        else
          CommonFormFunctions.displayDataSetGrid(grdResearch, _dsResearch);
        lblTitle.Text = string.Format(SEARCHLABEL, grdResearch.Rows.Count.ToString());
        // Hide the hidden columns
        applyUserOptions(); // go get the field list, column order, sorted column
        setHiddenColumns(); 
        foreach (KeyValuePair<string, string> entry in NumericFormattedColumns)
        {
          if (grdResearch.Columns.Contains(entry.Key))
          {
            grdResearch.Columns[entry.Key].DefaultCellStyle.Format = "n2";
            grdResearch.Columns[entry.Key].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
          }
        }
        setMRCTotal();
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
        _searching = false;
        if (!string.IsNullOrEmpty(_lastColumnSorted) && grdResearch.Columns.Contains(_lastColumnSorted))
        {
          grdResearch.Sort(grdResearch.Columns[_lastColumnSorted], _lastColumnSortDirection);
        }
        if (_lastSelectedRow >= 0)
        {
          if (_lastSelectedRow >= grdResearch.Rows.Count)
            _lastSelectedRow = grdResearch.Rows.Count - 1;
          if (_lastSelectedRow >= 0 && _lastSelectedRow < grdResearch.Rows.Count) // it is in range
            grdResearch.Rows[_lastSelectedRow].Selected = true;
        }

        Cursor.Current = Cursors.Default;
        raiseSearchPressedEvent();
      }
      catch (Exception ex)
      {
        MessageBox.Show("You entered an invalid search criteria. \r\nError detail is " + CommonFunctions.getInnerException(ex).Message);
      }
    }
    private void loadSearch()
    {
      if (!_searchColumnsLoaded || ForceReloadSearchColumns)
      {
        _searchColumnsLoaded = true;
        grdSearch.Rows.Clear();
        grdSearch.Columns.Clear();
        object oWhere = null;
        if (!string.IsNullOrEmpty(InnerWhere))
          oWhere = new string[] { null, InnerWhere };
        if (_fields.Count == 0)
          _fields = _dataSource.getResearchFields(NameType, oWhere);
        grdSearch.Rows.Clear();
        DataGridViewComboBoxCell op = new DataGridViewComboBoxCell();
        DataGridViewComboBoxCell fld = new DataGridViewComboBoxCell();

        foreach (KeyValuePair<string, string> entry in _fields)
          fld.Items.Add(entry.Key);
        op.Items.AddRange(operatorList);
        if (_includeGroupAsCriteria)
        {
          op.Items.Add(opINGROUPEXACT);
          op.Items.Add(opINGROUPLIKE);
        }
        DataGridViewColumn col = new DataGridViewColumn(fld);
        col.Name = colField;
        grdSearch.Columns.Add(col);
        col = new DataGridViewColumn(op);
        col.Name = colOperator;
        grdSearch.Columns.Add(col);
        grdSearch.Columns.Add(colValue, colValue);
      }
    }
    private void loadFieldList()
    {
      if (_canChangeDisplayFields && (!_displayFieldsLoaded || ForceReloadSearchColumns))
      {
        _loadingDisplayFields = true;
        if (_fields.Count == 0 || ForceReloadSearchColumns)
        {
          object oWhere = null;
          if (!string.IsNullOrEmpty(InnerWhere))
            oWhere = new string[] { null, InnerWhere };
          _fields = _dataSource.getResearchFields(NameType, oWhere);
        }
        lstFields.Items.Clear();
        int iRow = 0;
        foreach (KeyValuePair<string, string> entry in _fields)
        {
          lstFields.Items.Add(entry.Key);
          bool showField = _fieldsDefaultIsChecked;
          if (HiddenColumns.ContainsKey(entry.Key))
            showField = false;
          else
            if (!_fieldsDefaultIsChecked) // if the default state is unchecked, then basically add ALL the fields to hiddencolumns
              HiddenColumns.Add(entry.Key, null);
          lstFields.SetItemChecked(iRow++, showField);
        }
        _displayFieldsLoaded = true;
        _loadingDisplayFields = false;
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
        if ((op == opINGROUPLIKE || op == opINGROUPEXACT) // this is a group operator
          && (string.IsNullOrEmpty(fld) || (fld != "CustomerID" && fld != "Entity")) // and the fld is not a good one
            && !string.IsNullOrEmpty(_uniqueIdentifier)) // and we actually HAVE a good one
          fld = _uniqueIdentifier; // then we put the good one in        
        if (!string.IsNullOrEmpty(op) && !string.IsNullOrEmpty(fld))
        {
          if (firstTime)
            firstTime = false;
          else
            sbWhere.Append(" AND ");
          //if (fld.Equals("Customer", StringComparison.CurrentCultureIgnoreCase) &&
          //    grdResearch.Columns.Contains("CustomerName"))
          //  fld = "CustomerName";
          _searchCriteria.Add(fld, new string[] { op, val });
         if (op == opISBLANK || op == opISNOTBLANK)
            fld = string.Format("isnull({0},'')", fld);
         else
            sbWhere.Append("[");

          sbWhere.Append(fld);
          if (op != opISBLANK && op != opISNOTBLANK)
            sbWhere.Append("]");
          string dataType = _fields[fld];
          if (string.IsNullOrEmpty(dataType))
            dataType = "string";
          if (!isValidDataType(dataType, val, op))
          {
            MessageBox.Show(string.Format("Value {0} is not a valid value for field {1}", val, fld));
            return "ERROR";
          }
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
            case opISBLANK:
              sbWhere.Append(" = ''");
              break;
            case opISNOTBLANK:
              sbWhere.Append(" <> ''");
              break;
            case opISNULL:
              sbWhere.Append(" IS NULL");
              break;
            case opISNOTNULL:
              sbWhere.Append(" IS NOT NULL");
              break;
            case opIN:
              sbWhere.Append(string.Format(" IN ({0}) ", val));
              break;
            case opNOTIN:
              sbWhere.Append(string.Format(" NOT IN ({0}) ", val));
              break;
            case opBETWEEN:
              string fromVal = string.Empty;
              string toVal = string.Empty;
              int pos = val.IndexOf("AND", StringComparison.CurrentCultureIgnoreCase);
              if (pos > 0)
              {
                fromVal = val.Substring(0, pos).Trim().Replace("'","");
                toVal = val.Substring(pos + 3).Trim().Replace("'","");
              }
              if (!string.IsNullOrEmpty(fromVal) && !string.IsNullOrEmpty(toVal))
                sbWhere.Append(string.Format(" BETWEEN '{0}' AND '{1}' ", fromVal, toVal));
              else
                sbWhere.Append(string.Format(" LIKE '%{0}%'", val));
              break;
            case opINGROUPEXACT:
              sbWhere.Append(string.Format(" IN (Select Entity from GroupMembers Where GroupID = '{0}' and getdate() between isnull(StartDate, '1/1/1900') and isnull(EndDate, '12/31/2100'))", val));
              break;
            case opINGROUPLIKE:
              sbWhere.Append(string.Format(" IN (Select Entity from GroupMembers Where GroupID like '%{0}%' and getdate() between isnull(StartDate, '1/1/1900') and isnull(EndDate, '12/31/2100'))", val));
              break;
          }
        }
      }
      if (_idList != null && _idList.Count > 0 && !string.IsNullOrEmpty(IDListColumn) && _fields.ContainsKey(IDListColumn)) 
      {
        if (firstTime)
          firstTime = false;
        else
          sbWhere.Append(" AND ");
        // we also filter by the id list
        sbWhere.Append(string.Format(" {0} in (",IDListColumn));
        foreach (string id in _idList)
        {
          sbWhere.Append("'");
          sbWhere.Append(id);
          sbWhere.Append("',");
        }
        sbWhere.Length--; // get rid of final comma
        sbWhere.Append(")");
      }
      return sbWhere.ToString();
    }
    bool isValidDataType(string datatype, string val, string op)
    {
      switch (op)
      {
        case opISNULL:
        case opISNOTNULL:
        case opISBLANK:
        case opISNOTBLANK:
          return true; // there is no value for these operators, so it doesn't matter what is in them
        case opBETWEEN:
          if (string.IsNullOrEmpty(val))
            return false;
          else
            if (val.IndexOf(" AND ", StringComparison.CurrentCultureIgnoreCase) <= 0)
              return false;
          return true;
        case opIN:
        case opNOTIN:
            return true; // we need more logic here but for now we just return true
        default:
            switch (datatype.ToLower())
            {
              case "string":
                return true;
              case "datetime":
                return !string.IsNullOrEmpty(val) && CommonFunctions.IsDateTime(val) && val.Length > 5;
              case "float":
              case "decimal":
              case "double":
              case "money":
              case "int":
                return CommonFunctions.IsNumeric(val);
              case "bool":
                return CommonFunctions.IsBoolean(val);
              default:
                return true;
            }
      }
    }
    /// <summary>
    /// adjust panels according to CanChangeDisplaySearchCriteria
    /// </summary>
    private void adjustCanDisplaySearch()
    {
      // now hide or display the button and adjust the rest of the screen
      if (_canChangeDisplaySearchCriteria)
      {
        if (!btnCollapseSearch.Visible) // only adjust the screen if it needs it
        {
          btnCollapseSearch.Visible = true;
          int btnWidth = btnCollapseSearch.Width;
          int splitWidth = splitMain.Width - btnWidth;
          splitMain.Left = btnWidth + 1;
          splitMain.Width = splitWidth;
        }
      }
      else
      {
        CanChangeDisplayFields = false; // If we can't display the search, we can't display the fields cause that part of the screen is not there
        if (btnCollapseSearch.Visible) // only adjust the screen if it needs it
        {
          int splitWidth = splitMain.Width + btnCollapseSearch.Width;
          btnCollapseSearch.Visible = false;
          splitMain.Left = 1;
          splitMain.Width = splitWidth;
        }
      }
    }
    /// <summary>
    /// adjust Fields/Filter button according to the value of CanChangeDisplayFields
    /// </summary>
    private void adjustCanDisplayFields()
    {
      _autoRefreshWhenFieldChecked = ckAutoRefreshFields.Checked;
      // now hide or display the button and adjust the rest of the screen
      if (_canChangeDisplayFields)
      {
        btnShowFields.Visible = true;
        int btnCollapseHeight = splitSelect.Panel1.Height - btnCollapseSearch.Top - (_verticalButtonMargin * 2) - btnShowFields.Height;
        btnCollapseSearch.Height = btnCollapseHeight;
      }
      else
      {
        btnShowFields.Visible = false;
        int btnCollapseHeight = splitSelect.Panel1.Height - btnCollapseSearch.Top - _verticalButtonMargin;
        btnCollapseSearch.Height = btnCollapseHeight;
      }
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
      decimal quantity = 1;
      string colName = "MRC";
      string colQtyName = "";
      if (grdResearch.Columns.Contains("CommissionMRC"))
        colName = "CommissionMRC";
      else
        if (grdResearch.Columns.Contains("MRCRaw"))
          colName = "MRCRaw";
        else
          if (grdResearch.Columns.Contains("mthlyrecrev"))
            colName = "mthlyrecrev";
      if (grdResearch.Columns.Contains("Quantity"))
        colQtyName = "Quantity";
      else
        if (grdResearch.Columns.Contains("Qty"))
          colQtyName = "Qty";
        else
          if (grdResearch.Columns.Contains("RetailQty"))
            colQtyName = "RetailQty";
      foreach (DataGridViewRow row in grdResearch.Rows)
      {
        decimal mrc = CommonFunctions.CDecimal(row.Cells[colName].Value, 0);
        if (!string.IsNullOrEmpty(colQtyName))
          quantity = CommonFunctions.CDecimal(row.Cells[colQtyName].Value, 0);
        total += mrc * quantity;
      }
      return total;
    }
    private void saveNamedUserOptions(string name)
    {
      IScreenBase parent = CommonFormFunctions.getParentForm(this);
      if (parent == null || parent.SecurityContext == null)
        return;
      string user = parent.SecurityContext.User;
      UserOption option = GetSavedSearch(name, user);
      _dataSource.saveUserOption(option);
    }
    private void applyUserOption(UserOption option)
    {
      string optiontype = option.OptionName;
      switch (optiontype)
      {
        case CommonData.USEROPTIONTYPECOLUMNORDER:

            foreach (KeyValuePair<string, object> entry in option.Parms)
            {
              int colOrder = CommonFunctions.CInt(entry.Value);
              if (grdResearch.Columns.Contains(entry.Key) && colOrder >= 0 && colOrder < grdResearch.Columns.Count)
                grdResearch.Columns[entry.Key].DisplayIndex = colOrder;
            }
          break;
        case CommonData.USEROPTIONLASTCOLUMNSORTED:
            // since a dictionary does not have an index, we use foreach and then bail after the first one (there should only be one anyway)
            foreach (KeyValuePair<string, object> entry in option.Parms)
            {
              _lastColumnSorted = entry.Key;
              ListSortDirection dir = ListSortDirection.Descending;
              if (CommonFunctions.CString(entry.Value).Equals("Ascending", StringComparison.CurrentCultureIgnoreCase))
                dir = ListSortDirection.Ascending;
              _lastColumnSortDirection = dir;
              break;
            }

          break;
        case CommonData.USEROPTIONDISPLAYFIELDS:
          if (!_canChangeDisplayFields && !_autoSaveUserOptions)
          {
            HiddenColumns.Clear();
            int iField = 0;
            foreach (KeyValuePair<string, string> entry in _fields)
              lstFields.SetItemChecked(iField++, true);
          }
          else
          {
            if (_canChangeDisplayFields)
            {
              HiddenColumns.Clear();
              int iField = 0;
              foreach (KeyValuePair<string, string> entry in _fields)
              {
                if (option.Parms.ContainsKey(entry.Key))
                  lstFields.SetItemChecked(iField++, true);
                else
                {
                  HiddenColumns.Add(entry.Key, null);
                  lstFields.SetItemChecked(iField++, false);
                }
              }

            }
          }
          break;
        case CommonData.USEROPTIONSEARCHCRITERI:
          Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
          foreach (KeyValuePair<string, string[]> entry in _searchCriteria)
            criteria.Add(entry.Key, entry.Value);
          foreach (KeyValuePair<string, object> entry in option.Parms)
          {
            if (!criteria.ContainsKey(entry.Key))
            {
              string[] parts = CommonFunctions.CString(entry.Value).Split(new char[] { '=' });
              if (parts != null && parts.GetLength(0) == 2)
                criteria.Add(entry.Key, parts);
            }
          }
          SearchCriteria = criteria; //reset both the criteria collection and the search grid
          break;
      }
    }
    private void saveDefaultUserOptions()
    {
      if (_autoSaveUserOptions)
      {
        Dictionary<string, object> parms = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        foreach (DataGridViewColumn col in grdResearch.Columns)
          parms.Add(col.Name, col.DisplayIndex);
        IScreenBase parent = CommonFormFunctions.getParentForm(this);
        if (parent == null || parent.SecurityContext == null)
          return;
        else
          //if (!
          _dataSource.saveColumnOrder(NameType.ToString(), parent.SecurityContext.User, parms);
        //)
        //MessageBox.Show("Failed saving column order, database error");
        if (_canChangeDisplayFields)
        {
          parms.Clear();
          foreach (KeyValuePair<string, string> fld in _fields) // only save the displayed fields
            if (!HiddenColumns.ContainsKey(fld.Key))
              parms.Add(fld.Key, fld.Value);
          //if (!
          _dataSource.saveUserOption(NameType.ToString(), parent.SecurityContext.User, parms, CommonData.USEROPTIONDISPLAYFIELDS, "Display Fields");
          //)
          //MessageBox.Show("Failed saving display fields, database error");
        }
        if (!string.IsNullOrEmpty(_lastColumnSorted))
        {
          parms.Clear();
          parms.Add(_lastColumnSorted, _lastColumnSortDirection.ToString());
          //if (!
          _dataSource.saveUserOption(NameType.ToString(), parent.SecurityContext.User, parms, CommonData.USEROPTIONLASTCOLUMNSORTED, "Last Column Sorted");
          //)
          //MessageBox.Show("Failed saving last column sorted, database error");
        }
        // save the search criteria
        parms.Clear();
        if (SearchCriteria != null)
        {
          foreach (KeyValuePair<string, string[]> entry in SearchCriteria)
            parms.Add(entry.Key, string.Format("{0}={1}", entry.Value[0], entry.Value[1]));
          _dataSource.saveUserOption(NameType.ToString(), parent.SecurityContext.User, parms, CommonData.USEROPTIONSEARCHCRITERI, "Search Criteria");
        }
      }
    }
    private void applyUserOptions()
    {
      if (_useNamedSearches)
      {
        LoadNamedSearch(srchNamedSearch.Text);
        return;
      }
      if (!_autoSaveUserOptions)
      {
        applyUserOption(new UserOption() { Option = CommonData.USEROPTIONDISPLAYFIELDS});
        return;
      }
      IScreenBase parent = CommonFormFunctions.getParentForm(this);
      if (parent != null && parent.SecurityContext != null)
      {
        // column order
        Dictionary<string, object> userOptions = _dataSource.getColumnOrder(parent.SecurityContext.User, NameType.ToString());
        foreach (KeyValuePair<string, object> entry in userOptions)
        {
          int colOrder = CommonFunctions.CInt(entry.Value);
          if (grdResearch.Columns.Contains(entry.Key) && colOrder >= 0 && colOrder < grdResearch.Columns.Count)
            grdResearch.Columns[entry.Key].DisplayIndex = colOrder;
        }
        if (_canChangeDisplayFields)
        {
          userOptions = _dataSource.getUserOptions(parent.SecurityContext.User, NameType.ToString(), CommonData.USEROPTIONDISPLAYFIELDS);
          HiddenColumns.Clear();
          foreach (KeyValuePair<string, string> entry in _fields)
            if (!userOptions.ContainsKey(entry.Key))
              HiddenColumns.Add(entry.Key, null);
        }
        userOptions = _dataSource.getUserOptions(parent.SecurityContext.User, NameType.ToString(), CommonData.USEROPTIONLASTCOLUMNSORTED);
        // since a dictionary does not have an index, we use foreach and then bail after the first one (there should only be one anyway)
        foreach (KeyValuePair<string, object> entry in userOptions)
        {
          _lastColumnSorted = entry.Key;
          ListSortDirection dir = ListSortDirection.Descending;
          if (CommonFunctions.CString(entry.Value).Equals("Ascending",StringComparison.CurrentCultureIgnoreCase))
            dir = ListSortDirection.Ascending;
          _lastColumnSortDirection = dir;
          break;
        }
        userOptions = _dataSource.getUserOptions(parent.SecurityContext.User, NameType.ToString(), CommonData.USEROPTIONSEARCHCRITERI);
        Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        foreach (KeyValuePair<string, string[]> entry in _searchCriteria)
          criteria.Add(entry.Key, entry.Value);
        foreach (KeyValuePair<string, object> entry in userOptions)
        {
          if (!criteria.ContainsKey(entry.Key))
          {
            string[] parts = CommonFunctions.CString(entry.Value).Split(new char[] { '=' });
            if (parts != null && parts.GetLength(0) == 2)
              criteria.Add(entry.Key, parts);
          }
        }
        SearchCriteria = criteria; //reset both the criteria collection and the search grid
        
      }
    }
    private void checkResizeFieldsButton()
    {
      if (CanChangeDisplayFields)
      {
        if (splitMain.Height < MAINPANENORMALHEIGHT)
          btnShowFields.Height = Convert.ToInt16(Convert.ToDouble(splitMain.Height) * .50);
        else
          btnShowFields.Height = FIELDSBUTTONNORMALHEIGHT;
        btnShowFields.Top = splitSelect.Panel1.Height - btnShowFields.Height - _verticalButtonMargin;
        adjustCanDisplayFields();
      }
    }
    private void changeFieldList(string fieldName, bool showField)
    {
      if (showField)
      {
        if (HiddenColumns.ContainsKey(fieldName))
          HiddenColumns.Remove(fieldName);
      }
      else
        if (!HiddenColumns.ContainsKey(fieldName))
          HiddenColumns.Add(fieldName, null);
      saveDefaultUserOptions();
      if (_autoRefreshWhenFieldChecked)
        search();
    }
    private void clearAllFields()
    {
      _loadingDisplayFields = true;
      foreach (KeyValuePair<string, string> fld in _fields)
        if (!HiddenColumns.ContainsKey(fld.Key))
          HiddenColumns.Add(fld.Key, null);
      for (int i = 0; i < lstFields.Items.Count; i++)
        lstFields.SetItemChecked(i, false);
      _loadingDisplayFields = false;
      setHiddenColumns();
    }
    private void displayAllFields()
    {
      _loadingDisplayFields = true;
      HiddenColumns.Clear();
      for (int i = 0; i < lstFields.Items.Count; i++)
        lstFields.SetItemChecked(i, true);
      _loadingDisplayFields = false;
      setHiddenColumns();
    }
    private void setHiddenColumns()
    {
      unHideAllColumns();
      foreach (KeyValuePair<string, string> entry in HiddenColumns)
      {
        if (grdResearch.Columns.Contains(entry.Key))
          grdResearch.Columns[entry.Key].Visible = false;
        //else
        //  grdResearch.Columns[entry.Key].Visible = true;
      }
    }
    private void unHideAllColumns()
    {
      foreach (DataGridViewColumn col in grdResearch.Columns)
        col.Visible = true;
    }
    private void exportToExcel()
    {
      // if the user can adjust the display fields list, then pass in the list of hidden columns and we won't include them in the excel sheet
      CommonFormFunctions.exportToExcel(grdResearch, _canChangeDisplayFields ? HiddenColumns : null);
    }

    #endregion


  }
}
