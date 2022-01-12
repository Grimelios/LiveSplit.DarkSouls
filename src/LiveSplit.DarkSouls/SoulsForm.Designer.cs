namespace LiveSplit.DarkSouls
{
	partial class SoulsForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.soulsMasterControl1 = new LiveSplit.DarkSouls.Controls.SoulsMasterControl();
			this.SuspendLayout();
			// 
			// soulsMasterControl1
			// 
			this.soulsMasterControl1.Location = new System.Drawing.Point(13, 13);
			this.soulsMasterControl1.Name = "soulsMasterControl1";
			this.soulsMasterControl1.Size = new System.Drawing.Size(554, 588);
			this.soulsMasterControl1.TabIndex = 0;
			// 
			// SoulsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(494, 547);
			this.Controls.Add(this.soulsMasterControl1);
			this.Name = "SoulsForm";
			this.Text = "SoulsForm";
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.SoulsMasterControl soulsMasterControl1;
	}
}