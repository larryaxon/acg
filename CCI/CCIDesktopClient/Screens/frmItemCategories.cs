using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CCI.DesktopClient.Screens
{
  public partial class frmItemCategories : frmMaintenanceBase
  {
    Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public frmItemCategories()
    {
      InitializeComponent();
      init();
    }
    private void init()
    {
      TableName = "ItemCategories";
      load(parameters);
    }
  }
}
