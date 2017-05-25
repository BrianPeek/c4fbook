Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports Amib.Threading
Imports System.IO


Namespace SharedUtilities
	Public Enum WorkType
		UpdateFeeds 'just Update YouTube API feeds
		Download 'just download FLV files
		DownloadAndConvert 'download and convert FLV files
		Convert 'just convert FLV files
		All 'do all of the above
	End Enum


	Public Class InnerTubeFeedWorker
		Inherits BackgroundWorker
		#Region "Members"
		Private Shared worker As InnerTubeFeedWorker

		'Video Collections
		Private feedList As ObservableCollection(Of InnerTubeFeed)
		Private allVideos As List(Of InnerTubeVideo)

		'File Path
		Private settings As Setting

		'ThreadPools
		Private feedPool As SmartThreadPool
		Private downloadPool As SmartThreadPool
		Private convertPool As SmartThreadPool

		'ThreadPool results
		Private results As List(Of IWorkItemResult) = New List(Of IWorkItemResult)()
		#End Region

		Public Shared Function IsRunningAsync() As Boolean
			If worker Is Nothing Then
				Return False
			Else
				Return worker.IsBusy
			End If
		End Function

		Public Shared Function GetInstance(ByVal feedList As ObservableCollection(Of InnerTubeFeed), ByVal settings As Setting) As InnerTubeFeedWorker
			'first time creating it
			If worker Is Nothing Then
				worker = New InnerTubeFeedWorker(feedList, settings)
				Return worker
			Else
				'async operation happenning return async worker
				If worker.IsBusy Then
					Return worker
				Else
					'no async operation, reset worker
					worker = New InnerTubeFeedWorker(feedList, settings)
					Return worker
				End If
			End If
		End Function

		Private Shared Sub worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
			worker.Dispose()
		End Sub

		Private Sub New(ByVal feedList As ObservableCollection(Of InnerTubeFeed), ByVal settings As Setting)
			WorkerReportsProgress = True
			WorkerSupportsCancellation = True

			Me.feedList = feedList.ToObservableCollection()
			Me.settings = settings
			AddHandler RunWorkerCompleted, AddressOf worker_RunWorkerCompleted
		End Sub

		Protected Overrides Sub OnDoWork(ByVal e As DoWorkEventArgs)
			Dim work As WorkType = CType(e.Argument, WorkType)

			'Print that work starting
			WriteWorkerStartedMessages(work)

			'Check if we should just update feeds
			If work = WorkType.UpdateFeeds Then
				'Start Feed Update Work
				RunUpdateFeedPool()
			Else
				'otherwise only All should be updating feeds
				If work = WorkType.All Then
					RunUpdateFeedPool()
				End If

				allVideos = Utilities.Flatten(feedList)

				'Start Downloading Work
				RunDownloadPool(work)

				'Once downloads are complete, check if we need to do conversion work
				If work = WorkType.Convert OrElse work = WorkType.DownloadAndConvert OrElse work = WorkType.All Then
					RunConversionPool()
				End If

				'Once conversions are complete, check for sync work
				If work = WorkType.All Then
					WriteStatus("Starting sync")
					SyncToZuneAndiTunes()
					WriteStatus("Sync complete")
				End If

			End If

					WriteStatus("All Items Updated")

					'Update Items to ensure valid links
					e.Result = ValidateFeed.UpdateLinks(feedList, settings)

		End Sub

		Private Sub SyncToZuneAndiTunes()
			If settings.iTunesInstalled Then
				Dim iTunes As New iTunesSync()
				iTunes.Sync(settings.VideoPath)
			End If
			If settings.ZuneInstalled Then
				Dim zune As New ZuneSync()
				zune.Sync(settings.VideoPath)
			End If
		End Sub

		#Region "Pools"

		Private Sub RunUpdateFeedPool()
			results.Clear()
			feedPool = SetupSmartThreadPool(settings.UpdateFeedPoolThreads)

			'Add Items
			addFeedsToQueue()
			feedPool.Start()

			SmartThreadPool.WaitAll(results.ToArray())

			feedList.Clear()
			For Each r In results
				feedList.Add(CType(r.Result, InnerTubeFeed))
			Next r
			feedPool.Shutdown()

			WriteStatus("Update Feeds complete")
		End Sub

		Private Sub RunDownloadPool(ByVal work As WorkType)
			'Setup downloadPool
			downloadPool = SetupSmartThreadPool(settings.DownloadPoolThreads)

			'Select the items to add to the queue
			addDownloadTasksToQueue(work)
			downloadPool.Start()

			Dim success As Boolean = SmartThreadPool.WaitAll(results.ToArray())
			downloadPool.Shutdown()

			WriteStatus("Download Pool Completed, succeeded = " & success.ToString())
		End Sub

		Private Sub RunConversionPool()
			results.Clear()
			convertPool = SetupSmartThreadPool(settings.ConversionPoolThreads)
			addConversionsToQueue()
			convertPool.Start()
			Dim converted As Boolean = SmartThreadPool.WaitAll(results.ToArray())

			convertPool.Shutdown()

			WriteStatus("conversion Pool completed, success = " & converted.ToString())

		End Sub

		#End Region

		#Region "Queue Work Items"

		Private Sub addFeedsToQueue()
			For Each feed In Me.feedList
				Dim item As IWorkItemResult = feedPool.QueueWorkItem(AddressOf UpdateFeeds, feed)
				results.Add(item)
			Next feed
		End Sub

		Private Sub addDownloadTasksToQueue(ByVal work As WorkType)
			Select Case work
				Case WorkType.UpdateFeeds
					addImagesToQueue()
				Case WorkType.Download
					addVideosToQueue()
				Case WorkType.DownloadAndConvert
					addVideosToQueue()
				Case WorkType.All
					addImagesToQueue()
					addVideosToQueue()
				Case Else
			End Select
		End Sub

		Private Sub addImagesToQueue()

			'Queue image tasks
			For Each vid In Me.allVideos
				If (Not File.Exists(vid.DownloadedImage)) Then
					Dim item As IWorkItemResult = downloadPool.QueueWorkItem(AddressOf downloadImages, vid)
					results.Add(item)
				End If

			Next vid
		End Sub

		Private Sub addVideosToQueue()
			'////////Queue Downloading tasks
			'//////foreach (var feed in this.feedList)
			'//////{
				For Each vid In Me.allVideos
					'queue if converted files don't exist
					If (Not FileHelper.ConvertedFilesExist(vid, Me.settings)) Then
						Dim item As IWorkItemResult
						item = downloadPool.QueueWorkItem(AddressOf downloadVideo, vid)
						results.Add(item)
					End If
				Next vid

		End Sub

		Private Sub addConversionsToQueue()
			'loop through each video and add the downloadedFlv fullPath to the convertvideo task
			For Each feed In Me.feedList
				For Each vid In feed.FeedVideos
					'only add if video(s) don't already exist
					If (Not FileHelper.ConvertedFilesExist(vid,Me.settings)) Then
						Dim item As IWorkItemResult = convertPool.QueueWorkItem(AddressOf convertVideo, vid)
						results.Add(item)
					End If
				Next vid
			Next feed
		End Sub

		#End Region

		#Region "Async Tasks"
		Private Function UpdateFeeds(ByVal state As Object) As Object
			checkForCancellation()

			Dim feed As InnerTubeFeed = (CType(state, InnerTubeFeed)).Clone()
			Dim newVideos As ObservableCollection(Of InnerTubeVideo) = InnerTubeService.ConvertYouTubeXmlToObjects(New Uri(feed.FeedUrl),Me.settings)

			'pull just videos we don't have yet
			Dim newVids = From a In newVideos _
			              From b In (From n In newVideos _
			              Select n.Id).Except(From o In feed.FeedVideos Select o.Id) _
			              Where a.Id = b Select a

			For Each n In newVids
				feed.FeedVideos.Add(n)
			Next n

			Dim status As String = "updated feed: " & feed.FeedName
			WriteStatus(status)
			Return feed
		End Function

		Private Function downloadImages(ByVal state As Object) As Object
			checkForCancellation()

			'Convert to Feed Type
			Dim vid As InnerTubeVideo = CType(state, InnerTubeVideo)

			Download.DownloadImage(vid)


			Dim status As String
			If File.Exists(vid.DownloadedImage) Then
				status = String.Format("Image for Video: {0} SUCCESSFULLY downloaded to {1}",vid.Title, vid.DownloadedImage)
			Else
				status = String.Format("Image for Video: {0} FAILED to download to {1}", vid.Title, vid.DownloadedImage)
			End If


			WriteStatus(status)
			Return status
		End Function

		Private Function downloadVideo(ByVal state As Object) As Object
			'parse values    
			Dim vid As InnerTubeVideo = CType(state, InnerTubeVideo)

			WriteStatus("Starting download for: " & vid.Title)

			'Call download task, may take a few minutes to execute            
            Download.DownloadVideo(vid)

            WriteStatus("video downloaded to fullPath: " & vid.DownloadedFlv)
            Return vid.DownloadedFlv
		End Function

		Private Function convertVideo(ByVal state As Object) As Object
			checkForCancellation()
			Dim video As InnerTubeVideo = CType(state, InnerTubeVideo)

			WriteStatus("Starting conversion for file: " & video.Title)

			VideoConverter.ConvertFlv(video, ConversionType.Wmv)

			Dim status As String

			'only convert to MP4 if they've said to add to iTunes
			If settings.iTunesInstalled Then
				VideoConverter.ConvertFlv(video, ConversionType.Mp4)

				If File.Exists(video.DownloadedMp4) AndAlso (FileHelper.FileSize(video.DownloadedMp4) > 0) AndAlso File.Exists(video.DownloadedWmv) AndAlso (FileHelper.FileSize(video.DownloadedWmv) > 0) Then
					'Delete Flv

					If video.DownloadedFlv IsNot Nothing Then
						File.Delete(video.DownloadedFlv)
					End If

					status = String.Format("Conversion COMPLETE for: {0} at: {1} and {2}", video.Title, video.DownloadedMp4, video.DownloadedWmv)
				Else
					status = String.Format("Conversion FAILED for: {0} at: {1} and {2}", video.Title, video.DownloadedMp4, video.DownloadedWmv)
				End If

			Else
				If File.Exists(video.DownloadedWmv) AndAlso (FileHelper.FileSize(video.DownloadedWmv) > 0) Then
					'WMV created, delete the FLV
					File.Delete(video.DownloadedFlv)
					status = String.Format("Conversion COMPLETE for: {0} at: {1}", video.Title, video.DownloadedWmv)
				Else
					'WMV not created
					status = String.Format("Conversion FAILED for: {0} at: {1}", video.Title, video.DownloadedWmv)
				End If
			End If

			WriteStatus(status)
			Return status


		End Function

		#End Region        

		#Region "Helper Methods"

		Private Function SetupSmartThreadPool(ByVal threads As Integer) As SmartThreadPool
			Dim startInfo As New STPStartInfo()
			startInfo.StartSuspended = True
			startInfo.MaxWorkerThreads = threads
			startInfo.ThreadPriority = System.Threading.ThreadPriority.Normal
			Return New SmartThreadPool(startInfo)

		End Function

		Private Sub checkForCancellation()
			If Me.CancellationPending OrElse (Not Utilities.IsNetworkAvailable()) Then
				downloadPool.Cancel()
				downloadPool.Shutdown(True, New TimeSpan(0, 5, 0))
			End If
		End Sub

		Private Sub WriteStatus(ByVal status As String)
			Debug.WriteLine(status & Environment.NewLine)
			ReportProgress(50, status & Environment.NewLine)
		End Sub

		Private Sub WriteWorkerStartedMessages(ByVal t As WorkType)
			WriteStatus("BackgroundWorker Started with worktype: " & t.ToString())

			WriteStatus("Total Feeds: " & Me.feedList.Count)
			For Each feed In Me.feedList
				If feed.FeedVideos Is Nothing Then
					WriteStatus("Feed: " & feed.FeedName & " has zero videos")
				Else
					WriteStatus("Feed: " & feed.FeedName & " has " & feed.FeedVideos.Count & " video(s)")
				End If

			Next feed
		End Sub
		#End Region
	End Class
End Namespace
