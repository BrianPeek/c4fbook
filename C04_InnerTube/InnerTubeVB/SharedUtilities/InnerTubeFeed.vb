Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Collections.ObjectModel
Imports SharedUtilities

Namespace SharedUtilities
	Public Class InnerTubeFeed
        Implements ICloneable

		Private privateFeedName As String
		Public Property FeedName() As String
			Get
				Return privateFeedName
			End Get
			Set(ByVal value As String)
				privateFeedName = value
			End Set
        End Property

		Private privateFeedUrl As String
		Public Property FeedUrl() As String
			Get
				Return privateFeedUrl
			End Get
			Set(ByVal value As String)
				privateFeedUrl = value
			End Set
        End Property

		Private privateLastUpdated As DateTime
		Public Property LastUpdated() As DateTime
			Get
				Return privateLastUpdated
			End Get
			Set(ByVal value As DateTime)
				privateLastUpdated = value
			End Set
        End Property

        Private privateFeedVideos As ObservableCollection(Of InnerTubeVideo)
		Public Property FeedVideos() As ObservableCollection(Of InnerTubeVideo)
			Get
				Return privateFeedVideos
			End Get
            Set(ByVal value As ObservableCollection(Of InnerTubeVideo))
                privateFeedVideos = value
            End Set
		End Property

		#Region "ICloneable Members"

		Public Function Clone() As InnerTubeFeed
			Dim copy As InnerTubeFeed = CType((CType(Me, ICloneable)).Clone(), InnerTubeFeed)
			Dim feedVideoCopy As ObservableCollection(Of InnerTubeVideo) = New ObservableCollection(Of InnerTubeVideo)()

			If Me.FeedVideos IsNot Nothing Then
				For Each v In Me.FeedVideos
					feedVideoCopy.Add(v.Clone())
				Next v

				copy.FeedVideos = feedVideoCopy
				Return copy
			Else
				copy.FeedVideos = New ObservableCollection(Of InnerTubeVideo)()
				Return copy
			End If

		End Function

		Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
			Return Me.MemberwiseClone()
		End Function

		#End Region

	End Class
End Namespace
