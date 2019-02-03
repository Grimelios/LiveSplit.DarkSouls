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
			this.splitTypeComboBox = new System.Windows.Forms.ComboBox();
			this.splitDetailsPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// splitTypeComboBox
			// 
			this.splitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.splitTypeComboBox.FormattingEnabled = true;
			this.splitTypeComboBox.Items.AddRange(new object[] {
            "Bonfire",
            "Boss",
            "Covenant",
            "Item",
            "Npc",
            "Zone"});
			this.splitTypeComboBox.Location = new System.Drawing.Point(4, 4);
			this.splitTypeComboBox.Name = "splitTypeComboBox";
			this.splitTypeComboBox.Size = new System.Drawing.Size(131, 21);
			this.splitTypeComboBox.TabIndex = 0;
			this.splitTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.splitTypeComboBox_SelectedIndexChanged);
			// 
			// splitDetailsPanel
			// 
			this.splitDetailsPanel.Location = new System.Drawing.Point(142, 4);
			this.splitDetailsPanel.Name = "splitDetailsPanel";
			this.splitDetailsPanel.Size = new System.Drawing.Size(390, 21);
			this.splitDetailsPanel.TabIndex = 1;
			// 
			// SoulsSplitControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitDetailsPanel);
			this.Controls.Add(this.splitTypeComboBox);
			this.Name = "SoulsSplitControl";
			this.Size = new System.Drawing.Size(535, 200);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox splitTypeComboBox;
		private System.Windows.Forms.Panel splitDetailsPanel;
	}
}
