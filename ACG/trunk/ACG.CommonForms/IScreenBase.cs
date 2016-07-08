using System;
using System.Windows.Forms;
using System.ComponentModel;
using ACG.Common;

namespace ACG.CommonForms
{
  public interface IScreenBase
  {
    void Save();
    ISecurityContext SecurityContext { get; set; }
    event CancelEventHandler Closing;
    Form MdiParent { get; set; }
    FormWindowState WindowState { get; set; }
    void Show();
    void Activate();
  }
}
