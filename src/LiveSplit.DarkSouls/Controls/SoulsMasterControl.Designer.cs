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
            this.igtCheckbox = new System.Windows.Forms.CheckBox();
            this.resetCheckbox = new System.Windows.Forms.CheckBox();
            this.collectionControl = new LiveSplit.DarkSouls.Controls.SoulsSplitCollectionControl();
            this.startCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
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
            this.igtCheckbox.CheckedChanged += new System.EventHandler(this.igtCheckbox_CheckedChanged);
            // 
            // resetCheckbox
            // 
            this.resetCheckbox.AutoSize = true;
            this.resetCheckbox.Checked = true;
            this.resetCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resetCheckbox.Location = new System.Drawing.Point(116, 16);
            this.resetCheckbox.Name = "resetCheckbox";
            this.resetCheckbox.Size = new System.Drawing.Size(145, 17);
            this.resetCheckbox.TabIndex = 2;
            this.resetCheckbox.Text = "Reset equipment indexes";
            this.resetCheckbox.UseVisualStyleBackColor = true;
            // 
            // collectionControl
            // 
            this.collectionControl.BackColor = System.Drawing.SystemColors.Control;
            this.collectionControl.Location = new System.Drawing.Point(1, 37);
            this.collectionControl.Name = "collectionControl";
            this.collectionControl.Size = new System.Drawing.Size(461, 520);
            this.collectionControl.TabIndex = 0;
            this.collectionControl.UnfinishedCount = 0;
            this.collectionControl.Load += new System.EventHandler(this.collectionControl_Load);
            // 
            // startCheckbox
            // 
            this.startCheckbox.AutoSize = true;
            this.startCheckbox.Checked = true;
            this.startCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startCheckbox.Location = new System.Drawing.Point(267, 16);
            this.startCheckbox.Name = "startCheckbox";
            this.startCheckbox.Size = new System.Drawing.Size(137, 17);
            this.startCheckbox.TabIndex = 3;
            this.startCheckbox.Text = "Start timer automatically";
            this.startCheckbox.UseVisualStyleBackColor = true;
            // 
            // SoulsMasterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startCheckbox);
            this.Controls.Add(this.resetCheckbox);
            this.Controls.Add(this.igtCheckbox);
            this.Controls.Add(this.collectionControl);
            this.Name = "SoulsMasterControl";
            this.Size = new System.Drawing.Size(468, 524);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private SoulsSplitCollectionControl collectionControl;
		private System.Windows.Forms.CheckBox igtCheckbox;
		private System.Windows.Forms.CheckBox resetCheckbox;
		private System.Windows.Forms.CheckBox startCheckbox;
	}
}
