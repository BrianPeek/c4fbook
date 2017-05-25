'////////////////////////////////////////////////////////////////////////////////
'	Sequence.cs
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
	Public Enum MusicType
		Sample = 0
		MIDI = 1
	End Enum

	Public Class Sequence
		Private Const FILE_VERSION As String = "2"

		Public Title As String
		Public Artist As String
		Public MusicFile As String
		Public MusicLength As Integer
		Public Interval As Single ' in ms
		Public Version As Integer = Integer.Parse(FILE_VERSION)
		Public Channels As List(Of Channel) = New List(Of Channel)()
		Public MusicType As MusicType

		Public Sub New()
			Me.New(Nothing)
		End Sub

		Public Sub New(ByVal filename As String)
			If (Not String.IsNullOrEmpty(filename)) Then
				Load(filename)
			End If
		End Sub

		Public Sub Save(ByVal filename As String)
			' indent our output file
			Dim settings As New XmlWriterSettings()
			settings.Indent = True

			' create a new XML file
			Dim xml As XmlWriter = XmlWriter.Create(filename, settings)

			' write start tags
			xml.WriteStartDocument()

			' save the appropriate version
			Select Case Me.Version
				Case 1
					SaveV1Sequence(xml)
				Case Else
					SaveSequence(xml)
			End Select

			' write end tags
			xml.WriteEndDocument()

			' close it up
			xml.Flush()
			xml.Close()
		End Sub

		Private Sub SaveV1Sequence(ByVal xml As XmlWriter)
			Dim sb As New StringBuilder()

			xml.WriteComment("Sequence file for LightSequencer")

			xml.WriteStartElement("sequence")

				' write base config info
				xml.WriteStartElement("config")
					xml.WriteElementString("fileVersion", "1")
					xml.WriteElementString("numChannels", Me.Channels.Count.ToString())
					xml.WriteElementString("musicFile", Me.MusicFile)
					xml.WriteElementString("musicLength", Me.MusicLength.ToString())
				xml.WriteEndElement()

				xml.WriteStartElement("channels")

				For Each channel As Channel In Me.Channels
					' 1==on, 0==off...lame, but it works. :)
					For Each b As Boolean In channel.Data
						If b Then
							sb.Append(("1"))
						Else
							sb.Append(("0"))
						End If
					Next b

					' write channel specific info
					xml.WriteStartElement("channel")
						xml.WriteAttributeString("number", channel.Number.ToString())
						xml.WriteAttributeString("serialNumber", channel.SerialNumber.ToString())
						xml.WriteAttributeString("outputIndex", channel.OutputIndex.ToString())
						xml.WriteString(sb.ToString())
					xml.WriteEndElement()

					sb.Remove(0, sb.Length)
				Next channel

				xml.WriteEndElement()

			xml.WriteEndElement()
		End Sub

		Private Sub SaveSequence(ByVal xml As XmlWriter)
			Dim sb As New StringBuilder()

			xml.WriteComment("Sequence file for LightSequencer")

			xml.WriteStartElement("sequence")

				' write base config info
				xml.WriteStartElement("config")
					xml.WriteElementString("fileVersion", FILE_VERSION)
					xml.WriteElementString("musicFile", Me.MusicFile)
					xml.WriteElementString("musicLength", Me.MusicLength.ToString())
					xml.WriteElementString("numChannels", Me.Channels.Count.ToString())
					xml.WriteElementString("interval", Me.Interval.ToString())
				xml.WriteEndElement()

				' write music information
				xml.WriteStartElement("musicInfo")
					xml.WriteElementString("title", Me.Title)
					xml.WriteElementString("artist", Me.Artist)
				xml.WriteEndElement()

				' write channels
				xml.WriteStartElement("channels")

				For Each channel As Channel In Me.Channels
					' 1==on, 0==off...lame, but it works. :)
					For Each b As Boolean In channel.Data
						If b Then
							sb.Append(("1"))
						Else
							sb.Append(("0"))
						End If
					Next b

					' write channel specific info
					xml.WriteStartElement("channel")
						xml.WriteAttributeString("number", channel.Number.ToString())
						xml.WriteAttributeString("serialNumber", channel.SerialNumber.ToString())
						xml.WriteAttributeString("outputIndex", channel.OutputIndex.ToString())
						xml.WriteAttributeString("midiChannel", channel.MIDIChannel.ToString())
						xml.WriteString(sb.ToString())
					xml.WriteEndElement()

					sb.Remove(0, sb.Length)
				Next channel
				xml.WriteEndElement()
			xml.WriteEndElement()
		End Sub

		Public Sub Load(ByVal filename As String)
			' load the XML file
			Dim xmlDoc As New XmlDocument()
			xmlDoc.Load(filename)

			' grab the base config info
			Me.Version = Integer.Parse(xmlDoc.SelectSingleNode("sequence/config/fileVersion").InnerText)
			Select Case Me.Version
				Case 1
					LoadV1Sequence(xmlDoc)
				Case Else
					LoadSequence(xmlDoc)
			End Select
		End Sub

		Public Sub LoadV1Sequence(ByVal xmlDoc As XmlDocument)
			Dim node As XmlNode

			Me.Interval = 50

			Dim numChannels As Integer = Integer.Parse(xmlDoc.SelectSingleNode("sequence/config/numChannels").InnerText)
			Me.MusicFile = xmlDoc.SelectSingleNode("sequence/config/musicFile").InnerText
			Me.MusicLength = Integer.Parse(xmlDoc.SelectSingleNode("sequence/config/musicLength").InnerText)
			Me.MusicType = MusicType.Sample

			Me.Channels.Clear()

			' add each channel to our internal list for easy tracking
			For i As Integer = 0 To numChannels - 1
				node = xmlDoc.SelectSingleNode("sequence/channels/channel[@number='" & i.ToString() & "']")
				If node IsNot Nothing Then
					Dim serial As Integer = Integer.Parse(node.Attributes("serialNumber").Value)
					Dim data() As Char = node.InnerText.ToCharArray()
					Me.Channels.Add(New Channel(i, serial, Integer.Parse(node.Attributes("outputIndex").Value), data.Length))
					For j As Integer = 0 To data.Length - 1
						Me.Channels(i).Data(j) = (data(j) = "1"c)
					Next j
				Else
					Me.Channels.Add(New Channel(i, -1, -1))
				End If
			Next i
		End Sub

		Public Sub LoadSequence(ByVal xmlDoc As XmlDocument)
			Dim node As XmlNode

			' grab the base config info
			Me.MusicFile = xmlDoc.SelectSingleNode("sequence/config/musicFile").InnerText

			If Me.MusicFile.EndsWith(".mid") Then
				Me.MusicType = MusicType.MIDI
			End If

			Me.MusicLength = Integer.Parse(xmlDoc.SelectSingleNode("sequence/config/musicLength").InnerText)
			Me.Interval = Single.Parse(xmlDoc.SelectSingleNode("sequence/config/interval").InnerText)

			Dim numChannels As Integer = Integer.Parse(xmlDoc.SelectSingleNode("sequence/config/numChannels").InnerText)

			Me.Title = xmlDoc.SelectSingleNode("sequence/musicInfo/title").InnerText
			Me.Artist = xmlDoc.SelectSingleNode("sequence/musicInfo/artist").InnerText

			Me.Channels.Clear()

			' add each channel to our internal list for easy tracking
			For i As Integer = 0 To numChannels - 1
				node = xmlDoc.SelectSingleNode("sequence/channels/channel[@number='" & i.ToString() & "']")
				If node IsNot Nothing Then
					Dim serial As Integer = Integer.Parse(node.Attributes("serialNumber").Value)
					Dim data() As Char = node.InnerText.ToCharArray()

					' add the channel to the channel list
					Me.Channels.Add(New Channel(i, serial, Integer.Parse(node.Attributes("outputIndex").Value), Integer.Parse(node.Attributes("midiChannel").Value), data.Length))

					' read out the data
					For j As Integer = 0 To data.Length - 1
						Me.Channels(i).Data(j) = (data(j) = "1"c)
					Next j
				Else
					Me.Channels.Add(New Channel(i))
				End If
			Next i
		End Sub
	End Class
End Namespace
