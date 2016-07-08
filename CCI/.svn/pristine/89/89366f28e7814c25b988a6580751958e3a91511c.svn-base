using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCityHostedUSOCMaintenance : ScreenBase
  {
    #region module data

    private const string colWholesaleUSOC = "usocwholesale";
    private const string colWholesaleMRC = "mrcwholesale";
    private const string colWholesaleNRC = "nrcwholesale";
    private const string colWholesaleStartDate = "wholesalestartdate";
    private const string colWholesaleEndDate = "wholesaleenddate";
    private const string colWholesaleOnly = "wholesaleonly";
    private const string colRetailUSOC = "usocretail";
    private const string colRetailMRC = "mrcretail";
    private const string colRetailNRC = "nrcretail";
    private const string colRetailDescription = "descriptionretail";
    private const string colExternalDescription = "RIT Retail Bill Presentation";
    private const string colRetailStartDate = "retailstartdate";
    private const string colRetailEndDate = "retailenddate";
    private const string colRetailOnly = "retailonly";
    private const string colWholesaleDescription = "descriptionwholesale";
    private const string colDQCategory = "DQCategory";
    private const string colDealerQuote = "DealerQuoteYN";
    private const string colIsRecommended = "DQLessScreenOnly";
    private const string colUseMRC = "UseMRCinDQ";

    private const string colIsSaddleback = "IsSaddlebackUSOC";
    private const string colRetailActive = "retailactive";
    private const string colWholesaleActive = "wholesaleactive";
    private const string colExcludeFromExceptions = "ExcludeFromExceptions";

    private string _oldWholesaleUSOC = string.Empty;
    private string _oldRetailUSOC = string.Empty;
    private string _oldRetailDescrition = string.Empty;
    private string _oldWholesaleDescription = string.Empty;

    bool? _lastDealerQuoteYNValue = null;

    private bool _firstTime = true;

    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    #endregion
    public frmCityHostedUSOCMaintenance()
    {
      InitializeComponent();
      txtRetailUSOC.SearchExec = new SearchDataSourceProductList();
      txtWholesaleUSOC.SearchExec = new SearchDataSourceProductList();
      ((SearchDataSourceProductList)txtRetailUSOC.SearchExec).Carrier = "CityHosted";
      ((SearchDataSourceProductList)txtWholesaleUSOC.SearchExec).Carrier = "Saddleback";
      srchUSOCList.CanChangeDisplayFields = true;
     // txtWholesaleUSOC.BringToFront();
    }

    public override void Save()
    {
      #region init fields
      CommonData.USOCMaintenanceOperation operation; 
      string retailUSOC, chsCategory, wholesaleUSOC, dqcategory, 
        retailDescription, wholesaleDescription, externalDescription, externalCategory, taxCode; 
      decimal retailMRC, retailNRC, wholesaleMRC, wholesaleNRC;
      DateTime? retailStartDate,  retailEndDate, wholesaleStartDate, wholesaleEndDate; 
      bool isSaddlebackUSOC, excludeFromException,  retailOnly, wholesaleOnly, isRecommended, useMRC;
      wholesaleUSOC = retailUSOC = wholesaleDescription = retailDescription = wholesaleDescription = chsCategory = 
        externalDescription = externalCategory = taxCode = dqcategory = string.Empty;
      operation = CommonData.USOCMaintenanceOperation.UpdateExisting;
      isSaddlebackUSOC = excludeFromException = retailOnly = wholesaleOnly = isRecommended = useMRC = false;
      retailStartDate = retailEndDate = wholesaleStartDate = wholesaleEndDate = null;
      retailMRC = retailNRC = wholesaleMRC = wholesaleNRC = 0;
      retailUSOC = txtRetailUSOC.Text;
      wholesaleUSOC = txtWholesaleUSOC.Text;
      #endregion

      #region set operation & execute switch if required

      retailOnly = ckRetailOnly.Checked;
      wholesaleOnly = ckWholesaleOnly.Checked;
      //bool existsRetailUSOC = false;
      //if (!string.IsNullOrEmpty(retailUSOC) && _dataSource.existsProduct(DataSource.HOSTEDRETAILCARRIER, retailUSOC))
      //  existsRetailUSOC = true;
      //bool existsWholesaleUSOC = false;
      //if (!string.IsNullOrEmpty(wholesaleUSOC) && _dataSource.existsProduct(DataSource.HOSTEDWHOLESALECARRIER, wholesaleUSOC))
      //  existsWholesaleUSOC = true;
      if (retailOnly)
        operation = CommonData.USOCMaintenanceOperation.AddNewRetail;
      else if (wholesaleOnly)
        operation = CommonData.USOCMaintenanceOperation.AddNewWholesale;
      else if ((_oldRetailUSOC != null && !_oldRetailUSOC.Equals(retailUSOC, StringComparison.CurrentCultureIgnoreCase))
          || (_oldWholesaleUSOC!= null  && !_oldWholesaleUSOC.Equals(wholesaleUSOC, StringComparison.CurrentCultureIgnoreCase)))
        operation = CommonData.USOCMaintenanceOperation.SwitchWholesale;
      else if (string.IsNullOrEmpty(_oldRetailUSOC) && !string.IsNullOrEmpty(retailUSOC)) // this is a new RetailUSOC
        operation = CommonData.USOCMaintenanceOperation.AddNewRetail;
      else if (string.IsNullOrEmpty(_oldWholesaleUSOC) && !string.IsNullOrEmpty(wholesaleUSOC)) // this is a new WholesaleUSOC
        operation = CommonData.USOCMaintenanceOperation.AddNewWholesale;
      else if (string.IsNullOrEmpty(retailUSOC) && !string.IsNullOrEmpty(wholesaleUSOC))
        operation = CommonData.USOCMaintenanceOperation.AddNewWholesale; // ok, this is not and add, but an update, but we need this since there is no retail

      if (operation == CommonData.USOCMaintenanceOperation.SwitchWholesale)
      {
        DialogResult ans = MessageBox.Show(string.Format("You are connecting RetailUSOC <{0}> to WholesaleUSOC <{1}>. All other updates on this screen will be ignored unless you press Save again. Is that what you want to do?",
          retailUSOC, wholesaleUSOC), "Connect Retail to Wholesale USOC", MessageBoxButtons.YesNo);
        if (ans == System.Windows.Forms.DialogResult.Yes)
        {
          _oldRetailUSOC = retailUSOC;
          _oldWholesaleUSOC = wholesaleUSOC;
          _dataSource.reassignWholesaleUSOC(retailUSOC, wholesaleUSOC);
          reloadUsocList(false); // refresh the screen
          loadFields(srchUSOCList.SelectedRow);
          MessageBox.Show("USOCs reassigned");
        }
        return;
      }
      #endregion

      #region set common fields
      chsCategory = cboCHSCategory.Text;
      externalDescription = txtExternalDescription.Text;
      excludeFromException = ckExcludeFromExceptions.Checked;
      isSaddlebackUSOC = ckSaddlebackUSOC.Checked;
      externalCategory = cboRITCategory.Text;
      #endregion

      #region set retail fields
      if (operation == CommonData.USOCMaintenanceOperation.AddNewRetail || operation == CommonData.USOCMaintenanceOperation.UpdateExisting)
      {
        // populate retail fields
        if (string.IsNullOrEmpty(retailUSOC))
        {
          MessageBox.Show("You must have a valid USOC to save");
          return;
        }
        if (string.IsNullOrEmpty(txtRetailDescription.Text))
        {
          MessageBox.Show("You must have a Description to save");
          return;
        }
        retailDescription = txtRetailDescription.Text.Replace("'","");
        if (ckVariableRetailMRC.Checked)
          retailMRC = -1;
        else
          retailMRC = CommonFunctions.CDecimal(txtRetailMRC.Text);
        if (ckVariableRetailNRC.Checked)
          retailNRC = -1;
        else
          retailNRC = CommonFunctions.CDecimal(txtRetailNRC.Text);

        if (dtRetailStartDate.Checked)
          retailStartDate = dtRetailStartDate.Value;
        else
          retailStartDate = null;
        if (dtRetailEndDate.Checked)
          retailEndDate = dtRetailEndDate.Value;
        else
          retailEndDate = null;
        taxCode = cboTaxCode.Text;
      }
      #endregion

      #region set Dealer Quote Fields
      dqcategory = txtDQCategory.Text;
      isRecommended = ckIsRecommended.Checked;
      useMRC = ckUseMRC.Checked;
      #endregion

      #region set wholesale fields
      if (operation == CommonData.USOCMaintenanceOperation.AddNewWholesale || operation == CommonData.USOCMaintenanceOperation.UpdateExisting)
      {
        // populate wholesale fields
        if (string.IsNullOrEmpty(wholesaleUSOC) && !retailOnly)
        {
          MessageBox.Show("You do not have a Wholesale USOC and RetailOnly is not flagged");
          return;
        }
        if (!string.IsNullOrEmpty(wholesaleUSOC))
        {
          if (string.IsNullOrEmpty(txtWholesaleDescription.Text))
          {
            MessageBox.Show("You must have a Description to save");
            return;
          }
          
          wholesaleDescription = txtWholesaleDescription.Text.Replace("'", "");
          wholesaleMRC = CommonFunctions.CDecimal(txtWholesaleMRC.Text);
          wholesaleNRC = CommonFunctions.CDecimal(txtWholesaleNRC.Text);

          if (dtWholesaleStartDate.Checked)
            wholesaleStartDate = dtWholesaleStartDate.Value;
          else
            wholesaleStartDate = null;
          if (dtWholesaleEndDate.Checked)
            wholesaleEndDate = dtWholesaleEndDate.Value;
          else
            wholesaleEndDate = null;
        }
      }
      #endregion

      /*
       * fixup description
       * 
       * We have this potential anomoly where if the retail usoc and wholesale usoc are the same, then they 
       * share the same MasterProductList record. So, if one of the description changes, we need to make sure they both change, or
       * one can be overwritten.
       * 
       * In this case, we take Retail as the "master" and make sure the wholesale matches (only if it has changed)
       * 
       */
      bool notifyUserOfDescriptionChange = false;
      if (retailUSOC.Equals(wholesaleUSOC, StringComparison.CurrentCultureIgnoreCase))
        if (!_oldRetailDescrition.Equals(retailDescription, StringComparison.CurrentCultureIgnoreCase))
        {
          notifyUserOfDescriptionChange = true;
          wholesaleDescription = retailDescription; // retail changed, so set wholesale to match it
        }
        else if (!_oldWholesaleDescription.Equals(wholesaleDescription, StringComparison.CurrentCultureIgnoreCase))
        {
          notifyUserOfDescriptionChange = true;
          retailDescription = wholesaleDescription; // wholesale changed, so set retail to match it
        }
      // tell the user if we are going to do something perhaps unexpected
      if (notifyUserOfDescriptionChange)
        MessageBox.Show("Wholesale and Retail USOCs that are the same share the same description. You have changed one, so I have changed the other to match.");
      //if (retailOnly)
      //  MessageBox.Show("This USOC is flagged as RetailOnly. No Wholesale data will be saved");
      //else if (wholesaleOnly)
      //  MessageBox.Show("This USOC is flagged as WholesaleOnly. No Retail data will be saved");
      string errormsg = _dataSource.updateMatchedUSOC(operation, retailUSOC, wholesaleUSOC, retailDescription, wholesaleDescription, externalDescription,
        retailMRC, retailNRC, wholesaleMRC, wholesaleNRC, chsCategory, retailOnly, wholesaleOnly, retailStartDate, retailEndDate,
        wholesaleStartDate, wholesaleEndDate, isSaddlebackUSOC, excludeFromException, externalCategory, taxCode, SecurityContext.User);
      if (string.IsNullOrEmpty(errormsg))
      {

        errormsg = saveDealerQuoteData(retailUSOC, dqcategory, isRecommended, useMRC);

        if (string.IsNullOrEmpty(errormsg))
        {
          if (string.IsNullOrEmpty(_oldWholesaleUSOC) && !string.IsNullOrEmpty(wholesaleUSOC) && operation != CommonData.USOCMaintenanceOperation.SwitchWholesale && !retailOnly)
          {
            int? ret = _dataSource.reassignWholesaleUSOC(retailUSOC, wholesaleUSOC);
            if (ret != null && ret == -1)
              MessageBox.Show("Reassign to new Wholesale USOC was not successfull");
          }
          MessageBox.Show("USOCs saved");
          reloadUsocList(false); // refresh the screen
          loadFields(srchUSOCList.SelectedRow);
        }
        
      }
      if (!string.IsNullOrEmpty(errormsg))
        MessageBox.Show(string.Format("Error: {0}", errormsg));
    }

    #region form events
    private void frmCityHostedUSOCMaintenance_Load(object sender, EventArgs e)
    {
      srchUSOCList.Init(CCI.Common.CommonData.UnmatchedNameTypes.CityHostedUSOCs, null);
      srchUSOCList.ColumnOrderSaveEnabled = false;
      ckIncludeInactive_CheckedChanged(this, new EventArgs()); // force load to use inactive flag
      //reloadUsocList(true);
      refreshPickLists();
      srchUSOCList.ColumnOrderSaveEnabled = true;
      clear(true);
    }
    private void srchUSOCList_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      loadFields(e.SelectedRow);
    }
    private void srchRetail_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      //loadFields(e.SelectedRow);
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
      Save();
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show("Are you sure you want to delete this Wholesale USOC and all of its associated retail USOCs?", "Delete Wholesale USOC", MessageBoxButtons.YesNo);
      if (ans.ToString() == "Yes")
      {
        _dataSource.deleteWholesaleUSOC(txtWholesaleUSOC.Text);
        reloadUsocList(false); // refresh the screen      
      }

    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      clear();

    }
    //private void ckWholesaleConnect_CheckedChanged(object sender, EventArgs e)
    //{
    //  cboWholesaleConnect.Visible = ckWholesaleConnect.Checked;     
    //}

    //private void btnSaveRetail_Click(object sender, EventArgs e)
    //{
    //  if (string.IsNullOrEmpty(cboWholesaleUSOC.Text))
    //  {
    //    MessageBox.Show("You must have a Wholesale USOC to save");
    //    return;
    //  }
    //  if (string.IsNullOrEmpty(cboRetailUSOC.Text))
    //  {
    //    MessageBox.Show("You must have a Retail USOC to save");
    //    return;
    //  }
    //  if (string.IsNullOrEmpty(txtDescriptionRetail.Text))
    //  {
    //    MessageBox.Show("You must have a Description to save");
    //    return;
    //  }
    //  if (txtDescriptionRetail.Text.Contains("'"))
    //  {
    //    MessageBox.Show("Description must not have a single quote");
    //    return;
    //  }
    //  DateTime? startDate;
    //  if (dtRetailStartDate.Value.Equals(CommonData.PastDateTime))
    //    startDate = null;
    //  else
    //    startDate = (DateTime?)dtRetailStartDate.Value;
    //  DateTime? endDate;
    //  if (dtRetailEndDate.Value.Equals(CommonData.FutureDateTime))
    //    endDate = null;
    //  else
    //    endDate = (DateTime?)dtRetailEndDate.Value;
    //  decimal mrc;
    //  if (txtRetailMRC.Text.ToLower().StartsWith("v"))
    //    mrc = -1;
    //  else
    //    mrc = CommonFunctions.CDecimal(txtRetailMRC.Text);
    //  _dataSource.updateRetailUSOC(getItemID(cboRetailUSOC.Text), getItemID(cboWholesaleUSOC.Text), txtDescriptionRetail.Text, mrc,
    //    CommonFunctions.CDecimal(txtRetailNRC.Text), startDate, endDate, SecurityContext.User, ckRetailOnly.Checked, !ckIsNotWholesaleSaddlebackUSOC.Checked);
    //  if (ckRetailConnect.Checked) // we need to reassign all of the retail usocs attached to this wholesaleusoc to a different usoc
    //  {
    //    _dataSource.reassignWholesaleUSOC(getItemID(cboRetailUSOC.Text), getItemID(cboRetailConnect.Text), false);
    //    ckRetailConnect.Checked = false;
    //  }
    //  srchRetail.ReLoad();
    //  reloadUsocList(); // refresh the screen
    //}
    //private void btnDeleteRetail_Click(object sender, EventArgs e)
    //{
    //  DialogResult ans = MessageBox.Show("Are you sure you want to delete this retail USOC?", "Delete Wholesale USOC", MessageBoxButtons.YesNo);
    //  if (ans.ToString() == "Yes")
    //  {
    //    _dataSource.deleteRetailUSOC(cboRetailUSOC.Text);
    //    srchRetail.ReLoad();
    //    reloadUsocList();     
    //  }

    //}
    //private void btnNewRetail_Click(object sender, EventArgs e)
    //{
    //  clear(false);
    //}
    //private void ckRetailConnect_CheckedChanged(object sender, EventArgs e)
    //{
    //  cboRetailConnect.Visible = ckRetailConnect.Checked;
    //}
    private void ckIncludeNonSaddleback_CheckedChanged(object sender, EventArgs e)
    {
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      foreach (KeyValuePair<string, string[]> entry in srchUSOCList.SearchCriteria)
        criteria.Add(entry.Key, entry.Value);
      if (ckIncludeNonSaddleback.Checked)
      {
        if (criteria.ContainsKey(colIsSaddleback))
          criteria.Remove(colIsSaddleback);
      }
      else
      {
        string[] val = new string[] { ctlSearchGrid.opEQUALS, "Yes" };
        if (criteria.ContainsKey(colIsSaddleback))
          criteria[colIsSaddleback] = val;
        else
          criteria.Add(colIsSaddleback, val);
      }
      srchUSOCList.SearchCriteria = criteria;
      reloadUsocList(true);
    }
    private void ckIncludeInactive_CheckedChanged(object sender, EventArgs e)
    {
      Dictionary<string, string[]> criteria = new Dictionary<string,string[]>(StringComparer.CurrentCultureIgnoreCase);
      foreach (KeyValuePair<string, string[]> entry in srchUSOCList.SearchCriteria)
        criteria.Add(entry.Key, entry.Value);
      if (ckIncludeInactive.Checked)
      {
        if (criteria.ContainsKey(colRetailActive))
          criteria.Remove(colRetailActive);
      }
      else
      {
        string[] val = new string[] { ctlSearchGrid.opEQUALS, "Yes" };
        if (criteria.ContainsKey(colRetailActive))
          criteria[colRetailActive] = val;
        else
          criteria.Add(colRetailActive, val);
      }
      srchUSOCList.SearchCriteria = criteria;
      reloadUsocList(true);
    }
    private void ckRetailInactivate_CheckedChanged(object sender, EventArgs e)
    {
      if (ckRetailInactivate.Checked)
      {
        if (dtRetailStartDate.Value <= DateTime.Today)
        {
          dtRetailEndDate.Checked = true;
          if (dtRetailEndDate.Value >= DateTime.Today)
            dtRetailEndDate.Value = DateTime.Today.AddDays(-1);
        }
      }
      else
        dtRetailEndDate.Checked = false;
    }
    private void ckWholesaleInactivate_CheckedChanged(object sender, EventArgs e)
    {
      if (ckWholesaleInactivate.Checked)
      {
        if (dtWholesaleStartDate.Value <= DateTime.Today)
        {
          dtWholesaleEndDate.Checked = true;
          if (dtWholesaleEndDate.Value >= DateTime.Today)
            dtWholesaleEndDate.Value = DateTime.Today.AddDays(-1);
        }
      }
      else
        dtWholesaleEndDate.Checked = false;
    }
    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (srchUSOCList.SelectedRow != null)
        loadFields(srchUSOCList.SelectedRow);
      else
        clear(true);
    }
    private void ckVariableRetailMRC_CheckedChanged(object sender, EventArgs e)
    {
      ckVariableRetailNRC.Checked = ckVariableRetailMRC.Checked;
    }
    private void ckVariableRetailNRC_CheckedChanged(object sender, EventArgs e)
    {
      ckVariableRetailMRC.Checked = ckVariableRetailNRC.Checked;
    }

    #endregion

    #region private methods
    private string saveDealerQuoteData(string usoc, string section, bool isRecommended, bool useMRC)
    {
      string errormsg = null;
      int? retCode = null;
      if (ckDealerQuote.Checked && string.IsNullOrEmpty(section)) // we have dq data but no section/dq category?
        errormsg = "Regular USOC data has been updated. However, you must have a Dealer Quote Category to save Dealer Quote data";
      else
      {
        if (_lastDealerQuoteYNValue == null) //new usoc
        {
          if (ckDealerQuote.Checked) // checked and it is a new usoc
            retCode = _dataSource.updateDealerQuoteScreenData(usoc, section, isRecommended, useMRC, SecurityContext.User);
          // else 
          //   it is new and NOT checked, so do nothing
        }
        else
        {
          // NOT a new usoc
          //if (ckDealerQuote.Checked != (bool)_lastDealerQuoteYNValue) // they have changed the value 
          //{
            if (ckDealerQuote.Checked) // value has changed and they unchecked it
              retCode = _dataSource.updateDealerQuoteScreenData(usoc, section, isRecommended, useMRC, SecurityContext.User);
            else // value has changed and they checked it
              retCode = _dataSource.deleteScreenDefinition(CommonData.DEALERQUOTESCREEN, section, usoc); // then delete it
          //}
        }
        if (retCode != null && (int)retCode == -1)
          errormsg = "Database error in update of Dealer Quote Data";
      }
      return errormsg;
    }
    private void refreshPickLists()
    {
      if (_firstTime)
      {
        string[] ritCategories = _dataSource.getExternalCategories();
        cboRITCategory.Items.Clear();
        cboRITCategory.Items.AddRange(ritCategories);
        string[] chsCategories = _dataSource.getItemSubCategories(null);
        cboCHSCategory.Items.Clear();
        cboCHSCategory.Items.AddRange(chsCategories);
        cboTaxCode.Items.Clear();
        cboTaxCode.Items.AddRange(_dataSource.getTaxCodes());
        _firstTime = false;
        string[] screenSections = _dataSource.getCodeList(CommonData.DEALERQUOTESCREEN);
        txtDQCategory.Items.Clear();
        txtDQCategory.Items.AddRange(screenSections);
      }
    }
    private void loadFields(DataGridViewRow row)
    {
      if (row == null)
        return;

      clear(true);

      #region Wholesale data
      string wholesaleUSOC = _oldWholesaleUSOC = txtWholesaleUSOC.Text = CommonFunctions.CString(row.Cells[colWholesaleUSOC].Value);
      if (string.IsNullOrEmpty(wholesaleUSOC))
        txtWholesaleUSOC.AddNewMode = true;
      else
        txtWholesaleUSOC.AddNewMode = false;
      _oldWholesaleDescription = txtWholesaleDescription.Text = CommonFunctions.CString(row.Cells[colWholesaleDescription].Value);
      txtWholesaleMRC.Text = CommonFunctions.CString(row.Cells[colWholesaleMRC].Value);
      txtWholesaleNRC.Text = CommonFunctions.CString(row.Cells[colWholesaleNRC].Value);
      DateTime? dt = getDateValue(row.Cells[colWholesaleStartDate].Value);
      if (dt == null)
        dtWholesaleStartDate.Checked = false;
      else
        dtWholesaleStartDate.Value = (DateTime)dt;
      dt = getDateValue(row.Cells[colWholesaleEndDate].Value);
      if (dt == null )
        dtWholesaleEndDate.Checked = false;
      else
        dtWholesaleEndDate.Value = (DateTime)dt;
      ckWholesaleInactivate.Checked = !((!dtWholesaleStartDate.Checked || DateTime.Today >= dtWholesaleStartDate.Value)
        && (!dtWholesaleEndDate.Checked || DateTime.Today <= dtWholesaleEndDate.Value));
      ckWholesaleOnly.Checked = CommonFunctions.CBoolean(row.Cells[colWholesaleOnly].Value);
      //ckIsNotWholesaleSaddlebackUSOC.Checked = !CommonFunctions.CBoolean(row.Cells[colIsSaddleback].Value);
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add("USOCWholesale", new string[] { ctlSearchGrid.opEQUALS, wholesaleUSOC });
      #endregion

      #region Retail data
      _oldRetailUSOC = txtRetailUSOC.Text = CommonFunctions.CString(row.Cells[colRetailUSOC].Value);
      _oldRetailDescrition = txtRetailDescription.Text = CommonFunctions.CString(row.Cells[colRetailDescription].Value);
      txtExternalDescription.Text = CommonFunctions.CString(row.Cells[colExternalDescription].Value);
      cboRITCategory.Text = CommonFunctions.CString(row.Cells["RIT Category"].Value);
      cboCHSCategory.Text = CommonFunctions.CString(row.Cells["CHSCategory"].Value);
      string amountText = CommonFunctions.CString(row.Cells[colRetailMRC].Value);
      if (amountText.Equals("Variable", StringComparison.CurrentCultureIgnoreCase))
      {
        ckVariableRetailMRC.Checked = true;
        txtRetailMRC.Text = string.Empty;      
      }
      else
      {
        ckVariableRetailMRC.Checked = false;
        txtRetailMRC.Text = amountText;
      }
      amountText = CommonFunctions.CString(row.Cells[colRetailNRC].Value);
      if (amountText.Equals("Variable", StringComparison.CurrentCultureIgnoreCase))
      {
        ckVariableRetailNRC.Checked = true;
        txtRetailNRC.Text = string.Empty;
      }
      else
      {
        ckVariableRetailNRC.Checked = false;
        txtRetailNRC.Text = amountText;
      }
      dt = getDateValue(row.Cells[colRetailStartDate].Value);
      if (dt == null)
        dtRetailStartDate.Checked = false;
      else
        dtRetailStartDate.Value = (DateTime)dt;
      dt = getDateValue(row.Cells[colRetailEndDate].Value);
      if (dt == null)
        dtRetailEndDate.Checked = false;
      else
        dtRetailEndDate.Value = (DateTime)dt;
      ckRetailInactivate.Checked = !((!dtRetailStartDate.Checked || DateTime.Today >= dtRetailStartDate.Value) && 
        (!dtRetailEndDate.Checked || DateTime.Today <= dtRetailEndDate.Value));

      ckRetailOnly.Checked = CommonFunctions.CBoolean(row.Cells[colRetailOnly].Value);
      ckExcludeFromExceptions.Checked = CommonFunctions.CBoolean(row.Cells[colExcludeFromExceptions].Value);
      cboTaxCode.Text = CommonFunctions.CString(row.Cells["RIT TranTax"].Value);
      ckSaddlebackUSOC.Checked = CommonFunctions.CBoolean(row.Cells["IsSaddlebackUSOC"].Value);
      #endregion
      #region Dealer Quote data
      txtDQCategory.Text = CommonFunctions.CString(row.Cells[colDQCategory].Value);
      _lastDealerQuoteYNValue = ckDealerQuote.Checked = CommonFunctions.CBoolean(row.Cells[colDealerQuote].Value);
      ckIsRecommended.Checked = CommonFunctions.CBoolean(row.Cells[colIsRecommended].Value);
      ckUseMRC.Checked = CommonFunctions.CBoolean(row.Cells[colUseMRC].Value);
      #endregion
      // final fixup (set item search so they require an existing time
      txtRetailUSOC.AddNewMode = false; // you cannot enter a new nonexisting usoc
      if (!string.IsNullOrEmpty(txtWholesaleUSOC.Text))
        txtWholesaleUSOC.AddNewMode = false; // same
      else
        txtWholesaleUSOC.AddNewMode = true;
    }
    private void clear() { clear(true); }
    private void clear(bool clearWholesale)
    {
      if (clearWholesale)
      {
        txtWholesaleUSOC.Text = string.Empty;
        txtWholesaleDescription.Text = string.Empty;
        txtWholesaleMRC.Text = string.Empty;
        txtWholesaleNRC.Text = string.Empty;
        dtWholesaleStartDate.Value = CommonData.PastDateTime;
        dtWholesaleEndDate.Value = CommonData.FutureDateTime;
        dtWholesaleStartDate.Checked = false;
        dtWholesaleEndDate.Checked = false;
        ckWholesaleInactivate.Checked = false;
        ckWholesaleOnly.Checked = false;
        _oldWholesaleUSOC = string.Empty;
        _oldWholesaleDescription = string.Empty;
        txtWholesaleUSOC.AddNewMode = true; // allows entry of a usoc that is not in the list
      }
      cboCHSCategory.Text = string.Empty;
      cboRITCategory.Text = string.Empty;
      txtRetailUSOC.Text = string.Empty;
      txtRetailDescription.Text = string.Empty;
      _oldRetailDescrition = string.Empty;
      txtExternalDescription.Text = string.Empty;
      ckVariableRetailMRC.Checked = false;
      ckVariableRetailNRC.Checked = false;
      txtRetailMRC.Text = string.Empty;
      txtRetailNRC.Text = string.Empty;
      dtRetailStartDate.Value = CommonData.PastDateTime;
      dtRetailEndDate.Value = CommonData.FutureDateTime;
      dtRetailStartDate.Checked = false;
      dtRetailEndDate.Checked = false;
      ckRetailInactivate.Checked = false;
      ckRetailOnly.Checked = false;
      ckDealerQuote.Checked = false;
      txtDQCategory.Text = string.Empty;
      ckUseMRC.Checked = true;
      ckIsRecommended.Checked = false;
      ckSaddlebackUSOC.Checked = true;
      ckExcludeFromExceptions.Checked = false;
      _oldRetailUSOC = string.Empty;
      txtRetailUSOC.AddNewMode = true; // allows entry of a usoc that is not in the list
      _lastDealerQuoteYNValue = null;
    }
    private void reloadUsocList(bool resetLastRowSelected)
    {
      if (ckRefresh.Checked)
      {
        //srchUSOCList.LoadSearchGrid(srchUSOCList.SearchCriteria);
        srchUSOCList.ReLoad(resetLastRowSelected);
        //refreshPickLists();
      }
    }
    private string getItemID(string item)
    {
      if (!item.Contains(":"))
        return item;
      return item.Substring(0, item.IndexOf(":"));
    }
    private DateTime? getDateValue(object o)
    {
      if (o == null)
        return null;
      if (o.Equals(System.DBNull.Value))
        return null;
      if (o.GetType() != typeof(DateTime))
        return null;
      DateTime dt = (DateTime)o;
      if (dt == CommonData.PastDateTime || dt == CommonData.FutureDateTime)
        return null;
      return dt;
    }

    #endregion



  }
}
