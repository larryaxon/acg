using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ACG.App.Common;
using ACG.DesktopClient.Common;

namespace ACG.DesktopClient
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      SecurityContext securityContext = new SecurityContext();
      Form fLogin = null;
      while (!securityContext.Cancelled && !securityContext.IsLoggedIn)
      {
        fLogin = new frmLogin(securityContext);
        Application.Run(fLogin);
        if (securityContext.IsLoggedIn)
        {
          Application.Run(new MainForm(securityContext));
          securityContext.IsLoggedIn = false;
        }
        fLogin = null;
      }
    }
  }
}
