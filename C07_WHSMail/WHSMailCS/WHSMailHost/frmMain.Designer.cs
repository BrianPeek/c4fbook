namespace WHSMailHost
{
	partial class frmMain
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.niIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// niIcon
			// 
			this.niIcon.ContextMenuStrip = this.cmsMenu;
			this.niIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("niIcon.Icon")));
			this.niIcon.Text = "WHS Mail Host";
			this.niIcon.Visible = true;
			// 
			// cmsMenu
			// 
			this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExit});
			this.cmsMenu.Name = "cmsMenu";
			this.cmsMenu.Size = new System.Drawing.Size(153, 48);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(152, 22);
			this.mnuExit.Text = "&Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 264);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMain";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WHS Mail Host";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Resize += new System.EventHandler(this.frmMain_Resize);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.cmsMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon niIcon;
		private System.Windows.Forms.ContextMenuStrip cmsMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
	}
}

