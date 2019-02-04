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
			this.splitTypeComboBox = new LiveSplit.DarkSouls.Controls.SoulsDropdown();
			this.upButton = new System.Windows.Forms.Button();
			this.downButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// splitDetailsPanel
			// 
			this.splitDetailsPanel.Location = new System.Drawing.Point(86, 4);
			this.splitDetailsPanel.Name = "splitDetailsPanel";
			this.splitDetailsPanel.Size = new System.Drawing.Size(275, 21);
			this.splitDetailsPanel.TabIndex = 1;
			// 
			// splitTypeComboBox
			// 
			this.splitTypeComboBox.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.splitTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.splitTypeComboBox.FormattingEnabled = true;
			this.splitTypeComboBox.Items.AddRange(new object[] {
            "Bonfire",
            "Boss",
            "Covenant",
            "Item",
            "Npc",
            "Zone",
            "VeryLongStringMeantToTestEllipses"});
			this.splitTypeComboBox.Location = new System.Drawing.Point(4, 4);
			this.splitTypeComboBox.Name = "splitTypeComboBox";
			this.splitTypeComboBox.Size = new System.Drawing.Size(78, 21);
			this.splitTypeComboBox.TabIndex = 2;
			this.splitTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.splitTypeComboBox_SelectedIndexChanged);
			// 
			// upButton
			// 
			this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.upButton.Image = global::LiveSplit.DarkSouls.Resources.Up;
			this.upButton.Location = new System.Drawing.Point(365, 3);
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size(24, 23);
			this.upButton.TabIndex = 3;
			this.upButton.UseVisualStyleBackColor = true;
			// 
			// downButton
			// 
			this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.downButton.Image = global::LiveSplit.DarkSouls.Resources.Down;
			this.downButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.downButton.Location = new System.Drawing.Point(390, 3);
			this.downButton.Name = "downButton";
			this.downButton.Size = new System.Drawing.Size(24, 23);
			this.downButton.TabIndex = 4;
			this.downButton.UseVisualStyleBackColor = true;
			// 
			// deleteButton
			// 
			this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.deleteButton.Image = global::LiveSplit.DarkSouls.Resources.Delete;
			this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.deleteButton.Location = new System.Drawing.Point(415, 3);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(24, 23);
			this.deleteButton.TabIndex = 5;
			this.deleteButton.UseVisualStyleBackColor = true;
			// 
			// SoulsSplitControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.downButton);
			this.Controls.Add(this.upButton);
			this.Controls.Add(this.splitTypeComboBox);
			this.Controls.Add(this.splitDetailsPanel);
			this.Name = "SoulsSplitControl";
			this.Size = new System.Drawing.Size(464, 27);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel splitDetailsPanel;
		private SoulsDropdown splitTypeComboBox;
		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Button downButton;
		private System.Windows.Forms.Button deleteButton;
	}
}
