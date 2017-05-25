Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Xml.Serialization
Imports System.Xml

Namespace PeerCast
	Public Class Serializer
        Public Shared Function SerializeFileList() As String
            Dim videoList As List(Of String) = FileUtility.GetVideoList()

            Dim contents As New MemoryStream()
            Dim writer As New XmlTextWriter(contents, Text.Encoding.UTF8)

            Dim xs As New XmlSerializer(GetType(List(Of String)))
            xs.Serialize(writer, videoList)

            contents = CType(writer.BaseStream, MemoryStream)
            Dim encoding As New UTF8Encoding()

            Return encoding.GetString(contents.ToArray())
        End Function

		Public Shared Function DeserializeFileList(ByVal serializedList As String) As List(Of String)
			'deserialize the list of files           
			Dim encoding As New UTF8Encoding()
			Dim byteArray() As Byte = encoding.GetBytes(serializedList)
			Dim contents As New MemoryStream(byteArray)
            Dim writer As New XmlTextWriter(contents, Text.Encoding.UTF8)
			Dim xs As New XmlSerializer(GetType(List(Of String)))

			Return CType(xs.Deserialize(contents), List(Of String))
		End Function
	End Class
End Namespace
