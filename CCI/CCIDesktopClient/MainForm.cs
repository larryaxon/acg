using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;
using CCI.DesktopClient.Screens;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;
using TAGBOSS.Sys.AttributeEngine2;
using TAGBOSS.Sys.AttributeEngine2.ConvertToEAC;

namespace CCI.DesktopClient
{
  public partial class MainForm : Form
  {
    private int _childFormNumber = 0;
    private bool _isProdInstance = false;
    private SecurityContext _securityContext = null;
    private DataSource _dataSource = new DataSource();
    private ACG.Common.PickListEntries maintMenuList = null;
    private PickListEntries codeMenuList = null;
    public MainForm(SecurityContext s)
    {
      _isProdInstance = CommonData.SERVERCONFIGFILENAME == CommonData.SERVERCONFIGFILEPROD;
      string prodOrDev = _isProdInstance ? "Prod" : "Development";
      _securityContext = s;
      InitializeComponent();
      this.Text = string.Format("CCI Main ({0}/{1})", CommonData.DatabaseName, prodOrDev);
      checkSecurity(s);
      hideUnusedToolbarItems();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      createCodeMenu();
      createGenericMaintenanceMenu();
      string gotoForm = ConfigurationManager.AppSettings["AutoLoginScreen"];
      if (!string.IsNullOrEmpty(gotoForm))
      {
        Form frm = getFormFromName(gotoForm);
        if (frm != null)
          ShowForm(frm, true);
      }
    }
    #region generic menu and tool strip events
    private Form getFormFromName(string name)
    {
      string nmspace = this.GetType().Namespace + ".Screens";
      return (Form)Activator.CreateInstance(Type.GetType(nmspace + "." + name)); 
    }
    private void ShowNewForm(object sender, EventArgs e)
    {
      Form childForm = new Form();
      childForm.MdiParent = this;
      childForm.Text = "Window " + _childFormNumber++;
      childForm.Show();
    }
    private void OpenFile(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        string FileName = openFileDialog.FileName;
      }
    }
    private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
      if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        string FileName = saveFileDialog.FileName;
      }
    }
    private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }
    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }
    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }
    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }
    private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      toolStrip.Visible = toolBarToolStripMenuItem.Checked;
    }
    private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      statusStrip.Visible = statusBarToolStripMenuItem.Checked;
    }
    private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.Cascade);
    }
    private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.TileVertical);
    }
    private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.TileHorizontal);
    }
    private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.ArrangeIcons);
    }
    private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (Form childForm in MdiChildren)
      {
        childForm.Close();
      }
    }
    #endregion
    #region application specific menu and toolbar events

    private void saveToolStripButton_Click(object sender, EventArgs e)
    {
      foreach (Form frm in this.MdiChildren)
      {
        object[] pList = new object[] { };
        frm.GetType().GetMethod("Save").Invoke(frm, pList);
      }
    }

    #region active menus


    #region order menus
    private void followUpsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmFollowUps();
      ShowForm(frm, true);
    }

    #endregion

    #region Maintenance menus

    private void masterCodeListToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }
    private void calculationBasisToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCodeMaster("CALCBASIS");
      ShowForm(frm, true);
    }
    private void codeMasterMenu_Click(object sender, EventArgs e)
    {
      string name = ((ToolStripMenuItem)sender).Name;
      int pos = name.IndexOf("_");
      string code = name.Substring(0,pos);
      string desc = codeMenuList[code].Description;
      ScreenBase frm = new frmCodeMaster(code, desc);
      ShowForm(frm, true);
    }
    private void genericMaintenanceScreen_Click(object sender, EventArgs e)
    {
      string name = ((ToolStripMenuItem)sender).Name;
      int pos = name.IndexOf("_");
      string screen = name.Substring(0, pos);
      ACG.CommonForms.IScreenBase frm = new ACG.CommonForms.frmMaintenanceBase();
      ((ACG.CommonForms.frmMaintenanceBase)frm).Init(screen);
      ShowForm((Form)frm, true);
    }
    private void calculationTypeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCodeMaster("CALCTYPE");
      ShowForm(frm, true);
    }
    private void calculationTimingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCodeMaster("CALCTIMING");
      ShowForm(frm, true);
    }
    private void customerMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCustomers();
      ((frmCustomers)frm).EntityType = "Customer";
      ((frmCustomers)frm).CCISecurityContext = _securityContext;
      ((frmCustomers)frm).Init(string.Empty);
      //((frmCustomers)frm).RefreshAll = true;
      ShowForm(frm, true);
    }
    private void iBPProspectMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      iBPProspectMaintenanceToolStripMenuItem_Click(sender, e);
    }
    private void payorMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmEntityMaintenance();
      ((frmEntityMaintenance)frm).EntityType = "Payor";
      frm.SecurityContext = _securityContext;
      ((frmEntityMaintenance)frm).Init(string.Empty);
      ShowForm(frm, true);
    }
    private void carrierMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCarrierMaintenance();
      frm.SecurityContext = _securityContext;
      ((frmEntityMaintenance)frm).Init(string.Empty);
      ShowForm(frm, true);
    }
    private void entityTypeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCodeMaster("ENTITYTYPE");
      ShowForm(frm, true);
    }

    #endregion

    #region product menus
    private void productMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmProductList();
      frm.SecurityContext = _securityContext;
      ShowForm(frm, true);
    }    
    private void masterProductMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmMasterProductList();
      frm.SecurityContext = _securityContext;
      ShowForm(frm, true);
    }
    private void cityHostedUSOCsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedUSOCMaintenance();
      ShowForm(frm, true);
    }
    private void networkInventoryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmNetworkInventory();
      ShowForm(frm, true);
    }
    private void itemCategoryMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmItemCategories();
      ShowForm(frm, true);
    }

    #endregion

    #region cityhosted menus

    private void monthlyCycleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedProcess();
      ShowForm(frm, true);
    }
    private void customerUSOCMatchingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedCustomerUSOCMatching();
      ShowForm(frm, true);
    }
    private void dealerPricingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmDealerPricing();
      ShowForm(frm, true);
    }
    private void cityHostedUSOCMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedUSOCMaintenance();
      ShowForm(frm, true);
    }
    private void customersWithMissingInfoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmMissingSaddlebackCustomerInfo();
      ShowForm(frm, true);
    }
    private void cashReceiptsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCashReceipts();
      ShowForm(frm, true);
    }
    private void dealerMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmDealers();
      ((frmDealers)frm).EntityType = "Dealer";
      frm.SecurityContext = _securityContext;
      ((frmDealers)frm).Init(string.Empty);
      ShowForm(frm, true);
    }
    private void dealerMaintenanceToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      dealerMaintenanceToolStripMenuItem_Click(sender, e);
    }
    private void cityUsocMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cityHostedUSOCsToolStripMenuItem_Click(sender, e);
    }
    private void dealerQuotesScreenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmScreenDefinitionMaintenance();
      ShowForm(frm, true);
    }
    private void unmatchedCityHostedCustomersToolStripMenuItem_Click(object sender, EventArgs e)
    {
      frmUnmatchedNames frm = new frmUnmatchedNames();
      frm.NameType = CommonData.UnmatchedNameTypes.CityHostedCustomer;
      frm.ColumnName = "Customer";
      ShowForm(frm, true);
    }
    //private void importsToolStripMenuItem_Click(object sender, EventArgs e)
    //{
    //  ScreenBase frm = new frmImports();
    //  frm.SecurityContext = _securityContext;
    //  ShowForm(frm, true);
    //}
    private void customerMaintenanceToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCustomers();
      ((frmCustomers)frm).EntityType = "Customer";
      frm.SecurityContext = _securityContext;
      ((frmCustomers)frm).Init(string.Empty);
      //((frmCustomers)frm).RefreshAll = true;
      ShowForm(frm, true);
    }
    private void analyticsReportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAnalysisReport();
      ShowForm(frm, true);
    }

    private void oCCAdjustmentsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedOCCMaintenance();
      ShowForm(frm, true);
    }
    private void CreditMemosToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedCreditMemoMaintenance();
      ShowForm(frm, true);
    }

    private void groupMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmGroupMaintenance();
      ShowForm(frm, true);
    }
    private void dealerRetailUSOCPricingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedDealerRetailPriceMaintenance();
      ShowForm(frm, true);
    }
    #endregion

    #region admin menus

    private void attributeMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAttributes();
      ((frmAttributes)frm).RawMode = false;
      ShowForm(frm, true);
    }
    private void developerScreenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAdminDeveloper();
      frm.SecurityContext = _securityContext;
      ShowForm(frm, true);
    }
    private void entityMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAttributes();
      ShowForm(frm, true);
    }
    private void attributeMaintenanceRawModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAttributes();
      ((frmAttributes)frm).RawMode = true;
      ShowForm(frm, true);
    }
    private void userMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmUserMaintenance();
      ShowForm(frm, true);
    }

    #endregion

    #endregion

    #endregion
    #region private methods
    public void ShowForm(Form frm, bool singleInstance)
    {
      if (singleInstance)
        frm = (Form)CommonFormFunctions.FindMatchingChild(this, frm);
      frm.MdiParent = this;
      ((ACG.CommonForms.IScreenBase)frm).SecurityContext = _securityContext;
      frm.Show();
      frm.Activate();
      int cnt = this.MdiChildren.Count();
      if (cnt == 1)  // this is the first one
        frm.WindowState = FormWindowState.Maximized;
    }
    private void checkSecurity(SecurityContext s)
    {
      // this check was moved to Program.cs
      //
      //if (!s.Security.HasObjectAccess("CanAccessDevelopment") && !_isProdInstance)
      //{
      //  DialogResult ans = MessageBox.Show("This program is attached to the Development Database. Any work you do will NOT be saved into the production database. Do you want to Continue?", "Verify Production Database", MessageBoxButtons.YesNo);
      //  if ((ans == DialogResult.No))
      //    this.Close();
      //}

      // first let's take away
      if (!s.Security.HasObjectAccess("CommissionsScreens"))
      {
        unmatchedToolStripMenuItem.Visible = false;
        //productsToolStripMenuItem.Visible = false;
      }
      if (!s.Security.HasObjectAccess("DesktopAdmin"))
      {
        entityMaintenanceToolStripMenuItem.Visible = false;
        attributeMaintenanceRawModeToolStripMenuItem.Visible = false;
        attributeMaintenanceToolStripMenuItem.Visible = false;      
      }
      if (!s.Security.HasObjectAccess("AdminScreens"))
      {
        reportsToolStripMenuItem.Visible = false;
        adminToolStripMenuItem.Visible = false;
        //userMaintenanceToolStripMenuItem.Visible = false;      
      }
      if (!s.Security.HasObjectAccess("OpsScreens"))
      {
        entitiesToolStripMenuItem.Visible = false;
        masterCodeListToolStripMenuItem.Visible = false;
        //adminToolStripMenuItem.Visible = false;        
      }
      // now we add back
      if (!s.Security.HasObject("CCIAdmin"))
      {
        cityHostedUsocNewToolStripMenuItem.Visible = false;
        itemCategoryMaintenanceToolStripMenuItem.Visible = false;
      }
      if (s.Security.HasObject("CanEditUSOCs"))
      {
        cityHostedUsocNewToolStripMenuItem.Visible = true;
        cityHostedUsocNewToolStripMenuItem.Visible = true;
      }
      unmatchedCustomersFromImportToolStripMenuItem.Visible = false;  // disable this for now
    }
    private void hideUnusedToolbarItems()
    {
      saveAsToolStripMenuItem.Visible = false;
      exitToolStripMenuItem.Visible = false;
      cutToolStripMenuItem.Visible = false;
      copyToolStripMenuItem.Visible = false;
      pasteToolStripMenuItem.Visible = false;
      //toolBarToolStripMenuItem.Visible = false;
      fileMenu.Visible = false;
      editMenu.Visible = false;
      viewMenu.Visible = false;
      helpMenu.Visible = false;
      toolsMenu.Visible = false;
      copyToolStripMenuItem.Visible = false;
      newToolStripButton.Visible = false;
      openToolStripButton.Visible = false;
      printPreviewToolStripButton.Visible = false;
      printToolStripButton.Visible = false;
      helpToolStripButton.Visible = false;
    }
    private void createCodeMenu()      
    {
      codeMenuList = _dataSource.getCodeListDescriptions("master"); // get the master list of codes
      this.masterCodeListToolStripMenuItem.DropDownItems.Clear();
      foreach (PickListEntry codeEntry in codeMenuList)
      {
        if (!codeEntry.ID.Equals("Master", StringComparison.CurrentCultureIgnoreCase) // only CCIAdmins can edit the master menu
          || _securityContext.Security.HasObjectAccess("CCIADmin"))
        {
          ToolStripMenuItem menu = new ToolStripMenuItem();
          menu.Name = string.Format("{0}_codeMenuToolStripMenuItem", codeEntry.ID);
          menu.Text = codeEntry.Description;
          menu.Click += codeMasterMenu_Click;
          this.masterCodeListToolStripMenuItem.DropDownItems.Add(menu);
        }
      }
    }
    private void createGenericMaintenanceMenu()
    {
      maintMenuList = _dataSource.getGenericMaintenanceDescriptions(); // get the master list of codes
      this.maintenanceScreensToolStripMenuItem.DropDownItems.Clear();
      foreach (ACG.Common.PickListEntry codeEntry in maintMenuList)
      {
          ToolStripMenuItem menu = new ToolStripMenuItem();
          menu.Name = string.Format("{0}_genericMaintenanceMenuToolStripMenuItem", codeEntry.ID);
          menu.Text = codeEntry.Description;
          menu.Click += genericMaintenanceScreen_Click;
          this.maintenanceScreensToolStripMenuItem.DropDownItems.Add(menu);
      }
    }
    #endregion

    private void dataSourceMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ACG.CommonForms.IScreenBase frm = new ACG.CommonForms.frmDataSourceMaintenance();
      ((ACG.CommonForms.frmDataSourceMaintenance)frm).DisplayAnalyticsCheckbox = true;
      ShowForm((Form)frm, true);
    }

    private void orderEntryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmOrderEntry();
      ShowForm(frm, true);
    }

    private void entityEditToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new dlgEntityEdit();
      ShowForm(frm, true);
    }

    private void maintenanceTestToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ACG.CommonForms.IScreenBase frm = new ACG.CommonForms.frmMaintenanceBase();
      ((ACG.CommonForms.frmMaintenanceBase)frm).Init("CodeMaster");
      ShowForm((Form)frm, true);
    }

    private void maintTest2ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ACG.CommonForms.IScreenBase frm = new ACG.CommonForms.frmMaintenanceBase();
      ((ACG.CommonForms.frmMaintenanceBase)frm).Init("CalcBasis");
      ShowForm((Form)frm, true);
    }

    private void maintenanceScreensToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void genericMaintenanceScreenSetupToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ACG.CommonForms.IScreenBase frm = new ACG.CommonForms.frmGenericMaintenanceSetup();
      ShowForm((Form)frm, true);
    }

    private void cityHostedUsocNewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmCityHostedUSOCMaintenanceNew();
      ShowForm(frm, true);
    }

    private void wholealeUSOCImportExceptionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmImportUSOCExceptions();
      ShowForm(frm, true);
    }

    private void masterCustomerMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmMasterCustomerMaintenance();
      ShowForm(frm, true);
    }

    private void agentMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAgentMaintenance();
      ShowForm(frm, true);
    }

    private void agentMaintenanceToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmAgentMaintenance();
      ShowForm(frm, true);
    }

    private void masterCustomerMaintenanceToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmMasterCustomerMaintenance();
      ShowForm(frm, true);
    }

    private void testPDFToTextToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmPDFtoText();
      ShowForm(frm, true);
    }
  }
}
