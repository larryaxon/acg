using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACG.CommonForms
{
  public partial class ctlACGDate : UserControl
  {
    private DateTime? _value;
    public string Format { get; set; }
    public override string Text { get { return txtDate.Text; } set { setText(value); } }
    public DateTime? Value
    {
      get
      {
        return _value;
      }
      set
      {
        _value = value;
        if (value == null)
          txtDate.Text = null;
        else
          txtDate.Text = ((DateTime)_value).ToString(Format);
      }
    }
    public ctlACGDate()
    {
      Format = "d";
      InitializeComponent();
    }

    private void setText(string textDate)
    {
      DateTime dt;
      if (string.IsNullOrEmpty(textDate))
      {
        txtDate.Text = null;
        _value = null;
        return;
      }
      bool isDate = DateTime.TryParse(textDate, out dt);
      if (isDate)
      {
        _value = dt;
        txtDate.Text = dt.ToString(Format);
      }
      else
      {
        // dont change the value, the incoming is not valid
        //throwError(string.Format("{0} is not a valid Date", textDate));
      }
    }

    private void txtDate_Leave(object sender, EventArgs e)
    {
      setText(txtDate.Text);
    }

    private void txtDate_Validating(object sender, CancelEventArgs e)
    {
      if (!isValid(txtDate.Text))
        e.Cancel = true;
    }

    private bool isValid(string text)
    {
      if (string.IsNullOrEmpty(text))
        return true;
      DateTime dt;
      bool valid = DateTime.TryParse(text, out dt);
      if (valid)
        _value = dt;
      return valid;
    }

    private void txtDate_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
