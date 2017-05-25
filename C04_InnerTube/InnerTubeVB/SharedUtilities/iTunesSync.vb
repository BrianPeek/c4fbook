Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO
Imports iTunesLib

Namespace SharedUtilities
	Public Class iTunesSync
		Implements IVideoService
		Private iTunes As New iTunesApp()

		#Region "IVideoService Members"

		Public Sub Sync(ByVal filePath As String) Implements IVideoService.Sync
			'only get MP4 files
			Dim fileList() As String = Directory.GetFiles(filePath, "*.mp4",SearchOption.TopDirectoryOnly)
            Try
                For Each f In fileList
                    'Add file
                    iTunes.LibraryPlaylist.AddFile(f)
                Next f
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("iTunes error: " & ex.Message)
            Finally
                Me.iTunes = Nothing
            End Try
        End Sub

		#End Region

		Public Sub UpdateIPod()
			iTunes.UpdateIPod()
		End Sub

		Protected Overrides Sub Finalize()
            If Me.iTunes IsNot Nothing Then
                'cleanup 
                Me.iTunes = Nothing
            End If

		End Sub
	End Class
End Namespace
