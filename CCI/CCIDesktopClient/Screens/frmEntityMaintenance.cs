﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Screens
{
  public partial class frmEntityMaintenance : ScreenBase
  {
    #region private module data
    private bool _autoNumber = true;
    private bool _firstTime = true;
    protected bool _isLocked = false;
    protected string _entityTypeLabel = null;
    private ArrayList _states = new ArrayList();
    private ArrayList _entities = new ArrayList();
    private DataSource _dSource = null;
    private bool _changingStatus = false;
    protected bool _fillingScreen = false;
    private const string NOFIELD = "NOFIELD";
    private string[] contactDefaultFields = new string[] { "address1", "address2", "city", "state", "zip", "phone", "cellphone" };
    private string[] badEntityIDChars = new string[]  { " ", ",", "-", "'", ".", "/", "&" };
    #endregion

    #region protected module data
    protected bool _dirty = false;
    protected DateTime _effectiveDate = DateTime.Today;
    protected string _entityType = string.Empty;
    protected string _groupEntityType = string.Empty;
    protected string _entityOwner = "CCI"; // plug as default value for now
    protected string _entity = string.Empty;
    protected EntityAttributesCollection _eac = new EntityAttributesCollection();
    protected DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    protected bool _showAddress = true;
    protected bool _showContacts = true;
    protected bool _showGroups = true;
    #endregion

    #region public properties and data

    public const string colLASTMODIFIEDBY = "LastModifiedBy";
    public const string colLASTMODIFIEDDATETIME = "LastModifiedDateTime";
    public string EntityType 
    { 
      get { return _entityType; } 
      set 
      { 
        _entityType = value;
        if (string.IsNullOrEmpty(_groupEntityType))
          _groupEntityType = _entityType;
        lblEntity.Text = string.IsNullOrEmpty(_entityTypeLabel) ? _entityType : _entityTypeLabel + ":";
        ((SearchDataSourceEntity)ctlEntitySearch1.SearchExec).EntityType = _entityType;
        this.Text = string.Format("{0} Maintenance", string.IsNullOrEmpty(_entityTypeLabel) ? _entityType : _entityTypeLabel);
      } 
    }
    public string EntityOwner { get { return _entityOwner; } set { _entityOwner = value; } }
    public string Entity { get { return _entity; } set { _entity = value; } }
    public string NewEntityName { get; set; }
    public bool AutoNumber { get { return _autoNumber; } set { _autoNumber = value; } }
    protected ArrayList DisableExceptionFieldList = new ArrayList() { "ctlEntitySearch1", "btnNew" };
    public new SecurityContext CCISecurityContext { get { return (SecurityContext)SecurityContext; } set { SecurityContext = (SecurityContext)value; }  }
    #endregion

    public frmEntityMaintenance()
    {
      InitializeComponent();
      ctlEntitySearch1.SearchExec = new SearchDataSourceEntity(_entityType);
      NewEntityName = null;
      ctlLocations1.ColumnNames = new string[] { "Location", "Address1", "Address2", "City", "State", "Zip", "Phone", "Entity", "EntityOwner", "EntityType", "StartDate","EndDate" };
      ctlEntitySearch1.ClearSearchWhenComplete = false;
      txtEntity.SendToBack();
      txtLegalName.SendToBack();
      ctlEntitySearch1.BringToFront();

    }

    public void Init(string entity)
    {
      _fillingScreen = true;
      if (_firstTime)
      {
        _firstTime = false;
        if (string.IsNullOrEmpty(entity)) // but only if we are not already starting with an entity
          this.GetType().GetMethod("setScreenEnabled").Invoke(this, new object[] { false }); // lock screen until they find a record or press new
      }
      this.Text = string.Format("{0} Maintenance", string.IsNullOrEmpty(_entityTypeLabel) ? _entityType : _entityTypeLabel);
      ((SearchDataSourceEntity)ctlEntitySearch1.SearchExec).EntityType = _entityType;
      grpAddress.Visible = _showAddress;
      tabLocations.Visible = _showContacts;
      btnCreateLocation.Visible = _showContacts;
      if (!_showContacts)
      {
        tabMain.TabPages.Remove(tabLocations);
      }
      if (!string.IsNullOrEmpty(entity))
      {
        _entity = entity;
        txtEntity.Text = _entity;
        _eac = _dataSource.getEntity(_entity, _entityType, _effectiveDate);
          /*
        TAGAttribute a = _eac.Entities[_entity].ItemTypes["Entity"].Items["Customer"].Attributes["Dealer"];
        object val = a.Value;
        val = _eac.getValue(string.Format("{0}.Entity.Customer.Dealer", _entity));
           */
        loadStates();
        loadFields();

        if (_showContacts)
        {
          ctlLocations1.SecurityContext = SecurityContext;
          ctlLocations1.Init(_entity, _entityType);
        }


        txtAddress1.Focus();
        _changingStatus = true; // disable status changed event
        if (isActive())
          txtStatus.Text = "Actve";
        else
          txtStatus.Text = "Inactive";
        _changingStatus = false;
        if (tabMain.TabCount > 0)
          tabMain.SelectedIndex = 0;
        Refresh();
      }
      _fillingScreen = false;
    }
    public new void Save()
    {
      if (string.IsNullOrEmpty(_entity))
        txtEntity.Text = _entity = makeNewEntityID();   
      saveFields();
      setLastModifiedBy();
      _dataSource.saveEntity(_eac);
      ctlLocations1.Save();
      lblNewRecord.Visible = false;
      _dirty = false;
      MessageBox.Show("Record saved");
    }
    public void New()
    {
      NewWithName(null);
    }
    public void NewWithName(string entityName)
    {
      if (_isLocked)
        this.GetType().GetMethod("setScreenEnabled").Invoke(this, new object[] { true });
      clearAll();
      btnCancelNew.Visible = true;
      _eac.EffectiveDate = DateTime.Today;
      string newEntity;
      if (string.IsNullOrEmpty(entityName))
        txtLegalName.Text = Microsoft.VisualBasic.Interaction.InputBox("Enter New Entity Name");
      else
        txtLegalName.Text = entityName;
      newEntity = makeNewEntityID();
      txtEntity.Text = _entity = newEntity;
      Init(_entity);
      Entity en = new Entity();
      en.ID = _entity;
      en.Fields.AddField("Entity", newEntity);
      en.Fields.AddField("EntityType", _entityType);
      en.Fields.AddField("EntityOwner", _entityOwner);
      en.Fields.AddField(colLASTMODIFIEDBY, SecurityContext.User);
      en.Fields.AddField(colLASTMODIFIEDDATETIME, DateTime.Now);
      if (!string.IsNullOrEmpty(NewEntityName))
        en.Fields.AddField("LegalName", NewEntityName);
      _eac = new EntityAttributesCollection();
      _eac.Entities.Add(en);
      loadStates();
      loadFields();
      lblNewRecord.Visible = true;
      txtStatus.Text = "Active";
      ctlLocations1.SecurityContext = SecurityContext;
      ctlLocations1.Clear();
      ctlLocations1.EntityOwnerType = _entityType;
      ctlLocations1.EntityOwner = newEntity;
      txtLegalName.Focus();
      _dirty = true;
    }

    #region form events
    private void btnSave_Click(object sender, EventArgs e)
    {
      // use this syntax to make sure the method in the inherited form is the one we call
      object[] pList = new object[0];
      this.GetType().GetMethod("Save").Invoke(this, pList);
    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      // use this syntax to make sure the method in the inherited form is the one we call
      object[] pList = new object[0];
      this.GetType().GetMethod("New").Invoke(this, pList);
    }
    private void btnCancelNew_Click(object sender, EventArgs e)
    {
      // use this syntax to make sure the method in the inherited form is the one we call
      object[] pList = new object[0];
      if (lblNewRecord.Visible) // we are in new mode
        this.GetType().GetMethod("clearAll").Invoke(this, pList);
      else
      {
        pList = new object[] { ctlEntitySearch1.Text, ctlEntitySearch1.Description };
        this.GetType().GetMethod("selectEntity").Invoke(this, pList);
      }
      _dirty = false;
    }
    private void ctlEntitySearch1_OnSelected(object sender, EventArgs e)
    {
      selectEntity(ctlEntitySearch1.Text, ctlEntitySearch1.Description);
    }
    private void txtStatus_SelectedValueChanged(object sender, EventArgs e)
    {
      setStatus(txtStatus.Text);
    }
    private void txtLegalName_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
      {
        _dirty = true;
        NewEntityName = txtLegalName.Text;
      }
    }
    private void txtStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
    }
    private void txtAddress1_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
       _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void txtAddress2_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void txtCity_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void txtState_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void txtZip_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void txtPhone_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void txtCellPhone_TextChanged(object sender, EventArgs e)
    {
      if (!_fillingScreen)
        _dirty = true;
      setFieldValue((Control)sender, ((Control)sender).Text, false);
    }
    private void frmEntityMaintenance_Validating(object sender, CancelEventArgs e)
    {
      //checkDirtyOnLeave(e, true);
    }
    private void frmEntityMaintenance_Leave(object sender, EventArgs e)
    {
      checkDirtyOnLeave(e, false);
    }
    private void frmEntityMaintenance_Load(object sender, EventArgs e)
    {
      if (_entityType.Equals("Customer", StringComparison.CurrentCultureIgnoreCase))
      {
        lblStatus.Visible = false;
        txtStatus.Visible = false;
        ctlEntitySearch1.ShowTermedCheckBox = false;
      }
      else
      {
        lblStatus.Visible = true;
        txtStatus.Visible = true;
      }
    }

    #endregion
    #region private methods
    private void setLastModifiedBy()
    {
      if (_eac != null && _eac.Entities.Contains(_entity))
      {
        Entity e = _eac.Entities[_entity];
        if (e.Dirty)
        {
          if (e.Fields.Dirty)
          {
            e.Fields.setValue(colLASTMODIFIEDBY, SecurityContext.User);
            e.Fields.setValue(colLASTMODIFIEDDATETIME, DateTime.Today);
          }
          Item i = (Item)_eac.getValue(string.Format("{0}.{1}.{2}", _entity, "Entity", _entityType));
          if (i != null && e.Dirty)
          {
            i.setValue(colLASTMODIFIEDBY, SecurityContext.User,"string");
            i.setValue(colLASTMODIFIEDDATETIME, DateTime.Today,"datetime");
          }
        }
      }
    } 
    private void saveFields()
    {
      if (_eac == null)
        _eac = new EntityAttributesCollection();
      Entity en;
      if (!string.IsNullOrEmpty(_entity) && !_eac.Entities.Contains(_entity)) // this is a new one
      {
        // so create the entity object for it
        en = new Entity();
        en.ID = _entity;
        en.AddField("EntityType", _entityType);
        en.AddField("Entity", _entity);
        en.AddField("EntityOwner", _entityOwner);
        _eac.Entities.Add(en);
      }
      else
        if (_eac.Entities.Contains(_entity))
        {
          en = _eac.Entities[_entity];

          en.setValue("StartDate", txtStartDate.Value.ToString());
          en.setValue("EndDate", txtEndDate.Value.ToString());
        }
      txtLegalName.Text = txtLegalName.Text.Replace("'", "''"); // Fix legal name so it allows apostrophes
      saveFields(this);

    }
    private void saveFields(Control container)
    {
      foreach (Control c in container.Controls)
      {
        //if (!CommonFunctions.CString(c.Tag).Equals("Master", StringComparison.CurrentCultureIgnoreCase))
        Type t = c.GetType();
        if (t != typeof(ctlContacts)) // don't save the Locations (ctlContacts) because they are saved elsewhere
        {
          if (c.HasChildren && t != typeof(ACG.CommonForms.ctlSearch))
            saveFields(c);
          else
            setField(c);
        }
      }
    }
    private void setField(Control c)
    {
      if (_eac != null && _eac.Entities.Contains(_entity) && 
        (c.Name.StartsWith("txt") || c.Name.StartsWith("cbo")) &&
        (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(DateTimePicker) || c.GetType() == typeof(ACG.CommonForms.ctlSearch))
        && !CommonFunctions.CString(c.Tag).Equals("Master", StringComparison.CurrentCultureIgnoreCase))
      {
        string fldName = c.Name.Substring(3); //strip off the "txt" or "cbo" prefix
        Entity en = _eac.Entities[_entity];
        if (_dataSource.IsEntityField(fldName))
        {
          //TODO LMV66 This is a patch!!
          // LLA 2019 Fixed patch to work better if we want to save the entity owner
          // if the fields is entitytype or entityowner then it cannot be null, so just ignore
          if (string.IsNullOrEmpty(c.Text) && fldName.Equals("EntityType", StringComparison.CurrentCultureIgnoreCase))
            return;
          if (fldName.Equals("EntityOwner", StringComparison.CurrentCultureIgnoreCase))
          {
            if (c is ACG.CommonForms.ctlSearch)
            {
              ACG.CommonForms.ctlSearch srch = c as ACG.CommonForms.ctlSearch;
              string val = srch.Text;
              if (!string.IsNullOrWhiteSpace(val))
                en.Fields.setValue(fldName, val);
            }
            return;
          }
          // if we are here then we can set the value
          en.Fields.setValue(fldName, c.Text);
        }
        else
        {
          string val;
          if (c.GetType() == typeof(DateTimePicker))
            val = ((DateTimePicker)c).Value.ToShortDateString();
          else if (c.GetType() == typeof(ACG.CommonForms.ctlSearch))
            val = ((ACG.CommonForms.ctlSearch)c).Text;
          else
            val = c.Text;
          _eac.setValue(string.Format("{0}.Entity.{1}.{2}", _entity, _entityType, fldName), val);
          TAGAttribute a = _eac.Entities[_entity].ItemTypes["Entity"].Items[_entityType].Attributes[fldName];
          if (a.Dirty)
          {
            a.LastModifiedBy = SecurityContext.User;
            a.LastModifiedDateTime = DateTime.Now;
          }
        }
      }
    }
    private void loadFields()
    {
      loadFields(this);
    }
    private void loadFields(Control container)
    {
      foreach (Control c in container.Controls)
      {
        if (!c.Name.Equals("txtEntity"))
        {
          if (!CommonFunctions.CString(c.Tag).Equals("Master", StringComparison.CurrentCultureIgnoreCase))
          {
            if (c.HasChildren && c.GetType() != typeof(ACG.CommonForms.ctlSearch) && c.GetType() != typeof(ACG.CommonForms.ctlACGDate))
              loadFields(c);
            else
            {
              string val = getField(c);
              if (c.Name.StartsWith("txt") || c.Name.StartsWith("cbo"))
              {
                if (CommonFunctions.CString(val).Equals(NOFIELD))
                  setFieldValue(c, string.Empty, true);
                else
                {
                  if (val.StartsWith("@@"))
                    val = string.Empty;
                  //if (c.Name.Equals("txtStartDate", StringComparison.CurrentCultureIgnoreCase) || c.Name.Equals("txtEndDate", StringComparison.CurrentCultureIgnoreCase)) 
                  if (c is DateTimePicker)
                    if (val == null)
                      ((DateTimePicker)c).Checked = false;
                    else
                      ((DateTimePicker)c).Checked = true;
                  setFieldValue(c, val, true);
                }
              }
            }
          }
        }
      }
    }
    private void setFieldValue(Control c, string val, bool setTextValue)
    {
      if (setTextValue)
        if (c.GetType() == typeof(DateTimePicker))
          ((DateTimePicker)c).Value = CommonFunctions.CDateTime(val);
        else if (c.GetType() == typeof(ACG.CommonForms.ctlSearch))
          ((ACG.CommonForms.ctlSearch)c).Text = val;
        else
          c.Text = val;
      string fldName = c.Name.Substring(3); // strip off "txt"
      if (CommonFunctions.inList(contactDefaultFields, fldName))
      {
        if (ctlLocations1.DefaultValues.ContainsKey(fldName))
          ctlLocations1.DefaultValues[fldName] = val;
        else 
          ctlLocations1.DefaultValues.Add(fldName, val);
      }
    }
    private void clearFields()
    {
      CommonFormFunctions.clearFields(this);
    }

    private string getField(Control c)
    {
      if (!string.IsNullOrEmpty(_entity) && _eac != null && _eac.Entities.Contains(_entity) &&
        (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(ACG.CommonForms.ctlACGDate) || c.GetType() == typeof(ACG.CommonForms.ctlSearch))
        && !CommonFunctions.CString(c.Tag).Equals("Master", StringComparison.CurrentCultureIgnoreCase))
      {
        string fldName = c.Name.Substring(3); //strip off the "txt" or "cbo" prefix
        Entity en = _eac.Entities[_entity];
        if (en.Fields.Contains(fldName))
        {
          object val = en.Fields[fldName].Value;
          switch (fldName)
          {
            case "EndDate":
              if (((DateTime)val).Equals(new DateTime(2100,12,31)))
                return string.Empty;
              break;
            case "StartDate":
              if (((DateTime)val).Equals(new DateTime(1900, 1, 1)))
                return string.Empty;
              break;
          }
          return CommonFunctions.CString(val);
        }
        if (en.ItemTypes.Contains("Entity") && en.ItemTypes["Entity"].Items.Contains(_entityType))
        {
          Item record = en.ItemTypes["Entity"].Items[_entityType];
          if (record.Attributes.Contains(fldName))
            return CommonFunctions.CString(record.getValue(fldName));
          else
            return NOFIELD;
        }
        else
          return NOFIELD;
      }
      else
        return NOFIELD;
    }
    public void selectEntity(string entity, string fullName)
    {
     
      if (!string.IsNullOrEmpty(entity))
      {
        _fillingScreen = true;
        if (_isLocked)
          this.GetType().GetMethod("setScreenEnabled").Invoke(this, new object[] { true });
        _entity = entity;
        txtLegalName.Text = fullName;
        pnlLoading.Visible = true;
        Cursor saveCursor = this.Cursor;
        this.Cursor = Cursors.WaitCursor;
        this.Refresh();
        object[] pList = new object[] { entity };      
        this.GetType().GetMethod("Init").Invoke(this, pList); // this syntax is to make sure the inherited form method is the one we invoke
        //Init(_entity);
        pnlLoading.Visible = false;
        this.Cursor = saveCursor;
        txtEntity.Enabled = false; // can't edit the entity id once it is created.
        txtEntity.SendToBack();
        txtLegalName.SendToBack();
        ctlEntitySearch1.BringToFront();

        this.Refresh();
        _fillingScreen = false;
      }
    }
    private void loadStates()
    {
      if (_states.Count == 0)
      {
        if (CCISecurityContext.EntityUserInfo != null)
        {
          TableHeader thState = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, CCISecurityContext.EntityUserInfo.getValue("StateList"));
          if (thState == null)
          {
            EntityAttributesCollection dummyEAC = _dataSource.getAttributes("Dummy", "Entity", EntityType, null, DateTime.Today, true);
            thState = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, dummyEAC.getValue(string.Format("Dummy.Entity.{0}.StateList",EntityType)));
          }
          if (thState != null)
          {
            thState = thState.Sort("State");
            foreach (TableHeaderRow row in thState)
            {
              string state = CommonFunctions.CString(row["State"].Value);
              _states.Add(state);
              txtState.Items.Add(state);
            }
            if (_states != null)
            {
              ctlLocations1.StateList = (string[])_states.ToArray(typeof(string));
            }
          }
        }
      }
      
    }
    public void clearAll()
    {
      lblNewRecord.Visible = false;

      btnNew.Focus();
      //btnCancelNew.Visible = false;
      txtEntity.Text = string.Empty;
      _entity = string.Empty;
      txtLegalName.Text = string.Empty;
      txtEntity.Enabled = true;
      txtEntity.SendToBack();
      txtLegalName.SendToBack();
      ctlEntitySearch1.BringToFront();

      clearFields();
      ctlLocations1.Clear();
      _eac = new EntityAttributesCollection();
    }
    protected void setStatus(string status)
    {
      if (!_changingStatus && _eac != null && _eac.Entities.Contains(_entity))
      {
        Entity e = _eac.Entities[_entity];
        DateTime? oStartDate = (DateTime?)e.Fields.getValue("StartDate");
        DateTime? oEndDate = (DateTime?)e.Fields.getValue("EndDate");
        DateTime startDate = CommonData.PastDateTime;
        DateTime endDate = CommonData.FutureDateTime;
        if (oStartDate != null)
          startDate = (DateTime)oStartDate;
        if (oEndDate != null)
          endDate = (DateTime)oEndDate;
        bool active = DateTime.Today >= startDate && DateTime.Today <= endDate;

        switch (status.ToLower())
        {
          case "active":
            if (!active)  // only do something if status is changing
            {
              e.Fields.setValue("StartDate", DateTime.Today);
              if (endDate <= DateTime.Today)
                e.setValue("EndDate", CommonData.FutureDateTime);
            }
            break;
          case "inactive":
            if (active)  // only do something if status is changing
              e.setValue("EndDate", DateTime.Today.AddDays(-1)); // so set end date to yesterday
            break;
        }
      }
    }
    private bool isActive()
    {
      if (_eac == null || !_eac.Entities.Contains(_entity))
        return false;
      Entity e = _eac.Entities[_entity];
      DateTime? oStartDate = (DateTime?)e.Fields.getValue("StartDate");
      DateTime? oEndDate = (DateTime?)e.Fields.getValue("EndDate");
      DateTime startDate = CommonData.PastDateTime;
      DateTime endDate = CommonData.FutureDateTime;
      if (oStartDate != null)
        startDate = (DateTime)oStartDate;
      if (oEndDate != null)
        endDate = (DateTime)oEndDate;
      return DateTime.Today >= startDate && DateTime.Today <= endDate;
    }
    private void checkDirtyOnLeave(EventArgs e, bool canCancel)
    {
      if (_dirty)
      {
        MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
        string msgend = string.Empty;
        if (!canCancel)
        {
          buttons = MessageBoxButtons.YesNo;
          msgend = ", and Cancel to return to the screen.";
        }
        // for some wierd reason I could not get the MessageBoxResult type to instantiate, so I convert to int
        int ans = (int)MessageBox.Show("You have unsaved changes. Do you wish to Save? Press Yes to Save, No to not save" + msgend, "Unsaved Data", buttons);
        switch (ans)
        {
          case 6: // yes
            Save();
            break;
          case 7: // no
            break;
          case 2: // cancel
            if (canCancel)
            {
              ((CancelEventArgs)e).Cancel = true;
              this.Focus();
            }
            break;
        }
      }
    }
    public void setScreenEnabled(bool enabled)
    {
      CommonFormFunctions.setScreenEnabled(this, enabled,  (string[])DisableExceptionFieldList.ToArray(typeof(string) ));
      txtEntity.SendToBack();
      txtLegalName.SendToBack();
      ctlEntitySearch1.BringToFront();
      _isLocked = !enabled;
    }
    private string makeNewEntityID()
    {
      if (_autoNumber)
        return _dataSource.getNextNumericEntityID(_entityType);
      return _dataSource.MakeNewEntityID(NewEntityName);
      //string entityID = CommonFunctions.CString(NewEntityName).Trim().ToUpper();
      //entityID = entityID.Substring(0, Math.Min(22, entityID.Length));
      //foreach (string c in badEntityIDChars)
      //  entityID = entityID.Replace(c, "");
      //int seq = 0;
      //while (_dataSource.ExistsEntity(entityID))
      //  entityID = string.Format("{0}{1}", entityID, (seq++).ToString());
      //return entityID;
    }
    #endregion

    private void ctlLocations1_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      string location = CommonFunctions.CString(e.SelectedRow.Cells["Entity"].Value);
    }

    private void btnCreateLocation_Click(object sender, EventArgs e)
    {
      Dictionary<string, object> fields = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
      fields.Add("Address1", txtAddress1.Text);
      fields.Add("Address2", txtAddress2.Text);
      fields.Add("City", txtCity.Text);
      fields.Add("State", txtState.Text);
      fields.Add("Zip", txtZip.Text);
      fields.Add("Phone", txtPhone.Text);
      fields.Add("CellPhone", txtCellPhone.Text);
      fields.Add("Location", string.Format("{0} {1}-{2}", txtCity.Text, txtState.Text, txtAddress1.Text));
      ctlLocations1.CreateNewFromParent(fields);
    }
  }
}
