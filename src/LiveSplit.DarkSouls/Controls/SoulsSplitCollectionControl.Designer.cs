namespace LiveSplit.DarkSouls.Controls
{
	partial class SoulsSplitCollectionControl
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
			this.splitCountLabel = new System.Windows.Forms.Label();
			this.splitsPanel = new System.Windows.Forms.Panel();
			this.clearSplitsButton = new System.Windows.Forms.Button();
			this.addSplitButton = new System.Windows.Forms.Button();
			this.splitShadow = new System.Windows.Forms.Panel();
			this.unfinishedSplitsLabel = new System.Windows.Forms.Label();
			this.splitDetailsLabel = new LiveSplit.DarkSouls.Controls.SoulsLabel();
			this.splitTypeLabel = new LiveSplit.DarkSouls.Controls.SoulsLabel();
			this.SuspendLayout();
			// 
			// splitCountLabel
			// 
			this.splitCountLabel.AutoSize = true;
			this.splitCountLabel.Location = new System.Drawing.Point(174, 13);
			this.splitCountLabel.Name = "splitCountLabel";
			this.splitCountLabel.Size = new System.Drawing.Size(39, 13);
			this.splitCountLabel.TabIndex = 7;
			this.splitCountLabel.Text = "0 splits";
			// 
			// splitsPanel
			// 
			this.splitsPanel.AutoScroll = true;
			this.splitsPanel.BackColor = System.Drawing.SystemColors.Control;
			this.splitsPanel.Location = new System.Drawing.Point(1, 56);
			this.splitsPanel.Name = "splitsPanel";
			this.splitsPanel.Size = new System.Drawing.Size(458, 431);
			this.splitsPanel.TabIndex = 6;
			// 
			// clearSplitsButton
			// 
			this.clearSplitsButton.Enabled = false;
			this.clearSplitsButton.Image = global::LiveSplit.DarkSouls.Resources.Clear;
			this.clearSplitsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.clearSplitsButton.Location = new System.Drawing.Point(85, 7);
			this.clearSplitsButton.Name = "clearSplitsButton";
			this.clearSplitsButton.Size = new System.Drawing.Size(82, 24);
			this.clearSplitsButton.TabIndex = 5;
			this.clearSplitsButton.Text = "Clear Splits";
			this.clearSplitsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.clearSplitsButton.UseVisualStyleBackColor = true;
			this.clearSplitsButton.Click += new System.EventHandler(this.clearSplitsButton_Click);
			// 
			// addSplitButton
			// 
			this.addSplitButton.Image = global::LiveSplit.DarkSouls.Resources.Add;
			this.addSplitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.addSplitButton.Location = new System.Drawing.Point(12, 7);
			this.addSplitButton.Name = "addSplitButton";
			this.addSplitButton.Size = new System.Drawing.Size(71, 24);
			this.addSplitButton.TabIndex = 4;
			this.addSplitButton.Text = "Add Split";
			this.addSplitButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.addSplitButton.UseVisualStyleBackColor = true;
			this.addSplitButton.Click += new System.EventHandler(this.addSplitButton_Click);
			// 
			// splitShadow
			// 
			this.splitShadow.BackColor = System.Drawing.Color.PaleGreen;
			this.splitShadow.Location = new System.Drawing.Point(359, 7);
			this.splitShadow.Name = "splitShadow";
			this.splitShadow.Size = new System.Drawing.Size(100, 21);
			this.splitShadow.TabIndex = 8;
			this.splitShadow.Visible = false;
			// 
			// unfinishedSplitsLabel
			// 
			this.unfinishedSplitsLabel.AutoSize = true;
			this.unfinishedSplitsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.unfinishedSplitsLabel.Location = new System.Drawing.Point(225, 13);
			this.unfinishedSplitsLabel.Name = "unfinishedSplitsLabel";
			this.unfinishedSplitsLabel.Size = new System.Drawing.Size(90, 13);
			this.unfinishedSplitsLabel.TabIndex = 9;
			this.unfinishedSplitsLabel.Text = "0 unfinished splits";
			this.unfinishedSplitsLabel.Visible = false;
			// 
			// splitDetailsLabel
			// 
			this.splitDetailsLabel.Location = new System.Drawing.Point(93, 39);
			this.splitDetailsLabel.Name = "splitDetailsLabel";
			this.splitDetailsLabel.Size = new System.Drawing.Size(321, 13);
			this.splitDetailsLabel.TabIndex = 2;
			this.splitDetailsLabel.Text = "Details";
			// 
			// splitTypeLabel
			// 
			this.splitTypeLabel.Location = new System.Drawing.Point(13, 39);
			this.splitTypeLabel.Name = "splitTypeLabel";
			this.splitTypeLabel.Size = new System.Drawing.Size(76, 13);
			this.splitTypeLabel.TabIndex = 0;
			this.splitTypeLabel.Text = "Type";
			// 
			// SoulsSplitCollectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.unfinishedSplitsLabel);
			this.Controls.Add(this.splitShadow);
			this.Controls.Add(this.splitsPanel);
			this.Controls.Add(this.splitDetailsLabel);
			this.Controls.Add(this.splitTypeLabel);
			this.Controls.Add(this.splitCountLabel);
			this.Controls.Add(this.clearSplitsButton);
			this.Controls.Add(this.addSplitButton);
			this.Name = "SoulsSplitCollectionControl";
			this.Size = new System.Drawing.Size(461, 490);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private SoulsLabel splitTypeLabel;
		private SoulsLabel splitDetailsLabel;
		private System.Windows.Forms.Button addSplitButton;
		private System.Windows.Forms.Button clearSplitsButton;
		private System.Windows.Forms.Panel splitsPanel;
		private System.Windows.Forms.Label splitCountLabel;
		private System.Windows.Forms.Panel splitShadow;
		private System.Windows.Forms.Label unfinishedSplitsLabel;
	}
}
