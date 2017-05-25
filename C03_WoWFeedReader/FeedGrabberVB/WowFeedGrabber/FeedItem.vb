Imports Microsoft.VisualBasic
Imports System
Namespace WowFeedGrabber
	Public Class FeedItem
		Private privateTitle As String
		Public Property Title() As String
			Get
				Return privateTitle
			End Get
			Set(ByVal value As String)
				privateTitle = value
			End Set
		End Property
		Private privateContent As String
		Public Property Content() As String
			Get
				Return privateContent
			End Get
			Set(ByVal value As String)
				privateContent = value
			End Set
		End Property
	End Class
End Namespace
