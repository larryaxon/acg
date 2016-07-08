namespace CCI.DesktopClient.Common
{
  partial class ctlEntityGridBase
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
      this.flowEntityMaintenance = new System.Windows.Forms.FlowLayoutPanel();
      this.btnSwap = new System.Windows.Forms.Button();
      this.grdEntities = new System.Windows.Forms.DataGridView();
      this.ctlGroupView = new CCI.DesktopClient.Common.ctlEntityGroupMembers();
      ((System.ComponentModel.ISupportInitialize)(this.grdEntities)).BeginInit();
      this.SuspendLayout();
      // 
      // flowEntityMaintenance
      // 
      this.flowEntityMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.flowEntityMaintenance.AutoScroll = true;
      this.flowEntityMaintenance.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.flowEntityMaintenance.Location = new System.Drawing.Point(25, 3);
      this.flowEntityMaintenance.Name = "flowEntityMaintenance";
      this.flowEntityMaintenance.Size = new System.Drawing.Size(375, 446);
      this.flowEntityMaintenance.TabIndex = 0;
      this.flowEntityMaintenance.Resize += new System.EventHandler(this.flowEntityMaintenance_Resize);
      // 
      // btnSwap
      // 
      this.btnSwap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSwap.Location = new System.Drawing.Point(0, 2);
      this.btnSwap.Name = "btnSwap";
      this.btnSwap.Size = new System.Drawing.Size(19, 447);
      this.btnSwap.TabIndex = 2;
      this.btnSwap.Text = "<";
      this.btnSwap.UseVisualStyleBackColor = true;
      this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
      // 
      // grdEntities
      // 
      this.grdEntities.AllowUserToAddRows = false;
      this.grdEntities.AllowUserToDeleteRows = false;
      this.grdEntities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdEntities.Location = new System.Drawing.Point(406, 0);
      this.grdEntities.Name = "grdEntities";
      this.grdEntities.Size = new System.Drawing.Size(660, 449);
      this.grdEntities.TabIndex = 0;
      this.grdEntities.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdEntities_CellValueChanged);
      this.grdEntities.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdEntities_DataError);
      this.grdEntities.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdEntities_RowHeaderMouseClick);
      this.grdEntities.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdEntities_RowsAdded);
      this.grdEntities.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdEntities_UserAddedRow);
      // 
      // ctlGroupView
      // 
      this.ctlGroupView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ctlGroupView.CanSwap = true;
      this.ctlGroupView.DisplaySearch = false;
      this.ctlGroupView.Entity = "";
      this.ctlGroupView.EntityType = "";
      this.ctlGroupView.GroupDescription = "";
      this.ctlGroupView.GroupEntityType = "";
      this.ctlGroupView.IncludeGrandChilren = false;
      this.ctlGroupView.IsGroup = false;
      this.ctlGroupView.Location = new System.Drawing.Point(406, 3);
      this.ctlGroupView.MemberDescription = "Member";
      this.ctlGroupView.Merge = true;
      this.ctlGroupView.Name = "ctlGroupView";
      this.ctlGroupView.SearchDataSource = null;
      this.ctlGroupView.SecurityContext = null;
      this.ctlGroupView.SelectedMember = null;
      this.ctlGroupView.Size = new System.Drawing.Size(660, 446);
      this.ctlGroupView.TabIndex = 1;
      this.ctlGroupView.Visible = false;
      this.ctlGroupView.OnSelected += new System.EventHandler<System.EventArgs>(this.ctlGroupView_OnSelected);
      // 
      // ctlEntityGridBase
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grdEntities);
      this.Controls.Add(this.flowEntityMaintenance);
      this.Controls.Add(this.ctlGroupView);
      this.Controls.Add(this.btnSwap);
      this.Name = "ctlEntityGridBase";
      this.Size = new System.Drawing.Size(1069, 454);
      ((System.ComponentModel.ISupportInitialize)(this.grdEntities)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowEntityMaintenance;
    private System.Windows.Forms.Button btnSwap;
    private ctlEntityGroupMembers ctlGroupView;
    private System.Windows.Forms.DataGridView grdEntities;
  }
}
