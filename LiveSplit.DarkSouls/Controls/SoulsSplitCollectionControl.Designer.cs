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
			this.SuspendLayout();
			// 
			// splitCollectionBox
			// 
			this.splitCollectionBox.Location = new System.Drawing.Point(4, 4);
			this.splitCollectionBox.Name = "splitCollectionBox";
			this.splitCollectionBox.Size = new System.Drawing.Size(466, 421);
			this.splitCollectionBox.TabIndex = 0;
			this.splitCollectionBox.TabStop = false;
			this.splitCollectionBox.Text = "Splits";
			// 
			// SoulsSplitCollectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitCollectionBox);
			this.Name = "SoulsSplitCollectionControl";
			this.Size = new System.Drawing.Size(577, 467);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox splitCollectionBox;
	}
}
