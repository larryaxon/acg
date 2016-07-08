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

using ACG.Common;
using ACG.App.Common;
using ACG.Sys.Data;
using ACG.DesktopClient.Common;
using ACG.DesktopClient.Screens;


namespace ACG.DesktopClient
{
  public partial class MainForm : Form
  {
    private int _childFormNumber = 0;
    private SecurityContext _securityContext = null;
    private DataSource _dataSource = new DataSource();
    public MainForm(SecurityContext s)
    {
      _securityContext = s;
      InitializeComponent();
      checkSecurity(s);
      this.Text = "ACG Main - " + _securityContext.User;
      hideUnusedToolbarItems();
    }
    #region generic menu and tool strip events
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

    #region private methods
    private void showForm(ScreenBase frm)
    {
      ShowForm(frm, false);
    }
    public void ShowForm(ScreenBase frm, bool singleInstance)
    {
      if (singleInstance)
        frm = (ScreenBase)CommonFormFunctions.FindMatchingChild(this, frm);
      frm.MdiParent = this;
      frm.SecurityContext = _securityContext;
      frm.Show();
      frm.Activate();
      int cnt = this.MdiChildren.Count();
      if (cnt == 1)  // this is the first one
        frm.WindowState = FormWindowState.Maximized;
    }
    public void ShowForm(Form frm)
    {
      frm = (Form)CommonFormFunctions.FindMatchingChild(this, frm);
      frm.MdiParent = this;
      frm.Show();
      frm.Activate();
      int cnt = this.MdiChildren.Count();
      if (cnt == 1)  // this is the first one
        frm.WindowState = FormWindowState.Maximized;
    }
    private void checkSecurity(SecurityContext s)
    {
      if (!_securityContext.Security.HasObjectAccess("AdminScreens"))
      {
        adminToolStripMenuItem.Visible = false;
        maintenanceToolStripMenuItem.Visible = false;
        reportsToolStripMenuItem.Visible = false;
      }
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
    #endregion
    #region application specific menu and toolbar events
    private void customersToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmMaintenance();
      ((frmMaintenance)frm).TableName = "customers";
      ShowForm(frm, true);
    }
    private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmProjects();
      ShowForm(frm, true);
    }
    private void resourcesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmMaintenance();
      ((frmMaintenance)frm).TableName = "resources";
      ShowForm(frm, true);
    }
    private void ratesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmRates();
      ShowForm(frm, true);
    }
    private void timeEntryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmTimeEntry();
      ShowForm(frm, true);
    }
    private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmInvoicePrint();
      ShowForm(frm, true);
    }
    private void budgetQueryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmBudgetQuery();
      ShowForm(frm, true);
    }
    #endregion

  }
}
