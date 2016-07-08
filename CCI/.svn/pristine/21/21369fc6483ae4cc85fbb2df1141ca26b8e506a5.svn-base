using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Common
{
  public partial class ctlEntityEdit : UserControl
  {
    #region private method data
    private const string colCONTACTTYPE = "Relationship";
    private const string colISPRIMARY = "IsPrimary";
    private const string colSTATE = "State";
    private const string colEntityOwner = "EntityOwner";
    private bool _dirty = false;
    private DataSource _dSource = null;
    private string _entityType = "Contact";
    private string _entity = null;
    private const string textPrefix = "txt";
    private const string labelPrefix = "lbl";
    private const string buttonPrefix = "btn";
    private string[] _buttons = new string[] { "Save", "Cancel", "New", "Delete" };
    private int _labelWidth = 75;
    private int _textWidth = 200;
    private int _buttonWidth = 50;
    private DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    private DateTime _effectiveDate = DateTime.Today;
    protected string[] _states = null;
    private string[] _contactTypes = null;
    private string[] _columnNames = new string[] { "LegalName", "Phone", "Address1", "Address2", "City", "State", "Zip", "Entity", "EntityOwner", "EntityType", "Relationship" };
    #endregion

    #region public properties
    public string EntityOwnerType { get; set; }
    public string EntityOwner { get; set; }
    public ACG.Common.ISecurityContext SecurityContext { get; set; }
    public string EntityType { get { return _entityType; } set { _entityType = value; } }
    public string[] ColumnNames { get { return _columnNames; } set { _columnNames = value; } }
    #endregion

    public ctlEntityEdit()
    {
      InitializeComponent();
    }

    #region public methods

    public void Init(DataGridViewRow row)
    {
      initEditPane();
      loadEditPane(row);
    }
    public void Init(DataSet ds)
    {
      initEditPane();
      loadEditPane(ds);
    }
    public void Init(string entity)
    {
      initEditPane();
      using (DataSet ds = _dataSource.getEntityRaw(entity))
      {
        loadEditPane(ds);
      }
    }
    public void Save()
    {
      savePane();
    }
    public void Delete()
    {
      deleteFromPane();
    }
    public void Clear()
    {
      clearEditPane();
    }
    #endregion

    #region private methods
    private void loadEditPane(string entity)
    {
      using (DataSet ds = _dataSource.getEntityRaw(entity))
      {
        loadEditPane(ds);
      }
    }
    private void loadEditPane(DataSet ds)
    {
      if (string.IsNullOrEmpty(EntityOwner))
      {
        MessageBox.Show("Cannot load fields because EntityOwner is not set");
        return;
      }
      if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
      {
        ControlCollection pnl = flowEntityMaintenance.Controls;
        DataColumnCollection cols = ds.Tables[0].Columns;
        DataRow row = ds.Tables[0].Rows[0];
        foreach (string col in _columnNames)
        {
          Control txt = pnl[textPrefix + col];
          if (cols.Contains(col))
            setFieldValue(txt, row[col]);
          else if (col.Equals(colEntityOwner))
            setFieldValue(txt, EntityOwner);
          else if (col.Equals(colCONTACTTYPE))
            setFieldValue(txt, _dataSource.getGroupMemberRelationship(_entity, EntityOwner));
        }
      }
    }
    private void loadEditPane(DataGridViewRow row)
    {
      if (string.IsNullOrEmpty(EntityOwner))
      {
        MessageBox.Show("Cannot load fields because EntityOwner is not set");
        return;
      }
      ControlCollection pnl = flowEntityMaintenance.Controls;
      foreach (string col in _columnNames)
      {
        /*
         * one of the annoying things about DataGridViewRow is you can't check to see if a cell name exists without having access to grid.Columns. 
         * So we use try...catch to assign the cell, and see if it works
         */
        DataGridViewCell cell;
        try
        {
          cell = row.Cells[col];
        }
        catch
        {
          cell = null;
        }
        Control txt = pnl[textPrefix + col];
        if (cell != null)
          setFieldValue(txt, cell.Value);
        else if (col.Equals(colEntityOwner))
          setFieldValue(txt, EntityOwner);
        else if (col.Equals(colCONTACTTYPE))
          setFieldValue(txt, _dataSource.getGroupMemberRelationship(_entity, EntityOwner));
      }
    }
    private void setFieldValue(Control txt, object value)
    {
      if (txt.GetType() == typeof(CheckBox))
        ((CheckBox)txt).Checked = CommonFunctions.CBoolean(value);
      else
        txt.Text = CommonFunctions.CString(value);
      // sopmetimes the entityowner column may not be populated, in which case we put the valid value in
      if (txt.Name.Equals(textPrefix + colEntityOwner, StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrEmpty(txt.Text))
        txt.Text = EntityOwner;
    }
    private void clearEditPane()
    {
      ControlCollection pnl = flowEntityMaintenance.Controls;
      if (_columnNames != null && _columnNames.GetLength(0) > 0)
      {
        foreach (string col in _columnNames)
        {
          Control txt = pnl[textPrefix + col];
          txt.Text = string.Empty;
        }
      }
    }
    private void initEditPane()
    {
      // make sure we have contacttype
      if (!CommonFunctions.inList(_columnNames, colCONTACTTYPE))
      {
        string[] newList = new string[_columnNames.GetLength(0)+1];
        _columnNames.CopyTo(newList, 0);
        newList[_columnNames.GetLength(0)] = colCONTACTTYPE;
      }
      ControlCollection pnl = flowEntityMaintenance.Controls;
      if (pnl.Count != _columnNames.GetLength(0))
      {
        pnl.Clear();
        _textWidth = getTextWidth();
        foreach (string col in _columnNames)
        {
          Label lbl = new Label();
          lbl.Name = labelPrefix + col;
          lbl.Text = col;
          lbl.Width = _labelWidth;
          pnl.Add(lbl);
          Control txt;
          switch (col) // populate combos
          {
            case colCONTACTTYPE:
              txt = new ComboBox();
              ((ComboBox)txt).Items.AddRange(_contactTypes);
              break;
            case colSTATE:
              txt = new ComboBox();
              if (_states != null)
                ((ComboBox)txt).Items.AddRange(_states);
              break;
            case colISPRIMARY:
              txt = new CheckBox();
              break;
            default:
              txt = new TextBox();
              break;
          }
          txt.Name = textPrefix + col;
          txt.Width = _textWidth;
          pnl.Add(txt);
        }
        foreach (string b in _buttons)
        {
          Button btn = new Button();
          btn.Name = buttonPrefix + b;
          btn.Text = b;
          btn.Click += new EventHandler(btn_Click);
          this.Controls.Add(btn);
        }
      }
      else
        resizeFlowPanel();
    }
    private void resizeFlowPanel()
    {
      int width = flowEntityMaintenance.Width;
      _textWidth = getTextWidth();
      if (flowEntityMaintenance.Controls.Count > 0)
        foreach (Control c in flowEntityMaintenance.Controls)
          if (c.Name.StartsWith(textPrefix))
            c.Width = _textWidth;
    }
    private int getTextWidth()
    {
      return flowEntityMaintenance.Width - _labelWidth - flowEntityMaintenance.Margin.Left * 3 - 5;
    }
    private void savePane()
    {
      string entityColumnName = textPrefix + "Entity";
      // only save if there is an entity id and if it is not blank or null
      if (flowEntityMaintenance.Controls.ContainsKey(entityColumnName) && !string.IsNullOrEmpty(flowEntityMaintenance.Controls[entityColumnName].Text))
      {
        EntityAttributesCollection eac = new EntityAttributesCollection();
        eac.EffectiveDate = DateTime.Today;
        Entity e = new Entity();
        e.Fields.AddField("LastModifiedBy", SecurityContext.User);
        ItemType it = new ItemType();
        it.ID = "Entity";
        Item i = new Item();
        i.ID = _entityType;
        i.LastModifiedBy = SecurityContext.User;
        bool isPrimary = false;
        foreach (Control c in flowEntityMaintenance.Controls)
        {
          if (c.Name.StartsWith(textPrefix)) // only txt fields have data
          {
            object val;
            string name = c.Name.Substring(textPrefix.Length); // get column name by stripping off prefix
            if (c.GetType() == typeof(CheckBox))
              val = ((CheckBox)c).Checked;
            else
              val = CommonFunctions.CString(c.Text);
            if (name.Equals(colISPRIMARY))
              isPrimary = CommonFunctions.CBoolean(val); // just set this value, don't create a field or attribute cause this field is virtual
            else if (_dataSource.IsEntityField(name))
            {
              e.Fields.AddField(name, val);
              if (name.Equals("Entity"))
                e.ID = CommonFunctions.CString(val);
            }
            else
            {
              i.Attributes.AddAttribute(name, val);
              i.Attributes[name].LastModifiedBy = SecurityContext.User;
            }
          }
        }
        it.Items.Add(i);
        e.ItemTypes.Add(it);
        eac.Entities.Add(e);
        _dataSource.saveEntity(eac);
        _dataSource.memberSetPrimary(EntityOwner, e.ID, isPrimary, SecurityContext.User);
      }
    }
    private void deleteFromPane()
    {
      string entityColumnName = textPrefix + "Entity";
      // only delete if there is an entity id and if it is not blank or null
      if (flowEntityMaintenance.Controls.ContainsKey(entityColumnName))
      {
        Control c = flowEntityMaintenance.Controls[entityColumnName];
        if (!string.IsNullOrEmpty(c.Text))
        {
          string entity = c.Text;
          _dataSource.terminateEntity(entity);
          int? id = _dataSource.getMember(EntityOwner, entity);
          if (id != null && id > 0)
            _dataSource.updateGroupMember(id.ToString(), EntityOwner, entity, null, DateTime.Today.AddDays(-1), null, SecurityContext.User);
        }
      }
    }
    private string getID(string member)
    {
      if (string.IsNullOrEmpty(member))
        return string.Empty;
      int pos = member.IndexOf(":");
      if (pos < 0)
        return member;
      return member.Substring(0, pos).Trim();
    }
    #endregion

    #region custom events
    public delegate void DataChangedHandler(object sender, EventArgs e);
    public event DataChangedHandler DataChanged;
    protected void OnDataChanged(EventArgs e)
    {
      if (DataChanged != null)
      {
        DataChanged(this, e);
      }
    }
    private void raiseDataChanged()
    {
      OnDataChanged(new EventArgs());
    }
    #endregion

    #region form events
    private void btn_Click(object sender, EventArgs e)
    {
      string name = ((Button)sender).Name.Substring(3);
      switch (name)
      {
        case "Save":
          savePane();
          break;
        case "Cancel":
          //selectPane();
          break;
        case "New":
          clearEditPane();
          break;
        case "Delete":
          break;
      }
    }
    private void flowEntityMaintenance_Enter(object sender, EventArgs e)
    {
      if (_contactTypes == null)
        _contactTypes = _dataSource.getCodeList(colCONTACTTYPE);
      if (_states == null)
        _states = _dataSource.getCodeList(colSTATE);
    }

    #endregion

  }
}
