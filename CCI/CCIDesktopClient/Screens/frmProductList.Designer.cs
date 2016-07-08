namespace CCI.DesktopClient.Screens
{
  partial class frmProductList
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
      this.srchCarrier = new ACG.CommonForms.ctlSearch();
      this.txtLastModifiedDateTime = new System.Windows.Forms.TextBox();
      this.txtLastModifiedBy = new System.Windows.Forms.TextBox();
      this.dtEndDate = new System.Windows.Forms.DateTimePicker();
      this.dtStartDate = new System.Windows.Forms.DateTimePicker();
      this.label8 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtMRC = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAddNew = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.srchItem = new ACG.CommonForms.ctlSearch();
      this.txtNRC = new System.Windows.Forms.TextBox();
      this.lblAddNewMessage = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // srchCarrier
      // 
      this.srchCarrier.AddNewMode = false;
      this.srchCarrier.AutoSelectWhenMatch = false;
      this.srchCarrier.ClearSearchWhenComplete = false;
      this.srchCarrier.Collapsed = true;
      this.srchCarrier.CreatedNewItem = false;
      this.srchCarrier.DisplayOnlyID = false;
      this.srchCarrier.ID = "";
      this.srchCarrier.Location = new System.Drawing.Point(79, 3);
      this.srchCarrier.MaxHeight = 228;
      this.srchCarrier.Name = "srchCarrier";
      this.srchCarrier.ShowCustomerNameWhenSet = true;
      this.srchCarrier.ShowTermedCheckBox = false;
      this.srchCarrier.Size = new System.Drawing.Size(397, 21);
      this.srchCarrier.TabIndex = 0;
      this.srchCarrier.OnSelected += new System.EventHandler<System.EventArgs>(this.srchCarrier_OnSelected);
      // 
      // txtLastModifiedDateTime
      // 
      this.txtLastModifiedDateTime.Enabled = false;
      this.txtLastModifiedDateTime.Location = new System.Drawing.Point(154, 196);
      this.txtLastModifiedDateTime.Name = "txtLastModifiedDateTime";
      this.txtLastModifiedDateTime.Size = new System.Drawing.Size(322, 20);
      this.txtLastModifiedDateTime.TabIndex = 7;
      // 
      // txtLastModifiedBy
      // 
      this.txtLastModifiedBy.Enabled = false;
      this.txtLastModifiedBy.Location = new System.Drawing.Point(154, 170);
      this.txtLastModifiedBy.Name = "txtLastModifiedBy";
      this.txtLastModifiedBy.Size = new System.Drawing.Size(322, 20);
      this.txtLastModifiedBy.TabIndex = 6;
      // 
      // dtEndDate
      // 
      this.dtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtEndDate.Location = new System.Drawing.Point(154, 144);
      this.dtEndDate.Name = "dtEndDate";
      this.dtEndDate.Size = new System.Drawing.Size(109, 20);
      this.dtEndDate.TabIndex = 5;
      // 
      // dtStartDate
      // 
      this.dtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtStartDate.Location = new System.Drawing.Point(154, 118);
      this.dtStartDate.Name = "dtStartDate";
      this.dtStartDate.Size = new System.Drawing.Size(109, 20);
      this.dtStartDate.TabIndex = 4;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(12, 199);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(124, 13);
      this.label8.TabIndex = 31;
      this.label8.Text = "Last Modified Date/Time";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(12, 173);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(83, 13);
      this.label7.TabIndex = 30;
      this.label7.Text = "Last Modifed By";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 147);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(52, 13);
      this.label6.TabIndex = 29;
      this.label6.Text = "End Date";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 121);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(52, 13);
      this.label5.TabIndex = 28;
      this.label5.Text = "StartDate";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 69);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(31, 13);
      this.label4.TabIndex = 27;
      this.label4.Text = "MRC";
      // 
      // txtMRC
      // 
      this.txtMRC.Location = new System.Drawing.Point(154, 66);
      this.txtMRC.Name = "txtMRC";
      this.txtMRC.Size = new System.Drawing.Size(322, 20);
      this.txtMRC.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 25;
      this.label2.Text = "Carrier";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 95);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(30, 13);
      this.label3.TabIndex = 23;
      this.label3.Text = "NRC";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 32);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(64, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Search Item";
      // 
      // btnDelete
      // 
      this.btnDelete.Location = new System.Drawing.Point(498, 192);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(145, 23);
      this.btnDelete.TabIndex = 10;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnAddNew
      // 
      this.btnAddNew.Location = new System.Drawing.Point(498, 163);
      this.btnAddNew.Name = "btnAddNew";
      this.btnAddNew.Size = new System.Drawing.Size(145, 23);
      this.btnAddNew.TabIndex = 9;
      this.btnAddNew.Text = "Add New";
      this.btnAddNew.UseVisualStyleBackColor = true;
      this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(498, 137);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(145, 23);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // srchItem
      // 
      this.srchItem.AddNewMode = false;
      this.srchItem.AutoSelectWhenMatch = false;
      this.srchItem.ClearSearchWhenComplete = false;
      this.srchItem.Collapsed = true;
      this.srchItem.CreatedNewItem = false;
      this.srchItem.DisplayOnlyID = false;
      this.srchItem.ID = "";
      this.srchItem.Location = new System.Drawing.Point(79, 30);
      this.srchItem.MaxHeight = 228;
      this.srchItem.Name = "srchItem";
      this.srchItem.ShowCustomerNameWhenSet = true;
      this.srchItem.ShowTermedCheckBox = false;
      this.srchItem.Size = new System.Drawing.Size(397, 21);
      this.srchItem.TabIndex = 1;
      this.srchItem.OnSelected += new System.EventHandler<System.EventArgs>(this.srchItem_OnSelected);
      // 
      // txtNRC
      // 
      this.txtNRC.Location = new System.Drawing.Point(154, 92);
      this.txtNRC.Name = "txtNRC";
      this.txtNRC.Size = new System.Drawing.Size(322, 20);
      this.txtNRC.TabIndex = 3;
      // 
      // lblAddNewMessage
      // 
      this.lblAddNewMessage.AutoSize = true;
      this.lblAddNewMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAddNewMessage.ForeColor = System.Drawing.Color.Red;
      this.lblAddNewMessage.Location = new System.Drawing.Point(486, 33);
      this.lblAddNewMessage.Name = "lblAddNewMessage";
      this.lblAddNewMessage.Size = new System.Drawing.Size(118, 20);
      this.lblAddNewMessage.TabIndex = 32;
      this.lblAddNewMessage.Text = "** Add New **";
      this.lblAddNewMessage.Visible = false;
      // 
      // frmProductList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(837, 445);
      this.Controls.Add(this.lblAddNewMessage);
      this.Controls.Add(this.srchCarrier);
      this.Controls.Add(this.srchItem);
      this.Controls.Add(this.txtLastModifiedDateTime);
      this.Controls.Add(this.txtLastModifiedBy);
      this.Controls.Add(this.dtEndDate);
      this.Controls.Add(this.dtStartDate);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.txtMRC);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.btnAddNew);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.txtNRC);
      this.Name = "frmProductList";
      this.Text = "frmProductListcs";
      this.Load += new System.EventHandler(this.frmProductList_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAddNew;
    private System.Windows.Forms.Button btnSave;
    private ACG.CommonForms.ctlSearch srchItem;
    private System.Windows.Forms.TextBox txtNRC;
    private System.Windows.Forms.Label label3;
    private ACG.CommonForms.ctlSearch srchCarrier;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtMRC;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.DateTimePicker dtStartDate;
    private System.Windows.Forms.DateTimePicker dtEndDate;
    private System.Windows.Forms.TextBox txtLastModifiedBy;
    private System.Windows.Forms.TextBox txtLastModifiedDateTime;
    private System.Windows.Forms.Label lblAddNewMessage;
  }
}