using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class ScreenBase : Form, ACG.CommonForms.IScreenBase
  {
    public ACG.Common.ISecurityContext SecurityContext { get; set; }
    public ScreenBase()
    {
      InitializeComponent();
    }
    virtual public void  Save() { }
  }
}
