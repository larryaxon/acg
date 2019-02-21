using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.DesktopClient.Common;
using CCI.Common;
using CCI.Sys.Data;

using ChargifyNET;
using ACG.CommonForms;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCustomers : frmEntityMaintenance
  {
    private const string custTabName = "tabSaddlebackCustomer";
    private const string networkInventoryTabName = "tabNetworkInventory";
    private const string networkInventoryGridname = "srchNetworkInventory";
    public bool RefreshAll = false;
    private bool _canAccessChargify { get { return SecurityContext.Security.HasObjectAccess("CanAccessChargify"); } }

    public frmCustomers()
    {
      InitializeComponent();
      EntityType = "Customer";
      EntityOwner = "CCI";
      // don't disable these when the screen first comes up
      DisableExceptionFieldList.Add("btnRefreshFromCitycare");
      DisableExceptionFieldList.Add("btnMerge");
      txtEntity.SendToBack();
      txtLegalName.SendToBack();
      txtEntityOwner.SendToBack();
      txtEntityOwner.SearchExec = new SearchDataSourceEntity("MasterCustomer");
    }
    public new void Init(string entity)
    {
      base.Init(entity);

      // then add the CHS customer tab
      TabPage cust;
      if (!tabMain.TabPages.ContainsKey(custTabName)) // only add this the first time through
      {
        tabMain.TabPages.Add(custTabName, "Customer Info");
        cust = tabMain.TabPages[custTabName];

        Label labelAltID = new Label();
        labelAltID.Name = "lblAlternateID";
        labelAltID.Text = "Alternate Customer ID";

        Label labelPaymentType = new Label();
        labelPaymentType.Name = "lblPaymentType";
        labelPaymentType.Text = "Payment Type";
        TextBox textAltID = new TextBox();
        textAltID.Name = "txtAlternateID";
        textAltID.Validating += new CancelEventHandler(textAltID_Validating);

        ComboBox comboPaymentType = new ComboBox();
        comboPaymentType.Name = "txtPaymentType";
        comboPaymentType.Items.AddRange(new string[] { "ACH", "Check", "CreditCard" });


        //Label labelMasterCustomer = new Label();
        //labelMasterCustomer.Name = "lblMasterCustomer";
        //labelMasterCustomer.Text = "Master Customer";
        //ctlSearch txtEntityOwner = new ctlSearch();
        //txtEntityOwner.Name = "txtEntityOwner";
        //txtEntityOwner.ShowTermedCheckBox = false;
        //txtEntityOwner.ShowCustomerNameWhenSet = true;
        //txtEntityOwner.AutoAddNewMode = false;
        //txtEntityOwner.AutoSelectWhenMatch = false;
        //txtEntityOwner.AutoTabToNextControlOnSelect = false;
        //txtEntityOwner.ClearSearchWhenComplete = false;
        //txtEntityOwner.DisplayOnlyID = false;
        //txtEntityOwner.MustExistInList = true;

        Label labelAgent = new Label();
        labelAgent.Name = "lblAgent";
        labelAgent.Text = "Agent";
        ctlSearch  srchAgent = new ctlSearch();
        srchAgent.Name = "txtAgent";
        srchAgent.SearchExec = new SearchDataSourceEntity("Agent");
        srchAgent.ShowTermedCheckBox = false;
        srchAgent.ShowCustomerNameWhenSet = true;
        srchAgent.AutoAddNewMode = false;
        srchAgent.AutoSelectWhenMatch = false;
        srchAgent.AutoTabToNextControlOnSelect = false;
        srchAgent.ClearSearchWhenComplete = false;
        srchAgent.DisplayOnlyID = false;
        srchAgent.MustExistInList = true;

        //cust.Controls.Add(labelMasterCustomer);
        //cust.Controls.Add(txtEntityOwner);


        cust.Controls.Add(labelAltID);
        cust.Controls.Add(textAltID);

        cust.Controls.Add(labelPaymentType);
        cust.Controls.Add(comboPaymentType);

        cust.Controls.Add(labelAgent);
        cust.Controls.Add(srchAgent);

        int margin = 10;
        int labelleft = 10;
        int top = 20;
        int labelwidth = 135;
        int textwidth = 135;
        int textleft = labelleft + labelwidth + margin;

        //labelMasterCustomer.Left = labelleft;
        //labelMasterCustomer.Top = top; 
        //labelMasterCustomer.Width = labelwidth;
        //txtEntityOwner.Top = labelMasterCustomer.Top;
        //txtEntityOwner.Left = textleft;
        //txtEntityOwner.Width = textwidth * 3;
        //txtEntityOwner.Collapsed = true;

        labelAltID.Left = labelleft;
        labelAltID.Top = top;
        labelAltID.Width = labelwidth;
        textAltID.Left = textleft;
        textAltID.Top = top;
        textAltID.Width = textwidth;


        labelPaymentType.Left = labelleft;
        labelPaymentType.Top = labelAltID.Top + labelAltID.Height + margin;
        labelPaymentType.Width = labelwidth;
        comboPaymentType.Top = labelPaymentType.Top;
        comboPaymentType.Left = textleft;
        comboPaymentType.Width = textwidth;


        labelAgent.Left = labelleft;
        labelAgent.Top = labelPaymentType.Top + labelPaymentType.Height + margin;
        labelAgent.Width = labelwidth;
        srchAgent.Top = labelAgent.Top;
        srchAgent.Left = textleft;
        srchAgent.Width = textwidth * 3;

        srchAgent.Collapsed = true;



      }
      if (!tabMain.TabPages.ContainsKey(networkInventoryTabName))
      {
        tabMain.TabPages.Add(networkInventoryTabName, "Network Inventory");
        TabPage netinv = tabMain.TabPages[networkInventoryTabName];
        CCI.DesktopClient.Common.ctlSearchGrid srch = new CCI.DesktopClient.Common.ctlSearchGrid();
        srch.Name = networkInventoryGridname;
        netinv.Controls.Add(srch);
        srch.DisplaySearchCriteria = false;
        srch.HiddenColumns.Add("Customer", null);
        srch.Init(CommonData.UnmatchedNameTypes.CustomerNetworkInventory, networkInventoryGridname);
        srch.Dock = DockStyle.Fill;
      }
      if (_eac != null && _eac.Entities.Count > 0)
      {
        cust = tabMain.TabPages[custTabName];
        cust.Controls["txtAlternateID"].Text = CommonFunctions.CString(_eac.Entities[entity].Fields["AlternateID"].Value);
        cust.Controls["txtPaymentType"].Text = CommonFunctions.CString(_eac.getValue(entity + ".Entity.Customer.PaymentType"));
        string agentID = _dataSource.getAgentForCustomer(entity);
        if (!string.IsNullOrWhiteSpace(agentID))
          ((ctlSearch)cust.Controls["txtAgent"]).Text = agentID;
        //((ctlSearch)cust.Controls["srchAgent"]).Visible = true;
        //((ctlSearch)cust.Controls["txtEntityOwner"]).Text = CommonFunctions.CString(_eac.Entities[entity].Fields["EntityOwner"].Value); 

      }
      reloadNetworkInventory();
        
    }
    private void textAltID_Validating(object sender, CancelEventArgs e)
    {
    /*
    * Make changes to allow for the new vendor
    * 
    * old style: numeric changed to numeric text with leading zeros and a length of 8
    * 
    * new style: 'R' followed by a number
    */
      string msg = "SaddlebackID must be in the form: 00000999 where 999 is the saddleback Customer Number or in the form R999";
      string altID = ((TextBox)sender).Text;
      if (!string.IsNullOrEmpty(altID)) // only perform this check if there is one
      {
        if (!CommonFunctions.IsNumeric(altID) &&  // not old
            !altID.StartsWith("R", StringComparison.CurrentCultureIgnoreCase)) // and not new
        {
          e.Cancel = true;
          ((TextBox)sender).Undo();
          MessageBox.Show(msg);
        }
        if (altID.Length < 8 && !altID.StartsWith("R", StringComparison.CurrentCultureIgnoreCase))
        {
          altID = CommonFunctions.CInt(altID).ToString("00000000");
        }
        // look for another customer that has the same alternate id
        string entity = null;
        if (!lblNewRecord.Visible) // not a new record
          entity = Entity;
        if (_dataSource.existsAlternateID(entity, altID, EntityType))
        {
          e.Cancel = true;
          ((TextBox)sender).Undo();
          MessageBox.Show("That Saddleback ID already exists for another customer");
        }
        //((TextBox)sender).Text = altID;
      }
    }
    private void btnRefreshFromCitycare_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show("Are you sure you want to add all recently added customers in the City Care database to this database?", "Refresh Customers from CityCare", MessageBoxButtons.YesNo);
      if (ans == DialogResult.Yes)
      {
        Cursor.Current = Cursors.WaitCursor;
        _dataSource.updateDataFromCityCareCustomers(SecurityContext.User, RefreshAll);
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnMerge_Click(object sender, EventArgs e)
    {
      ScreenBase dlg = new dlgMergeCustomer();
      ((dlgMergeCustomer)dlg).FromCustomer = Entity;
      dlg.ShowDialog(this);
    }
    public void reloadNetworkInventory()
    {
      if (tabMain.Controls.ContainsKey(networkInventoryTabName))
        if (tabMain.Controls[networkInventoryTabName].Controls.ContainsKey(networkInventoryGridname))
        {
          if (!string.IsNullOrEmpty(_entity))
          {
            CCI.DesktopClient.Common.ctlSearchGrid srch = (CCI.DesktopClient.Common.ctlSearchGrid)tabMain.Controls[networkInventoryTabName].Controls[networkInventoryGridname];
            Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
            criteria.Add("Customer", new string[] { CCI.DesktopClient.Common.ctlSearchGrid.opEQUALS, _entity });
            srch.SearchCriteria = criteria;
            srch.ReLoad();
          }
        }
    }
    /// <summary>
    /// Overloads the base New() to check if we want to populate Customer with initial data from Chargify
    /// </summary>
    public new void New()
    {
      bool populateFromChargify = false;      
      string chargifySubscription = string.Empty;
      if (_canAccessChargify)
      {
        DialogResult ans = CCI.DesktopClient.Common.CommonFormFunctions.InputBox("Create New Customer from Chargify Subscription",
          "If you want to create this customer from a Chargify Subscription, enter it here. If you do not want to create a new Customer, press Cancel.",
          ref chargifySubscription);
        if (ans == System.Windows.Forms.DialogResult.Cancel)
          return;

        if (!string.IsNullOrEmpty(chargifySubscription) && CommonFunctions.IsInteger(chargifySubscription))
          populateFromChargify = true;
      }
      ICustomer customer = null;
      if (populateFromChargify)
        customer = getChargifyInfo(chargifySubscription);
      if (customer == null)
      {
        base.New();
        return;
      }
      
      string legalName = customer.Organization;
      if (string.IsNullOrEmpty(legalName))
        legalName = string.Format("{0} {0}", customer.FirstName, customer.LastName);
      legalName = legalName.Trim();
      if (string.IsNullOrEmpty(legalName))
        CCI.DesktopClient.Common.CommonFormFunctions.showMessage("This chargify customer has no name. We cannot add that customer");
      else
      {
        NewWithName(legalName);
        txtAddress1.Text = customer.ShippingAddress;
        txtAddress2.Text = customer.ShippingAddress2;
        txtCity.Text = customer.ShippingCity;
        txtState.Text = customer.ShippingState;
        txtZip.Text = customer.ShippingZip;
        txtPhone.Text = customer.Phone;
      }
      // test: add a charge as a test for invoicing....
      decimal amount = Convert.ToDecimal(50.00);
      addMonthlyCharge(chargifySubscription, amount);
    }
    private ICustomer getChargifyInfo(string chargifySubscriptionID)
    {
      try
      {
        Chargify chargify = new Chargify();
        int id = CommonFunctions.CInt(chargifySubscriptionID);
        //return chargify.getCustomer(id);
        ISubscription chargifySubscription = chargify.getSubscription(id);
        if (chargifySubscription == null)
        {
          CCI.DesktopClient.Common.CommonFormFunctions.showMessage("That subscription does not exist in Chargify");
          return null;
        }
        if (!tabMain.TabPages.ContainsKey(custTabName)) // now update the subscription on the screen and in the customer record
        {
          if (tabMain.TabPages[custTabName].Controls.ContainsKey("txtChargifySubscription"))
            tabMain.TabPages[custTabName].Controls["txtChargifySubscription"].Text = chargifySubscriptionID;
        }
        // now reset the initial charge component to zero so they don't get charged again
        chargify.resetStartupComponent(id);
        // and then return the customer
        return chargify.getCustomer(chargifySubscription.Customer.ChargifyID);

      }
      catch (Exception ex)
      {
        CCI.DesktopClient.Common.CommonFormFunctions.showException(ex);
        return null;
      }
    }

    private void addMonthlyCharge(string subscription, decimal amount )
    {
      int subscriptionID = CommonFunctions.CInt(subscription);
      try
      {
        new Chargify().createMonthlyCharge(subscriptionID, amount, "Monthly Charge");
      }
      catch (Exception ex)
      {
        CCI.DesktopClient.Common.CommonFormFunctions.showException(ex);
      }
    }
    private void copyBillingFromPrimaryAddress()
    {
      txtBillingAddress1.Text = txtAddress1.Text;
      txtBillingAddress2.Text = txtAddress2.Text;
      txtBillingCity.Text = txtCity.Text;
      txtBillingState.Text = txtState.Text;
      txtBillingZip.Text = txtZip.Text;
      txtBillingPhone.Text = txtPhone.Text;
      txtBillingCellPhone.Text = txtCellPhone.Text;
    }



    private void btnSyncBillingAddress_Click(object sender, EventArgs e)
    {
        copyBillingFromPrimaryAddress();
    }
  }
}
