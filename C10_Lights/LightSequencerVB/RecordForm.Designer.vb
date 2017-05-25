Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Partial Public Class RecordForm
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
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(RecordForm))
			Me.btnStart = New System.Windows.Forms.Button()
			Me.label1 = New System.Windows.Forms.Label()
			Me.btnCancel = New System.Windows.Forms.Button()
			Me.tmrCountdown = New System.Windows.Forms.Timer(Me.components)
			Me.lblCountdown = New System.Windows.Forms.Label()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.radAppend = New System.Windows.Forms.RadioButton()
			Me.radOverwrite = New System.Windows.Forms.RadioButton()
			Me.chkPlay = New System.Windows.Forms.CheckBox()
			Me.groupBox1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' btnStart
			' 
			Me.btnStart.Location = New System.Drawing.Point(163, 192)
			Me.btnStart.Name = "btnStart"
			Me.btnStart.Size = New System.Drawing.Size(75, 23)
			Me.btnStart.TabIndex = 0
			Me.btnStart.Text = "&Start"
			Me.btnStart.UseVisualStyleBackColor = True
'			Me.btnStart.Click += New System.EventHandler(Me.btnStart_Click);
			' 
			' label1
			' 
			Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
			Me.label1.Location = New System.Drawing.Point(8, 4)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(276, 88)
			Me.label1.TabIndex = 0
			Me.label1.Text = resources.GetString("label1.Text")
			Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
			' 
			' btnCancel
			' 
			Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.btnCancel.Location = New System.Drawing.Point(55, 192)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.Size = New System.Drawing.Size(75, 23)
			Me.btnCancel.TabIndex = 1
			Me.btnCancel.Text = "Cancel"
			Me.btnCancel.UseVisualStyleBackColor = True
'			Me.btnCancel.Click += New System.EventHandler(Me.btnCancel_Click);
			' 
			' tmrCountdown
			' 
			Me.tmrCountdown.Interval = 1000
'			Me.tmrCountdown.Tick += New System.EventHandler(Me.tmrCountdown_Tick);
			' 
			' lblCountdown
			' 
			Me.lblCountdown.AutoSize = True
			Me.lblCountdown.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte(0)))
			Me.lblCountdown.Location = New System.Drawing.Point(136, 164)
			Me.lblCountdown.Name = "lblCountdown"
			Me.lblCountdown.Size = New System.Drawing.Size(21, 24)
			Me.lblCountdown.TabIndex = 2
			Me.lblCountdown.Text = "3"
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.radAppend)
			Me.groupBox1.Controls.Add(Me.radOverwrite)
			Me.groupBox1.Location = New System.Drawing.Point(4, 96)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New System.Drawing.Size(284, 40)
			Me.groupBox1.TabIndex = 2
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Channel Behavior"
			' 
			' radAppend
			' 
			Me.radAppend.AutoSize = True
			Me.radAppend.Location = New System.Drawing.Point(140, 16)
			Me.radAppend.Name = "radAppend"
			Me.radAppend.Size = New System.Drawing.Size(127, 17)
			Me.radAppend.TabIndex = 1
			Me.radAppend.Text = "&Append channel data"
			Me.radAppend.UseVisualStyleBackColor = True
			' 
			' radOverwrite
			' 
			Me.radOverwrite.AutoSize = True
			Me.radOverwrite.Checked = True
			Me.radOverwrite.Location = New System.Drawing.Point(4, 16)
			Me.radOverwrite.Name = "radOverwrite"
			Me.radOverwrite.Size = New System.Drawing.Size(135, 17)
			Me.radOverwrite.TabIndex = 0
			Me.radOverwrite.TabStop = True
			Me.radOverwrite.Text = "&Overwrite channel data"
			Me.radOverwrite.UseVisualStyleBackColor = True
			' 
			' chkPlay
			' 
			Me.chkPlay.AutoSize = True
			Me.chkPlay.Location = New System.Drawing.Point(68, 140)
			Me.chkPlay.Name = "chkPlay"
			Me.chkPlay.Size = New System.Drawing.Size(155, 17)
			Me.chkPlay.TabIndex = 3
			Me.chkPlay.Text = "&Play existing channel data?"
			Me.chkPlay.UseVisualStyleBackColor = True
			' 
			' RecordForm
			' 
			Me.AcceptButton = Me.btnStart
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(292, 223)
			Me.Controls.Add(Me.chkPlay)
			Me.Controls.Add(Me.groupBox1)
			Me.Controls.Add(Me.lblCountdown)
			Me.Controls.Add(Me.label1)
			Me.Controls.Add(Me.btnCancel)
			Me.Controls.Add(Me.btnStart)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.KeyPreview = True
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "RecordForm"
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Recording..."
'			Me.KeyPress += New System.Windows.Forms.KeyPressEventHandler(Me.RecordForm_KeyPress);
'			Me.KeyUp += New System.Windows.Forms.KeyEventHandler(Me.RecordForm_KeyUp);
'			Me.FormClosing += New System.Windows.Forms.FormClosingEventHandler(Me.RecordForm_FormClosing);
'			Me.KeyDown += New System.Windows.Forms.KeyEventHandler(Me.RecordForm_KeyDown);
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private WithEvents btnStart As System.Windows.Forms.Button
		Private label1 As System.Windows.Forms.Label
		Private WithEvents btnCancel As System.Windows.Forms.Button
		Private WithEvents tmrCountdown As System.Windows.Forms.Timer
		Private lblCountdown As System.Windows.Forms.Label
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private radAppend As System.Windows.Forms.RadioButton
		Private radOverwrite As System.Windows.Forms.RadioButton
		Private chkPlay As System.Windows.Forms.CheckBox
	End Class
End Namespace