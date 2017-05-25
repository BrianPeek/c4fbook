Imports Microsoft.VisualBasic
Imports System
Namespace WiimoteWhiteboard
	Partial Public Class CalibrationForm
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
			Me.pbCalibrate = New System.Windows.Forms.PictureBox()
			CType(Me.pbCalibrate, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' pbCalibrate
			' 
			Me.pbCalibrate.Location = New System.Drawing.Point(66, 63)
			Me.pbCalibrate.Margin = New System.Windows.Forms.Padding(2)
			Me.pbCalibrate.Name = "pbCalibrate"
			Me.pbCalibrate.Size = New System.Drawing.Size(75, 41)
			Me.pbCalibrate.TabIndex = 0
			Me.pbCalibrate.TabStop = False
			' 
			' CalibrationForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(219, 211)
			Me.Controls.Add(Me.pbCalibrate)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
			Me.Margin = New System.Windows.Forms.Padding(2)
			Me.Name = "CalibrationForm"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
			Me.Text = "CalibrationForm"
'			Me.KeyDown += New System.Windows.Forms.KeyEventHandler(Me.CalibrationForm_KeyDown);
			CType(Me.pbCalibrate, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
		End Sub

		#End Region

		Private pbCalibrate As System.Windows.Forms.PictureBox
	End Class
End Namespace