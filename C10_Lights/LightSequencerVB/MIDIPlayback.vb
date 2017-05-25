'////////////////////////////////////////////////////////////////////////////////
'	MCIPlayback.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports Sanford.Multimedia.Midi
Imports Midi = Sanford.Multimedia.Midi

Namespace LightSequencer
	Public Class MIDIPlayback
		Implements IPlayback
		Private _MIDISequencer As Midi.Sequencer
		Private _MIDISequence As Midi.Sequence
		Private _MIDIOutDevice As Midi.OutputDevice

		Public Sub New()
			_MIDISequencer = New Midi.Sequencer()
			_MIDISequence = New Midi.Sequence()

			' event handlers for various message types
			AddHandler _MIDISequencer.ChannelMessagePlayed, AddressOf sequencer_ChannelMessagePlayed
			AddHandler _MIDISequencer.Chased, AddressOf sequencer_Chased
			AddHandler _MIDISequencer.Stopped, AddressOf sequencer_Stopped
		End Sub

		Public Sub Start() Implements IPlayback.Start
			_MIDISequencer.Start()
		End Sub

		Public Sub Load(ByVal seq As Sequence) Implements IPlayback.Load
			' grab the first MIDI device
			If _MIDIOutDevice Is Nothing Then
				_MIDIOutDevice = New Midi.OutputDevice(0)
			End If

			' load the MIDI file
			_MIDISequence.Load(seq.MusicFile)
			_MIDISequencer.Sequence = _MIDISequence
		End Sub

		Public Sub Unload() Implements IPlayback.Unload
			If _MIDISequence IsNot Nothing Then
				_MIDISequence.Clear()
			End If
			If _MIDIOutDevice IsNot Nothing Then
				_MIDIOutDevice.Close()
			End If
		End Sub

		Public Sub [Stop]() Implements IPlayback.Stop
			_MIDISequencer.Stop()
		End Sub

		Private Sub sequencer_Stopped(ByVal sender As Object, ByVal e As StoppedEventArgs)
			' send "stop" messages to the sound card
			For Each message As ChannelMessage In e.Messages
				If (Not _MIDIOutDevice.IsDisposed) Then
					_MIDIOutDevice.Send(message)
				End If
			Next message
		End Sub

		Private Sub sequencer_Chased(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.ChasedEventArgs)
			' send "chased" messages to the sound card
			For Each message As ChannelMessage In e.Messages
				If (Not _MIDIOutDevice.IsDisposed) Then
					_MIDIOutDevice.Send(message)
				End If
			Next message
		End Sub

		Private Sub sequencer_ChannelMessagePlayed(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.ChannelMessageEventArgs)
			' send each MIDI command to the sound card
			If (Not _MIDIOutDevice.IsDisposed) Then
				_MIDIOutDevice.Send(e.Message)
			End If
		End Sub
	End Class
End Namespace
