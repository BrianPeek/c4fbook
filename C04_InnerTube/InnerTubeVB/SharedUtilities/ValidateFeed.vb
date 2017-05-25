Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Collections.ObjectModel
Imports System.IO

Namespace SharedUtilities
	Public NotInheritable Class ValidateFeed
		Private Sub New()
		End Sub
		Public Shared Function UpdateLinks(ByVal feeds As ObservableCollection(Of InnerTubeFeed), ByVal settings As Setting) As ObservableCollection(Of InnerTubeFeed)
			For i As Integer = 0 To feeds.Count - 1
				For j As Integer = 0 To feeds(i).FeedVideos.Count - 1
					'Update download                    
					If File.Exists(feeds(i).FeedVideos(j).DownloadedFlv) Then
						feeds(i).FeedVideos(j).IsDownloaded = True
					End If

					'Update Converted Videos
					If settings.iTunesInstalled Then
                        If File.Exists(feeds(i).FeedVideos(j).DownloadedWmv) AndAlso _
                        File.Exists(feeds(i).FeedVideos(j).DownloadedMp4) Then
                            feeds(i).FeedVideos(j).IsConverted = True
                        End If
					Else
						If File.Exists(feeds(i).FeedVideos(j).DownloadedWmv) Then
							feeds(i).FeedVideos(j).IsConverted = True
						End If
					End If
				Next j
			Next i

			Return feeds

		End Function

	End Class
End Namespace
