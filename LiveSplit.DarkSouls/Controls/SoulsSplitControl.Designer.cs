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
			this.splitTypeComboBox = new LiveSplit.DarkSouls.Controls.SoulsDropdown();
			this.SuspendLayout();
			// 
			// splitDetailsPanel
			// 
			this.splitDetailsPanel.Location = new System.Drawing.Point(84, 4);
			this.splitDetailsPanel.Name = "splitDetailsPanel";
			this.splitDetailsPanel.Size = new System.Drawing.Size(327, 21);
			this.splitDetailsPanel.TabIndex = 1;
			// 
			// deleteButton
			// 
			this.deleteButton.Image = global::LiveSplit.DarkSouls.Resources.Delete;
			this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.deleteButton.Location = new System.Drawing.Point(415, 3);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(22, 23);
			this.deleteButton.TabIndex = 5;
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// splitTypeComboBox
			// 
			this.splitTypeComboBox.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splitTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.splitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.splitTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.splitTypeComboBox.FormattingEnabled = true;
			this.splitTypeComboBox.Items.AddRange(new object[] {
            "Bonfire",
            "Boss",
            "Covenant",
            "Ending",
            "Item",
            "Zone"});
			this.splitTypeComboBox.Location = new System.Drawing.Point(4, 4);
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
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.splitTypeComboBox);
			this.Controls.Add(this.splitDetailsPanel);
			this.Name = "SoulsSplitControl";
			this.Size = new System.Drawing.Size(464, 27);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel splitDetailsPanel;
		private SoulsDropdown splitTypeComboBox;
		private System.Windows.Forms.Button deleteButton;
	}
}
