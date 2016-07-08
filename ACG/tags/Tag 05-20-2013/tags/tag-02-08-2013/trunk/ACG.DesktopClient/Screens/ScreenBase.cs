using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;

namespace ACG.DesktopClient.Screens
{
  public partial class ScreenBase : Form
  {
    public SecurityContext SecurityContext { get; set; }
    public ScreenBase()
    {
      InitializeComponent();
    }
    public void Save() { }
  }
}
