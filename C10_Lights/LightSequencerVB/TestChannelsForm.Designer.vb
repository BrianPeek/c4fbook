Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Partial Public Class TestChannelsForm
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
			Me.label1 = New System.Windows.Forms.Label()
			Me.btnOK = New System.Windows.Forms.Button()
			Me.chkAllOn = New System.Windows.Forms.CheckBox()
			Me.SuspendLayout()
			' 
			' label1
			' 
			Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
			Me.label1.Location = New System.Drawing.Point(8, 4)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(276, 56)
			Me.label1.TabIndex = 2
			Me.label1.Text = "Press the number keys on the keyboard to test the available channels."
			Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
			' 
			' btnOK
			' 
			Me.btnOK.Location = New System.Drawing.Point(109, 92)
			Me.btnOK.Name = "btnOK"
			Me.btnOK.Size = New System.Drawing.Size(75, 23)
			Me.btnOK.TabIndex = 0
			Me.btnOK.Text = "&OK"
			Me.btnOK.UseVisualStyleBackColor = True
'			Me.btnOK.Click += New System.EventHandler(Me.btnOK_Click);
			' 
			' chkAllOn
			' 
			Me.chkAllOn.AutoSize = True
			Me.chkAllOn.Location = New System.Drawing.Point(119, 68)
			Me.chkAllOn.Name = "chkAllOn"
			Me.chkAllOn.Size = New System.Drawing.Size(54, 17)
			Me.chkAllOn.TabIndex = 1
			Me.chkAllOn.Text = "&All On"
			Me.chkAllOn.UseVisualStyleBackColor = True
'			Me.chkAllOn.CheckedChanged += New System.EventHandler(Me.chkAllOn_CheckedChanged);
			' 
			' TestChannelsForm
			' 
			Me.AcceptButton = Me.btnOK
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(292, 122)
			Me.Controls.Add(Me.chkAllOn)
			Me.Controls.Add(Me.btnOK)
			Me.Controls.Add(Me.label1)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.KeyPreview = True
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "TestChannelsForm"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Test Channels"
'			Me.KeyUp += New System.Windows.Forms.KeyEventHandler(Me.TestChannelsForm_KeyUp);
'			Me.KeyDown += New System.Windows.Forms.KeyEventHandler(Me.TestChannelsForm_KeyDown);
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private label1 As System.Windows.Forms.Label
		Private WithEvents btnOK As System.Windows.Forms.Button
		Private WithEvents chkAllOn As System.Windows.Forms.CheckBox
	End Class
End Namespace