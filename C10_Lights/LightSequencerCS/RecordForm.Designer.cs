namespace LightSequencer
{
	partial class RecordForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordForm));
			this.btnStart = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tmrCountdown = new System.Windows.Forms.Timer(this.components);
			this.lblCountdown = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radAppend = new System.Windows.Forms.RadioButton();
			this.radOverwrite = new System.Windows.Forms.RadioButton();
			this.chkPlay = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(163, 192);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "&Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(276, 88);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(55, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// tmrCountdown
			// 
			this.tmrCountdown.Interval = 1000;
			this.tmrCountdown.Tick += new System.EventHandler(this.tmrCountdown_Tick);
			// 
			// lblCountdown
			// 
			this.lblCountdown.AutoSize = true;
			this.lblCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCountdown.Location = new System.Drawing.Point(136, 164);
			this.lblCountdown.Name = "lblCountdown";
			this.lblCountdown.Size = new System.Drawing.Size(21, 24);
			this.lblCountdown.TabIndex = 2;
			this.lblCountdown.Text = "3";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radAppend);
			this.groupBox1.Controls.Add(this.radOverwrite);
			this.groupBox1.Location = new System.Drawing.Point(4, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(284, 40);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Channel Behavior";
			// 
			// radAppend
			// 
			this.radAppend.AutoSize = true;
			this.radAppend.Location = new System.Drawing.Point(140, 16);
			this.radAppend.Name = "radAppend";
			this.radAppend.Size = new System.Drawing.Size(127, 17);
			this.radAppend.TabIndex = 1;
			this.radAppend.Text = "&Append channel data";
			this.radAppend.UseVisualStyleBackColor = true;
			// 
			// radOverwrite
			// 
			this.radOverwrite.AutoSize = true;
			this.radOverwrite.Checked = true;
			this.radOverwrite.Location = new System.Drawing.Point(4, 16);
			this.radOverwrite.Name = "radOverwrite";
			this.radOverwrite.Size = new System.Drawing.Size(135, 17);
			this.radOverwrite.TabIndex = 0;
			this.radOverwrite.TabStop = true;
			this.radOverwrite.Text = "&Overwrite channel data";
			this.radOverwrite.UseVisualStyleBackColor = true;
			// 
			// chkPlay
			// 
			this.chkPlay.AutoSize = true;
			this.chkPlay.Location = new System.Drawing.Point(68, 140);
			this.chkPlay.Name = "chkPlay";
			this.chkPlay.Size = new System.Drawing.Size(155, 17);
			this.chkPlay.TabIndex = 3;
			this.chkPlay.Text = "&Play existing channel data?";
			this.chkPlay.UseVisualStyleBackColor = true;
			// 
			// RecordForm
			// 
			this.AcceptButton = this.btnStart;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 223);
			this.Controls.Add(this.chkPlay);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblCountdown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RecordForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Recording...";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RecordForm_KeyPress);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RecordForm_KeyUp);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecordForm_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RecordForm_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Timer tmrCountdown;
		private System.Windows.Forms.Label lblCountdown;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radAppend;
		private System.Windows.Forms.RadioButton radOverwrite;
		private System.Windows.Forms.CheckBox chkPlay;
	}
}