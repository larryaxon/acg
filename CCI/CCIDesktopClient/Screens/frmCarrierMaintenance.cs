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
  public partial class frmCarrierMaintenance : frmEntityMaintenance
  {
    public frmCarrierMaintenance()
    {
      InitializeComponent();
      EntityType = "Carrier";
      EntityOwner = "CCI";
      AutoNumber = false;
      _showGroups = false;
    }
  }
}
