Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace SharedUtilities
	Public NotInheritable Class Deleted

		Private Sub New()
        End Sub

		Public Shared Sub Delete(ByVal feedList As System.Collections.ObjectModel.ObservableCollection(Of InnerTubeFeed), ByVal deleteMe As InnerTubeVideo)
			For i As Integer = 0 To feedList.Count - 1
				feedList(i).FeedVideos.Remove(deleteMe)
			Next i

		End Sub

		Public Shared Sub Delete(ByVal feedList As System.Collections.ObjectModel.ObservableCollection(Of InnerTubeFeed), ByVal deleteFeed As InnerTubeFeed)
			feedList.Remove(deleteFeed)
        End Sub

	End Class
End Namespace
