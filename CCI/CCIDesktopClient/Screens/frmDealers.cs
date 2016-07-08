﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.DesktopClient.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class frmDealers : frmEntityMaintenance
  {
    private string _pricingLevel = string.Empty;
    //private DataSource _ds = null;
    //private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    enum MoveDirection { ToDealer, FromDealer }
    private string[] _pricinglevels;
    public frmDealers()
    {
      InitializeComponent();
      AutoNumber = false;
      _entityTypeLabel = "Dealer";
      EntityType = "Dealer";
      EntityOwner = "CCI";
      usocUSOC.SearchExec = new SearchDataSourceProductList();
      txtMasterDealer.SearchExec = new SearchDataSourceMasterDealer();
   
      ((SearchDataSourceProductList)usocUSOC.SearchExec).Carrier = "CityHosted"; // retail usocs
      dtEndDate.Value = CommonData.FutureDateTime;
      newCost();
      //_showGroups = false;
    }

    public new void Init(string entity)
    {
      _pricinglevels = _dataSource.getDealerPricingLevels();
      populateCombos();
      foreach (string level in _pricinglevels)
        if (!usocUSOC.AdditionalValues.ContainsKey(level))
          usocUSOC.AdditionalValues.Add(level, level + " Pricing Level");
      _pricingLevel = _dataSource.getDealerLevel(entity, DateTime.Today, "Bronze");
      if (!SecurityContext.Security.HasObjectAccess("CanDeleteDealerPricing"))
        btnDelete.Visible = false;
      base.Init(entity);
      if (string.IsNullOrEmpty(txtMasterDealer.Text))
        txtMasterDealer.Text = "None";
      //tabMain.TabPages["tabGroups"].Hide();
      if (!string.IsNullOrEmpty(entity))
      {
        loadDealerCost();
        loadDealerCustomers();
      }
      else
        grdDealerCost.Rows.Clear();
      CommonFormFunctions.fillComboBoxList(cboChannelManagerAssignedTo, _dataSource.getSalesPersonList());
    }
    public new void New()
    {
      base.New();
      if (grdDealerCost.DataSource != null)
        ((DataView)grdDealerCost.DataSource).Table.Rows.Clear();
      txtDealerAgreementEndDate.Value = DateTime.Today;
      txtDealerAgreementEndDate.Value = CommonData.FutureDateTime;
      txtMasterDealer.Text = "None";
      //grdDealerCost.Rows.Clear();
    }
    public new void Save()
    {
      save(true);
    }
    private void save(bool checkPricing)
    {
      if (!checkPricing || alreadyHasPricingLevel("Gold", CommonData.PastDateTime, CommonData.FutureDateTime))
      {
        base.Save();
        loadDealerCost();
        loadDealerCustomers();
      }
      else
        MessageBox.Show("You cannot save a dealer unless it has a pricing level");
    }

    #region private methods
    private void populateCombos()
    {
    }
    private void saveCost()
    {
      string usoc = usocUSOC.Text;
      if (string.IsNullOrEmpty(usoc))
      {
        MessageBox.Show("There is no USOC or Pricing Level to save");
        return;
      }
      if (string.IsNullOrEmpty(txtEntity.Text)) // This is a new dealer and it has not yet been saved
      {
        // so we save it, which will generate an entityid. Then we can save the pricing
        save(false);
      }
      if (CommonFunctions.inList(_pricinglevels, usoc)) // this is a pricing level, so let's check the dates
      {
//        if (DateTime.Today < dtStartDate.Value || DateTime.Today > dtEndDate.Value) // dealer level is expired
        if (DateTime.Today > dtEndDate.Value) //dealer level is expired
        {
          if (!SecurityContext.Security.HasObjectAccess("CCIAdmin"))
          {
            MessageBox.Show("You are not allowed to make a dealer pricing level inactive");
            return;
          }
          DialogResult ans = MessageBox.Show(@"You are making a pricing level expired or inactive. 
This can result in a bad dealer margin report. 
Are you sure you want to proceed?", "Warning: Dealer Pricing Level", MessageBoxButtons.YesNo);
          if (ans == System.Windows.Forms.DialogResult.No)
            return;
        }
        // now lets check to see if this is master dealer pricing. If so, only John C can save the record.
        if (usoc.StartsWith("Master", StringComparison.CurrentCultureIgnoreCase)) // this is a master pricing level
          if (!SecurityContext.Security.HasObjectAccess("CCIAdmin"))
          {
            MessageBox.Show("You are not allowed to edit master dealer pricing levels");
            return;
          }
      }
      // if they have a level, and it is not the same as the one I am saving, then we can't have two
      if (alreadyHasPricingLevel(usoc, dtStartDate.Value, dtEndDate.Value))
      {
        if (!SecurityContext.Security.HasObjectAccess("CCIAdmin"))
        {
          MessageBox.Show("You are giving this dealer a pricing level when they already have one. This is not allowed");
          return;
        }
//        DialogResult ans = MessageBox.Show(@"You are giving this dealer a pricing level when they already have one. 
//This can result in a bad dealer margin report. 
//Are you sure you want to proceed?", "Warning: Dealer Pricing Level", MessageBoxButtons.YesNo);
//        if (ans == System.Windows.Forms.DialogResult.No)
//          return; //return;
      }
      // if they have this usoc pricing active, then I can't add a new one
      if (hasOverlappingUSOC(usoc, dtStartDate.Value, dtEndDate.Value))
      {
        if (!SecurityContext.Security.HasObjectAccess("CCIAdmin"))
        {
          MessageBox.Show("You are not allowed to make a second pricing for this dealer");
          return;
        }
        MessageBox.Show("You are not allowed to have overlapping pricing levels");
        return;
//        DialogResult ans = MessageBox.Show(@"This dealer already has this usoc.  
//This can result in a bad dealer margin report. 
//Are you sure you want to proceed?", "Warning: Dealer Pricing Level", MessageBoxButtons.YesNo);
//        if (ans == System.Windows.Forms.DialogResult.No)
//          return; return;
      }
      _dataSource.updateDealerCost(Entity, usocUSOC.Text, usocCost.Text, usocInstall.Text, _pricingLevel, dtStartDate.Value, dtEndDate.Value, SecurityContext.User);
      loadDealerCost();
      newCost();
      MessageBox.Show("Dealer Pricing Record Saved");
    }
    private bool alreadyHasPricingLevel(string usoc, DateTime startDate, DateTime endDate)
    {
      return CommonFunctions.inList(_pricinglevels, usoc)
        && _dataSource.hasDealerLevel(Entity, usoc, startDate, endDate);
    }
    private bool hasOverlappingUSOC(string usoc, DateTime startDate, DateTime endDate)
    {
      return lblUSOCNew.Visible && _dataSource.hasOverlappingDealerUsocPrice(Entity, usoc, startDate, endDate);
    }
    private void deleteCost()
    {
      if (CommonFunctions.inList(_pricinglevels, usocUSOC.Text))
      {
        DialogResult ans = MessageBox.Show("You are deleting a pricing level. This is normally not allowed. Are you sure you want to proceed?", "Delete Dealer Pricing Level", MessageBoxButtons.YesNo);
        if (ans == System.Windows.Forms.DialogResult.No)
          return;
      }
      _dataSource.deleteDealerCost(Entity, usocUSOC.Text, dtStartDate.Value);
      loadDealerCost();
      newCost();
    }
    private void newCost()
    {
      usocCost.Text = string.Empty;
      usocInstall.Text = string.Empty;
      usocUSOC.Text = string.Empty;
      lblUSOCNew.Visible = true;
      dtStartDate.Value = DateTime.Today;
      dtEndDate.Value = CommonData.FutureDateTime;
      dtStartDate.Enabled = true;
    }
    private void loadDealerCost()
    {
      string dealer = Entity;
      DataSet ds = _dataSource.getDealerPriceList(dealer);
      ACG.CommonForms.CommonFormFunctions.displayDataSetGrid(grdDealerCost, ds);
      foreach (DataGridViewColumn col in grdDealerCost.Columns) // make columns readonly
        col.ReadOnly = true;
      grdDealerCost.Columns["Dealer"].Visible = false;
      dtStartDate.Enabled = false;
    }
    private void loadDealerCustomers()
    {
      lstAllCustomers.Items.Clear();
      lstDealerCustomers.Items.Clear();
      ArrayList results = _dataSource.getDealerCustomerList(Entity);
      if (results.Count > 1)
      {
        ArrayList notDealerList = (ArrayList)results[0];
        ArrayList dealerList = (ArrayList)results[1];
        lstAllCustomers.Items.AddRange(notDealerList.ToArray());
        lstDealerCustomers.Items.AddRange(dealerList.ToArray());
      }
    }
    private void moveCustomer(MoveDirection direction, bool justOne)
    {
      ArrayList customerList = new ArrayList();
      string dealer = string.Empty;
      ListBox fromList;
      ListBox toList;
      switch (direction)
      {
        case MoveDirection.FromDealer:
          dealer = "CCIDealer";
          fromList = lstDealerCustomers;
          toList = lstAllCustomers;
          break;
        default:
          dealer = Entity;
          fromList = lstAllCustomers;
          toList = lstDealerCustomers;
          break;
      }
      if (fromList.SelectedItems != null && fromList.SelectedItems.Count > 0)
      {
        if (justOne)
        {
          string cust = CommonFunctions.CString(fromList.SelectedItems[0]);
          customerList.Add(getCustomerID(cust));
          fromList.Items.Remove(cust);
          insertItem(toList, cust);
          //toList.Items.Add(cust);
        }
        else
        {
          ArrayList longlist = new ArrayList();
          foreach (string cust in fromList.SelectedItems)
          {
            customerList.Add(getCustomerID(cust));
            longlist.Add(cust);
          }
          foreach (string cust in longlist)
          {
            fromList.Items.Remove(cust);
            insertItem(toList, cust);
            //toList.Items.Add(cust);
          }
        }
        _dataSource.moveDealerCustomers(customerList, dealer, SecurityContext.User);
      }
      else
        MessageBox.Show("No items were selected");
    }
    private string getCustomerID(string customer)
    {
      if (string.IsNullOrEmpty(customer))
        return string.Empty;
      if (customer.IndexOf(":") > 0)
        return customer.Substring(0, customer.IndexOf(":"));
      else
        return string.Empty;
    }
    private void loadUSOC()
    {      
      if (grdDealerCost.SelectedRows != null && grdDealerCost.SelectedRows.Count > 0)
      {
        DataGridViewRow row = grdDealerCost.SelectedRows[0];
        string selectedUSOC = CommonFunctions.CString(row.Cells["USOC"].Value);
        usocUSOC.Text = selectedUSOC;
        usocCost.Text = CommonFunctions.CString(row.Cells["dealercost"].Value);
        usocInstall.Text = CommonFunctions.CString(row.Cells["install"].Value);
        dtStartDate.Value = CommonFunctions.CDateTime(row.Cells["StartDate"].Value, CommonData.PastDateTime);
        dtEndDate.Value = CommonFunctions.CDateTime(row.Cells["EndDate"].Value, CommonData.FutureDateTime);
        lblUSOCNew.Visible = false;
      }
    }
    private new void clearAll()
    {
      base.clearAll();
      newCost();
      lstAllCustomers.Items.Clear();
      lstDealerCustomers.Items.Clear();
    }
    private string getDesc(string member)
    {
      int iPos = member.IndexOf(':');
      if (iPos > 0 && iPos < member.Length - 2)
        return member.Substring(iPos + 1).Trim();
      else
        return member;
    }
    private new void selectEntity(string entity, string fullName)
    {
      base.selectEntity(entity, fullName);
      newCost();
    }
    private void insertItem(ListBox lst, string item)
    {
      string desc = getDesc(item);
      int iPos = 0;
      foreach (string lstItem in lst.Items)
      {
        // look through list until we find the one AFTER the alpha order
        if (desc.CompareTo(getDesc(lstItem)) <= 0)
          break;
        else
          iPos++;
      }
      lst.Items.Insert(iPos, item);
    }
    private bool hasDealerLevel()
    {
      //checks to make sure this dealer has a level
      Dictionary<string,object> level = _dataSource.getDealerLevel(Entity, DateTime.Now);
      if (level.Count == 0 || !level.ContainsKey("Level") || string.IsNullOrEmpty(CommonFunctions.CString(level["Level"])))
        return false;
      return alreadyHasPricingLevel(CommonFunctions.CString(level["ItemID"]), 
        CommonFunctions.CDateTime(level["StartDate"], CommonData.PastDateTime), 
        CommonFunctions.CDateTime(level["EndDate"], CommonData.FutureDateTime));
    }
    #endregion

    #region form events
    private void btnDelete_Click(object sender, EventArgs e)
    {
      deleteCost();
    }
    private void usocUSOC_SelectedIndexChanged(object sender, EventArgs e)
    {
//      if (alreadyHasPricingLevel(usocUSOC.ID, dtStartDate.Value, dtEndDate.Value)) // do we have two of them?
//      {
//        MessageBox.Show("This dealer already has a pricing level. You cannot have two.");
//        return;
//      }
      CCITable usocInfo = _dataSource.getDealerCostInfo(_pricingLevel, usocUSOC.Text, DateTime.Today);
      // get default values
      if (usocInfo != null && usocInfo.NumberRows > 0)
      {
        usocCost.Text = CommonFunctions.CString(usocInfo[0, "DealerCost"]);
        usocInstall.Text = CommonFunctions.CString(usocInfo[0, "Install"]);
      }

    }
    private void btnUSOCNew_Click(object sender, EventArgs e)
    {
      newCost();
    }
    private void btnUSOCSave_Click(object sender, EventArgs e)
    {
      saveCost();
    }    
    private void btnAddOneCustomer_Click(object sender, EventArgs e)
    {
      moveCustomer(MoveDirection.ToDealer, false);
    }
    private void btnSubtractOneCustomer_Click(object sender, EventArgs e)
    {
      moveCustomer(MoveDirection.FromDealer, false);
    }
    private void grdDealerCost_RowSelected(object sender, EventArgs e)
    {
      loadUSOC();
    }
    private void btnTerminate_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show("This will terminate the current entry as of yesterday, and create a new entry ready for you to save with a start date of today. Is this what you want to do?",
        "Terminate Pricing Level", MessageBoxButtons.YesNo);
      if (ans == System.Windows.Forms.DialogResult.Yes)
      {
        dtEndDate.Value = DateTime.Today.AddDays(-1);
        string usoc = usocUSOC.Text;
        string cost = usocCost.Text;
        string install = usocInstall.Text;
        saveCost();
        dtStartDate.Value = DateTime.Today;
        usocUSOC.Text = usoc;
        usocCost.Text = cost;
        usocInstall.Text = install;
        dtEndDate.Value = CommonData.FutureDateTime;
        lblUSOCNew.Visible = true;
      }
    }
    private void btnCancel_Click(object sender, EventArgs e)
    {
      loadUSOC();
    }
    #endregion


  }
}
