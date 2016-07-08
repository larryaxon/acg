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
  public partial class frmException : ScreenBase
  {
    public string CustomerID { get { return ctlException1.CustomerID; } set { ctlException1.CustomerID = value; } }
    public string ExceptionType { get { return ctlException1.ExceptionType; } set { ctlException1.ExceptionType = value; } }
    public string ReasonCode { get { return ctlException1.ReasonCode; } set { ctlException1.ReasonCode = value; } }
    public string Source { get { return ctlException1.Source; } set { ctlException1.Source = value; } }
    public string Destination { get { return ctlException1.Destination; } set { ctlException1.Destination = value; } }
    public string To { get { return ctlException1.To; } set { ctlException1.To = value; } }
    public string From { get { return ctlException1.From; } set { ctlException1.From = value; } }
    public string Comments { get { return ctlException1.Comments; } set { ctlException1.Comments = value; } }

    public bool CustomerIDVisible { get { return ctlException1.CustomerIDVisible; } set { ctlException1.CustomerIDVisible = value; } }
    public bool ExceptionTypeVisible { get { return ctlException1.ExceptionTypeVisible; } set { ctlException1.ExceptionTypeVisible = value; } }
    public bool ReasonCodeVisible { get { return ctlException1.ReasonCodeVisible; } set { ctlException1.ReasonCodeVisible = value; } }
    public bool SourceVisible { get { return ctlException1.SourceVisible; } set { ctlException1.SourceVisible = value; } }
    public bool DestinationVisible { get { return ctlException1.DestinationVisible; } set { ctlException1.DestinationVisible = value; } }
    public bool ToVisible { get { return ctlException1.ToVisible; } set { ctlException1.ToVisible = value; } }
    public bool FromVisible { get { return ctlException1.FromVisible; } set { ctlException1.FromVisible = value; } }
    public bool CommentsVisible { get { return ctlException1.CommentsVisible; } set { ctlException1.CommentsVisible = value; } }

    public bool CustomerIDEnabled { get { return ctlException1.CustomerIDEnabled; } set { ctlException1.CustomerIDEnabled = value; } }
    public bool ExceptionTypeEnabled { get { return ctlException1.ExceptionTypeEnabled; } set { ctlException1.ExceptionTypeEnabled = value; } }
    public bool ReasonCodeEnabled { get { return ctlException1.ReasonCodeEnabled; } set { ctlException1.ReasonCodeEnabled = value; } }
    public bool SourceEnabled { get { return ctlException1.SourceEnabled; } set { ctlException1.SourceEnabled = value; } }
    public bool DestinationEnabled { get { return ctlException1.DestinationEnabled; } set { ctlException1.DestinationEnabled = value; } }
    public bool ToEnabled { get { return ctlException1.ToEnabled; } set { ctlException1.ToEnabled = value; } }
    public bool FromEnabled { get { return ctlException1.FromEnabled; } set { ctlException1.FromEnabled = value; } }
    public bool CommentsEnabled { get { return ctlException1.CommentsEnabled; } set { ctlException1.CommentsEnabled = value; } }
    public frmException()
    {
      InitializeComponent();
    }

    private void frmException_Load(object sender, EventArgs e)
    {
      ctlException1.SecurityContext = SecurityContext;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      ctlException1.Save();
      this.Close();
    }
  }
}
