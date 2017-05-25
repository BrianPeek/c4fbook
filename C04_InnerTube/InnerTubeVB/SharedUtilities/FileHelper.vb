Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO

Namespace SharedUtilities
	Public Enum FileType
		Image
		Flv
		Mp4
		Wmv
	End Enum

	Public NotInheritable Class FileHelper
		Public Shared DefaultAppName As String = "InnerTube"
		Public Shared DefaultSettingsName As String = "settings.xml"
        Public Shared DefaultImage As String = "images/youtube.jpg"


		Private Sub New()
        End Sub

        Public Shared Function BuildXmlName(ByVal appName As String) As String
            Return appName & ".xml"
        End Function

        Public Shared Function BuildXmlName() As String
            Return DefaultAppName & ".xml"
        End Function

		Public Shared Function WriteLogFile(ByVal basePath As String, ByVal contents As String) As String
			Dim time As String = ReplaceIllegalCharacters(DateTime.Now.ToString(), "_")
			Dim fileName As String = String.Format("UpdateLog_{0}.txt", time)
			Dim fullPath As String = Path.Combine(basePath, fileName)

			File.WriteAllText(fullPath, contents)
			Return fullPath
		End Function

		Public Shared Function BuildFileName(ByVal filePath As String, ByVal fileName As String, ByVal type As FileType) As String
			Select Case type
				Case FileType.Image
					Return Path.Combine(filePath, String.Format("{0}.jpg",fileName))
				Case FileType.Flv
					Return Path.Combine(filePath, String.Format("{0}.flv", FileHelper.ReplaceIllegalCharacters(fileName)))
				Case FileType.Mp4
					Return Path.Combine(filePath, String.Format("{0}.mp4", FileHelper.ReplaceIllegalCharacters(fileName)))
				Case FileType.Wmv
					Return Path.Combine(filePath, String.Format("{0}.wmv", FileHelper.ReplaceIllegalCharacters(fileName)))
				Case Else
					Return String.Empty

			End Select
		End Function

		Public Shared Function FileSize(ByVal file As String) As Double
			Dim f As New FileInfo(file)
			Dim size As Double = f.Length / 1024
			Return size
		End Function

		Public Shared Function ReplaceIllegalCharacters(ByVal source As String, ByVal replacement As String) As String

			If (Not String.IsNullOrEmpty(replacement)) Then
				'make sure replacement doesn't have an illegal character
				replacement = ReplaceIllegalCharacters(replacement, String.Empty)
			Else
				replacement = String.Empty
			End If

			'Using LINQ to replace characters for fun
						'Replace illegal characters in DestinationFile name
						' The following are illegal: \ / : * ? < > | " 
            Dim query = From n In New String() {source} _
                        Select n.Replace("\", replacement) _
                        .Replace("/", replacement) _
                        .Replace(":", replacement) _
                        .Replace("*", replacement) _
                        .Replace("?", replacement) _
                        .Replace("<", replacement) _
                        .Replace(">", replacement) _
                        .Replace("|", replacement) _
                        .Replace("""", replacement) _
                        .Replace("-", replacement)
			Return query.First()

		End Function

		Public Shared Function ReplaceIllegalCharacters(ByVal source As String) As String
			Return ReplaceIllegalCharacters(source, String.Empty)
		End Function

		Public Shared Function BuildPath(ByRef subDir As String) As String
            'My.SpecialDirectories doesn't have a folder for "Videos", 
            ' but assume it exists

            Dim newPath As String = My.Computer.FileSystem.SpecialDirectories.MyMusic            
            Dim os As OperatingSystem = Environment.OSVersion

            If os.Version.Major >= 6 Then 'Vista or higher
                newPath = newPath.Replace("Music", "Videos")
            ElseIf os.Version.Major = 5 Then 'XP
                newPath = newPath.Replace("My Music", "My Videos")
            Else
                Throw New NotSupportedException(String.Format("The Operating System Version is not supported:{0}", Environment.OSVersion))
            End If

            'Ensure the directory exists, if it doesn't, build it
            If (Not Directory.Exists(newPath)) Then
                Directory.CreateDirectory(newPath)
            End If

            If String.IsNullOrEmpty(subDir) Then
                subDir = Path.Combine(newPath, FileHelper.DefaultAppName)
            Else
                subDir = Path.Combine(newPath, subDir)
            End If

            If (Not Directory.Exists(subDir)) Then
                Directory.CreateDirectory(subDir)
            End If

            Return newPath
		End Function

		Public Shared Function ConvertedFilesExist(ByVal video As InnerTubeVideo, ByVal settings As Setting) As Boolean
			'iTunes needs WMV & MP4
			If settings.iTunesInstalled Then
				If File.Exists(video.DownloadedWmv) AndAlso File.Exists(video.DownloadedMp4) Then
					Return True
				Else
					Return False
				End If
			Else
				If File.Exists(video.DownloadedWmv) Then
					Return True
				Else
					Return False
				End If
			End If
		End Function

	End Class
End Namespace
