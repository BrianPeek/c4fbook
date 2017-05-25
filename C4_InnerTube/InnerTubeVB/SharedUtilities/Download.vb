Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.IO
Imports SharedUtilities
Imports System.Diagnostics
Imports System.Collections.ObjectModel
Imports System.Web
Imports System.Collections.Specialized


Namespace SharedUtilities
    Public Class Download
        Public Shared Sub DownloadVideo(ByVal source As InnerTubeVideo)
            DownloadVideo(source, source.DownloadedFlv)
        End Sub

        Public Shared Sub DownloadVideo(ByVal source As InnerTubeVideo, ByVal destination As String)
            If (Not File.Exists(destination)) Then
                Dim final As New UriBuilder(source.DownloadLink)
                final.Query = "video_id=" & source.Id & "&" & CreateTokenRequest(source)

                Dim request As WebRequest = WebRequest.Create(final.ToString())
                request.Timeout = 500000

                Try
                    Dim response As WebResponse = request.GetResponse()

                    Using webStream As Stream = response.GetResponseStream()
                        Try
                            Dim _bufferSize As Integer = 65536

                            Using fs As FileStream = File.Create(destination, _bufferSize)
                                Dim readBytes As Integer = -1
                                Dim inBuffer(_bufferSize - 1) As Byte

                                'Loop until we hit the end
                                Do While readBytes <> 0
                                    'read data from web into filebuffer, then write to file
                                    readBytes = webStream.Read(inBuffer, 0, _bufferSize)
                                    fs.Write(inBuffer, 0, readBytes)
                                Loop
                            End Using
                        Catch ex As Exception
                            Debug.WriteLine("Error in Buffer Download")
                            Debug.Indent()
                            Debug.WriteLine(ex.Message)
                        End Try
                    End Using
                Catch ex As Exception
                    Debug.WriteLine("Error in request.GetResponse()")
                    Debug.Indent()
                    Debug.WriteLine(ex.Message)
                End Try
            End If
        End Sub

        Public Shared Sub DownloadVideo(ByVal watchUrl As String, ByVal destination As String)
            'Get links for video, then download
            Dim Videos As ObservableCollection(Of InnerTubeVideo) = InnerTubeService.GetSingleVideo(watchUrl)
            DownloadVideo(Videos(0), destination)

        End Sub

        Public Shared Sub DownloadImage(ByVal watchUrl As String, ByVal destination As String)

            Dim Videos As ObservableCollection(Of InnerTubeVideo) = InnerTubeService.GetSingleVideo(watchUrl)
            DownloadImage(Videos(0), destination)
        End Sub

        Public Shared Sub DownloadImage(ByVal source As InnerTubeVideo)
            DownloadImage(source, source.DownloadedImage)
        End Sub

        Public Shared Sub DownloadImage(ByVal source As InnerTubeVideo, ByVal destination As String)
            'if we haven't downloaded the image yet, download it
            If (Not File.Exists(destination)) Then
                Using wc As New WebClient()
                    wc.DownloadFile(New Uri(source.ThumbnailLink), destination)
                End Using
            End If
        End Sub

        Private Shared Function CreateTokenRequest(ByVal video As InnerTubeVideo) As String
            'YouTube variables
            Const jsVariable As String = "swfArgs"
            Const argName As String = "t"

            'get raw html from YouTube video page
            Dim rawHtml As String
            Using wc As New WebClient()
                rawHtml = wc.DownloadString(video.Link)
            End Using

            'extract the JavaScript name/value pairs
            Dim jsIndex As Integer = rawHtml.IndexOf(jsVariable)
            Dim startIndex As Integer = rawHtml.IndexOf("{", jsIndex)
            Dim endIndex As Integer = rawHtml.IndexOf("}", startIndex)
            Dim fullString As String = rawHtml.Substring(startIndex + 1, endIndex - startIndex - 1)

            'remove all quotes (") 
            fullString = fullString.Replace("""", "")

            'split all values
            Dim allArgs() As String = fullString.Split(","c)

            'loop through javascript parameters
            For Each swfArg As String In allArgs
                If swfArg.Trim().StartsWith(argName) Then
                    Dim nameValuePair = swfArg.Split(":"c)
                    Return String.Format("{0}={1}", argName, nameValuePair(1).Trim())
                End If
            Next swfArg

            Throw New Exception(String.Format("token not found in swfArgs, make sure {0} is accessible", video.Link))
        End Function


    End Class
End Namespace
