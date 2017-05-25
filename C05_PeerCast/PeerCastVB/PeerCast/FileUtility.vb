Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Namespace PeerCast
    Public Class FileUtility
        Public Shared Function GetVideoList() As List(Of String)
            Dim videos As List(Of String) = Directory.GetFiles(My.Settings.Default.FileDirectory, "*.wmv", SearchOption.TopDirectoryOnly).ToList()
            Return videos
        End Function
    End Class
End Namespace
