'////////////////////////////////////////////////////////////////////////////////
'	Playlist.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Namespace LightSequencer
	Public Class Playlist
		Private Const FILE_VERSION As String = "1"

		Public Version As Integer = Integer.Parse(FILE_VERSION)
		Public Filenames As List(Of String) = New List(Of String)()

		Public Sub New()
		End Sub

		Public Sub New(ByVal filename As String)
			Load(filename)
		End Sub

		Public Sub Save(ByVal filename As String)
			Dim settings As New XmlWriterSettings()
			settings.Indent = True

			Dim xml As XmlWriter = XmlWriter.Create(filename, settings)

			xml.WriteStartDocument()

			Select Case Me.Version
				Case Else
					SavePlaylist(xml)
			End Select

			xml.WriteEndDocument()

			xml.Flush()
			xml.Close()
		End Sub

		Private Sub SavePlaylist(ByVal xml As XmlWriter)
			Dim sb As New StringBuilder()

			xml.WriteComment("Playlist file for LightSequencer")

			xml.WriteStartElement("Playlist")

				' write base config info
				xml.WriteStartElement("config")
					xml.WriteElementString("fileVersion", FILE_VERSION)
					xml.WriteElementString("numSequences", Me.Filenames.Count.ToString())
				xml.WriteEndElement()

				xml.WriteStartElement("sequences")

				For Each file As String In Me.Filenames
					' write channel specific info
					xml.WriteStartElement("sequence")
						xml.WriteElementString("fileName", file)
					xml.WriteEndElement()
				Next file
				xml.WriteEndElement()
			xml.WriteEndElement()
		End Sub

		Public Sub Load(ByVal filename As String)
			Dim xmlDoc As New XmlDocument()
			xmlDoc.Load(filename)

			' grab the base config info
			Me.Version = Integer.Parse(xmlDoc.SelectSingleNode("Playlist/config/fileVersion").InnerText)
			Select Case Me.Version
				Case Else
					LoadPlaylist(xmlDoc)
			End Select
		End Sub

		Public Sub LoadPlaylist(ByVal xmlDoc As XmlDocument)
			Dim nodes As XmlNodeList = xmlDoc.SelectNodes("Playlist/sequences/sequence")
			For Each node As XmlNode In nodes
				Me.Filenames.Add(node("fileName").InnerText)
			Next node
		End Sub
	End Class
End Namespace
