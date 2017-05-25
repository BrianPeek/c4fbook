Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace SharedUtilities
	Public Class Setting

        Private privateAppName As String
		Public Property AppName() As String
			Get
				Return privateAppName
			End Get
			Set(ByVal value As String)
				privateAppName = value
			End Set
		End Property

        Private privateVideoPath As String
		Public Property VideoPath() As String
			Get
				Return privateVideoPath
			End Get
			Set(ByVal value As String)
				privateVideoPath = value
			End Set
		End Property

        Private privateSubPath As String
		Public Property SubPath() As String
			Get
				Return privateSubPath
			End Get
			Set(ByVal value As String)
				privateSubPath = value
			End Set
		End Property

        Private privateInnerTubeFeedFile As String
		Public Property InnerTubeFeedFile() As String
			Get
				Return privateInnerTubeFeedFile
			End Get
			Set(ByVal value As String)
				privateInnerTubeFeedFile = value
			End Set
		End Property

        Private privateFirstRun As Boolean
		Public Property FirstRun() As Boolean
			Get
				Return privateFirstRun
			End Get
			Set(ByVal value As Boolean)
				privateFirstRun = value
			End Set
		End Property

        Private privateZuneInstalled As Boolean
		Public Property ZuneInstalled() As Boolean
			Get
				Return privateZuneInstalled
			End Get
			Set(ByVal value As Boolean)
				privateZuneInstalled = value
			End Set
		End Property

        Private privateiTunesInstalled As Boolean
		Public Property iTunesInstalled() As Boolean
			Get
				Return privateiTunesInstalled
			End Get
			Set(ByVal value As Boolean)
				privateiTunesInstalled = value
			End Set
		End Property

        Private privateUpdateFeedPoolThreads As Integer
		Public Property UpdateFeedPoolThreads() As Integer
			Get
				Return privateUpdateFeedPoolThreads
			End Get
			Set(ByVal value As Integer)
				privateUpdateFeedPoolThreads = value
			End Set
		End Property

        Private privateDownloadPoolThreads As Integer
		Public Property DownloadPoolThreads() As Integer
			Get
				Return privateDownloadPoolThreads
			End Get
			Set(ByVal value As Integer)
				privateDownloadPoolThreads = value
			End Set
		End Property

        Private privateConversionPoolThreads As Integer
		Public Property ConversionPoolThreads() As Integer
			Get
				Return privateConversionPoolThreads
			End Get
			Set(ByVal value As Integer)
				privateConversionPoolThreads = value
			End Set
		End Property
	End Class
End Namespace
