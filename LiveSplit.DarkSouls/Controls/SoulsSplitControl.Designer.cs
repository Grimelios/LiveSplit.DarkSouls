namespace LiveSplit.DarkSouls.Controls
{
	partial class SoulsSplitControl
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
			this.splitDetailsPanel = new System.Windows.Forms.Panel();
			this.deleteButton = new System.Windows.Forms.Button();
			this.dragImage = new System.Windows.Forms.PictureBox();
			this.splitTypeComboBox = new LiveSplit.DarkSouls.Controls.SoulsDropdown();
			((System.ComponentModel.ISupportInitialize)(this.dragImage)).BeginInit();
			this.SuspendLayout();
			// 
			// splitDetailsPanel
			// 
			this.splitDetailsPanel.Location = new System.Drawing.Point(92, 4);
			this.splitDetailsPanel.Name = "splitDetailsPanel";
			this.splitDetailsPanel.Size = new System.Drawing.Size(321, 21);
			this.splitDetailsPanel.TabIndex = 1;
			// 
			// deleteButton
			// 
			this.deleteButton.Image = global::LiveSplit.DarkSouls.Resources.Delete;
			this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.deleteButton.Location = new System.Drawing.Point(416, 4);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(22, 22);
			this.deleteButton.TabIndex = 3;
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// dragImage
			// 
			this.dragImage.Image = global::LiveSplit.DarkSouls.Resources.Drag;
			this.dragImage.Location = new System.Drawing.Point(1, 4);
			this.dragImage.Name = "dragImage";
			this.dragImage.Size = new System.Drawing.Size(8, 21);
			this.dragImage.TabIndex = 4;
			this.dragImage.TabStop = false;
			this.dragImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragImage_MouseDown);
			this.dragImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragImage_MouseMove);
			this.dragImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragImage_MouseUp);
			// 
			// splitTypeComboBox
			// 
			this.splitTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.splitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.splitTypeComboBox.FormattingEnabled = true;
			this.splitTypeComboBox.Items.AddRange(new object[] {
            "Bonfire",
            "Boss",
            "Covenant",
            "Event",
			"Flag",
            "Item",
            "Manual",
            "Zone"});
			this.splitTypeComboBox.Location = new System.Drawing.Point(12, 4);
			this.splitTypeComboBox.Name = "splitTypeComboBox";
			this.splitTypeComboBox.Prompt = null;
			this.splitTypeComboBox.Size = new System.Drawing.Size(76, 21);
			this.splitTypeComboBox.TabIndex = 2;
			this.splitTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.splitTypeComboBox_SelectedIndexChanged);
			// 
			// SoulsSplitControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dragImage);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.splitTypeComboBox);
			this.Controls.Add(this.splitDetailsPanel);
			this.Name = "SoulsSplitControl";
			this.Size = new System.Drawing.Size(437, 27);
			((System.ComponentModel.ISupportInitialize)(this.dragImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel splitDetailsPanel;
		private SoulsDropdown splitTypeComboBox;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.PictureBox dragImage;
	}
}
