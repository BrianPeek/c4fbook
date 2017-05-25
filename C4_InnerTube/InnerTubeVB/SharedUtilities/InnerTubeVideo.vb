Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.Collections.ObjectModel
Imports System.Reflection

Namespace SharedUtilities
	Public Class InnerTubeVideo
		Implements ICloneable, IComparable(Of InnerTubeVideo)

        Private privateId As String
		Public Property Id() As String
			Get
				Return privateId
			End Get
			Set(ByVal value As String)
				privateId = value
			End Set
		End Property

        Private privateIsProtected As Boolean
		Public Property IsProtected() As Boolean
			Get
				Return privateIsProtected
			End Get
			Set(ByVal value As Boolean)
				privateIsProtected = value
			End Set
		End Property

        Private privateAuthor As String
		Public Property Author() As String
			Get
				Return privateAuthor
			End Get
			Set(ByVal value As String)
				privateAuthor = value
			End Set
		End Property

        Private privateCategories As Collection(Of String)
		Public Property Categories() As Collection(Of String)
			Get
				Return privateCategories
			End Get
            Set(ByVal value As Collection(Of String))
                privateCategories = value
            End Set
		End Property

        Private privateThumbnailLink As String
		Public Property ThumbnailLink() As String
			Get
				Return privateThumbnailLink
			End Get
			Set(ByVal value As String)
				privateThumbnailLink = value
			End Set
		End Property

        Private privateLink As String
		Public Property Link() As String
			Get
				Return privateLink
			End Get
			Set(ByVal value As String)
				privateLink = value
			End Set
		End Property

        Private privateEmbedLink As String
		Public Property EmbedLink() As String
			Get
				Return privateEmbedLink
			End Get
			Set(ByVal value As String)
				privateEmbedLink = value
			End Set
		End Property

        Private privateDownloadLink As String
		Public Property DownloadLink() As String
			Get
				Return privateDownloadLink
			End Get
			Set(ByVal value As String)
				privateDownloadLink = value
			End Set
		End Property

        Private privateTitle As String
		Public Property Title() As String
			Get
				Return privateTitle
			End Get
			Set(ByVal value As String)
				privateTitle = value
			End Set
		End Property

        Private privateDescription As String
		Public Property Description() As String
			Get
				Return privateDescription
			End Get
			Set(ByVal value As String)
				privateDescription = value
			End Set
		End Property

        Private privateViews As Integer
		Public Property Views() As Integer
			Get
				Return privateViews
			End Get
			Set(ByVal value As Integer)
				privateViews = value
			End Set
		End Property

        Private privateNumRaters As Integer
		Public Property NumRaters() As Integer
			Get
				Return privateNumRaters
			End Get
			Set(ByVal value As Integer)
				privateNumRaters = value
			End Set
		End Property

        Private privateAvgRating As Single
		Public Property AvgRating() As Single
			Get
				Return privateAvgRating
			End Get
			Set(ByVal value As Single)
				privateAvgRating = value
			End Set
		End Property

        Private privateComments As Integer
		Public Property Comments() As Integer
			Get
				Return privateComments
			End Get
			Set(ByVal value As Integer)
				privateComments = value
			End Set
		End Property

        Private privateDuration As Integer
		Public Property Duration() As Integer
			Get
				Return privateDuration
			End Get
			Set(ByVal value As Integer)
				privateDuration = value
			End Set
		End Property

        Private privatePublished As DateTime
		Public Property Published() As DateTime
			Get
				Return privatePublished
			End Get
			Set(ByVal value As DateTime)
				privatePublished = value
			End Set
		End Property

        Private privateUpdated As DateTime
		Public Property Updated() As DateTime
			Get
				Return privateUpdated
			End Get
			Set(ByVal value As DateTime)
				privateUpdated = value
			End Set
		End Property

        Private privatePath As String
		Public Property Path() As String
			Get
				Return privatePath
			End Get
			Set(ByVal value As String)
				privatePath = value
			End Set
		End Property

		Private privateDownloadTime As DateTime
		Public Property DownloadTime() As DateTime
			Get
				Return privateDownloadTime
			End Get
			Set(ByVal value As DateTime)
				privateDownloadTime = value
			End Set
		End Property

        Private privateDownloadedFlv As String
		Public Property DownloadedFlv() As String
			Get
				Return privateDownloadedFlv
			End Get
			Set(ByVal value As String)
				privateDownloadedFlv = value
			End Set
		End Property

        Private privateDownloadedWmv As String
		Public Property DownloadedWmv() As String
			Get
				Return privateDownloadedWmv
			End Get
			Set(ByVal value As String)
				privateDownloadedWmv = value
			End Set
		End Property

        Private privateDownloadedMp4 As String
		Public Property DownloadedMp4() As String
			Get
				Return privateDownloadedMp4
			End Get
			Set(ByVal value As String)
				privateDownloadedMp4 = value
			End Set
		End Property

        Private privateDownloadedImage As String
		Public Property DownloadedImage() As String
			Get
				Return privateDownloadedImage
			End Get
			Set(ByVal value As String)
				privateDownloadedImage = value
			End Set
		End Property

        Private privateIsConverted As Boolean
		Public Property IsConverted() As Boolean
			Get
				Return privateIsConverted
			End Get
			Set(ByVal value As Boolean)
				privateIsConverted = value
			End Set
		End Property

        Private privateIsDownloaded As Boolean
		Public Property IsDownloaded() As Boolean
			Get
				Return privateIsDownloaded
			End Get
			Set(ByVal value As Boolean)
				privateIsDownloaded = value
			End Set
		End Property

		Public Overrides Function ToString() As String
			Dim sb As New StringBuilder()
			sb.Append("ID: " & Me.Id & Environment.NewLine)
			sb.Append("Author: " & Me.Author & Environment.NewLine)
			sb.Append("Title: " & Me.Title & Environment.NewLine)
			sb.Append("Download Link: " & Me.DownloadLink & Environment.NewLine)
			Return sb.ToString()
		End Function

		#Region "ICloneable Members"

		Public Function Clone() As InnerTubeVideo
			Dim copy As InnerTubeVideo = CType((CType(Me, ICloneable)).Clone(), InnerTubeVideo)
			Dim categoryCopy As Collection(Of String) = New Collection(Of String)()
			For Each c In Me.Categories
				categoryCopy.Add(CStr(c.Clone()))
			Next c
			copy.Categories = categoryCopy
			Return copy
		End Function


		Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
			Return Me.MemberwiseClone()
		End Function

		#End Region

		#Region "IComparable<InnerTubeVideo> Members"

		Public Function CompareTo(ByVal other As InnerTubeVideo) As Integer Implements IComparable(Of InnerTubeVideo).CompareTo
			Return Me.Id.CompareTo(other.Id)
		End Function

		#End Region

    End Class
End Namespace
