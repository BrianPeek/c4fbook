Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Partial Public Class NewSequenceForm
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
			Me.txtFile = New System.Windows.Forms.TextBox()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.chkHold = New System.Windows.Forms.CheckBox()
			Me.label3 = New System.Windows.Forms.Label()
			Me.txtSeconds = New System.Windows.Forms.TextBox()
			Me.txtMinutes = New System.Windows.Forms.TextBox()
			Me.label1 = New System.Windows.Forms.Label()
			Me.btnBrowse = New System.Windows.Forms.Button()
			Me.btnOK = New System.Windows.Forms.Button()
			Me.btnCancel = New System.Windows.Forms.Button()
			Me.groupBox2 = New System.Windows.Forms.GroupBox()
			Me.label4 = New System.Windows.Forms.Label()
			Me.dgvDevices = New System.Windows.Forms.DataGridView()
			Me.groupBox3 = New System.Windows.Forms.GroupBox()
			Me.txtTitle = New System.Windows.Forms.TextBox()
			Me.label2 = New System.Windows.Forms.Label()
			Me.txtArtist = New System.Windows.Forms.TextBox()
			Me.label8 = New System.Windows.Forms.Label()
			Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.colSerialNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.colChannels = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.colMIDIChannel = New System.Windows.Forms.DataGridViewComboBoxColumn()
			Me.colPhidgetOutput = New System.Windows.Forms.DataGridViewTextBoxColumn()
			Me.groupBox1.SuspendLayout()
			Me.groupBox2.SuspendLayout()
			CType(Me.dgvDevices, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.groupBox3.SuspendLayout()
			Me.SuspendLayout()
			' 
			' txtFile
			' 
			Me.txtFile.Location = New System.Drawing.Point(8, 16)
			Me.txtFile.Name = "txtFile"
			Me.txtFile.Size = New System.Drawing.Size(332, 20)
			Me.txtFile.TabIndex = 0
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.chkHold)
			Me.groupBox1.Controls.Add(Me.label3)
			Me.groupBox1.Controls.Add(Me.txtSeconds)
			Me.groupBox1.Controls.Add(Me.txtMinutes)
			Me.groupBox1.Controls.Add(Me.label1)
			Me.groupBox1.Controls.Add(Me.btnBrowse)
			Me.groupBox1.Controls.Add(Me.txtFile)
			Me.groupBox1.Location = New System.Drawing.Point(4, 4)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New System.Drawing.Size(444, 68)
			Me.groupBox1.TabIndex = 0
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Music File"
			' 
			' chkHold
			' 
			Me.chkHold.AutoSize = True
			Me.chkHold.Enabled = False
			Me.chkHold.Location = New System.Drawing.Point(200, 44)
			Me.chkHold.Name = "chkHold"
			Me.chkHold.Size = New System.Drawing.Size(83, 17)
			Me.chkHold.TabIndex = 5
			Me.chkHold.Text = "Hold notes?"
			Me.chkHold.UseVisualStyleBackColor = True
			' 
			' label3
			' 
			Me.label3.AutoSize = True
			Me.label3.Location = New System.Drawing.Point(120, 44)
			Me.label3.Name = "label3"
			Me.label3.Size = New System.Drawing.Size(10, 13)
			Me.label3.TabIndex = 3
			Me.label3.Text = ":"
			' 
			' txtSeconds
			' 
			Me.txtSeconds.Location = New System.Drawing.Point(132, 40)
			Me.txtSeconds.MaxLength = 2
			Me.txtSeconds.Name = "txtSeconds"
			Me.txtSeconds.Size = New System.Drawing.Size(28, 20)
			Me.txtSeconds.TabIndex = 3
			Me.txtSeconds.Text = "00"
'			Me.txtSeconds.TextChanged += New System.EventHandler(Me.txtSeconds_TextChanged);
			' 
			' txtMinutes
			' 
			Me.txtMinutes.Location = New System.Drawing.Point(92, 40)
			Me.txtMinutes.MaxLength = 2
			Me.txtMinutes.Name = "txtMinutes"
			Me.txtMinutes.Size = New System.Drawing.Size(28, 20)
			Me.txtMinutes.TabIndex = 2
			Me.txtMinutes.Text = "1"
			Me.txtMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
'			Me.txtMinutes.TextChanged += New System.EventHandler(Me.txtMinutes_TextChanged);
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Location = New System.Drawing.Point(8, 44)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(81, 13)
			Me.label1.TabIndex = 4
			Me.label1.Text = "Length (mm:ss):"
			' 
			' btnBrowse
			' 
			Me.btnBrowse.Location = New System.Drawing.Point(344, 16)
			Me.btnBrowse.Name = "btnBrowse"
			Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
			Me.btnBrowse.TabIndex = 1
			Me.btnBrowse.Text = "&Browse"
			Me.btnBrowse.UseVisualStyleBackColor = True
'			Me.btnBrowse.Click += New System.EventHandler(Me.btnBrowse_Click);
			' 
			' btnOK
			' 
			Me.btnOK.Location = New System.Drawing.Point(428, 440)
			Me.btnOK.Name = "btnOK"
			Me.btnOK.Size = New System.Drawing.Size(75, 23)
			Me.btnOK.TabIndex = 0
			Me.btnOK.Text = "&OK"
			Me.btnOK.UseVisualStyleBackColor = True
'			Me.btnOK.Click += New System.EventHandler(Me.btnOK_Click);
			' 
			' btnCancel
			' 
			Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.btnCancel.Location = New System.Drawing.Point(64, 440)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.Size = New System.Drawing.Size(75, 23)
			Me.btnCancel.TabIndex = 1
			Me.btnCancel.Text = "&Cancel"
			Me.btnCancel.UseVisualStyleBackColor = True
'			Me.btnCancel.Click += New System.EventHandler(Me.btnCancel_Click);
			' 
			' groupBox2
			' 
			Me.groupBox2.Controls.Add(Me.label4)
			Me.groupBox2.Controls.Add(Me.dgvDevices)
			Me.groupBox2.Location = New System.Drawing.Point(4, 140)
			Me.groupBox2.Name = "groupBox2"
			Me.groupBox2.Size = New System.Drawing.Size(568, 296)
			Me.groupBox2.TabIndex = 1
			Me.groupBox2.TabStop = False
			Me.groupBox2.Text = "Phidget Configuration"
			' 
			' label4
			' 
			Me.label4.AutoSize = True
			Me.label4.Location = New System.Drawing.Point(8, 24)
			Me.label4.Name = "label4"
			Me.label4.Size = New System.Drawing.Size(124, 13)
			Me.label4.TabIndex = 7
			Me.label4.Text = "Connected Interface Kits"
			' 
			' dgvDevices
			' 
			Me.dgvDevices.AllowUserToAddRows = False
			Me.dgvDevices.AllowUserToDeleteRows = False
			Me.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
			Me.dgvDevices.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() { Me.colName, Me.colSerialNumber, Me.colChannels, Me.colMIDIChannel, Me.colPhidgetOutput})
			Me.dgvDevices.Location = New System.Drawing.Point(8, 40)
			Me.dgvDevices.Name = "dgvDevices"
			Me.dgvDevices.RowHeadersVisible = False
			Me.dgvDevices.Size = New System.Drawing.Size(552, 252)
			Me.dgvDevices.TabIndex = 8
'			Me.dgvDevices.DataError += New System.Windows.Forms.DataGridViewDataErrorEventHandler(Me.dgvDevices_DataError);
			' 
			' groupBox3
			' 
			Me.groupBox3.Controls.Add(Me.txtTitle)
			Me.groupBox3.Controls.Add(Me.label2)
			Me.groupBox3.Controls.Add(Me.txtArtist)
			Me.groupBox3.Controls.Add(Me.label8)
			Me.groupBox3.Location = New System.Drawing.Point(4, 72)
			Me.groupBox3.Name = "groupBox3"
			Me.groupBox3.Size = New System.Drawing.Size(444, 68)
			Me.groupBox3.TabIndex = 2
			Me.groupBox3.TabStop = False
			Me.groupBox3.Text = "Music Info"
			' 
			' txtTitle
			' 
			Me.txtTitle.Location = New System.Drawing.Point(104, 16)
			Me.txtTitle.Name = "txtTitle"
			Me.txtTitle.Size = New System.Drawing.Size(332, 20)
			Me.txtTitle.TabIndex = 0
			' 
			' label2
			' 
			Me.label2.AutoSize = True
			Me.label2.Location = New System.Drawing.Point(8, 20)
			Me.label2.Name = "label2"
			Me.label2.Size = New System.Drawing.Size(27, 13)
			Me.label2.TabIndex = 0
			Me.label2.Text = "Title"
			' 
			' txtArtist
			' 
			Me.txtArtist.Location = New System.Drawing.Point(104, 40)
			Me.txtArtist.Name = "txtArtist"
			Me.txtArtist.Size = New System.Drawing.Size(332, 20)
			Me.txtArtist.TabIndex = 1
			' 
			' label8
			' 
			Me.label8.AutoSize = True
			Me.label8.Location = New System.Drawing.Point(8, 44)
			Me.label8.Name = "label8"
			Me.label8.Size = New System.Drawing.Size(30, 13)
			Me.label8.TabIndex = 0
			Me.label8.Text = "Artist"
			' 
			' colName
			' 
			Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
			Me.colName.HeaderText = "Device Name"
			Me.colName.Name = "colName"
			Me.colName.ReadOnly = True
			Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
			Me.colName.Width = 150
			' 
			' colSerialNumber
			' 
			Me.colSerialNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
			Me.colSerialNumber.HeaderText = "Serial Number"
			Me.colSerialNumber.Name = "colSerialNumber"
			Me.colSerialNumber.ReadOnly = True
			Me.colSerialNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
			Me.colSerialNumber.Width = 79
			' 
			' colChannels
			' 
			Me.colChannels.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
			Me.colChannels.HeaderText = "Channel"
			Me.colChannels.Name = "colChannels"
			Me.colChannels.ReadOnly = True
			Me.colChannels.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
			Me.colChannels.Width = 52
			' 
			' colMIDIChannel
			' 
			Me.colMIDIChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
			Me.colMIDIChannel.HeaderText = "MIDI Channel"
			Me.colMIDIChannel.Name = "colMIDIChannel"
			Me.colMIDIChannel.Resizable = System.Windows.Forms.DataGridViewTriState.True
			Me.colMIDIChannel.Visible = False
			Me.colMIDIChannel.Width = 150
			' 
			' colPhidgetOutput
			' 
			Me.colPhidgetOutput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
			Me.colPhidgetOutput.HeaderText = "Output Port"
			Me.colPhidgetOutput.Name = "colPhidgetOutput"
			Me.colPhidgetOutput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
			Me.colPhidgetOutput.Width = 67
			' 
			' NewSequenceForm
			' 
			Me.AcceptButton = Me.btnOK
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.CancelButton = Me.btnCancel
			Me.ClientSize = New System.Drawing.Size(578, 467)
			Me.Controls.Add(Me.groupBox3)
			Me.Controls.Add(Me.groupBox2)
			Me.Controls.Add(Me.btnCancel)
			Me.Controls.Add(Me.btnOK)
			Me.Controls.Add(Me.groupBox1)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "NewSequenceForm"
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "New Sequence"
'			Me.FormClosed += New System.Windows.Forms.FormClosedEventHandler(Me.NewSequenceForm_FormClosed);
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			Me.groupBox2.ResumeLayout(False)
			Me.groupBox2.PerformLayout()
			CType(Me.dgvDevices, System.ComponentModel.ISupportInitialize).EndInit()
			Me.groupBox3.ResumeLayout(False)
			Me.groupBox3.PerformLayout()
			Me.ResumeLayout(False)

		End Sub
		#End Region

		Private txtFile As System.Windows.Forms.TextBox
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private WithEvents btnBrowse As System.Windows.Forms.Button
		Private WithEvents btnOK As System.Windows.Forms.Button
		Private WithEvents btnCancel As System.Windows.Forms.Button
		Private label1 As System.Windows.Forms.Label
		Private groupBox2 As System.Windows.Forms.GroupBox
		Private WithEvents txtSeconds As System.Windows.Forms.TextBox
		Private WithEvents txtMinutes As System.Windows.Forms.TextBox
		Private label3 As System.Windows.Forms.Label
		Private label4 As System.Windows.Forms.Label
		Private WithEvents dgvDevices As System.Windows.Forms.DataGridView
		Private groupBox3 As System.Windows.Forms.GroupBox
		Private txtTitle As System.Windows.Forms.TextBox
		Private label2 As System.Windows.Forms.Label
		Private txtArtist As System.Windows.Forms.TextBox
		Private label8 As System.Windows.Forms.Label
		Private chkHold As System.Windows.Forms.CheckBox
		Private colName As System.Windows.Forms.DataGridViewTextBoxColumn
		Private colSerialNumber As System.Windows.Forms.DataGridViewTextBoxColumn
		Private colChannels As System.Windows.Forms.DataGridViewTextBoxColumn
		Private colMIDIChannel As System.Windows.Forms.DataGridViewComboBoxColumn
		Private colPhidgetOutput As System.Windows.Forms.DataGridViewTextBoxColumn
	End Class
End Namespace