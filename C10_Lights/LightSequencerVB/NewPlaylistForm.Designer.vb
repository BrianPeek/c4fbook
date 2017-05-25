Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Partial Public Class NewPlaylistForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.lbSequences = New System.Windows.Forms.ListBox()
			Me.btnDown = New System.Windows.Forms.Button()
			Me.btnAdd = New System.Windows.Forms.Button()
			Me.btnCancel = New System.Windows.Forms.Button()
			Me.btnSave = New System.Windows.Forms.Button()
			Me.btnUp = New System.Windows.Forms.Button()
			Me.btnPlay = New System.Windows.Forms.Button()
			Me.chkRepeat = New System.Windows.Forms.CheckBox()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.lblStatus = New System.Windows.Forms.Label()
			Me.btnNext = New System.Windows.Forms.Button()
			Me.btnPrev = New System.Windows.Forms.Button()
			Me.gbPlaylist = New System.Windows.Forms.GroupBox()
			Me.btnRemove = New System.Windows.Forms.Button()
			Me.groupBox1.SuspendLayout()
			Me.gbPlaylist.SuspendLayout()
			Me.SuspendLayout()
			' 
			' lbSequences
			' 
			Me.lbSequences.FormattingEnabled = True
			Me.lbSequences.Location = New System.Drawing.Point(12, 16)
			Me.lbSequences.Name = "lbSequences"
			Me.lbSequences.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
			Me.lbSequences.Size = New System.Drawing.Size(384, 264)
			Me.lbSequences.TabIndex = 0
			' 
			' btnDown
			' 
			Me.btnDown.Font = New System.Drawing.Font("Marlett", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte(2)))
			Me.btnDown.Location = New System.Drawing.Point(408, 132)
			Me.btnDown.Name = "btnDown"
			Me.btnDown.Size = New System.Drawing.Size(75, 23)
			Me.btnDown.TabIndex = 1
			Me.btnDown.Text = "6"
			Me.btnDown.UseVisualStyleBackColor = True
'			Me.btnDown.Click += New System.EventHandler(Me.btnDown_Click);
			' 
			' btnAdd
			' 
			Me.btnAdd.Location = New System.Drawing.Point(12, 284)
			Me.btnAdd.Name = "btnAdd"
			Me.btnAdd.Size = New System.Drawing.Size(116, 23)
			Me.btnAdd.TabIndex = 3
			Me.btnAdd.Text = "&Add Sequence(s)..."
			Me.btnAdd.UseVisualStyleBackColor = True
'			Me.btnAdd.Click += New System.EventHandler(Me.btnAdd_Click);
			' 
			' btnCancel
			' 
			Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.btnCancel.Location = New System.Drawing.Point(236, 284)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.Size = New System.Drawing.Size(75, 23)
			Me.btnCancel.TabIndex = 4
			Me.btnCancel.Text = "&Cancel"
			Me.btnCancel.UseVisualStyleBackColor = True
			' 
			' btnSave
			' 
			Me.btnSave.Location = New System.Drawing.Point(320, 284)
			Me.btnSave.Name = "btnSave"
			Me.btnSave.Size = New System.Drawing.Size(75, 23)
			Me.btnSave.TabIndex = 5
			Me.btnSave.Text = "&Save"
			Me.btnSave.UseVisualStyleBackColor = True
'			Me.btnSave.Click += New System.EventHandler(Me.btnSave_Click);
			' 
			' btnUp
			' 
			Me.btnUp.Font = New System.Drawing.Font("Marlett", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte(2)))
			Me.btnUp.Location = New System.Drawing.Point(408, 100)
			Me.btnUp.Name = "btnUp"
			Me.btnUp.Size = New System.Drawing.Size(75, 23)
			Me.btnUp.TabIndex = 6
			Me.btnUp.Text = "5"
			Me.btnUp.UseVisualStyleBackColor = True
'			Me.btnUp.Click += New System.EventHandler(Me.btnUp_Click);
			' 
			' btnPlay
			' 
			Me.btnPlay.Location = New System.Drawing.Point(8, 24)
			Me.btnPlay.Name = "btnPlay"
			Me.btnPlay.Size = New System.Drawing.Size(75, 23)
			Me.btnPlay.TabIndex = 7
			Me.btnPlay.Text = "&Play"
			Me.btnPlay.UseVisualStyleBackColor = True
'			Me.btnPlay.Click += New System.EventHandler(Me.btnPlay_Click);
			' 
			' chkRepeat
			' 
			Me.chkRepeat.AutoSize = True
			Me.chkRepeat.Location = New System.Drawing.Point(296, 28)
			Me.chkRepeat.Name = "chkRepeat"
			Me.chkRepeat.Size = New System.Drawing.Size(61, 17)
			Me.chkRepeat.TabIndex = 9
			Me.chkRepeat.Text = "Repeat"
			Me.chkRepeat.UseVisualStyleBackColor = True
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.lblStatus)
			Me.groupBox1.Controls.Add(Me.btnNext)
			Me.groupBox1.Controls.Add(Me.btnPrev)
			Me.groupBox1.Controls.Add(Me.chkRepeat)
			Me.groupBox1.Controls.Add(Me.btnPlay)
			Me.groupBox1.Location = New System.Drawing.Point(8, 320)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New System.Drawing.Size(488, 72)
			Me.groupBox1.TabIndex = 10
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Play Control"
			' 
			' lblStatus
			' 
			Me.lblStatus.AutoSize = True
			Me.lblStatus.Location = New System.Drawing.Point(12, 52)
			Me.lblStatus.Name = "lblStatus"
			Me.lblStatus.Size = New System.Drawing.Size(56, 13)
			Me.lblStatus.TabIndex = 12
			Me.lblStatus.Text = "Stopped..."
			' 
			' btnNext
			' 
			Me.btnNext.Enabled = False
			Me.btnNext.Location = New System.Drawing.Point(196, 24)
			Me.btnNext.Name = "btnNext"
			Me.btnNext.Size = New System.Drawing.Size(75, 23)
			Me.btnNext.TabIndex = 11
			Me.btnNext.Text = "Nex&t"
			Me.btnNext.UseVisualStyleBackColor = True
'			Me.btnNext.Click += New System.EventHandler(Me.btnNext_Click);
			' 
			' btnPrev
			' 
			Me.btnPrev.Enabled = False
			Me.btnPrev.Location = New System.Drawing.Point(116, 24)
			Me.btnPrev.Name = "btnPrev"
			Me.btnPrev.Size = New System.Drawing.Size(75, 23)
			Me.btnPrev.TabIndex = 10
			Me.btnPrev.Text = "Pre&v"
			Me.btnPrev.UseVisualStyleBackColor = True
'			Me.btnPrev.Click += New System.EventHandler(Me.btnPrev_Click);
			' 
			' gbPlaylist
			' 
			Me.gbPlaylist.Controls.Add(Me.btnRemove)
			Me.gbPlaylist.Controls.Add(Me.lbSequences)
			Me.gbPlaylist.Controls.Add(Me.btnDown)
			Me.gbPlaylist.Controls.Add(Me.btnUp)
			Me.gbPlaylist.Controls.Add(Me.btnAdd)
			Me.gbPlaylist.Controls.Add(Me.btnSave)
			Me.gbPlaylist.Controls.Add(Me.btnCancel)
			Me.gbPlaylist.Location = New System.Drawing.Point(4, 4)
			Me.gbPlaylist.Name = "gbPlaylist"
			Me.gbPlaylist.Size = New System.Drawing.Size(492, 316)
			Me.gbPlaylist.TabIndex = 11
			Me.gbPlaylist.TabStop = False
			Me.gbPlaylist.Text = "Playlist"
			' 
			' btnRemove
			' 
			Me.btnRemove.Location = New System.Drawing.Point(132, 284)
			Me.btnRemove.Name = "btnRemove"
			Me.btnRemove.Size = New System.Drawing.Size(75, 23)
			Me.btnRemove.TabIndex = 7
			Me.btnRemove.Text = "&Remove"
			Me.btnRemove.UseVisualStyleBackColor = True
'			Me.btnRemove.Click += New System.EventHandler(Me.btnRemove_Click);
			' 
			' NewPlaylistForm
			' 
			Me.AcceptButton = Me.btnSave
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.CancelButton = Me.btnCancel
			Me.ClientSize = New System.Drawing.Size(507, 400)
			Me.Controls.Add(Me.gbPlaylist)
			Me.Controls.Add(Me.groupBox1)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "NewPlaylistForm"
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Playlist"
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			Me.gbPlaylist.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private lbSequences As System.Windows.Forms.ListBox
		Private WithEvents btnDown As System.Windows.Forms.Button
		Private WithEvents btnAdd As System.Windows.Forms.Button
		Private btnCancel As System.Windows.Forms.Button
		Private WithEvents btnSave As System.Windows.Forms.Button
		Private WithEvents btnUp As System.Windows.Forms.Button
		Private WithEvents btnPlay As System.Windows.Forms.Button
		Private chkRepeat As System.Windows.Forms.CheckBox
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private WithEvents btnNext As System.Windows.Forms.Button
		Private WithEvents btnPrev As System.Windows.Forms.Button
		Private lblStatus As System.Windows.Forms.Label
		Private gbPlaylist As System.Windows.Forms.GroupBox
		Private WithEvents btnRemove As System.Windows.Forms.Button
	End Class
End Namespace