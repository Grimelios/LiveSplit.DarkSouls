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
			this.splitCollectionBox = new System.Windows.Forms.GroupBox();
			this.clearSplitsButton = new System.Windows.Forms.Button();
			this.addSplitButton = new System.Windows.Forms.Button();
			this.splitDetailsLabel = new LiveSplit.DarkSouls.Controls.SoulsLabel();
			this.splitTypeLabel = new LiveSplit.DarkSouls.Controls.SoulsLabel();
			this.splitsPanel = new System.Windows.Forms.Panel();
			this.splitCollectionBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitCollectionBox
			// 
			this.splitCollectionBox.Controls.Add(this.splitsPanel);
			this.splitCollectionBox.Controls.Add(this.clearSplitsButton);
			this.splitCollectionBox.Controls.Add(this.addSplitButton);
			this.splitCollectionBox.Controls.Add(this.splitDetailsLabel);
			this.splitCollectionBox.Controls.Add(this.splitTypeLabel);
			this.splitCollectionBox.Location = new System.Drawing.Point(4, 4);
			this.splitCollectionBox.Name = "splitCollectionBox";
			this.splitCollectionBox.Size = new System.Drawing.Size(466, 421);
			this.splitCollectionBox.TabIndex = 0;
			this.splitCollectionBox.TabStop = false;
			this.splitCollectionBox.Text = "Splits";
			// 
			// clearSplitsButton
			// 
			this.clearSplitsButton.Image = global::LiveSplit.DarkSouls.Resources.Delete;
			this.clearSplitsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.clearSplitsButton.Location = new System.Drawing.Point(85, 19);
			this.clearSplitsButton.Name = "clearSplitsButton";
			this.clearSplitsButton.Size = new System.Drawing.Size(85, 24);
			this.clearSplitsButton.TabIndex = 5;
			this.clearSplitsButton.Text = "Clear Splits";
			this.clearSplitsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.clearSplitsButton.UseVisualStyleBackColor = true;
			this.clearSplitsButton.Click += new System.EventHandler(this.clearSplitsButton_Click);
			// 
			// addSplitButton
			// 
			this.addSplitButton.Image = global::LiveSplit.DarkSouls.Resources.Add;
			this.addSplitButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.addSplitButton.Location = new System.Drawing.Point(7, 20);
			this.addSplitButton.Name = "addSplitButton";
			this.addSplitButton.Size = new System.Drawing.Size(72, 23);
			this.addSplitButton.TabIndex = 4;
			this.addSplitButton.Text = "Add Split";
			this.addSplitButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.addSplitButton.UseVisualStyleBackColor = true;
			this.addSplitButton.Click += new System.EventHandler(this.addSplitButton_Click);
			// 
			// splitDetailsLabel
			// 
			this.splitDetailsLabel.Location = new System.Drawing.Point(96, 71);
			this.splitDetailsLabel.Name = "splitDetailsLabel";
			this.splitDetailsLabel.Size = new System.Drawing.Size(342, 13);
			this.splitDetailsLabel.TabIndex = 2;
			this.splitDetailsLabel.Text = "Details";
			// 
			// splitTypeLabel
			// 
			this.splitTypeLabel.Location = new System.Drawing.Point(11, 71);
			this.splitTypeLabel.Name = "splitTypeLabel";
			this.splitTypeLabel.Size = new System.Drawing.Size(81, 13);
			this.splitTypeLabel.TabIndex = 0;
			this.splitTypeLabel.Text = "Type";
			// 
			// splitsPanel
			// 
			this.splitsPanel.Location = new System.Drawing.Point(7, 88);
			this.splitsPanel.Name = "splitsPanel";
			this.splitsPanel.Size = new System.Drawing.Size(431, 327);
			this.splitsPanel.TabIndex = 6;
			// 
			// SoulsSplitCollectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitCollectionBox);
			this.Name = "SoulsSplitCollectionControl";
			this.Size = new System.Drawing.Size(577, 467);
			this.splitCollectionBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox splitCollectionBox;
		private SoulsLabel splitTypeLabel;
		private SoulsLabel splitDetailsLabel;
		private System.Windows.Forms.Button addSplitButton;
		private System.Windows.Forms.Button clearSplitsButton;
		private System.Windows.Forms.Panel splitsPanel;
	}
}
