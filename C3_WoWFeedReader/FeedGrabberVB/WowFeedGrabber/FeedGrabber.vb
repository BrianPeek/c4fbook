Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.IO
Imports System.Threading
Imports System.Lua
Imports System.Lua.Serialization
Imports Microsoft.Feeds.Interop

Namespace WowFeedGrabber
	Public Class FeedGrabber
		Private Const feedsVariableName As String = "FEED_READER_FEEDS"

		Private ReadOnly savedVariablesPath As String
		Private fileSystemWatcher As FileSystemWatcher
		Private feedsManager As IFeedsManager

		Public Sub New(ByVal savedVariablesPath As String)
			If savedVariablesPath Is Nothing Then
				Throw New ArgumentNullException("savedVariablesPath")
			End If

			Me.savedVariablesPath = savedVariablesPath

			InitializeFileSystemWatcher()
			InitializeFeedsManager()
		End Sub

		Private Sub InitializeFileSystemWatcher()
			fileSystemWatcher = New FileSystemWatcher(Path.GetDirectoryName(savedVariablesPath), Path.GetFileName(savedVariablesPath))
			fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName
			AddHandler fileSystemWatcher.Changed, AddressOf OnFileChangedOrDeleted
			AddHandler fileSystemWatcher.Deleted, AddressOf OnFileChangedOrDeleted
			fileSystemWatcher.EnableRaisingEvents = True
		End Sub

		Private Sub InitializeFeedsManager()
			feedsManager = New FeedsManagerClass()
		End Sub

		Private Sub OnFileChangedOrDeleted(ByVal sender As Object, ByVal e As FileSystemEventArgs)
			' Suspend change events
			fileSystemWatcher.EnableRaisingEvents = False

			Dim success As Boolean = False

			Do
				Try
					Me.SaveFeeds()
					success = True
				Catch e1 As IOException
					Thread.Sleep(5)
				End Try
			Loop While Not success

			' Resume change events
			fileSystemWatcher.EnableRaisingEvents = True
		End Sub

		Public Sub EnableBackgroundUpdates()
			feedsManager.BackgroundSync(FEEDS_BACKGROUNDSYNC_ACTION.FBSA_ENABLE)
		End Sub

		Public Sub SaveFeeds()
			Dim feeds() As Feed = Me.GetTopLevelFeeds()
			If feeds IsNot Nothing Then
                Using lw As LuaWriter = LuaWriter.Create(savedVariablesPath)
                    Dim luaSerializer = New LuaSerializer()

                    lw.WriteStartAssignment(feedsVariableName)
                    luaSerializer.Serialize(lw, feeds)
                    lw.WriteEndAssignment()
                End Using
			End If
		End Sub

		Private Function GetTopLevelFeeds() As Feed()
			Dim rootFolder = CType(feedsManager.RootFolder, IFeedFolder)
			Dim topLevelFeeds = CType(rootFolder.Feeds, IFeedsEnum)

			Dim feeds = New List(Of Feed)(topLevelFeeds.Count)

			For Each feed As IFeed In topLevelFeeds
				' If a feed is not yet downloaded, download now
				If feed.DownloadStatus <> FEEDS_DOWNLOAD_STATUS.FDS_DOWNLOADED Then
					feed.Download()
				End If

				' Skip feed if it could not be downloaded
				If feed.DownloadStatus <> FEEDS_DOWNLOAD_STATUS.FDS_DOWNLOADED Then
					Continue For
				End If

				feeds.Add(CreateFeed(feed))
			Next feed

			Return feeds.ToArray()
		End Function

		Private Shared Function CreateFeed(ByVal feed As IFeed) As Feed
			Dim feedItems = CType(feed.Items, IFeedsEnum)
			Return New Feed With {.Title = feed.Title, .Items = feedItems.OfType(Of IFeedItem)().Select(Function(feedItem) CreateFeedItem(feedItem)).ToArray()}
		End Function

		Private Shared Function CreateFeedItem(ByVal feedItem As IFeedItem) As FeedItem
			Return New FeedItem With {.Title = HtmlConvert.ToPlainText(feedItem.Title), .Content = HtmlConvert.ToPlainText(feedItem.Description)}
		End Function
	End Class
End Namespace