using ACG.Report;

namespace ACG.DesktopClient.Screens
{
  partial class frmInvoiceReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
      this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
      this.ACGDataSet = new ACG.Report.ACGDataSet();
      this.vw_InvoiceTimeBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.vw_InvoiceTimeTableAdapter = new ACG.Report.ACGDataSetTableAdapters.vw_InvoiceTimeTableAdapter();
      ((System.ComponentModel.ISupportInitialize)(this.ACGDataSet)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.vw_InvoiceTimeBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // reportViewer1
      // 
      this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
      reportDataSource1.Name = "dsTimeReport";
      reportDataSource1.Value = this.vw_InvoiceTimeBindingSource;
      this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
      this.reportViewer1.LocalReport.ReportEmbeddedResource = "ACG.Report.rptTimeInvoice.rdlc";
      this.reportViewer1.Location = new System.Drawing.Point(0, 0);
      this.reportViewer1.Name = "reportViewer1";
      this.reportViewer1.Size = new System.Drawing.Size(682, 386);
      this.reportViewer1.TabIndex = 0;
      // 
      // ACGDataSet
      // 
      this.ACGDataSet.DataSetName = "ACGDataSet";
      this.ACGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // vw_InvoiceTimeBindingSource
      // 
      this.vw_InvoiceTimeBindingSource.DataMember = "vw_InvoiceTime";
      this.vw_InvoiceTimeBindingSource.DataSource = this.ACGDataSet;
      // 
      // vw_InvoiceTimeTableAdapter
      // 
      this.vw_InvoiceTimeTableAdapter.ClearBeforeFill = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(682, 386);
      this.Controls.Add(this.reportViewer1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.ACGDataSet)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.vw_InvoiceTimeBindingSource)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource vw_InvoiceTimeBindingSource;
        private ACGDataSet ACGDataSet;
        private new ACG.Report.ACGDataSetTableAdapters.vw_InvoiceTimeTableAdapter vw_InvoiceTimeTableAdapter;
    }
}

