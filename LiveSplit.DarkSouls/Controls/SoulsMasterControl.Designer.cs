namespace LiveSplit.DarkSouls.Controls
{
	partial class SoulsMasterControl
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
			this.CollectionControl = new LiveSplit.DarkSouls.Controls.SoulsSplitCollectionControl();
			this.igtCheckbox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// splitCollection
			// 
			this.CollectionControl.Location = new System.Drawing.Point(6, 37);
			this.CollectionControl.Name = "CollectionControl";
			this.CollectionControl.Size = new System.Drawing.Size(461, 520);
			this.CollectionControl.TabIndex = 0;
			// 
			// igtCheckbox
			// 
			this.igtCheckbox.AutoSize = true;
			this.igtCheckbox.Checked = true;
			this.igtCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.igtCheckbox.Location = new System.Drawing.Point(14, 16);
			this.igtCheckbox.Name = "igtCheckbox";
			this.igtCheckbox.Size = new System.Drawing.Size(96, 17);
			this.igtCheckbox.TabIndex = 1;
			this.igtCheckbox.Text = "Use game time";
			this.igtCheckbox.UseVisualStyleBackColor = true;
			// 
			// SoulsMasterControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.igtCheckbox);
			this.Controls.Add(this.CollectionControl);
			this.Name = "SoulsMasterControl";
			this.Size = new System.Drawing.Size(468, 524);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox igtCheckbox;
	}
}
