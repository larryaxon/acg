using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class frmDealerPricing : ScreenBase
  {
    public frmDealerPricing()
    {
      InitializeComponent();
    }

    private void frmDealerPricing_Load(object sender, EventArgs e)
    {
      cboLevel.Items.AddRange(new DataSource().getDealerPricingLevels());
      ctlDealerPricingMaintenance.TableName = "HostedDealerCosts";
      ctlDealerPricingMaintenance.SecurityContext = SecurityContext;
      ctlDealerPricingMaintenance.HiddenColumns.Add("Dealer", null);      
      cboLevel.Text = "Bronze"; // also fires cboLevel_SelectedIndexChanged, which loads the grid
    }

    private void cboLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
      loadGrid();
    }
    private void loadGrid()
    {
      Dictionary<string, string> parameters = new Dictionary<string,string>(StringComparer.CurrentCultureIgnoreCase);
      parameters.Add("dealer", cboLevel.Text);
      if (ctlDealerPricingMaintenance.DefaultValues.ContainsKey("dealer"))
        ctlDealerPricingMaintenance.DefaultValues["dealer"] = cboLevel.Text;
      else
        ctlDealerPricingMaintenance.DefaultValues.Add("dealer", cboLevel.Text);
      ctlDealerPricingMaintenance.load(parameters);
    }

    private void btnCloneLevel_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtToLevel.Text) || string.IsNullOrEmpty(cboLevel.Text))
      {
        MessageBox.Show("You must have a Level Selected and enter a new level to clone to");
        return;
      }
      DataSource dataSource = new DataSource();
      dataSource.cloneDealerCostLevel(cboLevel.Text, txtToLevel.Text, dtStartDate.Value, SecurityContext.User);
      cboLevel.Items.Clear();
      cboLevel.Items.AddRange(new DataSource().getDealerPricingLevels());
      MessageBox.Show("Clone of pricing level is complete");
    }

    private void btnDeleteLevel_Click(object sender, EventArgs e)
    {
      string pricingLevel = cboLevel.Text;
      if (string.IsNullOrEmpty(pricingLevel))
      if (string.IsNullOrEmpty(txtToLevel.Text) || string.IsNullOrEmpty(cboLevel.Text))
      {
        MessageBox.Show("You must have a Level Selected to delete it");
        return;
      }
      DialogResult ans = MessageBox.Show(string.Format("Are you sure you want to delete pricing level {0}", pricingLevel), "Delete Pricing Level", MessageBoxButtons.YesNo);
      if (ans == DialogResult.Yes) // if they said yes
      {
        // now delete the level
        DataSource dataSource = new DataSource();
        dataSource.deleteDealerCostLevel(pricingLevel);
      }
      cboLevel.Items.Clear();
      cboLevel.Items.AddRange(new DataSource().getDealerPricingLevels());
      cboLevel.Text = string.Empty;
      loadGrid();
    }
  }
}
