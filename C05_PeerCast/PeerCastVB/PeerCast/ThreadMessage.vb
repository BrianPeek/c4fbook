Imports Microsoft.VisualBasic
Imports System
Namespace PeerCast
	Public Class ThreadMessage
		Private privateMessageType As UiMessage
		Public Property MessageType() As UiMessage
			Get
				Return privateMessageType
			End Get
			Set(ByVal value As UiMessage)
				privateMessageType = value
			End Set
		End Property
		Private privateMessage As String
		Public Property Message() As String
			Get
				Return privateMessage
			End Get
			Set(ByVal value As String)
				privateMessage = value
			End Set
		End Property

		Public Shared Function Create(ByVal messageType As UiMessage, ByVal message As String) As ThreadMessage
			Return New ThreadMessage() With {.MessageType = messageType, .Message = message}
		End Function
	End Class
End Namespace
