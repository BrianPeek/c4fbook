namespace LightSequencer
{
	partial class NewSequenceForm
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
			this.txtFile = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkHold = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSeconds = new System.Windows.Forms.TextBox();
			this.txtMinutes = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.dgvDevices = new System.Windows.Forms.DataGridView();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtArtist = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colChannels = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colMIDIChannel = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colPhidgetOutput = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFile
			// 
			this.txtFile.Location = new System.Drawing.Point(8, 16);
			this.txtFile.Name = "txtFile";
			this.txtFile.Size = new System.Drawing.Size(332, 20);
			this.txtFile.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkHold);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtSeconds);
			this.groupBox1.Controls.Add(this.txtMinutes);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.txtFile);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(444, 68);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Music File";
			// 
			// chkHold
			// 
			this.chkHold.AutoSize = true;
			this.chkHold.Enabled = false;
			this.chkHold.Location = new System.Drawing.Point(200, 44);
			this.chkHold.Name = "chkHold";
			this.chkHold.Size = new System.Drawing.Size(83, 17);
			this.chkHold.TabIndex = 5;
			this.chkHold.Text = "Hold notes?";
			this.chkHold.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(120, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(10, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = ":";
			// 
			// txtSeconds
			// 
			this.txtSeconds.Location = new System.Drawing.Point(132, 40);
			this.txtSeconds.MaxLength = 2;
			this.txtSeconds.Name = "txtSeconds";
			this.txtSeconds.Size = new System.Drawing.Size(28, 20);
			this.txtSeconds.TabIndex = 3;
			this.txtSeconds.Text = "00";
			this.txtSeconds.TextChanged += new System.EventHandler(this.txtSeconds_TextChanged);
			// 
			// txtMinutes
			// 
			this.txtMinutes.Location = new System.Drawing.Point(92, 40);
			this.txtMinutes.MaxLength = 2;
			this.txtMinutes.Name = "txtMinutes";
			this.txtMinutes.Size = new System.Drawing.Size(28, 20);
			this.txtMinutes.TabIndex = 2;
			this.txtMinutes.Text = "1";
			this.txtMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtMinutes.TextChanged += new System.EventHandler(this.txtMinutes_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Length (mm:ss):";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(344, 16);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "&Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(428, 440);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(64, 440);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.dgvDevices);
			this.groupBox2.Location = new System.Drawing.Point(4, 140);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(568, 296);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Phidget Configuration";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(124, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Connected Interface Kits";
			// 
			// dgvDevices
			// 
			this.dgvDevices.AllowUserToAddRows = false;
			this.dgvDevices.AllowUserToDeleteRows = false;
			this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDevices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colSerialNumber,
            this.colChannels,
            this.colMIDIChannel,
            this.colPhidgetOutput});
			this.dgvDevices.Location = new System.Drawing.Point(8, 40);
			this.dgvDevices.Name = "dgvDevices";
			this.dgvDevices.RowHeadersVisible = false;
			this.dgvDevices.Size = new System.Drawing.Size(552, 252);
			this.dgvDevices.TabIndex = 8;
			this.dgvDevices.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDevices_DataError);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.txtTitle);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.txtArtist);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Location = new System.Drawing.Point(4, 72);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(444, 68);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Music Info";
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(104, 16);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(332, 20);
			this.txtTitle.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Title";
			// 
			// txtArtist
			// 
			this.txtArtist.Location = new System.Drawing.Point(104, 40);
			this.txtArtist.Name = "txtArtist";
			this.txtArtist.Size = new System.Drawing.Size(332, 20);
			this.txtArtist.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 44);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(30, 13);
			this.label8.TabIndex = 0;
			this.label8.Text = "Artist";
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colName.HeaderText = "Device Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colName.Width = 150;
			// 
			// colSerialNumber
			// 
			this.colSerialNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colSerialNumber.HeaderText = "Serial Number";
			this.colSerialNumber.Name = "colSerialNumber";
			this.colSerialNumber.ReadOnly = true;
			this.colSerialNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colSerialNumber.Width = 79;
			// 
			// colChannels
			// 
			this.colChannels.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colChannels.HeaderText = "Channel";
			this.colChannels.Name = "colChannels";
			this.colChannels.ReadOnly = true;
			this.colChannels.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colChannels.Width = 52;
			// 
			// colMIDIChannel
			// 
			this.colMIDIChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colMIDIChannel.HeaderText = "MIDI Channel";
			this.colMIDIChannel.Name = "colMIDIChannel";
			this.colMIDIChannel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colMIDIChannel.Visible = false;
			this.colMIDIChannel.Width = 150;
			// 
			// colPhidgetOutput
			// 
			this.colPhidgetOutput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colPhidgetOutput.HeaderText = "Output Port";
			this.colPhidgetOutput.Name = "colPhidgetOutput";
			this.colPhidgetOutput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colPhidgetOutput.Width = 67;
			// 
			// NewSequenceForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(578, 467);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewSequenceForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Sequence";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewSequenceForm_FormClosed);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtSeconds;
		private System.Windows.Forms.TextBox txtMinutes;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataGridView dgvDevices;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtArtist;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.CheckBox chkHold;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSerialNumber;
		private System.Windows.Forms.DataGridViewTextBoxColumn colChannels;
		private System.Windows.Forms.DataGridViewComboBoxColumn colMIDIChannel;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPhidgetOutput;
	}
}