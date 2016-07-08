using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ACG.CommonForms
{
  public class MaintenanceGridRowSelectedArgs : EventArgs
  {
    public DataGridViewRow SelectedRow { get; set; }
    public string SelectedColumn { get; set; }
    public List<string> IDList { get; set; }
    public MaintenanceGridRowSelectedArgs()
    {
      SelectedRow = null;
      IDList = null;
      SelectedColumn = null;
    }
  }
}
