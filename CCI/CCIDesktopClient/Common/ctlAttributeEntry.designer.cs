namespace CCI.DesktopClient.Common
{
  partial class ctlAttributeEntry
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
      this.lblAttributeValues = new System.Windows.Forms.Label();
      this.txtAttributeValues = new System.Windows.Forms.TextBox();
      this.lblAttributes = new System.Windows.Forms.Label();
      this.txtAttributes = new System.Windows.Forms.TextBox();
      this.txtItemTypes = new System.Windows.Forms.TextBox();
      this.lblItemTypes = new System.Windows.Forms.Label();
      this.lblItems = new System.Windows.Forms.Label();
      this.txtItems = new System.Windows.Forms.TextBox();
      this.txtEntities = new CCI.DesktopClient.Common.ctlEntitySearch();
      this.lblEntity = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblAttributeValues
      // 
      this.lblAttributeValues.AutoSize = true;
      this.lblAttributeValues.Location = new System.Drawing.Point(13, 100);
      this.lblAttributeValues.Name = "lblAttributeValues";
      this.lblAttributeValues.Size = new System.Drawing.Size(39, 13);
      this.lblAttributeValues.TabIndex = 21;
      this.lblAttributeValues.Text = "Values";
      // 
      // txtAttributeValues
      // 
      this.txtAttributeValues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtAttributeValues.Location = new System.Drawing.Point(76, 100);
      this.txtAttributeValues.Name = "txtAttributeValues";
      this.txtAttributeValues.Size = new System.Drawing.Size(332, 20);
      this.txtAttributeValues.TabIndex = 4;
      // 
      // lblAttributes
      // 
      this.lblAttributes.AutoSize = true;
      this.lblAttributes.Location = new System.Drawing.Point(13, 77);
      this.lblAttributes.Name = "lblAttributes";
      this.lblAttributes.Size = new System.Drawing.Size(51, 13);
      this.lblAttributes.TabIndex = 19;
      this.lblAttributes.Text = "Attributes";
      // 
      // txtAttributes
      // 
      this.txtAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtAttributes.Location = new System.Drawing.Point(76, 77);
      this.txtAttributes.Name = "txtAttributes";
      this.txtAttributes.Size = new System.Drawing.Size(332, 20);
      this.txtAttributes.TabIndex = 3;
      // 
      // txtItemTypes
      // 
      this.txtItemTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtItemTypes.Location = new System.Drawing.Point(76, 31);
      this.txtItemTypes.Name = "txtItemTypes";
      this.txtItemTypes.Size = new System.Drawing.Size(332, 20);
      this.txtItemTypes.TabIndex = 1;
      // 
      // lblItemTypes
      // 
      this.lblItemTypes.AutoSize = true;
      this.lblItemTypes.Location = new System.Drawing.Point(13, 31);
      this.lblItemTypes.Name = "lblItemTypes";
      this.lblItemTypes.Size = new System.Drawing.Size(56, 13);
      this.lblItemTypes.TabIndex = 15;
      this.lblItemTypes.Text = "ItemTypes";
      // 
      // lblItems
      // 
      this.lblItems.AutoSize = true;
      this.lblItems.Location = new System.Drawing.Point(13, 54);
      this.lblItems.Name = "lblItems";
      this.lblItems.Size = new System.Drawing.Size(32, 13);
      this.lblItems.TabIndex = 17;
      this.lblItems.Text = "Items";
      // 
      // txtItems
      // 
      this.txtItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtItems.Location = new System.Drawing.Point(76, 54);
      this.txtItems.Name = "txtItems";
      this.txtItems.Size = new System.Drawing.Size(332, 20);
      this.txtItems.TabIndex = 2;
      // 
      // txtEntities
      // 
      this.txtEntities.Collapsed = true;
      this.txtEntities.Entity = "";
      this.txtEntities.EntityOwner = "CCI";
      this.txtEntities.EntityType = "";
      this.txtEntities.Location = new System.Drawing.Point(76, 5);
      this.txtEntities.Name = "txtEntities";
      this.txtEntities.Size = new System.Drawing.Size(332, 23);
      this.txtEntities.TabIndex = 22;
      // 
      // lblEntity
      // 
      this.lblEntity.AutoSize = true;
      this.lblEntity.Location = new System.Drawing.Point(13, 11);
      this.lblEntity.Name = "lblEntity";
      this.lblEntity.Size = new System.Drawing.Size(33, 13);
      this.lblEntity.TabIndex = 23;
      this.lblEntity.Text = "Entity";
      // 
      // ctlAttributeEntry
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lblEntity);
      this.Controls.Add(this.txtEntities);
      this.Controls.Add(this.lblAttributeValues);
      this.Controls.Add(this.txtAttributeValues);
      this.Controls.Add(this.lblAttributes);
      this.Controls.Add(this.txtAttributes);
      this.Controls.Add(this.txtItemTypes);
      this.Controls.Add(this.lblItemTypes);
      this.Controls.Add(this.lblItems);
      this.Controls.Add(this.txtItems);
      this.Name = "ctlAttributeEntry";
      this.Size = new System.Drawing.Size(420, 129);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblAttributeValues;
    private System.Windows.Forms.TextBox txtAttributeValues;
    private System.Windows.Forms.Label lblAttributes;
    private System.Windows.Forms.TextBox txtAttributes;
    private System.Windows.Forms.TextBox txtItemTypes;
    private System.Windows.Forms.Label lblItemTypes;
    private System.Windows.Forms.Label lblItems;
    private System.Windows.Forms.TextBox txtItems;
    private ctlEntitySearch txtEntities;
    private System.Windows.Forms.Label lblEntity;

  }
}
