namespace CCI.DesktopClient.Screens
{
  partial class frmCityHostedNameMatches
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
      this.srchNameMatches = new ACG.CommonForms.ctlSearchGrid();
      this.txtsaddlebackName = new System.Windows.Forms.TextBox();
      this.txtcustomerID = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.txtnewcustomerID = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // srchNameMatches
      // 
      this.srchNameMatches.AllowSortByColumn = true;
      this.srchNameMatches.AutoRefreshWhenFieldChecked = false;
      this.srchNameMatches.CanChangeDisplayFields = true;
      this.srchNameMatches.CanChangeDisplaySearchCriteria = true;
      this.srchNameMatches.ColumnName = "";
      this.srchNameMatches.DisplayFields = false;
      this.srchNameMatches.DisplaySearchCriteria = true;
      this.srchNameMatches.FieldsDefaultIsChecked = true;
      this.srchNameMatches.ForceReloadSearchColumns = false;
      this.srchNameMatches.IDList = null;
      this.srchNameMatches.IncludeGroupAsCriteria = false;
      this.srchNameMatches.InnerWhere = "";
      this.srchNameMatches.Location = new System.Drawing.Point(2, 2);
      this.srchNameMatches.Name = "srchNameMatches";
      this.srchNameMatches.NameType = null;
      this.srchNameMatches.SearchCriteria = null;
      this.srchNameMatches.Size = new System.Drawing.Size(1193, 448);
      this.srchNameMatches.TabIndex = 0;
      this.srchNameMatches.Title = "Search (0 Records Found)";
      this.srchNameMatches.UniqueIdentifier = "ID";
      this.srchNameMatches.UseNamedSearches = false;
      this.srchNameMatches.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchNameMatches_RowSelected);
      // 
      // txtsaddlebackName
      // 
      this.txtsaddlebackName.Location = new System.Drawing.Point(130, 474);
      this.txtsaddlebackName.Name = "txtsaddlebackName";
      this.txtsaddlebackName.Size = new System.Drawing.Size(299, 20);
      this.txtsaddlebackName.TabIndex = 2;
      // 
      // txtcustomerID
      // 
      this.txtcustomerID.Location = new System.Drawing.Point(100, 511);
      this.txtcustomerID.Name = "txtcustomerID";
      this.txtcustomerID.Size = new System.Drawing.Size(100, 20);
      this.txtcustomerID.TabIndex = 3;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(26, 477);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(98, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Saddleback Name:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(24, 505);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 26);
      this.label2.TabIndex = 5;
      this.label2.Text = "Linked To\r\nCustomer:";
      // 
      // btnDelete
      // 
      this.btnDelete.Location = new System.Drawing.Point(1120, 511);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 7;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(1024, 511);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(265, 506);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(80, 26);
      this.label3.TabIndex = 10;
      this.label3.Text = "New Linked To\r\nCustomer:";
      // 
      // txtnewcustomerID
      // 
      this.txtnewcustomerID.Location = new System.Drawing.Point(363, 511);
      this.txtnewcustomerID.Name = "txtnewcustomerID";
      this.txtnewcustomerID.Size = new System.Drawing.Size(100, 20);
      this.txtnewcustomerID.TabIndex = 9;
      // 
      // frmCityHostedNameMatches
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1207, 541);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.txtnewcustomerID);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txtcustomerID);
      this.Controls.Add(this.txtsaddlebackName);
      this.Controls.Add(this.srchNameMatches);
      this.Name = "frmCityHostedNameMatches";
      this.Text = "frmCityHostedNameMatches";
      this.Load += new System.EventHandler(this.frmCityHostedNameMatches_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private ACG.CommonForms.ctlSearchGrid srchNameMatches;
    private System.Windows.Forms.TextBox txtsaddlebackName;
    private System.Windows.Forms.TextBox txtcustomerID;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtnewcustomerID;
  }
}