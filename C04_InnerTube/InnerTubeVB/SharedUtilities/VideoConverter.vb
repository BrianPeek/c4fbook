Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO
Imports SharedUtilities
Imports System.Net
Imports System.Collections.ObjectModel
Imports System.Diagnostics


Namespace SharedUtilities
    Public Enum ConversionType
        Mp4
        Wmv
    End Enum

    Public NotInheritable Class VideoConverter
        Private Sub New()
        End Sub
        Public Shared Sub ConvertFlv(ByVal sourceFile As String, ByVal destinationFile As String, ByVal conversion As ConversionType)

            Dim cmdLineArgs As String = String.Empty

            'build command line for ffmpeg
            Select Case conversion
                Case ConversionType.Mp4
                    cmdLineArgs = String.Format(" -i  ""{0}"" ""{1}""", sourceFile, destinationFile)
                Case ConversionType.Wmv
                    cmdLineArgs = String.Format(" -i  ""{0}"" -vcodec wmv2 ""{1}""", sourceFile, destinationFile)
            End Select

            ConvertFlv(sourceFile, destinationFile, cmdLineArgs)
        End Sub

        Public Shared Sub ConvertFlv(ByVal source As InnerTubeVideo, ByVal conversion As ConversionType)

            Dim title As String = FileHelper.ReplaceIllegalCharacters(source.Title)
            Dim author As String = FileHelper.ReplaceIllegalCharacters(source.Author)
            Dim description As String = FileHelper.ReplaceIllegalCharacters(source.Description)

            'set values based on switch
            Dim cmdLineArgs As String = String.Empty
            Dim destination As String = String.Empty

            Select Case conversion

                Case ConversionType.Mp4
                    'ffmpeg.exe -title "Chocolate Rain" -author "TayZonday" -comment "Original Song by Tay Zonday" -i "Chocolate Rain.flv" "Chocolate Rain.mp4"
                    destination = source.DownloadedMp4
                    cmdLineArgs = String.Format(" -title ""{0}"" -author ""{1}"" -comment ""{2}"" -i  ""{3}"" ""{4}""", title, author, description, source.DownloadedFlv, destination)
                Case ConversionType.Wmv
                    'ffmpeg.exe -title "Chocolate Rain" -author "TayZonday" -comment "Original Song by Tay Zonday" -i "Chocolate Rain.flv" -vcodec wmv2 "Chocolate Rain.wmv"                    
                    destination = source.DownloadedWmv
                    cmdLineArgs = String.Format(" -title ""{0}"" -author ""{1}"" -comment ""{2}"" -i  ""{3}"" -vcodec wmv2 ""{4}""", title, author, description, source.DownloadedFlv, destination)
            End Select
            ConvertFlv(source.DownloadedFlv, destination, cmdLineArgs)
        End Sub

        Private Shared Sub ConvertFlv(ByVal sourceFile As String, ByVal destination As String, ByVal cmdLineArgs As String)
            'point to ffmpeg conversion tool
            Dim exePath As String = Path.Combine(My.Computer.FileSystem.CurrentDirectory, "ffmpeg\ffmpeg.exe")            

            'ensure sourceFile files exist and the destination doesn't
            If File.Exists(sourceFile) AndAlso File.Exists(exePath) AndAlso (Not File.Exists(destination)) Then

                'Start a Process externally as we're converting from the command line
                Using convert As New Process()
                    'Set properties
                    convert.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    convert.StartInfo.CreateNoWindow = True
                    convert.StartInfo.RedirectStandardOutput = True
                    convert.StartInfo.UseShellExecute = False
                    convert.StartInfo.Arguments = cmdLineArgs
                    convert.StartInfo.FileName = exePath

                    convert.Start()
                    convert.WaitForExit()
                End Using
            End If
        End Sub
    End Class
End Namespace