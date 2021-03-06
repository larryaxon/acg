﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACG.CommonForms
{
  public partial class ctlACGDatex : TextBox
  {
    private DateTime? _value;
    private string _text;
    public string Format { get; set; }
    public override string Text { get { return _text; } set { setText(value); base.Text = _text; } }
    public DateTime? Value { get { return _value; } }

    public ctlACGDatex() : base()
    {
        Format = "d";
    }

    public override string ToString()
    {
      return this.ToString(Format);
    }

    public string ToString(string format)
    {
      if (string.IsNullOrEmpty(_text))
        return null;
      return _text;
    }

    //private void setValue(DateTime? dt)
    //{
    //  if (dt == null)
    //  {
    //    _value = null;
    //    _text = null;
    //  }
    //  else
    //  {
    //    _value = dt;
    //    _text = ((DateTime)dt).ToString(Format);
    //  }
    //}

    private void setText(string textDate)
    {
      DateTime dt;
      if (string.IsNullOrEmpty(textDate))
      {
        _text = null;
        _value = null;
        return;
      }
      bool isDate = DateTime.TryParse(textDate, out dt);
      if (isDate)
      {
        _value = dt;
        _text = dt.ToString(Format);
      }
      else
      {
        // dont change the value, the incoming is not valid
        //throwError(string.Format("{0} is not a valid Date", textDate));
      }
    }
    private void throwError(string message)
    {
      throw new Exception(message);
    }

    ///************************************************************************************************************
    // * 
    // * Events
    // * 
    // ************************************************************************************************************/
    //public event EventHandler ButtonClick;

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //  //bubble the event up to the parent
    //  if (this.ButtonClick != null)
    //    this.ButtonClick(this, e);
    //}
  }
}
