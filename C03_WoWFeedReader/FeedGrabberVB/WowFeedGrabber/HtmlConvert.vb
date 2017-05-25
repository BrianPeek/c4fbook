Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Namespace WowFeedGrabber
    Public NotInheritable Class HtmlConvert
        Private Shared characterPlaceholders As New Dictionary(Of String, String)

        Shared Sub New()
            characterPlaceholders.Add("&amp;", "&")
            characterPlaceholders.Add("&lt;", "<")
            characterPlaceholders.Add("&gt;", ">")
            characterPlaceholders.Add("&quot;", "'")
            characterPlaceholders.Add("&apos;", "'")


        End Sub


        Public Shared Function ToPlainText(ByVal html As String) As String
            If html Is Nothing Then
                Throw New ArgumentNullException("html")
            End If

            Return ReplaceCharacterPlaceholders(StripHtmlTags(html))
        End Function


        Private Shared Function StripHtmlTags(ByVal html As String) As String
            Return Regex.Replace(html, "<(/?)([^>]+)>", String.Empty)

        End Function

        Private Shared Function ReplaceCharacterPlaceholders(ByVal text As String) As String
            For Each placeholder In characterPlaceholders
                text = text.Replace(placeholder.Key, placeholder.Value)
            Next placeholder

            Return text
        End Function
    End Class
End Namespace
