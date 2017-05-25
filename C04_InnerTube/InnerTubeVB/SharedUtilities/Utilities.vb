Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.IO.IsolatedStorage
Imports System.Net.NetworkInformation

Namespace SharedUtilities

    Public NotInheritable Class Utilities
        Private Sub New()
        End Sub
        Public Shared Function ParseQueryStringParameters(ByVal url As String) As Dictionary(Of String, String)
            Dim result = New Dictionary(Of String, String)()

            'convert to Uri to get Query, then get Query substring 
            Dim queryParams As String = New Uri(url).Query.Substring(1)

            Dim AmpSplit = queryParams.Split("&"c)

            For Each s In AmpSplit
                If (Not String.IsNullOrEmpty(s)) Then
                    Dim EqualSplit = s.Split("="c)

                    result.Add(EqualSplit(0), EqualSplit(1))
                End If
            Next s
            Return result
        End Function

        Public Shared Function GetEnumNames(ByVal source As Type) As String()
            If source.BaseType Is GetType(System.Enum) Then
                Return System.Enum.GetNames(source)
            Else
                Throw New ArgumentException("Type passed in is not an enum")
            End If

        End Function

        Public Shared Function IsNetworkAvailable() As Boolean
            Return NetworkInterface.GetIsNetworkAvailable()

        End Function

        Public Shared Function Flatten(ByVal source As ObservableCollection(Of InnerTubeFeed)) As List(Of InnerTubeVideo)
            Dim vids As Dictionary(Of String, InnerTubeVideo) = New Dictionary(Of String, InnerTubeVideo)()
            For Each feed In source
                For Each vid In feed.FeedVideos
                    If (Not vids.ContainsKey(vid.Id)) Then
                        vids.Add(vid.Id, vid)
                    End If
                Next vid
            Next feed
            Return vids.Values.ToList()
        End Function

    End Class
End Namespace
