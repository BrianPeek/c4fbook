namespace LightSequencer
{
	partial class NewPlaylistForm
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
			this.lbSequences = new System.Windows.Forms.ListBox();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnPlay = new System.Windows.Forms.Button();
			this.chkRepeat = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrev = new System.Windows.Forms.Button();
			this.gbPlaylist = new System.Windows.Forms.GroupBox();
			this.btnRemove = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.gbPlaylist.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbSequences
			// 
			this.lbSequences.FormattingEnabled = true;
			this.lbSequences.Location = new System.Drawing.Point(12, 16);
			this.lbSequences.Name = "lbSequences";
			this.lbSequences.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbSequences.Size = new System.Drawing.Size(384, 264);
			this.lbSequences.TabIndex = 0;
			// 
			// btnDown
			// 
			this.btnDown.Font = new System.Drawing.Font("Marlett", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnDown.Location = new System.Drawing.Point(408, 132);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(75, 23);
			this.btnDown.TabIndex = 1;
			this.btnDown.Text = "6";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(12, 284);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(116, 23);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "&Add Sequence(s)...";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(236, 284);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(320, 284);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnUp
			// 
			this.btnUp.Font = new System.Drawing.Font("Marlett", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnUp.Location = new System.Drawing.Point(408, 100);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(75, 23);
			this.btnUp.TabIndex = 6;
			this.btnUp.Text = "5";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnPlay
			// 
			this.btnPlay.Location = new System.Drawing.Point(8, 24);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(75, 23);
			this.btnPlay.TabIndex = 7;
			this.btnPlay.Text = "&Play";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// chkRepeat
			// 
			this.chkRepeat.AutoSize = true;
			this.chkRepeat.Location = new System.Drawing.Point(296, 28);
			this.chkRepeat.Name = "chkRepeat";
			this.chkRepeat.Size = new System.Drawing.Size(61, 17);
			this.chkRepeat.TabIndex = 9;
			this.chkRepeat.Text = "Repeat";
			this.chkRepeat.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblStatus);
			this.groupBox1.Controls.Add(this.btnNext);
			this.groupBox1.Controls.Add(this.btnPrev);
			this.groupBox1.Controls.Add(this.chkRepeat);
			this.groupBox1.Controls.Add(this.btnPlay);
			this.groupBox1.Location = new System.Drawing.Point(8, 320);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(488, 72);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Play Control";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(12, 52);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(56, 13);
			this.lblStatus.TabIndex = 12;
			this.lblStatus.Text = "Stopped...";
			// 
			// btnNext
			// 
			this.btnNext.Enabled = false;
			this.btnNext.Location = new System.Drawing.Point(196, 24);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 11;
			this.btnNext.Text = "Nex&t";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrev
			// 
			this.btnPrev.Enabled = false;
			this.btnPrev.Location = new System.Drawing.Point(116, 24);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.Size = new System.Drawing.Size(75, 23);
			this.btnPrev.TabIndex = 10;
			this.btnPrev.Text = "Pre&v";
			this.btnPrev.UseVisualStyleBackColor = true;
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// gbPlaylist
			// 
			this.gbPlaylist.Controls.Add(this.btnRemove);
			this.gbPlaylist.Controls.Add(this.lbSequences);
			this.gbPlaylist.Controls.Add(this.btnDown);
			this.gbPlaylist.Controls.Add(this.btnUp);
			this.gbPlaylist.Controls.Add(this.btnAdd);
			this.gbPlaylist.Controls.Add(this.btnSave);
			this.gbPlaylist.Controls.Add(this.btnCancel);
			this.gbPlaylist.Location = new System.Drawing.Point(4, 4);
			this.gbPlaylist.Name = "gbPlaylist";
			this.gbPlaylist.Size = new System.Drawing.Size(492, 316);
			this.gbPlaylist.TabIndex = 11;
			this.gbPlaylist.TabStop = false;
			this.gbPlaylist.Text = "Playlist";
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(132, 284);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 7;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// NewPlaylistForm
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(507, 400);
			this.Controls.Add(this.gbPlaylist);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewPlaylistForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Playlist";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.gbPlaylist.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbSequences;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.CheckBox chkRepeat;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.GroupBox gbPlaylist;
		private System.Windows.Forms.Button btnRemove;
	}
}