namespace CCI.DesktopClient.Common
{
  partial class ctlException
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblExceptionType = new System.Windows.Forms.Label();
      this.lblReasonCode = new System.Windows.Forms.Label();
      this.lblSource = new System.Windows.Forms.Label();
      this.lblCustomer = new System.Windows.Forms.Label();
      this.lblFrom = new System.Windows.Forms.Label();
      this.lblTo = new System.Windows.Forms.Label();
      this.lblComments = new System.Windows.Forms.Label();
      this.txtExceptionType = new System.Windows.Forms.ComboBox();
      this.txtReasonCode = new System.Windows.Forms.ComboBox();
      this.txtSource = new System.Windows.Forms.TextBox();
      this.txtComments = new System.Windows.Forms.TextBox();
      this.txtDestination = new System.Windows.Forms.TextBox();
      this.lblDestination = new System.Windows.Forms.Label();
      this.txtFrom = new System.Windows.Forms.TextBox();
      this.txtTo = new System.Windows.Forms.TextBox();
      this.txtCustomerID = new CCI.DesktopClient.Common.ctlEntitySearch();
      this.SuspendLayout();
      // 
      // lblExceptionType
      // 
      this.lblExceptionType.AutoSize = true;
      this.lblExceptionType.Location = new System.Drawing.Point(17, 14);
      this.lblExceptionType.Name = "lblExceptionType";
      this.lblExceptionType.Size = new System.Drawing.Size(81, 13);
      this.lblExceptionType.TabIndex = 0;
      this.lblExceptionType.Text = "Exception Type";
      // 
      // lblReasonCode
      // 
      this.lblReasonCode.AutoSize = true;
      this.lblReasonCode.Location = new System.Drawing.Point(17, 40);
      this.lblReasonCode.Name = "lblReasonCode";
      this.lblReasonCode.Size = new System.Drawing.Size(72, 13);
      this.lblReasonCode.TabIndex = 1;
      this.lblReasonCode.Text = "Reason Code";
      // 
      // lblSource
      // 
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new System.Drawing.Point(17, 66);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new System.Drawing.Size(41, 13);
      this.lblSource.TabIndex = 2;
      this.lblSource.Text = "Source";
      // 
      // lblCustomer
      // 
      this.lblCustomer.AutoSize = true;
      this.lblCustomer.Location = new System.Drawing.Point(17, 116);
      this.lblCustomer.Name = "lblCustomer";
      this.lblCustomer.Size = new System.Drawing.Size(51, 13);
      this.lblCustomer.TabIndex = 3;
      this.lblCustomer.Text = "Customer";
      // 
      // lblFrom
      // 
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new System.Drawing.Point(17, 142);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new System.Drawing.Size(30, 13);
      this.lblFrom.TabIndex = 4;
      this.lblFrom.Text = "From";
      // 
      // lblTo
      // 
      this.lblTo.AutoSize = true;
      this.lblTo.Location = new System.Drawing.Point(17, 168);
      this.lblTo.Name = "lblTo";
      this.lblTo.Size = new System.Drawing.Size(20, 13);
      this.lblTo.TabIndex = 5;
      this.lblTo.Text = "To";
      // 
      // lblComments
      // 
      this.lblComments.AutoSize = true;
      this.lblComments.Location = new System.Drawing.Point(17, 194);
      this.lblComments.Name = "lblComments";
      this.lblComments.Size = new System.Drawing.Size(56, 13);
      this.lblComments.TabIndex = 6;
      this.lblComments.Text = "Comments";
      // 
      // txtExceptionType
      // 
      this.txtExceptionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtExceptionType.FormattingEnabled = true;
      this.txtExceptionType.Location = new System.Drawing.Point(102, 11);
      this.txtExceptionType.Name = "txtExceptionType";
      this.txtExceptionType.Size = new System.Drawing.Size(294, 21);
      this.txtExceptionType.TabIndex = 0;
      // 
      // txtReasonCode
      // 
      this.txtReasonCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtReasonCode.FormattingEnabled = true;
      this.txtReasonCode.Location = new System.Drawing.Point(102, 37);
      this.txtReasonCode.Name = "txtReasonCode";
      this.txtReasonCode.Size = new System.Drawing.Size(294, 21);
      this.txtReasonCode.TabIndex = 1;
      // 
      // txtSource
      // 
      this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSource.Location = new System.Drawing.Point(102, 63);
      this.txtSource.Name = "txtSource";
      this.txtSource.Size = new System.Drawing.Size(294, 20);
      this.txtSource.TabIndex = 2;
      // 
      // txtComments
      // 
      this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtComments.Location = new System.Drawing.Point(102, 191);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new System.Drawing.Size(294, 156);
      this.txtComments.TabIndex = 7;
      // 
      // txtDestination
      // 
      this.txtDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDestination.Location = new System.Drawing.Point(102, 88);
      this.txtDestination.Name = "txtDestination";
      this.txtDestination.Size = new System.Drawing.Size(294, 20);
      this.txtDestination.TabIndex = 3;
      // 
      // lblDestination
      // 
      this.lblDestination.AutoSize = true;
      this.lblDestination.Location = new System.Drawing.Point(17, 91);
      this.lblDestination.Name = "lblDestination";
      this.lblDestination.Size = new System.Drawing.Size(60, 13);
      this.lblDestination.TabIndex = 14;
      this.lblDestination.Text = "Destination";
      // 
      // txtFrom
      // 
      this.txtFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFrom.Location = new System.Drawing.Point(102, 139);
      this.txtFrom.Name = "txtFrom";
      this.txtFrom.Size = new System.Drawing.Size(294, 20);
      this.txtFrom.TabIndex = 18;
      // 
      // txtTo
      // 
      this.txtTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTo.Location = new System.Drawing.Point(102, 165);
      this.txtTo.Name = "txtTo";
      this.txtTo.Size = new System.Drawing.Size(294, 20);
      this.txtTo.TabIndex = 19;
      // 
      // txtCustomerID
      // 
      this.txtCustomerID.AutoSelectWhenMatch = false;
      this.txtCustomerID.ClearSearchWhenComplete = false;
      this.txtCustomerID.Collapsed = true;
      this.txtCustomerID.Entity = "";
      this.txtCustomerID.EntityOwner = "CCI";
      this.txtCustomerID.EntityType = "";
      this.txtCustomerID.Location = new System.Drawing.Point(102, 113);
      this.txtCustomerID.MaxHeight = 204;
      this.txtCustomerID.Name = "txtCustomerID";
      this.txtCustomerID.ShowCustomerNameWhenSet = false;
      this.txtCustomerID.ShowTermedCheckBox = true;
      this.txtCustomerID.Size = new System.Drawing.Size(294, 20);
      this.txtCustomerID.TabIndex = 17;
      // 
      // ctlException
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.txtTo);
      this.Controls.Add(this.txtFrom);
      this.Controls.Add(this.txtCustomerID);
      this.Controls.Add(this.txtDestination);
      this.Controls.Add(this.lblDestination);
      this.Controls.Add(this.txtComments);
      this.Controls.Add(this.txtSource);
      this.Controls.Add(this.txtReasonCode);
      this.Controls.Add(this.txtExceptionType);
      this.Controls.Add(this.lblComments);
      this.Controls.Add(this.lblTo);
      this.Controls.Add(this.lblFrom);
      this.Controls.Add(this.lblCustomer);
      this.Controls.Add(this.lblSource);
      this.Controls.Add(this.lblReasonCode);
      this.Controls.Add(this.lblExceptionType);
      this.Name = "ctlException";
      this.Size = new System.Drawing.Size(404, 350);
      this.Load += new System.EventHandler(this.ctlException_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblExceptionType;
    private System.Windows.Forms.Label lblReasonCode;
    private System.Windows.Forms.Label lblSource;
    private System.Windows.Forms.Label lblCustomer;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.Label lblTo;
    private System.Windows.Forms.Label lblComments;
    private System.Windows.Forms.ComboBox txtExceptionType;
    private System.Windows.Forms.ComboBox txtReasonCode;
    private System.Windows.Forms.TextBox txtSource;
    private System.Windows.Forms.TextBox txtComments;
    private System.Windows.Forms.TextBox txtDestination;
    private System.Windows.Forms.Label lblDestination;
    private ctlEntitySearch txtCustomerID;
    private System.Windows.Forms.TextBox txtFrom;
    private System.Windows.Forms.TextBox txtTo;
  }
}
