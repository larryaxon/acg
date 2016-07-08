using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ACG.DesktopClient.Common
{
  public class MaintenanceGridRowSelectedArgs : EventArgs
  {
    public DataGridViewRow SelectedRow { get; set; }
    public MaintenanceGridRowSelectedArgs()
    {
      SelectedRow = null;
    }
  }
}
