namespace CCI.DesktopClient.Common
{
  partial class ctlGroupMembers
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
      this.components = new System.ComponentModel.Container();
      this.splitDealerCustomers = new System.Windows.Forms.SplitContainer();
      this.btnSwap = new System.Windows.Forms.Button();
      this.btnAddGroup = new System.Windows.Forms.Button();
      this.btnSubractMember = new System.Windows.Forms.Button();
      this.btnAddMember = new System.Windows.Forms.Button();
      this.lstNonMembers = new System.Windows.Forms.ListBox();
      this.lblNonMembers = new System.Windows.Forms.Label();
      this.srchMember = new ACG.CommonForms.ctlSearch();
      this.lstMembers = new System.Windows.Forms.ListBox();
      this.lblMembers = new System.Windows.Forms.Label();
      this.memberRightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.memberTogglePrimary = new System.Windows.Forms.ToolStripMenuItem();
      this.splitDealerCustomers.Panel1.SuspendLayout();
      this.splitDealerCustomers.Panel2.SuspendLayout();
      this.splitDealerCustomers.SuspendLayout();
      this.memberRightClickMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitDealerCustomers
      // 
      this.splitDealerCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitDealerCustomers.Location = new System.Drawing.Point(0, 0);
      this.splitDealerCustomers.Name = "splitDealerCustomers";
      // 
      // splitDealerCustomers.Panel1
      // 
      this.splitDealerCustomers.Panel1.Controls.Add(this.btnSwap);
      this.splitDealerCustomers.Panel1.Controls.Add(this.btnAddGroup);
      this.splitDealerCustomers.Panel1.Controls.Add(this.btnSubractMember);
      this.splitDealerCustomers.Panel1.Controls.Add(this.btnAddMember);
      this.splitDealerCustomers.Panel1.Controls.Add(this.lstNonMembers);
      this.splitDealerCustomers.Panel1.Controls.Add(this.lblNonMembers);
      // 
      // splitDealerCustomers.Panel2
      // 
      this.splitDealerCustomers.Panel2.Controls.Add(this.srchMember);
      this.splitDealerCustomers.Panel2.Controls.Add(this.lstMembers);
      this.splitDealerCustomers.Panel2.Controls.Add(this.lblMembers);
      this.splitDealerCustomers.Size = new System.Drawing.Size(604, 544);
      this.splitDealerCustomers.SplitterDistance = 338;
      this.splitDealerCustomers.TabIndex = 3;
      // 
      // btnSwap
      // 
      this.btnSwap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSwap.Location = new System.Drawing.Point(256, 114);
      this.btnSwap.Name = "btnSwap";
      this.btnSwap.Size = new System.Drawing.Size(75, 23);
      this.btnSwap.TabIndex = 7;
      this.btnSwap.Text = "<< -- >>";
      this.btnSwap.UseVisualStyleBackColor = true;
      this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
      // 
      // btnAddGroup
      // 
      this.btnAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddGroup.Location = new System.Drawing.Point(256, 160);
      this.btnAddGroup.Name = "btnAddGroup";
      this.btnAddGroup.Size = new System.Drawing.Size(75, 23);
      this.btnAddGroup.TabIndex = 6;
      this.btnAddGroup.Text = "Add Group";
      this.btnAddGroup.UseVisualStyleBackColor = true;
      this.btnAddGroup.Visible = false;
      this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
      // 
      // btnSubractMember
      // 
      this.btnSubractMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSubractMember.Location = new System.Drawing.Point(256, 73);
      this.btnSubractMember.Name = "btnSubractMember";
      this.btnSubractMember.Size = new System.Drawing.Size(75, 23);
      this.btnSubractMember.TabIndex = 5;
      this.btnSubractMember.Text = "<";
      this.btnSubractMember.UseVisualStyleBackColor = true;
      this.btnSubractMember.Click += new System.EventHandler(this.btnSubtractMember_Click);
      // 
      // btnAddMember
      // 
      this.btnAddMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddMember.Location = new System.Drawing.Point(256, 31);
      this.btnAddMember.Name = "btnAddMember";
      this.btnAddMember.Size = new System.Drawing.Size(75, 23);
      this.btnAddMember.TabIndex = 3;
      this.btnAddMember.Text = ">";
      this.btnAddMember.UseVisualStyleBackColor = true;
      this.btnAddMember.Click += new System.EventHandler(this.btnAddMember_Click);
      // 
      // lstNonMembers
      // 
      this.lstNonMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstNonMembers.FormattingEnabled = true;
      this.lstNonMembers.Location = new System.Drawing.Point(8, 31);
      this.lstNonMembers.Name = "lstNonMembers";
      this.lstNonMembers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lstNonMembers.Size = new System.Drawing.Size(224, 511);
      this.lstNonMembers.TabIndex = 2;
      // 
      // lblNonMembers
      // 
      this.lblNonMembers.AutoSize = true;
      this.lblNonMembers.Location = new System.Drawing.Point(3, 10);
      this.lblNonMembers.Name = "lblNonMembers";
      this.lblNonMembers.Size = new System.Drawing.Size(216, 13);
      this.lblNonMembers.TabIndex = 1;
      this.lblNonMembers.Text = "Customers that do NOT belong to this dealer";
      // 
      // srchMember
      // 
      this.srchMember.AddNewMode = false;
      this.srchMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchMember.AutoAddNewMode = false;
      this.srchMember.AutoSelectWhenMatch = false;
      this.srchMember.AutoTabToNextControlOnSelect = false;
      this.srchMember.ClearSearchWhenComplete = true;
      this.srchMember.Collapsed = true;
      this.srchMember.CreatedNewItem = false;
      this.srchMember.DisplayOnlyID = false;
      this.srchMember.ID = "";
      this.srchMember.Location = new System.Drawing.Point(4, 29);
      this.srchMember.MaxHeight = 228;
      this.srchMember.MustExistInList = true;
      this.srchMember.MustExistMessage = "You must enter a valid member";
      this.srchMember.Name = "srchMember";
      this.srchMember.ShowCustomerNameWhenSet = false;
      this.srchMember.ShowTermedCheckBox = false;
      this.srchMember.Size = new System.Drawing.Size(250, 24);
      this.srchMember.TabIndex = 0;
      this.srchMember.OnSelected += new System.EventHandler<System.EventArgs>(this.srchMember_OnSelected);
      // 
      // lstMembers
      // 
      this.lstMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstMembers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.lstMembers.FormattingEnabled = true;
      this.lstMembers.Location = new System.Drawing.Point(6, 57);
      this.lstMembers.Name = "lstMembers";
      this.lstMembers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lstMembers.Size = new System.Drawing.Size(249, 485);
      this.lstMembers.TabIndex = 1;
      this.lstMembers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstMembers_MouseDown);
      // 
      // lblMembers
      // 
      this.lblMembers.AutoSize = true;
      this.lblMembers.Location = new System.Drawing.Point(3, 10);
      this.lblMembers.Name = "lblMembers";
      this.lblMembers.Size = new System.Drawing.Size(181, 13);
      this.lblMembers.TabIndex = 0;
      this.lblMembers.Text = "Customers that belong to this dealer. ";
      // 
      // memberRightClickMenu
      // 
      this.memberRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memberTogglePrimary});
      this.memberRightClickMenu.Name = "memberRightClickMenu";
      this.memberRightClickMenu.Size = new System.Drawing.Size(197, 26);
      // 
      // memberTogglePrimary
      // 
      this.memberTogglePrimary.AutoToolTip = true;
      this.memberTogglePrimary.Name = "memberTogglePrimary";
      this.memberTogglePrimary.Size = new System.Drawing.Size(196, 22);
      this.memberTogglePrimary.Text = "Toggle Primary On/Off";
      this.memberTogglePrimary.ToolTipText = "Primary (Off/On)";
      this.memberTogglePrimary.Click += new System.EventHandler(this.memberTogglePrimary_Click);
      // 
      // ctlGroupMembers
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitDealerCustomers);
      this.Name = "ctlGroupMembers";
      this.Size = new System.Drawing.Size(604, 544);
      this.splitDealerCustomers.Panel1.ResumeLayout(false);
      this.splitDealerCustomers.Panel1.PerformLayout();
      this.splitDealerCustomers.Panel2.ResumeLayout(false);
      this.splitDealerCustomers.Panel2.PerformLayout();
      this.splitDealerCustomers.ResumeLayout(false);
      this.memberRightClickMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitDealerCustomers;
    private System.Windows.Forms.Button btnSubractMember;
    private System.Windows.Forms.Button btnAddMember;
    private System.Windows.Forms.ListBox lstNonMembers;
    protected System.Windows.Forms.Label lblNonMembers;
    protected System.Windows.Forms.ListBox lstMembers;
    private System.Windows.Forms.Label lblMembers;
    protected System.Windows.Forms.Button btnAddGroup;
    private System.Windows.Forms.Button btnSwap;
    private System.Windows.Forms.ContextMenuStrip memberRightClickMenu;
    private System.Windows.Forms.ToolStripMenuItem memberTogglePrimary;
    private ACG.CommonForms.ctlSearch srchMember;
  }
}
