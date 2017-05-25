'////////////////////////////////////////////////////////////////////////////////
'	SequencePlayerV2.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Threading

Namespace LightSequencer
	Public Class SequencePlayerV2
		Inherits SequencePlayer
		Private _playback As IPlayback
		Private _sequence As Sequence
		Private _tickCount As Integer
		Private _stopWatch As Stopwatch
		Private _playbackThread As Thread
		Private _playing As Boolean

		Public Sub New(ByVal s As Sequence)
			_sequence = s

			Select Case _sequence.MusicType
				Case MusicType.Sample
					_playback = New MCIPlayback()
				Case MusicType.MIDI
					_playback = New MIDIPlayback()
			End Select

			' close any open music files
			_playback.Unload()

			_playback.Load(s)
		End Sub

		Public Overrides Sub Start()
			_playbackThread = New Thread(New ThreadStart(AddressOf ThreadHandler))
			_playbackThread.Name = "V2 playback thread"
			_playbackThread.Start()
		End Sub

		Public Overrides Sub [Stop]()
			_playing = False
		End Sub

		Private Sub ThreadHandler()
			Dim last As Single = 0
			_tickCount = 0

			_playback.Load(_sequence)

			_stopWatch = New Stopwatch()
			_stopWatch.Start()

			_playback.Start()

			_playing = True

			Do While _playing
				' if we hit our interval
				If (_stopWatch.ElapsedMilliseconds - last) >= _sequence.Interval Then
					' make sure we're still inside the bounds of the song
					If _tickCount >= _sequence.Channels(0).Data.Length Then
						' dump out and let anyone listening know it's all over
						Thread.Sleep(100)
						_stopWatch.Stop()
						_playback.Stop()
						OnSequenceStopped(New EventArgs())
						Return
					End If

					' every time we tick, set the output port for the current channel on or off
					For Each c As Channel In _sequence.Channels
						If PhidgetHandler.IFKits(c.SerialNumber).outputs(c.OutputIndex) <> c.Data(_tickCount) Then
							PhidgetHandler.IFKits(c.SerialNumber).outputs(c.OutputIndex) = c.Data(_tickCount)
						End If
					Next c

					_tickCount += 1
					last = (_stopWatch.ElapsedMilliseconds - ((_stopWatch.ElapsedMilliseconds - last) - _sequence.Interval))
				Else
					Thread.Sleep(CInt(Fix(_stopWatch.ElapsedMilliseconds - last))) ' give the CPU a break until it's time to act again
				End If
			Loop
			Thread.Sleep(100)
			_stopWatch.Stop()
			_playback.Stop()
		End Sub

		Public Overrides Sub Unload()
			Me.Stop()
			_playback.Unload()
		End Sub

		Public Overrides Sub Load()
			Select Case _sequence.MusicType
				Case MusicType.Sample
					_playback = New MCIPlayback()
				Case MusicType.MIDI
					_playback = New MIDIPlayback()
			End Select
		End Sub
	End Class
End Namespace
