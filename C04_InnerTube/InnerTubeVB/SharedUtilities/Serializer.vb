Imports Microsoft.VisualBasic
Imports System.Xml.Serialization
Imports System.IO

Namespace SharedUtilities
	Public Class Serializer(Of T)

        Public Sub Serialize(ByVal value As T, ByVal path As String)
            'Tell the serializer what type we're serializing
            Dim xs As New XmlSerializer(GetType(T))

            Using write As TextWriter = New StreamWriter(path)
                xs.Serialize(write, value)
            End Using

        End Sub

		Public Function Deserialize(ByVal path As String) As T
			Dim xs As New XmlSerializer(GetType(T))

			Using read As TextReader = New StreamReader(path)
				Return CType(xs.Deserialize(read), T)
			End Using
		End Function
	End Class
End Namespace
