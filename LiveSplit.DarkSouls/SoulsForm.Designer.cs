﻿namespace LiveSplit.DarkSouls
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
			this.soulsSplitControl1 = new LiveSplit.DarkSouls.Controls.SoulsSplitControl();
			this.SuspendLayout();
			// 
			// soulsSplitControl1
			// 
			this.soulsSplitControl1.Location = new System.Drawing.Point(13, 13);
			this.soulsSplitControl1.Name = "soulsSplitControl1";
			this.soulsSplitControl1.Size = new System.Drawing.Size(535, 200);
			this.soulsSplitControl1.TabIndex = 0;
			// 
			// SoulsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.soulsSplitControl1);
			this.Name = "SoulsForm";
			this.Text = "SoulsForm";
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.SoulsSplitControl soulsSplitControl1;
	}
}