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
  public partial class ctlTimePicker : UserControl
  {
    private string _hours;
    private string _minutes;
    private string _ampm;

    private string _timePicked;

    public string TimePicked
    {
      get { return _timePicked; }
      set 
      { 
        _timePicked = value;
        txtTimeResult.Text = value;
      }
    }

    private Form _CallingForm;
    private String _CallingField;

    public ctlTimePicker()
    {
      InitializeComponent();
    }

    public void Init(Form CallingForm, String CallingField)
    {
      this.Visible = true;
      _hours = Convert.ToString(DateTime.Now.Hour);
      _minutes = Convert.ToString(DateTime.Now.Minute);
      if (Convert.ToInt32(_minutes) < 8)
        _minutes = "00";
      else if (Convert.ToInt32(_minutes) < 23)
        _minutes = "15";
      else if (Convert.ToInt32(_minutes) < 38)
        _minutes = "30";
      else if (Convert.ToInt32(_minutes) < 53)
        _minutes = "45";
      else
      {
        _hours = Convert.ToString(Convert.ToInt32(_hours) + 1);
        _minutes = "00";
      }
      if (DateTime.Now.Hour > 12)
        _ampm = "PM";
      else
        _ampm = "AM";
      _CallingForm = CallingForm;
      _CallingField = CallingField;

      setTimeResult();
    }

    private void setTimeResult()
    {
      this.txtTimeResult.Text = String.Format("{0}:{1} {2}", _hours, _minutes, _ampm);
      _timePicked = this.txtTimeResult.Text;
    }

    private void lnkAM_Click(object sender, EventArgs e)
    {
      _ampm = "AM";
      setTimeResult();
    }

    private void lnkPM_Click(object sender, EventArgs e)
    {
      _ampm = "PM";
      setTimeResult();
    }

    private void lnkHour1_Click(object sender, EventArgs e)
    {
      _hours = "01";
      setTimeResult();

    }

    private void lnkHour2_Click(object sender, EventArgs e)
    {
      _hours = "02";
      setTimeResult();

    }

    private void lnkHour3_Click(object sender, EventArgs e)
    {
      _hours = "03";
      setTimeResult();

    }

    private void lnkHour4_Click(object sender, EventArgs e)
    {
      _hours = "04";
      setTimeResult();

    }

    private void lnkHour5_Click(object sender, EventArgs e)
    {
      _hours = "05";
      setTimeResult();

    }

    private void lnkHour6_Click(object sender, EventArgs e)
    {
      _hours = "06";
      setTimeResult();

    }

    private void lnkHour7_Click(object sender, EventArgs e)
    {
      _hours = "07";
      setTimeResult();

    }

    private void lnkHour8_Click(object sender, EventArgs e)
    {
      _hours = "08";
      setTimeResult();

    }

    private void lnkHour9_Click(object sender, EventArgs e)
    {
      _hours = "09";
      setTimeResult();

    }

    private void lnkHour10_Click(object sender, EventArgs e)
    {
      _hours = "10";
      setTimeResult();

    }

    private void lnkHour11_Click(object sender, EventArgs e)
    {
      _hours = "11";
      setTimeResult();

    }

    private void lnkHour12_Click(object sender, EventArgs e)
    {
      _hours = "12";
      setTimeResult();

    }

    private void lnkMinutes00_Click(object sender, EventArgs e)
    {
      _minutes = "00";
      setTimeResult();

    }

    private void lnkMinutes15_Click(object sender, EventArgs e)
    {
      _minutes = "15";
      setTimeResult();

    }

    private void lnkMinutes30_Click(object sender, EventArgs e)
    {
      _minutes = "30";
      setTimeResult();

    }

    private void lnkMinutes45_Click(object sender, EventArgs e)
    {
      _minutes = "45";
      setTimeResult();

    }

    private void lnkReturnTime_Click(object sender, EventArgs e)
    {
      DateTime validtime;
      if (DateTime.TryParse(_timePicked, out validtime))
        hideTimePicker();
      else
        txtTimeResult.Text = "Error";
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      txtTimeResult.Text = "";
      _timePicked = "";
      hideTimePicker();
    }

    private void txtTimeResult_Click(object sender, EventArgs e)
    {
      showTimePicker();
    }
    private void showTimePicker()
    {
      this.Height = 204;
      this.Width = 223;
    }
    private void hideTimePicker()
    {
      this.Height = 30;
      this.Width = 82;
    }

  }
}
