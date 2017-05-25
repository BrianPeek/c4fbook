Imports Microsoft.VisualBasic
Imports System
Namespace WowFeedGrabber
	Public Class Feed
		Private privateTitle As String
		Public Property Title() As String
			Get
				Return privateTitle
			End Get
			Set(ByVal value As String)
				privateTitle = value
			End Set
		End Property
		Private privateItems As FeedItem()
		Public Property Items() As FeedItem()
			Get
				Return privateItems
			End Get
			Set(ByVal value As FeedItem())
				privateItems = value
			End Set
		End Property
	End Class
End Namespace
