using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Common
{
  public partial class ctlAttributeValue : UserControl
  {
    #region module data
    //private TAGMDICommon _common = null;
    internal object[] valueTypeList = new object[] {
            "TableHeader",
            "RefInherit",
            "Include",
            "Func",
            "Value"};
    object[,] _valueHistoryColumnDefinition = new object[,] {  { "HistoryID", typeof(string), true, "ID" }, 
                                                    { "StartDate", typeof(DateTime), false, "Start Date" },
                                                    { "EndDate", typeof(DateTime), false, "End Date" },
                                                    { "ValueType", typeof(string), false, "Value Type" },
                                                    { "Value", typeof(object), true, "Value" },
                                                    { "LastModifiedBy", typeof(string), true, "Last Modified By" },
                                                    { "LastModifiedDateTime", typeof(DateTime), true, "Last Modified Date/Time" } };
    object[,] _itemHistoryColumnDefinition = new object[,] {  { "HistoryID", typeof(string), true, "ID" }, 
                                                    { "StartDate", typeof(DateTime), false, "Start Date" },
                                                    { "EndDate", typeof(DateTime), false, "End Date" } };
    private string _oldItemID = null;
    private object _value = null;
    private string _valueHistoryID = null;
    private string _itemHistoryID = null;
    private DateTime _startDate = DateTime.Today;
    private DateTime _endDate = DateTime.Today;
    private string _valueType = null;
    private bool _inDisplayGrid = false;
    private const int _adjustForValueLabel = 20;
    private string _attributeName = string.Empty;
    private Dictionaries _dictionary { get { return DictionaryFactory.getInstance().getDictionary(); } }
    private TableHeader _thValue
    {
      get
      {
        if (_value.GetType() == typeof(TableHeader))
          return (TableHeader)_value;
        else
          return new TableHeader();
      }
    }
    private bool _rawMode = false;
    private bool _editMode = false;
    private bool _gridIsDataset = false;
    private bool _inNewRow = false;
    private DateTime _effectiveDate = DateTime.Today;
    List<DataGridViewRow> _thGridLastRowsSelected = null;
    private int _originalValueHeight = 0;
    private string[] typeDelimiters = new string[] { "." };
    private bool _displayFunctionTree = false;
    private object _eac = null;
    #endregion
    #region public properties
    public object EACobject {
      get { return (EntityAttributesCollection)_eac; }
      set 
      {
        if (value == null)
          _eac = null;
        else
          if (value.GetType() == typeof(EntityAttributesCollection))
            _eac = value;
          else
            _eac = null;
      }
    }
    private EntityAttributesCollection EAC { get { return (EntityAttributesCollection)_eac; } set { _eac = value; } }
    public DateTime EffectiveDate { get { return _effectiveDate; } set { _effectiveDate = value; } }
    public string EntityID { get { return txbEntity2.Text; } set { txbEntity2.Text = value; } }
    public string ItemTypeID { get { return txbItemType2.Text; } set { txbItemType2.Text = value; } }
    public string ItemID 
    { 
      get { return txbItem2.Text; } 
      set 
      {
        _oldItemID = value;
        txbItem2.Text = value;
        if (value != null && value != string.Empty &&           // if Item exists, and...
            (AttributeID == null || AttributeID == string.Empty) )// if attribute is not selected
          displayItemHistoryGrid();
      } 
    }
    public string AttributeID 
    { 
      get { return txbAttribute2.Text; } 
      set 
      { 
        txbAttribute2.Text = value;
        if (value != null && value != string.Empty)
          displayValueHistoryGrid();
      } 
    }
    public string ValueType 
    {
      get { return _valueType; } 
      set 
      {
        if (value == null || value == string.Empty)  // intialization, so do nothing
          return;
        _valueType = value;
        Item item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
        if (item != null)
        {
          if (item.Attributes.Contains(AttributeID))
          {
            TAGAttribute a = item.Attributes[AttributeID];
            a.ValueType = value;
          }
        }
      } 
    }
    public string ValueHistoryID { get { return _valueHistoryID; } set { _valueHistoryID = value; } }
    public string ItemHistoryID { get { return _itemHistoryID; } set { _itemHistoryID = value; } }
    public DateTime StartDate { get { return _startDate; } set { _startDate = value; } }
    public DateTime EndDate { get { return _endDate; } set { _endDate = value; } }
    public bool EditMode { get { return _editMode; } set { _editMode = value; checkEditMode(); } }
    public bool RawMode {get { return _rawMode; } set { _rawMode = value; } }
    public bool DisplayFunctionTree
    {
      get { return _displayFunctionTree; }
      set { _displayFunctionTree = value; }
    }
    public object Value
    {
      get { return _value; }
      set
      {
        _value = value;
        bool isValueTextBox = true;
        if (value != null)
        {
          // get the last segment of the type
          string thisType = value.GetType().ToString();
          string[] typeParts = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, thisType, typeDelimiters);
          if (typeParts == null || typeParts.GetLength(0) == 0)
            return;
          string type = typeParts[typeParts.GetLength(0) - 1];
          switch (type)
          {
            case "TableHeader":
              isValueTextBox = false;
              displayTableHeaderGrid();
              break;
            case "DataSet":
              isValueTextBox = false;
              displayDataSetGrid(grdTableHeader, (DataSet)_value);
              break;
            //case "String":
            //  if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, ValueType)).Equals("Include", StringComparison.CurrentCultureIgnoreCase))
            //    isInclude = true;
            //  else
            //    if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, ValueType)).Equals("func", StringComparison.CurrentCultureIgnoreCase))
            //      isFunction = true;
            //  break;
          }
        }

        if (isValueTextBox)
        {
          grdTableHeader.Visible = false;
          chbEqualOperator.Visible = false;
          txbValue2.Visible = true;
          txbValue2.Text = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, _value);
        }
      }
    }
    public Dictionaries Dictionary { get { return DictionaryFactory.getInstance().getDictionary(); } }
    #endregion
    #region constructors
    public ctlAttributeValue()
    {
      InitializeComponent();
      _originalValueHeight = txbValue2.Height;
      //this.cmbValueType2.Items.AddRange(valueTypeList);
      disableAllControls();
    }
    #endregion
    #region public methods
    public void Clear()
    {
      EntityID = null;
      ItemTypeID = null;
      ItemID = null;
      AttributeID = null;
      ValueType = null;
      Value = null;
      ValueHistoryID = null;
    }

    #endregion
    #region events
    #region custom events
    public delegate void AttributeChangeEventHandler(object sender, AttributeChangeEventArgs e);
    public delegate void AttributeSaveEventHandler(object sender, EventArgs e);
    public event AttributeChangeEventHandler AttributeChange;
    public event AttributeSaveEventHandler AttributeSave;
    protected virtual void OnAttributeChange(AttributeChangeEventArgs e)
    {
      if (AttributeChange != null)
      {
        cmdSaveAttributes.Enabled = true;
        //Invokes the delegates.
        AttributeChange(this, e);
      }
    }
    protected virtual void OnAttrbuteSave(EventArgs e)
    {
      if (AttributeSave != null && EAC != null)
      {
        cmdSaveAttributes.Enabled = false;
        AttributeSave(this, e);
      }
    }
    private void raiseChangeEvent(AttributeChangeEventArgs.ChangeType type)
    {
      AttributeChangeEventArgs e = new AttributeChangeEventArgs(EntityID, ItemTypeID, ItemID, AttributeID, ValueType,
        StartDate, EndDate, Value, AttributeChangeEventArgs.ChangeType.ItemType);
      OnAttributeChange(e);
    }
    #endregion
    #region grdTableHeader event methods

    private void grdTableHeader_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (_inDisplayGrid || _gridIsDataset)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid, or if it is not a tableheader

      string headerText = grdTableHeader.Columns[e.ColumnIndex].HeaderText;
      int rowIndex = e.RowIndex;

      // Abort validation if cell is not in the [+/-] column.
      if (!headerText.Equals("[+/-]"))
        return;

      // Confirm that the cell is not empty.
      if (!(string.IsNullOrEmpty(e.FormattedValue.ToString())))
      {
        if (e.FormattedValue.ToString() != "+" && e.FormattedValue.ToString() != "-")
        {
          grdTableHeader.Rows[e.RowIndex].ErrorText = "Only [+/-] is accepted in this column!";
          e.Cancel = true;
        }
      }

    }
    private void grdTableHeader_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (_inDisplayGrid || _gridIsDataset)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid, or if it is not a tableheader

      // Clear the row error in case the user presses ESC.   
      grdTableHeader.Rows[e.RowIndex].ErrorText = String.Empty;
    }
    private void grdTableHeader_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      //if (!(RawMode && txbEntities2.Text.Trim().ToLower() == TAGFunctions.DEFAULT_ENTITY))   //If we are not in raw mode AND editing the Default entity
      if (!(EditMode))   //If we are not in edit mode
        return;  //We validate ONLY in RAW MODE

      if (_inDisplayGrid || _gridIsDataset)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid, or if it is not a tableheader

      if (!grdTableHeader.IsCurrentRowDirty)
        return;
      DataGridViewRow grdRow = grdTableHeader.Rows[e.RowIndex];
      if (grdRow.Cells[2].Value != null && grdRow.Cells[2].Value.ToString() != string.Empty)
      {
        bool isValid = true;
        string errorMessage = string.Empty;
        bool warn = false;
        TableHeaderRow thRow;
        object[] values = new object[_thValue.Columns.Count];
        for (int iCell = 0; iCell < _thValue.Columns.Count; iCell++)
        {
          DataGridViewCell cell = grdRow.Cells[iCell + 2];
          // TODO: do the validation here
          values[iCell] = cell.Value;
        }
        thRow = new TableHeaderRow(_thValue.Columns, values);  //, _dictionary);
        thRow.Source = _thValue.Source;

        foreach (Field a in thRow.Cells)
        {
          isValid = _dictionary.IsValid(a.Value, null, "value", (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, _dictionary.AttributeProperty(a.ID, "DataType")), string.Empty, out warn, out errorMessage);
          if (!isValid)
            break;
        }
        // TODO do validation here
        if (isValid)
        {
          TableHeaderUniqueKey thUniqKey;
          if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, grdRow.Cells[1].Value) == "-")
          {
            if (grdRow.Cells[0].Value != null)
            {
              thRow.Op = "-";
              thUniqKey = (TableHeaderUniqueKey)grdRow.Cells[0].Value;

              //We are deleting a row, let's remove it from the rows collection too!
              if (_thValue.Rows.Contains(thUniqKey))
                _thValue.Rows.Remove(thUniqKey);

              if (!_thValue.DeletedKeyList.ContainsKey(thUniqKey))
                _thValue.DeletedKeyList.Add(thUniqKey, _thValue.Source);
            }
          }
          else
          {
            if (grdRow.IsNewRow || grdRow.Cells[0].Value == null)
            {
              thUniqKey = new TableHeaderUniqueKey(_thValue.Columns, values, _thValue.KeyNames);
              while (_thValue.Contains(thUniqKey))
                thUniqKey.Sequence++;
              grdRow.Cells[0].Value = thUniqKey;
            }
            else
              thUniqKey = (TableHeaderUniqueKey)grdRow.Cells[0].Value;

            _thValue.AddRow(thUniqKey, thRow);

            if (_thValue.DeletedKeyList.ContainsKey(thUniqKey))
              _thValue.DeletedKeyList.Remove(thUniqKey);
          }

          //If we have modified a value then we must set the dirty flag for this attribute!
          if (AttributeID != null)
          {
            TAGAttribute a = ((Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID))).Attributes[AttributeID];
            setAttributeValue(a, _thValue);
            raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute); // tell the container we changed something
          }
        }
        else
        {
          e.Cancel = true;
          TAGExceptionMessage tm = new TAGExceptionMessage("Tableheader edit", "Validate cell", errorMessage);
          CommonFormFunctions.showMessage(tm.ToString());
        }
      }
    }
    private void grdTableHeader_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      //if (!(RawMode && txbEntities2.Text.Trim().ToLower() == TAGFunctions.DEFAULT_ENTITY))   //If we are not in raw mode AND editing the Default entity
      if (!(EditMode))   //If we are not in raw mode
        return;  //We delete ONLY in RAW MODE

      if (_inDisplayGrid)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid!
      // if there were rows selected
      if (_thGridLastRowsSelected != null)
      {
        _thValue.MarkForDelete = true;
        foreach (DataGridViewRow grdRow in _thGridLastRowsSelected) // we delete them all from the list
        {
          TableHeaderUniqueKey thUniqKey = (TableHeaderUniqueKey)grdRow.Cells[0].Value; // get the unique key from the row
          if (_thValue.DeletedKeyList.ContainsKey(thUniqKey))
            _thValue.DeletedKeyList.Remove(thUniqKey);
          else
            if (_thValue.Contains(thUniqKey))
              _thValue.Remove(thUniqKey);
        }
      }
      _thGridLastRowsSelected = null; // and reset the list so if we get called twice or more we don't try to delete them
      setvalue(); // now update the attribute in the eac so we have the new value for the grid
    }
    private void grdTableHeader_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (_gridIsDataset)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid, or if it is not a tableheader
      // we need to do this cause when we delete a row (or rows), it goes away before grdTableHeader_RowsRemoved is called.
      if (grdTableHeader.SelectedRows == null || grdTableHeader.SelectedRows.Count == 0)
        _thGridLastRowsSelected = null;
      else
      {
        _thGridLastRowsSelected = new List<DataGridViewRow>();
        foreach (DataGridViewRow row in grdTableHeader.SelectedRows)
          _thGridLastRowsSelected.Add(row);
      }
    }
    private void grdTableHeader_Validated(object sender, EventArgs e)
    {
      if (_gridIsDataset)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid, or if it is not a tableheader
      raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
    }
    /// <summary>
    /// Copy table header grid rows
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (grdTableHeader.SelectedRows != null && grdTableHeader.SelectedRows.Count > 0)
      {
        foreach (DataGridViewRow grdRow in grdTableHeader.SelectedRows)            // make a copy of each of the rows selected
        {
          TableHeaderUniqueKey key = 
            (TableHeaderUniqueKey)((TableHeaderUniqueKey)grdRow.Cells[0].Value).Clone();  // get a copy of the key for this row
          TableHeaderRow thRow = (TableHeaderRow)_thValue[key].Clone();           // also make a copy of the row
          while (_thValue.Contains(key)) key.Sequence++;                          // make the key unique by incrementing the sequence
          _thValue.AddRow(key, thRow);                                            // then add a copy of the row with the new key                            
        }
        setvalue();                                                               // make sure the new tableheader is updated in the eac
        displayTableHeaderGrid();                                                 // now refresh so we can see it
      }
      else
        CommonFormFunctions.showMessage("No rows are selected");
    }
    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (grdTableHeader.SelectedRows != null && grdTableHeader.SelectedRows.Count > 0)
      {
        foreach (DataGridViewRow grdRow in grdTableHeader.SelectedRows)
          grdTableHeader.Rows.Remove(grdRow);
      }
      else
        CommonFormFunctions.showMessage("No rows are selected");
    }
    private void chbEqualOperator_CheckedChanged(object sender, EventArgs e)
    {
      _thValue.CanMergeNewSource = !chbEqualOperator.Checked;
      if (_thValue.Count > 0)
      {
        if (chbEqualOperator.Checked)
          _thValue.Rows[0].Op = TableHeader.OPERATORSTOPMERGE;
        else
          _thValue.Rows[0].Op = TableHeader.OPERATORADD;
      }
      setvalue(); // now update the attribute in the eac so we have the new value for the grid
    }


    #endregion grdTableHeader events methods
    #region grdHistory event methods

    private void grdHistory_KeyUp(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Delete:
          if (grdHistory.Columns.Count == _valueHistoryColumnDefinition.GetLength(0))
            deleteValueHistoryRow();
          break;
      }
    }
    private void grdHistory_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (!_inDisplayGrid)
      {
        DataGridViewRow row = grdHistory.CurrentRow;
        bool isValueHistory;
        object[,] columnDefinition;
        if (row.Cells.Count != _valueHistoryColumnDefinition.GetLength(0))
        {
          columnDefinition = _itemHistoryColumnDefinition;
          isValueHistory = false;
        }
        else
        {
          columnDefinition = _valueHistoryColumnDefinition;
          isValueHistory = true;
        }
        if (row != null && row.IsNewRow && !_inNewRow)
        {
          btnDelHistoryRow.Visible = false;
          if (isValueHistory)
            addValueHistoryRow(row);
          else
            addItemHistoryRow(row);
        }
        else
        {
          btnDelHistoryRow.Visible = true;
          ValueHistoryID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells[(string)columnDefinition[0, 0]].Value);  // reset history id to current row
          try
          {
            Value = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes[AttributeID].Values[ValueHistoryID].Value;
          }
          catch (Exception ex)
          {
            Value = string.Empty;
          }
        }
      }
    }
    private void grdHistory_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      _inNewRow = false;
      if (_inDisplayGrid)   // don't try to validate if we are just loading the grid for display
        return;
      const string colStartDate = "startdate";
      const string colEndDate = "enddate";
      const string colValueType = "valuetype";
      bool isItemHistory = false;
      if (grdHistory.Columns.Count == _itemHistoryColumnDefinition.GetLength(0))
        isItemHistory = true;
      DataView dv = (DataView)grdHistory.DataSource;
      DataTable dt = dv.Table;
      DataGridViewRow row = grdHistory.Rows[e.RowIndex];
      Item item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
      bool dataChanged = false;
      if (item != null)
      {
        if (isItemHistory)
        {
          string itemHistoryID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells[0].Value);
          if (item.ItemHistoryRecords.Contains(itemHistoryID))
          {
            ItemHistory ih = item.ItemHistoryRecords[itemHistoryID];
            foreach (DataGridViewColumn col in grdHistory.Columns)
            {
              switch (col.Name.ToLower())
              {
                case colStartDate:
                  DateTime startDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row.Cells[col.Name].Value, TAGFunctions.PastDateTime);
                  if (startDate != ih.StartDate) // startdate was changed
                  {
                    // changing the startdate changed the id so we remove the old one, add the new one, and then update the grid
                    item.ItemHistoryRecords.MarkForDelete = false;
                    item.ItemHistoryRecords.Remove(itemHistoryID);
                    ih.StartDate = startDate;
                    item.ItemHistoryRecords.Add(ih);
                    setReadOnly(_itemHistoryColumnDefinition, dt, false);
                    row.Cells[0].Value = ih.ID;    // start date change changes the ID, so we have to update
                    setReadOnly(_itemHistoryColumnDefinition, dt, true);
                    dataChanged = true;
                  }
                  break;
                case colEndDate:
                  DateTime endDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row.Cells[col.Name].Value, TAGFunctions.FutureDateTime);
                  if (endDate != ih.EndDate) // startdate was changed
                  {
                    ih.EndDate = endDate;
                    dataChanged = true;
                  }
                  break;
                // don't do anything with the historyid
              }
            }
          }
          if (dataChanged)
            raiseChangeEvent(AttributeChangeEventArgs.ChangeType.ItemHistory);
        }
        else
        {
          // otherwise this is value history
          TAGAttribute a = item.Attributes[AttributeID];
          if (a != null)
          {
            if (a.HasHistory && a.Values.Contains(ValueHistoryID))
            {
              ValueHistory vh = a.Values[ValueHistoryID];
              foreach (DataGridViewColumn col in grdHistory.Columns)
              {
                switch (col.Name.ToLower())
                {
                  case colStartDate:
                    DateTime startDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row.Cells[col.Name].Value, TAGFunctions.PastDateTime);
                    if (startDate != vh.StartDate) // startdate was changed
                    {
                      // changing the startdate changed the id so we remove the old one, add the new one, and then update the grid
                      a.Values.MarkForDelete = false;
                      a.Values.Remove(ValueHistoryID);
                      vh.StartDate = startDate;
                      a.Values.Add(vh);
                      setReadOnly(_valueHistoryColumnDefinition, dt, false);
                      row.Cells[0].Value = vh.ID;
                      setReadOnly(_valueHistoryColumnDefinition, dt, false);
                      dataChanged = true;
                    }
                    break;
                  case colEndDate:
                    DateTime endDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row.Cells[col.Name].Value, TAGFunctions.FutureDateTime);
                    if (endDate != vh.EndDate) // startdate was changed
                    {
                      vh.EndDate = endDate;
                      dataChanged = true;
                    }
                    break;
                  case colValueType:
                    string valueType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells[col.Name].Value);
                    if (!valueType.Equals(vh.ValueType, StringComparison.CurrentCultureIgnoreCase)) // value type was changed
                    {
                      vh.ValueType = valueType;
                      dataChanged = true;
                    }
                    break;
                }
              }
              if (dataChanged)
              {
                //if (a.Values.WillOverlap(vh))
                //{
                //  cell.ErrorText = "Overlapping dates are not allowed";
                //  e.Cancel - true;
                //}
                //raiseChangeEvent(AttributeChangeEventArgs.ChangeType.ValueHistory);
              }
            }
          }
        }
      }

    }
    private void grdHistory_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
    }    
    private void grdHistory_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (_inDisplayGrid)    //This was fired because of a call from displayTableHeaderGrid
        return;  //We do NOT run this event if we are processing the displayTableHeaderGrid, or if it is not a tableheader
      DataGridViewRow row = grdHistory.Rows[e.RowIndex];
      //if (!row.IsNewRow)
      // Clear the row error in case the user presses ESC.   
      row.ErrorText = String.Empty;
    }

    #endregion
    #region other events
    private void txbValue2_TextChanged(object sender, EventArgs e)
    {

    }
    private void txbValue2_Validated(object sender, EventArgs e)
    {
      if (AttributeID != string.Empty)
      {
        if (_dictionary.AttributeProperties().Contains(AttributeID))
          _value = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, txbValue2.Text, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, _dictionary.AttributeProperty(AttributeID, "DataType")));
        else
          _value = txbValue2.Text;
        setvalue();
        raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
      }
    }
    private void cmbValueType2_SelectedIndexChanged(object sender, EventArgs e)
    {
      raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
    }
    private void txbAttribute2_TextChanged(object sender, EventArgs e)
    {
      //raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
    }
    private void txbItem2_TextChanged(object sender, EventArgs e)
    {
      //raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
    }
    private void txbItemType2_TextChanged(object sender, EventArgs e)
    {
      //raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
    }
    private void ctlAttributeValue_Validating(object sender, CancelEventArgs e)
    {
      //if (EAC == null || EntityID == string.Empty || ItemTypeID == string.Empty || ItemID == string.Empty
      //  || AttributeID == string.Empty)
      //{
      //  CommonFormFunctions.showMessage("Cannot edit a Value, there is not attribute selected");
      //  e.Cancel = true;
      //}
      //string errorMessage = string.Empty;
      //bool warn = false;
      //if (!Dictionary.IsValid(Value, null, ValueType, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Dictionary.AttributeProperty(AttributeID, "DataType")), 
      //  string.Empty, out warn, out errorMessage))
      //{
      //  e.Cancel = true;
      //  CommonFormFunctions.showMessage(errorMessage);
      //}
    }
    private void ctlIncludeEntry1_OnChanged(object sender, EventArgs e)
    {
      //_value = ctlIncludeEntry1.Text;
      setvalue();
      raiseChangeEvent(AttributeChangeEventArgs.ChangeType.ItemHistory);
    }
    private void txbItem2_Validated(object sender, EventArgs e)
    {
      if (_oldItemID != null && _oldItemID.EndsWith("_1"))  // we are renaming an item created by a user clone/copy
      {
        if (EAC != null && EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID))
        {
          ItemType it = EAC.Entities[EntityID].ItemTypes[ItemTypeID];
          if (it.Contains(_oldItemID))
          {
            Item newItem = it.Items[_oldItemID];
            it.MarkForDelete = false;
            it.Items.Remove(_oldItemID); // get rid of the old one
            it.MarkForDelete = true;
            newItem.ID = ItemID;                                                    // rename to what the user asked for
            newItem.Deleted = false;
            newItem.Dirty = true;
            foreach (TAGAttribute a in newItem.Attributes)                          // make each attribute dirty so it saves them all
              a.Dirty = true;
            it.Items.Add(newItem);       // and add it back
            cmdSaveAttributes.Enabled = true;                                       // and allow the user to save it
          }
        }
      }
    }
    private void txbItem2_Validating(object sender, CancelEventArgs e)
    {
      if (txbItem2.Text != null && txbItem2.Text.EndsWith("_1"))  //   this is an unsaved, cloned item
      {
        CommonFormFunctions.showMessage("Item ID must not end with '_1'");
        e.Cancel = true;
      }
      else
      {
        if (_oldItemID != null && _oldItemID.EndsWith("_1"))  // this is an item created by a user clone/copy
        {
          if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID))
          {
            if (EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID)) // there is already one out there
            {
              CommonFormFunctions.showMessage(string.Format("Item {0} already exists in {1}.{2}", ItemID, EntityID, ItemTypeID));
              e.Cancel = true;
            }
          }
        }
      }
    }
    #endregion
    #region command buttons
    private void cmdSaveAttributes_Click(object sender, EventArgs e)
    {
      OnAttrbuteSave(e);
    }
    private void cmdCkFunc_Click(object sender, EventArgs e)
    {
      if (RawMode)
        if (ValueType.Equals("func", StringComparison.CurrentCultureIgnoreCase))
        {
          string returnMessage = Dictionary.IsValidFunction((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Value), AttributeID);
          if (!returnMessage.Equals(Dictionaries.ISVALIDFUNC))
            CommonFormFunctions.showMessage(returnMessage);
        }
        else
          CommonFormFunctions.showMessage("Must be a valuetype of func to use this button");
      else
        CommonFormFunctions.showMessage("Must be in raw mode to use this button");
    }
    private void btnAddIT_Click(object sender, EventArgs e)
    {
      if (RawMode)
      {
        if (EntityID != string.Empty && ItemTypeID != string.Empty && EAC.Entities.Contains(EntityID) && !EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID))
        {
          ItemType newIT = new ItemType();
          newIT.ID = ItemTypeID.Trim();
          EAC.Entities[EntityID].ItemTypes.Add(newIT);    //This adds the new itemType to the current collection!
          EAC.Entities[EntityID].ItemTypes.Dirty = true;
          btnAddIT2.Visible = false;
          raiseChangeEvent(AttributeChangeEventArgs.ChangeType.ItemType);
        }
      }
    }
    private void btnAddItem_Click(object sender, EventArgs e)
    {
      //if (RawMode)
      //{
        if (EntityID != string.Empty && ItemTypeID != string.Empty && ItemID != string.Empty)
        {
          Item newItem = new Item();
          newItem.ID = ItemID.Trim();
          EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Add(newItem);    //This adds the new itemType to the current collection!
          EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Dirty = true;

          btnDelItem2.Top = btnAddItem2.Top;
          btnDelItem2.Left = btnAddItem2.Left;
          btnAddItem2.Visible = false;
          btnDelItem2.Visible = true;
          raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Item);
        }
      //}
    }
    private void btnDelItem2_Click(object sender, EventArgs e)
    {
      //if (RawMode)
      //{
        if (EntityID != string.Empty && ItemTypeID != string.Empty && ItemID != string.Empty)
        {
          if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
                && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID))
          {
            bool bMarkForDelete = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.MarkForDelete;
            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.MarkForDelete = true;
            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Remove(ItemID);    //This adds the new itemType to the current collection!
            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.MarkForDelete = bMarkForDelete;

            ItemID = string.Empty;
            AttributeID = string.Empty;
            ValueHistoryID = string.Empty;
            Value = string.Empty;

            btnAddItem2.Visible = true;
            btnDelItem2.Visible = false;
            raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
          }
        }
      //}
    }
    private void btnAddAttr2_Click(object sender, EventArgs e)
    {
      //if (RawMode)
      //{
        if (EntityID != string.Empty && ItemTypeID != string.Empty && ItemID != string.Empty && AttributeID != string.Empty)
        {
          if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
                && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID)
                && !EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Contains(AttributeID) )
          {
            TAGAttribute newItemAttr = new TAGAttribute();
            ValueHistory newItemAttrHst = new ValueHistory();

            newItemAttr.OverwriteValue = true;
            newItemAttr.ID = AttributeID.Trim();

            if (_valueType != null)
              newItemAttr.ValueType = ValueType;
            else
            {
              newItemAttr.ValueType = TAGFunctions.VALUE;
              //for (int i = 0; i < cmbValueType2.Items.Count; i++)
              //  if (cmbValueType2.Items[i].ToString().Trim().ToLower() == newItemAttr.ValueType)
              //    cmbValueType2.SelectedItem = cmbValueType2.Items[i];
            }
            newItemAttr.Value = Value;

            if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, _value)).Trim() != string.Empty)
            {
              _startDate = TAGFunctions.PastDateTime;
              _endDate = TAGFunctions.FutureDateTime;

              newItemAttrHst.ID = TAGFunctions.PastDateTime.ToString();
              newItemAttrHst.StartDate = TAGFunctions.PastDateTime;
              newItemAttrHst.EndDate = TAGFunctions.FutureDateTime;
              string selectedAttrHistID = newItemAttrHst.ID;
              newItemAttrHst.ValueType = ValueType;
              newItemAttrHst.Value = Value;
              newItemAttr.Values.Add(newItemAttrHst);    //Add the new value history with at least this first valuehistory
            }

            Item item = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID];
            item.Attributes.Add(newItemAttr);
            item.Dirty = true;
            btnDelAttr2.Top = btnAddAttr2.Top;
            btnDelAttr2.Left = btnAddAttr2.Left;
            btnAddAttr2.Visible = false;
            btnDelAttr2.Visible = true;
            raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
          }
        }
      //}
    }
    private void btnDelAttr2_Click(object sender, EventArgs e)
    {
      //if (RawMode)
      //{
        if (EntityID != string.Empty && ItemTypeID != string.Empty && ItemID != string.Empty && AttributeID != string.Empty)
        {
          if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
                && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID)
                && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Contains(AttributeID))
          {
            bool bMarkForDelete = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes.MarkForDelete;
            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes.MarkForDelete = true;
            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes.Remove(AttributeID);    //This adds the new itemType to the current collection!
            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes.MarkForDelete = bMarkForDelete;

            EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Dirty = true;

            AttributeID = string.Empty;
            ValueHistoryID = string.Empty;
            Value = string.Empty;

            btnAddAttr2.Visible = true;
            btnDelAttr2.Visible = false;
            raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
          }
        }
      //}
    }
    private void btnDelIT_Click(object sender, EventArgs e)
    {
      if (EntityID != string.Empty
          && ItemTypeID != string.Empty)
      {
        if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID))
        {
          Entity entity = EAC.Entities[EntityID];
          bool saveMarkForDelete = entity.ItemTypes.MarkForDelete;
          entity.ItemTypes.Remove(ItemTypeID);
          entity.ItemTypes.MarkForDelete = saveMarkForDelete;
        }
      }
    }
    private void btnDelHistoryRow_Click(object sender, EventArgs e)
    {
      if (grdHistory.Columns.Count == _valueHistoryColumnDefinition.GetLength(0))
        deleteValueHistoryRow();
      else
        deleteItemHistoryRow();
    }
    #endregion

    #endregion
    #region private methods

    public void SetEnabledControls()
    {
      disableAllControls();
      if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, EntityID) != string.Empty)
      {
        if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, ItemTypeID) == string.Empty)
        {
          txbItemType2.Enabled = true;
          btnAddIT2.Visible = true;
        }
        else
        {
          if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, ItemID) == string.Empty)
          {
            btnDelIT.Visible = true;
            txbItem2.Enabled = true;
            btnAddItem2.Visible = true;
          }
          else
          {
            if (txbItem2.Text.EndsWith("_1")) // this was a copied/cloned item
              txbItem2.Enabled = true;
            if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, AttributeID) == string.Empty)
            {
              btnDelItem2.Visible = true;
              txbAttribute2.Enabled = true;
              btnAddAttr2.Visible = true;
            }
            else
            {
              if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, ValueHistoryID) == string.Empty)
              {
                btnDelAttr2.Visible = true;
              }
              else
              {
                if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, ValueType)).Equals(TAGFunctions.FUNC, StringComparison.CurrentCultureIgnoreCase))
                  cmdCkFunc.Visible = true;
              }
            }
          }
        }
      }
    }
    private void disableAllControls()
    {
      txbEntity2.Enabled = false;
      txbItemType2.Enabled = false;
      txbItem2.Enabled = false;
      txbAttribute2.Enabled = false;
      btnAddAttr2.Visible = false;
      btnDelAttr2.Visible = false;
      btnAddItem2.Visible = false;
      btnDelItem2.Visible = false;
      btnAddIT2.Visible = false;
      btnDelIT.Visible = false;
      btnDelItem2.Visible = false;
      cmdCkFunc.Visible = false;
    }
    private void displayDataSetGrid(DataGridView grid, DataSet ds)
    {
      if (ds != null && ds.Tables.Count > 0)
      {
        _inDisplayGrid = true;   //Set the flag to true, since we are entering into this method
        _gridIsDataset = true;
        CommonFormFunctions.displayDataSetGrid(grid, ds);
        bool isValueHistory = (grid.Name == grdHistory.Name && grdHistory.Columns.Count == _valueHistoryColumnDefinition.GetLength(0));
        if (grid.Name == grdTableHeader.Name)
        {
          grid.Top = txbValue2.Top;
          grid.Left = txbValue2.Left;
          txbValue2.Visible = false;
        }
        else
        {
          if (isValueHistory) // if this is the value history grid
          {
            // then set up the combobox for ValueType column
            foreach (DataGridViewRow row in grid.Rows)
            {
              DataGridViewComboBoxCell valueTypeCell = new DataGridViewComboBoxCell();
              valueTypeCell.ValueType = (Type)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.dsDataType, TAGFunctions.DATATYPESTRING) ;
              valueTypeCell.Items.AddRange(valueTypeList);
              valueTypeCell.Value = row.Cells["Value"].Value;
              row.Cells["ValueType"] = valueTypeCell;
            }
          }
        }
        _gridIsDataset = false;
        _inDisplayGrid = false;   //Set the flag to true, since we are entering into this method
      }
    }
    private void displayTableHeaderGrid()
    {
      _inDisplayGrid = true;   //Set the flag to true, since we are entering into this method

      if (_thValue != null)
      {
        _gridIsDataset = false;
        chbEqualOperator.Checked = !(_thValue.CanMergeNewSource);

        grdTableHeader.Rows.Clear();
        grdTableHeader.Columns.Clear();
        grdTableHeader.DataSource = null;

        for (int k1 = 0; k1 < _thValue.Columns.Count; k1++)
        {
          DataGridViewColumn dataCol = null;
          if (k1 == 0)
          {
            dataCol = new DataGridViewColumn();
            dataCol.Name = "thRowID";
            dataCol.HeaderText = "Row ID";
            dataCol.CellTemplate = new DataGridViewTextBoxCell();
            dataCol.Visible = false;
            grdTableHeader.Columns.Add(dataCol);

            dataCol = new DataGridViewColumn();
            dataCol.Name = "plusMinus";
            dataCol.HeaderText = "[+/-]";
            dataCol.CellTemplate = new DataGridViewTextBoxCell();
            dataCol.Visible = _rawMode;   // only display the +/- column in raw mode
            dataCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataCol.SortMode = DataGridViewColumnSortMode.Automatic;
            grdTableHeader.Columns.Add(dataCol);

          }
          TableHeaderColumn col = _thValue.Columns[k1];
          dataCol = new DataGridViewColumn();
          dataCol.Name = col.OriginalID;
          dataCol.HeaderText = col.Caption;
          PickList pickList = null;
          //
          // the problem with this approach is that the picklist attribute will only be there if it is in the same raw item as the tableheader
          // Michelle has approved this on 1/18/2011
          //
          string pickListAttribute = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Dictionary.AttributeProperty(col.OriginalID, "PickList"));
          if (EAC == null)
            dataCol.CellTemplate = new DataGridViewTextBoxCell();
          else
          {
            Item currentItem = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
            if (currentItem != null && currentItem.Attributes.Contains(pickListAttribute))
              pickList = ((TableHeader)TAGFunctions.evaluateFunction(
                TAGFunctions.EnumFunctions.CTableHeader, pickListAttribute, currentItem.Attributes[pickListAttribute].Value)).ToPickList();
            if (pickList != null)
            {
              dataCol.CellTemplate = new DataGridViewComboBoxCell();
              ((DataGridViewComboBoxCell)dataCol.CellTemplate).Items.AddRange(populateGridPicklist(pickList));
            }
            else
              dataCol.CellTemplate = new DataGridViewTextBoxCell();
          }
          dataCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
          dataCol.SortMode = DataGridViewColumnSortMode.Automatic;
          grdTableHeader.Columns.Add(dataCol);
        }


        foreach (TableHeaderUniqueKey rowUniqID in _thValue.rowCollection.Keys)
        {
          TableHeaderRow row = _thValue.rowCollection[rowUniqID];
          if (!row.Deleted)
          {
            DataGridViewRow dataRow = new DataGridViewRow();
            for (int k1 = 0; k1 < _thValue.Columns.Count; k1++)
            {
              TableHeaderColumn col = _thValue.Columns[k1];
              DataGridViewTextBoxCell cell = null;
              if (k1 == 0)
              {
                cell = new DataGridViewTextBoxCell();
                cell.Value = rowUniqID;
                dataRow.Cells.Add(cell);

                cell = new DataGridViewTextBoxCell();
                if (row.Op == "-")
                  cell.Value = "-";
                else
                  cell.Value = "+";

                dataRow.Cells.Add(cell);
              }
              cell = new DataGridViewTextBoxCell();
              cell.Value = row[col.OriginalID].Value;
              dataRow.Cells.Add(cell);
            }
            grdTableHeader.Rows.Add(dataRow);
          }
        }

        //AddDeleted row collection!
        foreach (TableHeaderUniqueKey rowUniqID in _thValue.DeletedKeyList.Keys)
        {
          object[] values = rowUniqID.ToRowValues(_thValue.Columns);

          TableHeaderRow row = new TableHeaderRow(_thValue.Columns, values);
          DataGridViewRow dataRow = new DataGridViewRow();
          for (int k1 = 0; k1 < _thValue.Columns.Count; k1++)
          {
            TableHeaderColumn col = _thValue.Columns[k1];

            DataGridViewTextBoxCell cell = null;
            if (k1 == 0)
            {
              cell = new DataGridViewTextBoxCell();
              cell.Value = rowUniqID;
              dataRow.Cells.Add(cell);

              cell = new DataGridViewTextBoxCell();
              cell.Value = "-";
              dataRow.Cells.Add(cell);
            }
            cell = new DataGridViewTextBoxCell();
            cell.Value = row[col.OriginalID].Value;
            dataRow.Cells.Add(cell);
          }
          grdTableHeader.Rows.Add(dataRow);
        }

        grdTableHeader.Top = txbValue2.Top;
        grdTableHeader.Left = txbValue2.Left;
        txbValue2.Visible = false;
        grdTableHeader.Visible = true;
        chbEqualOperator.Visible = _rawMode && grdTableHeader.Visible;
        grdTableHeader.Refresh();

      }
      _inDisplayGrid = false;
    }
    private void displayValueHistoryGrid()
    {
      Item item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
      if (item != null && item.Attributes.Contains(AttributeID))
      {
        TAGAttribute a = item.Attributes[AttributeID];
        if (a.HasHistory)
          displayValueHistoryGrid(a.Values);
      }
    }
    private void displayValueHistoryGrid(ValueHistoryCollection history)
    {
      int lastRow = 0;
      if (grdHistory.CurrentRow != null)
        lastRow = grdHistory.CurrentRow.Index;
      DataSet ds = loadValueHistory(history);
      displayDataSetGrid(grdHistory, ds);
      if (lastRow > 0 && lastRow < grdHistory.Rows.Count) // valid row index
        grdHistory.Rows[lastRow].Selected = true;
      grdHistory.Refresh();
    }
    private void displayItemHistoryGrid()
    {
      if (EAC != null)
      {
        Item item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
        if (item != null && item.ItemHistoryRecords.Count > 0)
          displayItemHistoryGrid(item.ItemHistoryRecords);
        else
          displayItemHistoryGrid(new ItemHistoryCollection());
      }
    }
    private void displayItemHistoryGrid(ItemHistoryCollection itemHistory)
    {
      DataSet ds = loadItemHistory(itemHistory);
      displayDataSetGrid(grdHistory, ds);
      grdHistory.Refresh();
    }
    private DataSet loadValueHistory(ValueHistoryCollection history)
    {
      DataSet ds = new DataSet("ValueHistory");
      DataTable dt = new DataTable("Value");
      for (int i = 0; i < _valueHistoryColumnDefinition.GetLength(0); i++)
      {
        DataColumn dataCol = new DataColumn((string)_valueHistoryColumnDefinition[i, 0], (Type)_valueHistoryColumnDefinition[i, 1]);
        //dataCol.ReadOnly = (bool)columnDefinition[i, 2];
        dataCol.Caption = (string)_valueHistoryColumnDefinition[i, 3];
        dt.Columns.Add(dataCol);
      }
      ds.Tables.Add(dt);
      history.Sort();
      foreach (ValueHistory vh in history)
      {
        DataRow row = dt.NewRow();
        foreach (DataColumn column in dt.Columns)
        {
          object value = null;
          switch (column.ColumnName)
          {
            case "HistoryID":
              value = vh.ID;
              break;
            case "StartDate":
              if (vh.StartDate == TAGFunctions.PastDateTime)
                value = null;
              else
                value = vh.StartDate;
              break;
            case "EndDate":
              if (vh.EndDate == TAGFunctions.FutureDateTime)
                value = null;
              else
                value = vh.EndDate;
              break;
            case "ValueType":
              value = "Value";  // if no match default is Value
              // get the case matching version from the picklist and use that for the valuetype so combobox doesn't fail
              value = getValidValueType(vh.ValueType); 
              break;
            case "Value":
              value = vh.Value;
              break;
            case "LastModifiedBy":
              value = vh.LastModifiedBy;
              break;
            case "LastModifiedDateTime":
              value = vh.LastModifiedDateTime;
              break;
          }
          row.SetField(column, value);
        }
        dt.Rows.Add(row);
      }
      setReadOnly(_valueHistoryColumnDefinition, dt, true);
      return ds;
    }
    private DataSet loadItemHistory(ItemHistoryCollection history)
    {
      DataSet ds = new DataSet("ItemHistory");
      DataTable dt = new DataTable("Value");
      for (int i = 0; i < _itemHistoryColumnDefinition.GetLength(0); i++)
      {
        DataColumn dataCol = new DataColumn((string)_itemHistoryColumnDefinition[i, 0], (Type)_itemHistoryColumnDefinition[i, 1]);
        //dataCol.ReadOnly = (bool)columnDefinition[i, 2];
        dataCol.Caption = (string)_itemHistoryColumnDefinition[i, 3];
        dt.Columns.Add(dataCol);
      }
      ds.Tables.Add(dt);
      history.Sort();
      foreach (ItemHistory ih in history)
      {
        DataRow row = dt.NewRow();
        foreach (DataColumn column in dt.Columns)
        {
          object value = null;
          switch (column.ColumnName)
          {
            case "HistoryID":
              value = ih.ID;
              break;
            case "StartDate":
              if (ih.StartDate == TAGFunctions.PastDateTime)
                value = null;
              else
                value = ih.StartDate;
              break;
            case "EndDate":
              if (ih.EndDate == TAGFunctions.FutureDateTime)
                value = null;
              else
                value = ih.EndDate;
              break;
          }
          row.SetField(column, value);
        }
        dt.Rows.Add(row);
      }
      setReadOnly(_itemHistoryColumnDefinition, dt, true);
      return ds;
    }
    private void setReadOnly(object[,] columnDefinition, DataTable dt, bool readOnly)
    {
      for (int i = 0; i < columnDefinition.GetLength(0); i++)
      {
        DataColumn dataCol = dt.Columns[(string)columnDefinition[i, 0]];
        if (readOnly) // if true, then use the ones in the definition
          dataCol.ReadOnly = (bool)columnDefinition[i, 2];
        else
          dataCol.ReadOnly = false; // otherwise turn them all off    
      }
    }
    private void deleteValueHistoryRow()
    {
      if (RawMode)
      {
        bool changedSomething = false;
        TAGAttribute a = null;
        if (grdHistory.SelectedRows.Count > 0)
        {
          foreach (DataGridViewRow row in grdHistory.SelectedRows)
          {
            a = deleteValueHistoryRow((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells[0].Value));
            if (a != null)
              changedSomething = true;
          }
        }
        else
        {
          a = deleteValueHistoryRow(ValueHistoryID);
          if (a != null)
            changedSomething = true;
        }
        if (changedSomething)
        {
          if (grdHistory.Rows.Count > 0)  // if there are any rows left, select the first one
          {
            DataGridViewRow newSelctedRow = grdHistory.Rows[0];
            newSelctedRow.Selected = true;
            ValueHistoryID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, newSelctedRow.Cells[0].Value);
            Value = a.Values[ValueHistoryID].Value;
          }
          else
          {
            ValueHistoryID = string.Empty;
            Value = string.Empty;
          }
          raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
          if (a != null)
          {
            loadValueHistory(a.Values);
          }
        }
      }
    }
    private TAGAttribute deleteValueHistoryRow(string historyID)
    {
      TAGAttribute a;
      if (EntityID != string.Empty
          && ItemTypeID != string.Empty
          && ItemID != string.Empty
          && AttributeID != string.Empty)
      {
        if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
              && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID)
              && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Contains(AttributeID)
              && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes[AttributeID].Values.Contains(historyID))
        {
          bool bMarkForDelete = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes[AttributeID].Values.MarkForDelete;
          a = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes[AttributeID];
          a.Values.MarkForDelete = false;
          a.Values.Remove(ValueHistoryID);    //This adds the new itemType to the current collection!
          a.Values.MarkForDelete = bMarkForDelete;

          a.Dirty = true;
          DataGridViewRow row = getHistoryRow(historyID);
          if (row != null)
            grdHistory.Rows.Remove(row);
          return a;
        }
        else
          CommonFormFunctions.showMessage("Attribute does not exist");
      }
      else
        CommonFormFunctions.showMessage("Attribute does not exist");
      return null;
    }
    private void deleteItemHistoryRow()
    {
      if (RawMode)
      {
        bool changedSomething = false;
        Item item = null;
        if (grdHistory.SelectedRows.Count > 0)
        {
          foreach (DataGridViewRow row in grdHistory.SelectedRows)
          {
            if (EntityID != string.Empty
              && ItemTypeID != string.Empty
              && ItemID != string.Empty)
            {
              string historyID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells["HistoryID"].Value);
              item = deleteItemHistoryRow(historyID);
              if (item != null)
                changedSomething = true;
            }
          }
        }
        else
        {
          item = deleteItemHistoryRow(ItemHistoryID);
          if (item != null)
            changedSomething = true;
        }
        if (changedSomething)
        {
          raiseChangeEvent(AttributeChangeEventArgs.ChangeType.ItemHistory);
          if (item != null)
          {
            loadItemHistory(item.ItemHistoryRecords);
          }
        }
      }
    }
    private Item deleteItemHistoryRow(string historyID)
    {
      if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
            && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID)
            && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].ItemHistoryRecords.Contains(historyID))
      {
        Item item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
        bool bMarkForDelete = item.ItemHistoryRecords.MarkForDelete;
        item.ItemHistoryRecords.MarkForDelete = false;
        item.ItemHistoryRecords.Remove(historyID);    //This adds the new itemType to the current collection!
        item.ItemHistoryRecords.MarkForDelete = bMarkForDelete;
        DataGridViewRow deletedRow = getHistoryRow(historyID);
        if (deletedRow != null)
          grdHistory.Rows.Remove(deletedRow);
        item.Dirty = true;
        if (grdHistory.Rows.Count > 0)  // if there are any left, we select the first row
        {
          DataGridViewRow row = grdHistory.Rows[0];
          ItemHistoryID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells[0]);
        }
        ItemHistoryID = string.Empty;
        return item;
      }
      else
        return null;
    }
    private void addItemHistoryRow(DataGridViewRow row)
    {
      if (RawMode)
      {
        _inNewRow = true;
        if (EntityID != string.Empty
          && ItemTypeID != string.Empty
          && ItemID != string.Empty)
        {
          Item item = null;
          if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
             && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID))
          {
            item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
            if (item != null)
            {
              ItemHistory newIH = null;
              ItemHistory oldIH = null;
              
              if (item.ItemHistoryRecords.Count > 0)
              {
                string currentID = string.Empty;
                string highestID = string.Empty;
                //DateTime highestEndDate = TAGFunctions.PastDateTime;
                foreach (ItemHistory myIH in item.ItemHistoryRecords)
                {
                  highestID = myIH.ID;
                  //if (myIH.EndDate > highestEndDate)
                  //  highestEndDate = myIH.EndDate;
                  if (myIH.StartDate <= _effectiveDate && _effectiveDate <= myIH.EndDate)
                  {
                    currentID = myIH.ID;
                    break;
                  }
                }
                if (currentID == string.Empty)
                  oldIH = item.ItemHistoryRecords[highestID];
                else
                  oldIH = item.ItemHistoryRecords[currentID];
                newIH = (ItemHistory)oldIH.Clone();
                newIH.EndDate = TAGFunctions.FutureDateTime;
                newIH.StartDate = _effectiveDate;
                if (oldIH.EndDate > _effectiveDate)
                  oldIH.EndDate = _effectiveDate.AddDays(-1);
              }
              else
              {
                newIH = new ItemHistory();
                newIH.StartDate = _effectiveDate;
                newIH.EndDate = TAGFunctions.FutureDateTime;
              }

              item.ItemHistoryRecords.Add(newIH);
              item.Dirty = true;
              DataTable dt = ((DataView)grdHistory.DataSource).Table;
              setReadOnly(_itemHistoryColumnDefinition, dt, false); // turn off read only
              row.Cells["HistoryID"].Value = newIH.ID;
              row.Cells["StartDate"].Value = newIH.StartDate;
              row.Cells["EndDate"].Value = newIH.EndDate;
              DataGridViewRow oldRow = null;
              if (oldIH != null)
                oldRow = getHistoryRow(oldIH.ID);
              if (oldRow != null)
                oldRow.Cells["EndDate"].Value = newIH.StartDate.AddDays(-1);
              ValueHistoryID = newIH.ID;
              raiseChangeEvent(AttributeChangeEventArgs.ChangeType.ItemHistory);
            }

            grdHistory.Rows[grdHistory.Rows.Count - 1].Cells[1].Selected = true;  // set cursor to the current row          }
          }
        }
      }
      
    }
    private void addValueHistoryRow(DataGridViewRow row)
    {
      if (RawMode)
      {
        _inNewRow = true;
        if (EntityID != string.Empty
          && ItemTypeID != string.Empty
          && ItemID != string.Empty
          && AttributeID != string.Empty)
        {
          TAGAttribute a = null;
          if (EAC.Entities.Contains(EntityID) && EAC.Entities[EntityID].ItemTypes.Contains(ItemTypeID)
                && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items.Contains(ItemID)
                && EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Contains(AttributeID))
          {
            a = EAC.Entities[EntityID].ItemTypes[ItemTypeID].Items[ItemID].Attributes[AttributeID];

            ValueHistory newVH = null;
            ValueHistory oldVH = null;
            string highestHistoryID = string.Empty;
            if (a.Values != null && a.Values.Count > 0)
            {
              string currentID = string.Empty;
              foreach (ValueHistory myVH in a.Values)
              {
                highestHistoryID = myVH.ID;
                if (myVH.StartDate <= _effectiveDate && _effectiveDate <= myVH.EndDate)
                {
                  currentID = myVH.ID;
                  break;
                }
              }
              if (currentID == string.Empty)
                currentID = highestHistoryID;
              oldVH = a.Values[currentID];
              newVH = (ValueHistory)oldVH.Clone();
              newVH.EndDate = TAGFunctions.FutureDateTime;
              newVH.StartDate = _effectiveDate;
              if (oldVH.EndDate > _effectiveDate)
                oldVH.EndDate = _effectiveDate.AddDays(-1);
            }
            else
            {
              newVH = new ValueHistory();
              newVH.StartDate = _effectiveDate;
              newVH.EndDate = TAGFunctions.FutureDateTime;
              newVH.ValueType = a.ValueType;
              newVH.Value = a.Value;
            }

            a.Values.Add(newVH);
            a.Dirty = true;
            DataTable dt = ((DataView)grdHistory.DataSource).Table;
            setReadOnly(_valueHistoryColumnDefinition, dt, false); // turn off read only
            row.Cells["HistoryID"].Value = newVH.ID;
            row.Cells["StartDate"].Value = newVH.StartDate;
            row.Cells["EndDate"].Value = newVH.EndDate;
            row.Cells["ValueType"].Value = getValidValueType(newVH.ValueType);
            row.Cells["Value"].Value = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, newVH.Value);
            grdHistory.NotifyCurrentCellDirty(true);
            Value = newVH.Value;
            DataGridViewRow oldRow = null;
            if (oldVH != null)
              oldRow = getHistoryRow(oldVH.ID);
            if (oldRow != null)
              oldRow.Cells["EndDate"].Value = newVH.StartDate.AddDays(-1);
            ValueHistoryID = newVH.ID;
            raiseChangeEvent(AttributeChangeEventArgs.ChangeType.Attribute);
          }

          grdHistory.Rows[grdHistory.Rows.Count - 1].Cells[1].Selected = true;  // set cursor to the current row

        }
      }

    }
    private DataGridViewRow getHistoryRow(string historyID)
    {
      if (grdHistory.Rows != null && grdHistory.Rows.Count > 0)
      {
        foreach (DataGridViewRow row in grdHistory.Rows)
          if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row.Cells["HistoryID"].Value)).Equals(historyID))
            return row;
      }
      return null;
    }
    private string getValidValueType(string dataValueType)
    {
      foreach (string vt in valueTypeList)
        if (vt.Equals(dataValueType, StringComparison.CurrentCultureIgnoreCase))
          return vt;
      return "Value";
    }
    private void setvalue()
    {
      Item item = (Item)EAC.getValue(string.Format("{0}.{1}.{2}", EntityID, ItemTypeID, ItemID));
      if (item != null && item.Attributes.Contains(AttributeID))
      {
        TAGAttribute a = item.Attributes[AttributeID];
        setAttributeValue(a, _value);
      }
    }
    private void setAttributeValue(TAGAttribute a, object value)
    {
      ValueHistory vhCurrent = null;
      foreach (ValueHistory vh in a.Values)
        if (vh.StartDate <= _effectiveDate && _effectiveDate <= vh.EndDate)
        {
          vhCurrent = vh;
          break;
        }
      if (vhCurrent != null)
      {
        vhCurrent.Value = value;
        a.Dirty = true;
        a.OverwriteValue = true;
        a.Value = value;
        a.OverwriteValue = false;
        displayValueHistoryGrid();
        grdHistory.Refresh();
        cmdSaveAttributes.Enabled = true;
      }
    }
    private object[] populateGridPicklist(PickList pickList)
    {
      object[] returnList = new object[pickList.Count];
      for (int i = 0; i < pickList.Count; i++)
        returnList[i] = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, pickList[i, 0]);
      return returnList;
    }
    private void checkEditMode()
    {
      if (_editMode)
      {
        txbValue2.Top = pnlCurrentEAC2.Top + pnlCurrentEAC2.Height + _adjustForValueLabel + 5;
        txbValue2.Height = this.Height - txbValue2.Top - _adjustForValueLabel;
        grdTableHeader.Top = txbValue2.Top;
        grdTableHeader.Height = txbValue2.Height;
        chbEqualOperator.Visible = _rawMode && grdTableHeader.Visible;
        lblValue2.Top = grdTableHeader.Top - _adjustForValueLabel;
        chbEqualOperator.Top = grdTableHeader.Top - _adjustForValueLabel; ;
        pnlCurrentEAC2.Visible = true;
        cmdSaveAttributes.Visible = true;
        cmdSaveAttributes.Enabled = false;
        grdTableHeader.ContextMenuStrip = rightClickMenuTableheaderGrid;
      }
      else
      {
        txbValue2.Top = pnlCurrentEAC2.Top + _adjustForValueLabel + 5;
        txbValue2.Height = this.Height - txbValue2.Top - _adjustForValueLabel;
        grdTableHeader.Top = txbValue2.Top;
        grdTableHeader.Height = txbValue2.Height;
        lblValue2.Top = txbValue2.Top - _adjustForValueLabel;
        chbEqualOperator.Top = grdTableHeader.Top - _adjustForValueLabel; ;
        pnlCurrentEAC2.Visible = false;
        cmdSaveAttributes.Visible = false;
        grdTableHeader.ContextMenuStrip = null;
      }
    }
    #endregion


  }
}
