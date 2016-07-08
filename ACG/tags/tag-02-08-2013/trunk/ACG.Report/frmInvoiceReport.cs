using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;
using ACG.DesktopClient;
using ACG.DesktopClient.Screens;
using ACG.Report;
using ACG.Report.ACGDataSetTableAdapters;


namespace ACG.DesktopClient.Screens
{
    public partial class frmInvoiceReport : Form
    {
      public frmInvoiceReport()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          // TODO: This line of code loads data into the 'ACGDataSet.vw_InvoiceTime' table. You can move, or remove it, as needed.
          this.vw_InvoiceTimeTableAdapter.Fill(this.ACGDataSet.vw_InvoiceTime);
            this.reportViewer1.RefreshReport();
        }
    }
}