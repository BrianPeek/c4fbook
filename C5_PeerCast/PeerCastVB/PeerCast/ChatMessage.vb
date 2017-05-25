Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace PeerCast
	Public Class ChatMessage
		Private privateCommandType As ChatCommand
		Public Property CommandType() As ChatCommand
			Get
				Return privateCommandType
			End Get
			Set(ByVal value As ChatCommand)
				privateCommandType = value
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

		Public Shared Function Create(ByVal command As ChatCommand, ByVal message As String) As ChatMessage
			Return New ChatMessage() With {.CommandType = command, .Message = message}
		End Function


		Public Shared Function P2PLibParse(ByVal text As String) As ChatMessage
			'ex:"StreamVideo said: C:\Users\Public\Bear.wmv"            

			'split the string based on spaces
			Dim msgParts = text.Split(" "c)

			'combine the first parts of the text
			Dim firstPart As String = msgParts(0) & " " & msgParts(1) & " "

			'now remove from the original text
			Dim message As String = text.Remove(0, firstPart.Length)

			'create a text and return it
			Dim msg As New ChatMessage()

			'create a ChatCommand enum
			msg.CommandType = CType(System.Enum.Parse(GetType(ChatCommand), msgParts(0)), ChatCommand)
			msg.Message = message
			Return msg
		End Function
	End Class


End Namespace
