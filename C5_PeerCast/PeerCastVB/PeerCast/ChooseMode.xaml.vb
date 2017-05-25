Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls

Namespace PeerCast
	Partial Public Class ChooseMode
		Inherits Window
		Public Sub New()
			InitializeComponent()
			checkIfPathExists()
		End Sub

		Private Sub checkIfPathExists()
			If (Not Directory.Exists(My.Settings.Default.FileDirectory)) Then
				My.Settings.Default.FileDirectory = Environment.SpecialFolder.MyDocuments.ToString()
				My.Settings.Default.Save()
			End If
		End Sub

		Private Sub SetMode(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim button = CType(sender, Button)
			If button.Name = "Server" Then
				App.IsServerMode = True
			Else
				App.IsServerMode = False
			End If

			'open Main Window
			Dim w As New MainWindow()
			w.Show()

			'close current Window
			Me.Close()
		End Sub
	End Class
End Namespace
