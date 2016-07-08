namespace CCI.DesktopClient.Screens
{
  partial class frmGroups
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtGroup = new ACG.CommonForms.ctlSearch();
      this.ctlManageGroup = new CCI.DesktopClient.Common.ctlGroupMembers();
      this.label3 = new System.Windows.Forms.Label();
      this.txtGroupName = new System.Windows.Forms.TextBox();
      this.btnSave = new System.Windows.Forms.Button();
      this.ckClear = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(292, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(304, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "Create/Refresh/Manage Groups";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(36, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Group";
      // 
      // txtGroup
      // 
      this.txtGroup.AddNewMode = false;
      this.txtGroup.AutoSelectWhenMatch = false;
      this.txtGroup.ClearSearchWhenComplete = false;
      this.txtGroup.Collapsed = true;
      this.txtGroup.CreatedNewItem = false;
      this.txtGroup.DisplayOnlyID = false;
      this.txtGroup.ID = "";
      this.txtGroup.Location = new System.Drawing.Point(54, 47);
      this.txtGroup.MaxHeight = 228;
      this.txtGroup.Name = "txtGroup";
      this.txtGroup.ShowCustomerNameWhenSet = true;
      this.txtGroup.ShowTermedCheckBox = false;
      this.txtGroup.Size = new System.Drawing.Size(303, 20);
      this.txtGroup.TabIndex = 2;
      this.txtGroup.OnSelected += new System.EventHandler<System.EventArgs>(this.txtGroup_OnSelected);
      // 
      // ctlManageGroup
      // 
      this.ctlManageGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ctlManageGroup.GroupDescription = "Group";
      this.ctlManageGroup.Location = new System.Drawing.Point(7, 102);
      this.ctlManageGroup.MemberDescription = "Member";
      this.ctlManageGroup.Name = "ctlManageGroup";
      this.ctlManageGroup.Size = new System.Drawing.Size(908, 452);
      this.ctlManageGroup.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 75);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(35, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Name";
      // 
      // txtGroupName
      // 
      this.txtGroupName.Location = new System.Drawing.Point(59, 72);
      this.txtGroupName.Name = "txtGroupName";
      this.txtGroupName.Size = new System.Drawing.Size(289, 20);
      this.txtGroupName.TabIndex = 5;
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(383, 49);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // ckClear
      // 
      this.ckClear.AutoSize = true;
      this.ckClear.Checked = true;
      this.ckClear.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckClear.Location = new System.Drawing.Point(481, 52);
      this.ckClear.Name = "ckClear";
      this.ckClear.Size = new System.Drawing.Size(217, 17);
      this.ckClear.TabIndex = 7;
      this.ckClear.Text = "Clear out any prior members of this group\r\n";
      this.ckClear.UseVisualStyleBackColor = true;
      // 
      // frmGroups
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(919, 558);
      this.Controls.Add(this.txtGroup);
      this.Controls.Add(this.ckClear);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.txtGroupName);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.ctlManageGroup);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "frmGroups";
      this.Text = "frmGroups";
      this.Load += new System.EventHandler(this.frmGroups_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private ACG.CommonForms.ctlSearch txtGroup;
    private Common.ctlGroupMembers ctlManageGroup;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtGroupName;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.CheckBox ckClear;
  }
}