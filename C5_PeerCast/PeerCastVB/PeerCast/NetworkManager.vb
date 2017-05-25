Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows
Imports C4F.VistaP2P.Common

Namespace PeerCast
	Public Class NetworkManager
		#Region "Variables"
		Public P2pWorker As BackgroundWorker
		Private userName As String = "machine" & DateTime.Now.Ticks
		Private networkName As String
		Private password As String
		Private p2p As P2PLib
		Private videoHttpListener As StreamingHttpListener
		Private progressBarValue As Integer = 0
		Public CurrentState As SystemState = SystemState.LoggedOut
		#End Region

		Public Sub New()
			P2pWorker = BackgroundWorkerUtility.Create()
		End Sub

		Public Sub StartConnection(ByVal networkName As String, ByVal password As String)
			Me.networkName = networkName
			Me.password = password

			If ValidateResetConnection() Then
				'update the network state
				Me.CurrentState = SystemState.LoggingIn

				AddHandler P2pWorker.DoWork, AddressOf P2pWorkerAsync

				'start working
				P2pWorker.RunWorkerAsync()
			End If
		End Sub

		'p2p may not "die" when setting to null so need to check here        
		Private Function ValidateResetConnection() As Boolean
			If p2p IsNot Nothing Then
				MessageBox.Show("An Error occurred when trying to initialize the Network. Please restart the application")
				Return False
			End If
			Return True
		End Function

		Private Sub P2pWorkerAsync(ByVal sender As Object, ByVal doWorkArgs As DoWorkEventArgs)
			'Send back a text that we're logging in
			P2pWorker.ReportProgress(0, ThreadMessage.Create(UiMessage.ToggleUi, String.Empty))

			'Create new p2p
			p2p = New P2PLib()

			'Connect to the PeerChannel
			Dim start As Boolean = p2p.PeerChannelWrapperStart(userName, networkName, password)

			'check for connection error
			If (Not start) Then
				EndConnection()
			End If

			SubscribeToPeerChannelEvents()

			'PeerChannel connection may take a while so cycle the ProgressBar
			CycleProgressBarUntilConnected()

			SitAndWait()
		End Sub

		Private Sub SubscribeToPeerChannelEvents()
			'hook up the events we care about...text and status for the server
			AddHandler p2p.StatusChanged, AddressOf P2PLib_StatusChanged
			AddHandler p2p.TextPeerChannelHelper.ChatChanged, AddressOf P2PLib_ChatChanged

			'only setup the listener in client mode
			If (Not App.IsServerMode) Then
				AddHandler p2p.StreamedVideo.StreamChanged, AddressOf P2PLib_StreamChanged
			End If

			'required by server for start/send stream, used by client for StreamChanged event defined above)
            videoHttpListener = New StreamingHttpListener(userName & "/video/", AddressOf MediaFinished, AddressOf LogMessage, AddressOf p2p.StreamedVideo.StartStream, AddressOf p2p.StreamedVideo.SendStream)

		End Sub

		Private Sub CycleProgressBarUntilConnected()
			'spin while we log in and update the progress bar.
			Do While Me.CurrentState = SystemState.LoggingIn
				System.Threading.Thread.Sleep(100)
				If progressBarValue >= 100 Then
					progressBarValue = 0
				Else
					progressBarValue += 1
				End If

				'Send back to UI thread
				P2pWorker.ReportProgress(progressBarValue)
				If P2pWorker.CancellationPending Then
					P2pWorker.ReportProgress(0)
				End If
			Loop
			'when complete, report 100 as the progress
			P2pWorker.ReportProgress(100)
		End Sub

		Private Sub SitAndWait()
			'continuous loop to sit and wait for PeerChannel or form events
			Do
				System.Threading.Thread.Sleep(1000)
				If P2pWorker IsNot Nothing Then
					If P2pWorker.CancellationPending OrElse CurrentState = SystemState.LoggedOut Then
						P2pWorker.ReportProgress(0)
						Exit Do
					End If
				End If
			Loop
		End Sub

		Protected Sub P2PLib_StatusChanged(ByVal sender As Object, ByVal e As StatusChangedEventArgs)
            If userName = e.Member Then
                If e.NewNodeJoined Then
                    Me.CurrentState = SystemState.LoggedIn
                    P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.UpdateStatus, "Connected"))
                End If

                If e.NodeLeft Then
                    P2pWorker.ReportProgress(0, ThreadMessage.Create(UiMessage.UpdateStatus, "Disconnected..."))
                    Me.CurrentState = SystemState.LoggedOut
                End If
            End If
		End Sub



		#Region "Chat Methods"

		Public Sub SendChatMessage(ByVal message As ChatMessage)
			p2p.TextPeerChannelHelper.SendTextMessage(message.CommandType.ToString(), message.Message)
		End Sub

		Protected Sub P2PLib_ChatChanged(ByVal sender As Object, ByVal e As ChatChangedEventArgs)
			'Sends text to chat window
			P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.Log, e.Message))

			If e.Message.StartsWith("machine") Then
				Return 'exit
			End If

			Dim msg As ChatMessage = ChatMessage.P2PLibParse(e.Message)
			If App.IsServerMode Then
				ProcessServerCommand(msg)
			Else
				If msg.CommandType = ChatCommand.ReturnList Then
					ChatMessage.P2PLibParse(e.Message)
					P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.ReceivedVideoList, msg.Message))
				End If
			End If
		End Sub

		Private Sub ProcessServerCommand(ByVal msg As ChatMessage)
			Select Case msg.CommandType
				Case ChatCommand.GetList
					Dim list As String = Serializer.SerializeFileList()
					Me.SendChatMessage(ChatMessage.Create(ChatCommand.ReturnList, list))
				Case ChatCommand.StreamVideo
					If videoHttpListener IsNot Nothing Then
						'Cancel current stream if streaming
						If p2p.StreamedVideo.StreamedState = StreamedStateType.Communicating Then
							p2p.StreamedVideo.CancelSendingStream()
						End If
						'start streaming file back to the client
						videoHttpListener.SendStreamingData(msg.Message)
					End If
			End Select
		End Sub


		#End Region

		#Region "Video Streaming Methods"

		Private Sub P2PLib_StreamChanged(ByVal sender As Object, ByVal e As StreamedChangedEventArgs)
			'take packet from server & use videoHttpListener to write to localhost
			videoHttpListener.StreamedChanged(e)

			'set client to listen to stream 
			If e.StreamedPacket.packetNumber = 0 Then
				Dim file As String = StreamingHttpListener.SafeHttpString(e.StreamedPacket.fileName)
				Dim url As String = String.Format("http://localhost:8088/{0}/video/{1}", userName, file)
				P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.StreamVideo, url))
			End If
		End Sub

		Protected Sub MediaFinished(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
			'End connection on error
			If e.Error IsNot Nothing Then
				MessageBox.Show(String.Format("Error streaming this video:{0}", e.Error.Message))
				EndConnection()
			End If
		End Sub

		Public Sub LogMessage(ByVal msg As String)
            P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.Log, msg))
		End Sub
		#End Region

		Public Sub EndConnection()
			'make sure we're not calling this multiple times
			If Me.CurrentState <> SystemState.LoggedOut Then
				Me.CurrentState = SystemState.LoggedOut

				'reset UI after changing SystemState
				P2pWorker.ReportProgress(0, ThreadMessage.Create(UiMessage.ToggleUi, String.Empty))

				'close and set classes to null
				If p2p IsNot Nothing Then
					p2p.Close()
					p2p = Nothing
				End If

				If videoHttpListener IsNot Nothing Then
					videoHttpListener.Stop()
					videoHttpListener.Quit()
					videoHttpListener = Nothing
				End If

				If P2pWorker IsNot Nothing Then
					P2pWorker.CancelAsync()
				End If
			End If
		End Sub


	End Class
End Namespace
