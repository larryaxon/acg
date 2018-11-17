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
  public partial class frmMasterCustomerMaintenance : frmEntityMaintenance
  {
    public frmMasterCustomerMaintenance()
    {
      InitializeComponent();
      EntityType = "MasterCustomer";
      EntityOwner = "CCI";
      AutoNumber = false;
      _showGroups = false;
    }
  }
}
