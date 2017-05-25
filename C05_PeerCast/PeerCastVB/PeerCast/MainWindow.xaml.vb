Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Windows
Imports C4F.VistaP2P.Common



Namespace PeerCast

	Partial Public Class MainWindow
		Inherits Window
		#Region "Properties"
		'net is the wrapper for all async/P2P work
		Private net As New NetworkManager()
		Private fullWindow As FullScreen
		#End Region

		Public Sub New()
			InitializeComponent()
			FirstLoad()
		End Sub

		Private Sub FirstLoad()
			'Set Text
			If App.IsServerMode Then
				AppModeLabel.Content = "Server Mode"
				FilePath.Content = My.Settings.Default.FileDirectory
				ClientControls.Visibility = Visibility.Hidden
			Else
				AppModeLabel.Content = "Client Mode"
				ServerControls.Visibility = Visibility.Hidden
			End If
			'Setup Event handlers as all messages from NetworkManager comes through here
			AddHandler net.P2pWorker.ProgressChanged, AddressOf P2PWorker_ProgressChanged
			AddHandler net.P2pWorker.RunWorkerCompleted, AddressOf P2PBackgroundWorkerCompleted
		End Sub


		'Sign In Button starts Connection
		Private Sub SignInButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			If net.CurrentState = SystemState.LoggedOut Then
				If ValidateFields() Then
					net.StartConnection(NetworkName.Text, Password.Text)
				End If
			Else
				net.EndConnection()
			End If
		End Sub

		Private Function ValidateFields() As Boolean
			If String.IsNullOrEmpty(NetworkName.Text) AndAlso NetworkName.Text.Trim().Contains(" "c) Then
				MessageBox.Show("Please Enter a non-null NetworkName that contains only alpha-numeric characters")
				Return False
			End If
			Return True
		End Function

		#Region "Network Manager Commands"

		Private Sub UpdateVideoList(ByVal serializedList As String)
			Dim videos As List(Of String) = Serializer.DeserializeFileList(serializedList)
			'clear list
			MediaList.Items.Clear()

			'add each item
			For Each s As String In videos
				MediaList.Items.Add(s)
			Next s
		End Sub

		Public Sub PlayVideo(ByVal url As String)
			If fullWindow Is Nothing Then
				MediaPlayer.Source = New Uri(url)
				MediaPlayer.Play()
			Else
				fullWindow.SetMediaPlayer(New Uri(url))
				fullWindow.Show()
			End If
		End Sub

		Private Sub ToggleUi()
			If net.CurrentState = SystemState.LoggedIn OrElse net.CurrentState = SystemState.LoggingIn Then
				SignInButton.Content = "Sign Out"
				StatusValue.Content = "Signing In..."
				GetList.IsEnabled = True
				Play.IsEnabled = True
				FullScreen.IsEnabled = True
				MediaList.IsEnabled = True
			Else
				SignInButton.Content = "Sign In"
				StatusValue.Content = "Disconnected"
				GetList.IsEnabled = False
				Play.IsEnabled = False
				FullScreen.IsEnabled = False
				MediaList.IsEnabled = False
			End If
		End Sub

		#End Region

		#Region "P2P Background Worker Events"
		Private Sub P2PWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
			'update progress bar
			progressBar1.Value = e.ProgressPercentage

			'Read messages from the Network Manager
			If e.UserState IsNot Nothing Then
				Dim message As ThreadMessage = CType(e.UserState, ThreadMessage)
				Select Case message.MessageType
					Case UiMessage.UpdateStatus
						StatusValue.Content = message.Message
					Case UiMessage.Log
						Messages.Items.Add(message.Message)
					Case UiMessage.StreamVideo
						PlayVideo(message.Message)
					Case UiMessage.ReceivedVideoList
						UpdateVideoList(message.Message)
					Case UiMessage.ToggleUi
						ToggleUi()
				End Select
			End If
		End Sub

		Protected Sub P2PBackgroundWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
			'error occured
			If e.Error IsNot Nothing Then
				MessageBox.Show(String.Format("An error occurred in the background thread: {0}", e.Error.Message))
			Else
				net.EndConnection()
			End If
		End Sub
		#End Region

		#Region "Client Only Buttons"

		Private Sub GetList_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			'make a request to get the server's video list
			net.SendChatMessage(ChatMessage.Create(ChatCommand.GetList, String.Empty))
		End Sub

		Private Sub Play_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			'make a request to play a video
			net.SendChatMessage(ChatMessage.Create(ChatCommand.StreamVideo, CStr(MediaList.SelectedItem)))
		End Sub

		Private Sub FullScreen_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			'open a new window full screen
			If fullWindow Is Nothing Then
				fullWindow = New FullScreen()
				AddHandler fullWindow.Closing, AddressOf fullWindow_Closing
			End If
			net.SendChatMessage(ChatMessage.Create(ChatCommand.StreamVideo, CStr(MediaList.SelectedItem)))
		End Sub

		Private Sub fullWindow_Closing(ByVal sender As Object, ByVal e As CancelEventArgs)
			'assign to null so that we won't try to play a video with a Closed window which throws an exception
			fullWindow = Nothing
		End Sub

		#End Region    

		#Region "Server Only Button"
		Private Sub OpenDialog_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim folder = New System.Windows.Forms.FolderBrowserDialog()
			folder.RootFolder = Environment.SpecialFolder.MyComputer

			Dim r As System.Windows.Forms.DialogResult = folder.ShowDialog()
			If r = System.Windows.Forms.DialogResult.OK Then
				My.Settings.Default.FileDirectory = folder.SelectedPath
				My.Settings.Default.Save()
				FilePath.Content = folder.SelectedPath
			End If
		End Sub
		#End Region

		#Region "Other Form Events"
		Protected Overrides Sub OnClosed(ByVal e As EventArgs)
			MyBase.OnClosed(e)

			If net.CurrentState = SystemState.LoggingIn OrElse net.CurrentState = SystemState.LoggedIn Then
				net.EndConnection()
			End If
		End Sub

		Private Sub ClearMessages_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Messages.Items.Clear()
		End Sub
		#End Region


	End Class


End Namespace
