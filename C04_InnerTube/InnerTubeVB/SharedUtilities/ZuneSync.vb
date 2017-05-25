Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Linq
Imports System.Text
Imports Microsoft.Win32
Imports System.Collections.ObjectModel
Imports System.IO

Namespace SharedUtilities

	Public Enum ZuneMonitoredFolders
		MonitoredAudioFolders
		MonitoredVideoFolders
		MonitoredPhotoFolders
		MonitoredPodcastFolders
	End Enum

	Public Class ZuneSync
		Implements IVideoService
		Public Shared Function GetZuneMonitoredFolders(ByVal folder As ZuneMonitoredFolders) As String()
			'Pull registry
            Dim hive As String = "Software\Microsoft\Zune\Groveler\"
            Dim values() As String = CType(My.Computer.Registry.CurrentUser.OpenSubKey(hive).GetValue(folder.ToString()), String())

			Return values
		End Function


		#Region "VideoService Members"
		Public Sub Sync(ByVal filePath As String) Implements IVideoService.Sync
			Dim currentFolders() As String = ZuneSync.GetZuneMonitoredFolders(ZuneMonitoredFolders.MonitoredVideoFolders)

			Dim found As Boolean = currentFolders.Contains(filePath)
			'check if we are already adding the files to the folder
			If (Not found) Then

				'copy the files to the first specified directory   
				If currentFolders.Length >0 Then
					Dim destinationPath As String = currentFolders(0)
					Dim Files() As String = Directory.GetFiles(filePath, "*.wmv", SearchOption.TopDirectoryOnly)
					For Each f In Files
						File.Copy(f, destinationPath, True)
					Next f

				Else
					Throw New ArgumentException("Zune is not configured to monitor *any* " & "folders, to fix this, open zune.exe, click settings, and add a video folder")
				End If

			End If
        End Sub
		#End Region

	End Class


End Namespace



