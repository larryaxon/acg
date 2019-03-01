using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CCI.DesktopClient.Screens
{
  public partial class frmAgentMaintenance : frmEntityMaintenance
  {
    public frmAgentMaintenance()
    {
      InitializeComponent();
      EntityType = "Agent";
      EntityOwner = "CCI";
      AutoNumber = true;
      _showGroups = false;
      ctlEntitySearch1.BringToFront();
      SrchCustomers.Init("AgentCustomerList", null);
      SrchCustomers.HiddenColumns.Add("Agent",null);
      lblEntity.Text = "ID";
    }
    public new void Init(string entity) 
    {
      base.Init(entity);
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add("a.Value", new string[] { Common.ctlSearchGrid.opEQUALS, entity });
      SrchCustomers.SearchCriteria = criteria;
      SrchCustomers.ReLoad();
    }
  }
}
