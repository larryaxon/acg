namespace CCI.DesktopClient.Common
{
  partial class ctlEntitySearch
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
      this.lstSearchList = new System.Windows.Forms.ListBox();
      this.txtSearch = new System.Windows.Forms.TextBox();
      this.ckIncludeTermed = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // lstSearchList
      // 
      this.lstSearchList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstSearchList.FormattingEnabled = true;
      this.lstSearchList.Location = new System.Drawing.Point(3, 26);
      this.lstSearchList.Name = "lstSearchList";
      this.lstSearchList.Size = new System.Drawing.Size(288, 173);
      this.lstSearchList.TabIndex = 4;
      this.lstSearchList.Tag = "Master";
      this.lstSearchList.Visible = false;
      this.lstSearchList.DoubleClick += new System.EventHandler(this.lstSearchList_DoubleClick);
      this.lstSearchList.Leave += new System.EventHandler(this.lstSearchList_Leave);
      // 
      // txtSearch
      // 
      this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSearch.Location = new System.Drawing.Point(3, 3);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new System.Drawing.Size(188, 20);
      this.txtSearch.TabIndex = 3;
      this.txtSearch.Tag = "Master";
      this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
      this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
      this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
      // 
      // ckIncludeTermed
      // 
      this.ckIncludeTermed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ckIncludeTermed.AutoSize = true;
      this.ckIncludeTermed.Location = new System.Drawing.Point(197, 6);
      this.ckIncludeTermed.Name = "ckIncludeTermed";
      this.ckIncludeTermed.Size = new System.Drawing.Size(94, 17);
      this.ckIncludeTermed.TabIndex = 5;
      this.ckIncludeTermed.Text = "Show Inactive";
      this.ckIncludeTermed.UseVisualStyleBackColor = true;
      // 
      // ctlEntitySearch
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.ckIncludeTermed);
      this.Controls.Add(this.lstSearchList);
      this.Controls.Add(this.txtSearch);
      this.Name = "ctlEntitySearch";
      this.Size = new System.Drawing.Size(294, 201);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox lstSearchList;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.CheckBox ckIncludeTermed;
  }
}
