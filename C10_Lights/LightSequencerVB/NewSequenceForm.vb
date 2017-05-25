'////////////////////////////////////////////////////////////////////////////////
'	NewSequenceForm.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
Imports Phidgets
Imports Midi = Sanford.Multimedia.Midi

Namespace LightSequencer
	Partial Public Class NewSequenceForm
		Inherits Form
		' store away information on a MIDI channel
		Private Structure MIDIChannel
			Private _channel As Integer
			Private _name As String

			Public Property Channel() As Integer
				Get
					Return Me._channel
				End Get
				Set(ByVal value As Integer)
					Me._channel = value
				End Set
			End Property

			Public Property Name() As String
				Get
					Return Me._name
				End Get
				Set(ByVal value As String)
					Me._name = value
				End Set
			End Property

			Public ReadOnly Property Display() As String
				Get
					Return Me.Channel & ": " & Me.Name
				End Get
			End Property

			Public Sub New(ByVal c As Integer, ByVal n As String)
				_channel = c
				_name = n
			End Sub
		End Structure

		Private _numChannels As Integer = 0
		Private _sequence As Sequence
		Private _edit As Boolean = False

		' binding to the combo box in the device list
		Private _MIDIChannels As BindingList(Of MIDIChannel)

		' map a midi channel to a phidget output
		Private _midiMap As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer,Integer)()

		Public Sub New()
			Me.New(Nothing)
		End Sub

		Public Sub New(ByVal s As Sequence)
			InitializeComponent()

			' respond to change events from the new PhidgetHandler class
			AddHandler PhidgetHandler.PhidgetsChanged, AddressOf PhidgetHandler_PhidgetsChanged

			If PhidgetHandler.IFKits.Count = 0 Then
				btnOK.Enabled = False
				MessageBox.Show("No Phidget Interface Kits found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If

			' we're in edit mode...fill in existing data
			If s IsNot Nothing Then
				Me.Text = "Edit Title Info"

				txtArtist.Text = s.Artist
				txtFile.Text = s.MusicFile
				txtMinutes.Text = (s.MusicLength \ 60).ToString()
				txtSeconds.Text = ((s.MusicLength Mod 60) - 3).ToString()
				txtTitle.Text = s.Title

				_sequence = s
				_edit = True
				txtMinutes.Enabled = False
				txtSeconds.Enabled = False
				txtFile.Enabled = False
				btnBrowse.Enabled = False
				chkHold.Enabled = False

				If _sequence.MusicType = MusicType.MIDI Then
					GetMIDIInfo(_sequence.MusicFile, False)
					SwitchGrid(True)
				End If
			End If

			' setup the device grid
			FillPhidgetGrid()
		End Sub

		Private Sub PhidgetHandler_PhidgetsChanged(ByVal sender As Object, ByVal e As EventArgs)
			FillPhidgetGrid()
		End Sub

		Private Sub FillPhidgetGrid()
			dgvDevices.Rows.Clear()
			_numChannels = 0

			For Each ik As InterfaceKit In PhidgetHandler.IFKits.Values
				Dim currentChannel As Integer = _numChannels

				' parse out the number of output channels the device supports (analog in/digitial in/digitial out)
				_numChannels += ik.outputs.Count

				' add device to the grid
				For i As Integer = 0 To ik.outputs.Count - 1
					dgvDevices.Rows.Add(ik.Name, ik.SerialNumber, i+1+currentChannel, -1, i)

					If _edit AndAlso i < _sequence.Channels.Count Then
						If _sequence.MusicType = MusicType.MIDI Then
							dgvDevices.Rows(i).Cells("colMIDIChannel").Value = _sequence.Channels(i).MIDIChannel
						End If

						dgvDevices.Rows(i).Cells("colPhidgetOutput").Value = _sequence.Channels(i).OutputIndex
					Else
						' if we have MIDI info, fill in the defaults
						If _sequence IsNot Nothing AndAlso _sequence.MusicType = MusicType.MIDI AndAlso _MIDIChannels IsNot Nothing AndAlso i < _MIDIChannels.Count Then
							dgvDevices.Rows(i).Cells("colMIDIChannel").Value = i
						End If
					End If
				Next i
			Next ik

			' enable/disable OK button
			btnOK.Enabled = (dgvDevices.Rows.Count > 0)
		End Sub

		Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse.Click
			Dim ofd As New OpenFileDialog()
			ofd.Title = "Locate music file..."
			ofd.Filter = "Music files (*.mp3;*.wma;*.wav;*.mid)|*.mp3;*.wma;*.wav;*.mid|All files (*.*)|*.*"
			If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				txtFile.Text = ofd.FileName

				' if we have a MIDI file, show the MIDI column
				If txtFile.Text.EndsWith(".mid") Then
					chkHold.Enabled = True
					GetMIDIInfo(txtFile.Text, True)
					SwitchGrid(True)
				Else
					chkHold.Enabled = False
					SwitchGrid(False)
				End If
			End If
		End Sub

		Private Sub SwitchGrid(ByVal midi As Boolean)
			If midi Then
				' enable MIDI column
				dgvDevices.Columns("colMIDIChannel").Visible = True
				dgvDevices.Columns("colChannels").Visible = False

				' bind the MIDI channels to the combo
				CType(dgvDevices.Columns("colMIDIChannel"), DataGridViewComboBoxColumn).DisplayMember = "Display"
				CType(dgvDevices.Columns("colMIDIChannel"), DataGridViewComboBoxColumn).ValueMember = "Channel"
				CType(dgvDevices.Columns("colMIDIChannel"), DataGridViewComboBoxColumn).DataSource = _MIDIChannels

				' default select every channel
				For i As Integer = 0 To dgvDevices.Rows.Count - 1
					If i < _MIDIChannels.Count Then
						dgvDevices.Rows(i).Cells("colMIDIChannel").Value = i
					End If
				Next i
			Else
				' disable MIDI column
				dgvDevices.Columns("colMIDIChannel").Visible = False
				dgvDevices.Columns("colChannels").Visible = True
			End If
		End Sub

		Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
			If txtMinutes.Text.Trim().Length = 0 OrElse txtSeconds.Text.Trim().Length = 0 OrElse Integer.Parse(txtSeconds.Text) > 59 Then
				MessageBox.Show("Please enter a valid sequence length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return
			End If

			If (Not _edit) Then
				_sequence = New Sequence()
			End If

			If txtFile.Text.EndsWith(".mid") Then
				_sequence.MusicType = MusicType.MIDI
			Else
				_sequence.MusicType = MusicType.Sample
			End If

			_sequence.Title = txtTitle.Text
			_sequence.Artist = txtArtist.Text

			If _edit Then
				' if we have more channels than our devices can handle, remove the old channels
				If _sequence.Channels.Count > dgvDevices.Rows.Count Then
					_sequence.Channels.RemoveRange(dgvDevices.Rows.Count, _sequence.Channels.Count - dgvDevices.Rows.Count)
				End If

				Dim i As Integer = 0

				' get chanel info
				For Each row As DataGridViewRow In dgvDevices.Rows
					If i < _sequence.Channels.Count Then
						_sequence.Channels(i).SerialNumber = Integer.Parse(row.Cells("colSerialNumber").Value.ToString())
						_sequence.Channels(i).OutputIndex = Integer.Parse(row.Cells("colPhidgetOutput").Value.ToString())
						_sequence.Channels(i).MIDIChannel = Integer.Parse(row.Cells("colMIDIChannel").Value.ToString())
					End If
					i += 1
				Next row
			Else
				_sequence.Interval = 50 ' TODO: someday this will be different
				_sequence.MusicFile = txtFile.Text
				_sequence.MusicLength = ((Integer.Parse(txtMinutes.Text) * 60) + Integer.Parse(txtSeconds.Text) + 3)

				' if we're not editing, create new channels
				Dim i As Integer = 0

				For Each row As DataGridViewRow In dgvDevices.Rows
					_sequence.Channels.Add(New Channel(i, Integer.Parse(row.Cells("colSerialNumber").Value.ToString()), Integer.Parse(row.Cells("colPhidgetOutput").Value.ToString()), Integer.Parse(row.Cells("colMIDIChannel").Value.ToString()), CInt(Fix(1000/_sequence.Interval)) * _sequence.MusicLength))
					i += 1
					_midiMap(Integer.Parse(row.Cells("colMIDIChannel").Value.ToString())) = Integer.Parse(row.Cells("colPhidgetOutput").Value.ToString())
				Next row

				If _sequence.MusicType = MusicType.MIDI Then
					ProcessMIDI()
				End If
			End If

			DialogResult = System.Windows.Forms.DialogResult.OK
			Me.Close()
		End Sub

		Private Sub GetMIDIInfo(ByVal file As String, ByVal fillTime As Boolean)
			Dim MIDISequence As Midi.Sequence

			Dim tempo As Integer = 500000
			Dim maxtick As Integer = 0
			Dim msPerTick As Single = (tempo\48)/1000.0f
			Dim title As String = String.Empty

			_MIDIChannels = New BindingList(Of MIDIChannel)()

			' load the MIDI file
			MIDISequence = New Midi.Sequence()
			MIDISequence.Load(file)

			' we don't handle non-PPQN MIDI files
			If MIDISequence.SequenceType <> Sanford.Multimedia.Midi.SequenceType.Ppqn Then
				MessageBox.Show("Unsupported MIDI type...sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return
			End If

			For Each t As Midi.Track In MIDISequence
				' get the command enumerator
				Dim en As IEnumerator(Of Midi.MidiEvent) = t.Iterator().GetEnumerator()
				Dim channelAdded As Boolean = False
				Do While en.MoveNext()
					Dim e As Midi.MidiEvent = en.Current
					Select Case e.MidiMessage.MessageType
						Case Sanford.Multimedia.Midi.MessageType.Channel ' track the # of channels
							Dim channel As Midi.ChannelMessage = CType(e.MidiMessage, Midi.ChannelMessage)
							If (Not channelAdded) Then
								_MIDIChannels.Add(New MIDIChannel(channel.MidiChannel, title))
								channelAdded = True
							End If

						Case Sanford.Multimedia.Midi.MessageType.Meta
							Dim meta As Midi.MetaMessage = CType(e.MidiMessage, Midi.MetaMessage)
							Select Case meta.MetaType
								' cache away the track name for the grid
								Case Sanford.Multimedia.Midi.MetaType.TrackName
									title = Encoding.ASCII.GetString(meta.GetBytes())

								' get the tempo and convert to a time value we can use
								Case Sanford.Multimedia.Midi.MetaType.Tempo
									tempo = meta.GetBytes()(0) << 16 Or meta.GetBytes()(1) << 8 Or meta.GetBytes()(2)
									msPerTick = (tempo/MIDISequence.Division)/1000.0f
							End Select
					End Select

					' find the highest time value
					If e.AbsoluteTicks > maxtick Then
						maxtick = e.AbsoluteTicks
					End If
				Loop
			Next t
			' and use that value to fill in the minutes/seconds
			If fillTime Then
				txtMinutes.Text = ((CInt(Fix(msPerTick * maxtick))/1000) / 60).ToString()
				txtSeconds.Text = ((CInt(Fix(msPerTick * maxtick))/1000) Mod 60+1).ToString()
			End If
		End Sub

		Private Sub ProcessMIDI()
			Dim _MIDISequence As Midi.Sequence

			' default MIDI tempos/milliseconds per tick
			Dim tempo As Integer = 500000
			Dim msPerTick As Single = (tempo\48)/1000.0f

			_MIDISequence = New Midi.Sequence()
			_MIDISequence.Load(_sequence.MusicFile)

			If _MIDISequence.SequenceType <> Sanford.Multimedia.Midi.SequenceType.Ppqn Then
				MessageBox.Show("Unsupported MIDI type...sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return
			End If

			For Each t As Midi.Track In _MIDISequence
				Dim en As IEnumerator(Of Midi.MidiEvent) = t.Iterator().GetEnumerator()

				Do While en.MoveNext()
					Dim e As Midi.MidiEvent = en.Current
					Select Case e.MidiMessage.MessageType
						' starta  new channel
						Case Sanford.Multimedia.Midi.MessageType.Channel
							Dim channel As Midi.ChannelMessage = CType(e.MidiMessage, Midi.ChannelMessage)

							' if it's a note on command and it's in our mapping list
							If channel.Command = Sanford.Multimedia.Midi.ChannelCommand.NoteOn AndAlso _midiMap.ContainsKey(channel.MidiChannel) AndAlso CInt(Fix((e.AbsoluteTicks*msPerTick)/50)) < _sequence.Channels(_midiMap(channel.MidiChannel)).Data.Length Then
								' this means the note is on
								If channel.Data2 > 0 Then
									_sequence.Channels(_midiMap(channel.MidiChannel)).Data(CInt(Fix((e.AbsoluteTicks*msPerTick)/50))) = True
								Else
									' the note is off
									_sequence.Channels(_midiMap(channel.MidiChannel)).Data(CInt(Fix((e.AbsoluteTicks*msPerTick)/50))) = False
									If chkHold.Checked Then
										' backfill the grid
										Dim i As Integer = CInt(Fix((e.AbsoluteTicks*msPerTick)/50))
										Do While i > 0 AndAlso Not _sequence.Channels(_midiMap(channel.MidiChannel)).Data(i)
											_sequence.Channels(_midiMap(channel.MidiChannel)).Data(i) = True
											i -= 1
										Loop
									End If
								End If
							End If
							' true note off...don't see this used much
							If channel.Command = Sanford.Multimedia.Midi.ChannelCommand.NoteOff AndAlso _midiMap.ContainsKey(channel.MidiChannel) Then
								_sequence.Channels(_midiMap(channel.MidiChannel)).Data(CInt(Fix((e.AbsoluteTicks*msPerTick)/50))) = False
								If chkHold.Checked Then
									Dim i As Integer = CInt(Fix((e.AbsoluteTicks*msPerTick)/50))
									Do While i > 0 AndAlso Not _sequence.Channels(_midiMap(channel.MidiChannel)).Data(i)
										_sequence.Channels(_midiMap(channel.MidiChannel)).Data(i) = True
										i -= 1
									Loop
								End If
							End If
						Case Sanford.Multimedia.Midi.MessageType.Meta
							Dim meta As Midi.MetaMessage = CType(e.MidiMessage, Midi.MetaMessage)
							Select Case meta.MetaType
								' again, get the tempo value
								Case Sanford.Multimedia.Midi.MetaType.Tempo
									tempo = meta.GetBytes()(0) << 16 Or meta.GetBytes()(1) << 8 Or meta.GetBytes()(2)
									msPerTick = (tempo/_MIDISequence.Division)/1000.0f
							End Select
					End Select
				Loop
			Next t
		End Sub

		Private Sub txtMinutes_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtMinutes.TextChanged
			CheckDigits(txtMinutes)
		End Sub

		Private Sub txtSeconds_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSeconds.TextChanged
			CheckDigits(txtSeconds)
		End Sub

		' ensure valid data is entered into our minutes/seconds boxes
		Private Sub CheckDigits(ByVal tb As TextBox)
			For Each c As Char In tb.Text
				If (Not Char.IsDigit(c)) Then
					tb.Text = String.Empty
				End If
			Next c
		End Sub

		Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
			DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.Close()
		End Sub

		Private Sub NewSequenceForm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
			' remove the event handler
			RemoveHandler PhidgetHandler.PhidgetsChanged, AddressOf PhidgetHandler_PhidgetsChanged
		End Sub

		Private Sub dgvDevices_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvDevices.DataError
			' I dunno what the deal is with this, but the DGV hates me.
		End Sub

		Public ReadOnly Property Sequence() As Sequence
			Get
				Return _sequence
			End Get
		End Property
	End Class
End Namespace