Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace WHSMailHost
	Public Partial Class frmMain
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As EventArgs) _
			Handles mnuExit.Click
			' exit the application
			Me.Close()
		End Sub

		Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As EventArgs) _
			Handles MyBase.Resize
			' hide the form
			Me.Hide()
		End Sub

		Private Sub frmMain_Load(ByVal sender As Object, ByVal e As EventArgs) _
			Handles MyBase.Load
			' start the WCF service
			MyServiceHost.StartService()
		End Sub

		Private Sub frmMain_FormClosing(ByVal sender As Object, _
										ByVal e As FormClosingEventArgs) _
			Handles MyBase.FormClosing
			' stop the WCF service
			MyServiceHost.StopService()
		End Sub
	End Class
End Namespace