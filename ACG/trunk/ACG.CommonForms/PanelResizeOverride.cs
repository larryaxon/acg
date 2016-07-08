using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACG.CommonForms
{
  public class PanelResizeOverride : Panel
  {
    protected override void OnSizeChanged(EventArgs e)
    {

      if (this.Handle != null)
      {

        this.BeginInvoke((MethodInvoker)delegate
        {

          base.OnSizeChanged(e);

        });

      }

    }

     
  }
}
