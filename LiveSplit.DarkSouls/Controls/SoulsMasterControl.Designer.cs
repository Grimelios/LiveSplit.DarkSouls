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
			this.soulsSplitCollectionControl1 = new LiveSplit.DarkSouls.Controls.SoulsSplitCollectionControl();
			this.SuspendLayout();
			// 
			// soulsSplitCollectionControl1
			// 
			this.soulsSplitCollectionControl1.Location = new System.Drawing.Point(4, 4);
			this.soulsSplitCollectionControl1.Name = "soulsSplitCollectionControl1";
			this.soulsSplitCollectionControl1.Size = new System.Drawing.Size(577, 467);
			this.soulsSplitCollectionControl1.TabIndex = 0;
			// 
			// SoulsMasterControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.soulsSplitCollectionControl1);
			this.Name = "SoulsMasterControl";
			this.Size = new System.Drawing.Size(554, 588);
			this.ResumeLayout(false);

		}

		#endregion

		private SoulsSplitCollectionControl soulsSplitCollectionControl1;
	}
}
