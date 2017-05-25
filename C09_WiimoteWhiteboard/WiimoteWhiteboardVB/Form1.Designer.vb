Imports Microsoft.VisualBasic
Imports System
Namespace WiimoteWhiteboard
	Partial Public Class Form1
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
			Me.pbBattery = New System.Windows.Forms.ProgressBar()
			Me.lblBattery = New System.Windows.Forms.Label()
			Me.groupBox4 = New System.Windows.Forms.GroupBox()
			Me.cbCursorControl = New System.Windows.Forms.CheckBox()
			Me.btnCalibrate = New System.Windows.Forms.Button()
			Me.lblIRVisible = New System.Windows.Forms.Label()
			Me.lblTrackingUtil = New System.Windows.Forms.Label()
			Me.groupBox4.SuspendLayout()
			Me.SuspendLayout()
			' 
			' pbBattery
			' 
			Me.pbBattery.Location = New System.Drawing.Point(8, 20)
			Me.pbBattery.Maximum = 200
			Me.pbBattery.Name = "pbBattery"
			Me.pbBattery.Size = New System.Drawing.Size(64, 23)
			Me.pbBattery.Step = 1
			Me.pbBattery.TabIndex = 6
			' 
			' lblBattery
			' 
			Me.lblBattery.AutoSize = True
			Me.lblBattery.Location = New System.Drawing.Point(79, 20)
			Me.lblBattery.Name = "lblBattery"
			Me.lblBattery.Size = New System.Drawing.Size(35, 13)
			Me.lblBattery.TabIndex = 9
			Me.lblBattery.Text = "label1"
			' 
			' groupBox4
			' 
			Me.groupBox4.Controls.Add(Me.pbBattery)
			Me.groupBox4.Controls.Add(Me.lblBattery)
			Me.groupBox4.Location = New System.Drawing.Point(10, 11)
			Me.groupBox4.Name = "groupBox4"
			Me.groupBox4.Size = New System.Drawing.Size(121, 46)
			Me.groupBox4.TabIndex = 21
			Me.groupBox4.TabStop = False
			Me.groupBox4.Text = "Wiimote Battery"
			' 
			' cbCursorControl
			' 
			Me.cbCursorControl.AutoSize = True
			Me.cbCursorControl.Location = New System.Drawing.Point(18, 115)
			Me.cbCursorControl.Margin = New System.Windows.Forms.Padding(2)
			Me.cbCursorControl.Name = "cbCursorControl"
			Me.cbCursorControl.Size = New System.Drawing.Size(92, 17)
			Me.cbCursorControl.TabIndex = 22
			Me.cbCursorControl.Text = "Cursor Control"
			Me.cbCursorControl.UseVisualStyleBackColor = True
'			Me.cbCursorControl.CheckedChanged += New System.EventHandler(Me.cbCursorControl_CheckedChanged);
			' 
			' btnCalibrate
			' 
			Me.btnCalibrate.Location = New System.Drawing.Point(9, 145)
			Me.btnCalibrate.Margin = New System.Windows.Forms.Padding(2)
			Me.btnCalibrate.Name = "btnCalibrate"
			Me.btnCalibrate.Size = New System.Drawing.Size(121, 52)
			Me.btnCalibrate.TabIndex = 24
			Me.btnCalibrate.Text = "Calibrate Location (Wiimote A)"
			Me.btnCalibrate.UseVisualStyleBackColor = True
'			Me.btnCalibrate.Click += New System.EventHandler(Me.btnCalibrate_Click);
			' 
			' lblIRVisible
			' 
			Me.lblIRVisible.AutoSize = True
			Me.lblIRVisible.Location = New System.Drawing.Point(10, 71)
			Me.lblIRVisible.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
			Me.lblIRVisible.Name = "lblIRVisible"
			Me.lblIRVisible.Size = New System.Drawing.Size(80, 13)
			Me.lblIRVisible.TabIndex = 25
			Me.lblIRVisible.Text = "Visible IR dots: "
			' 
			' lblTrackingUtil
			' 
			Me.lblTrackingUtil.AutoSize = True
			Me.lblTrackingUtil.Location = New System.Drawing.Point(10, 92)
			Me.lblTrackingUtil.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
			Me.lblTrackingUtil.Name = "lblTrackingUtil"
			Me.lblTrackingUtil.Size = New System.Drawing.Size(109, 13)
			Me.lblTrackingUtil.TabIndex = 26
			Me.lblTrackingUtil.Text = "Tracking Utilization: --"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(140, 209)
			Me.Controls.Add(Me.lblTrackingUtil)
			Me.Controls.Add(Me.lblIRVisible)
			Me.Controls.Add(Me.btnCalibrate)
			Me.Controls.Add(Me.cbCursorControl)
			Me.Controls.Add(Me.groupBox4)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
			Me.MaximizeBox = False
			Me.Name = "Form1"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "Wiimote Whiteboard"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
'			Me.FormClosed += New System.Windows.Forms.FormClosedEventHandler(Me.Form1_FormClosed);
			Me.groupBox4.ResumeLayout(False)
			Me.groupBox4.PerformLayout()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private pbBattery As System.Windows.Forms.ProgressBar
		Private lblBattery As System.Windows.Forms.Label
		Private groupBox4 As System.Windows.Forms.GroupBox
		Private WithEvents cbCursorControl As System.Windows.Forms.CheckBox
		Private WithEvents btnCalibrate As System.Windows.Forms.Button
		Private lblIRVisible As System.Windows.Forms.Label
		Private lblTrackingUtil As System.Windows.Forms.Label
	End Class
End Namespace

