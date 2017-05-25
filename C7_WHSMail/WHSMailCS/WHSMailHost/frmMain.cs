using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WHSMailHost
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			// exit the application
			this.Close();
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			// hide the form
			this.Hide();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			// start the WCF service
			MyServiceHost.StartService();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			// stop the WCF service
			MyServiceHost.StopService();
		}
	}
}