'////////////////////////////////////////////////////////////////////////////////
'	SequencePlayerV1.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Namespace LightSequencer
	Public Class SequencePlayerV1
		Inherits SequencePlayer
		Private _playback As IPlayback
		Private tmrTimer As Timer
		Private _sequence As Sequence
		Private _tickCount As Integer

		Public Sub New(ByVal s As Sequence)
			_sequence = s

			' V1 sequence only knows about the MCI engine
			_playback = New MCIPlayback()

			' close any open music files
			_playback.Unload()

			_playback.Load(s)

			tmrTimer = New Timer()
			tmrTimer.Interval = CInt(Fix(_sequence.Interval))
			AddHandler tmrTimer.Tick, AddressOf tmrTimer_Tick
		End Sub

		Private Sub tmrTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			If _tickCount >= _sequence.Channels(0).Data.Length Then
				_playback.Stop()
				tmrTimer.Enabled = False
				OnSequenceStopped(New EventArgs())
				Return
			End If

			' every time we tick, set the output port for the current channel on or off
			For Each c As Channel In _sequence.Channels
				PhidgetHandler.IFKits(c.SerialNumber).outputs(c.OutputIndex) = c.Data(_tickCount)
			Next c

			_tickCount += 1
		End Sub

		Public Overrides Sub Start()
			_tickCount = 0
			Application.DoEvents() ' the timer doesn't want to start all the time...
			tmrTimer.Enabled = True
			_playback.Start()
		End Sub

		Public Overrides Sub [Stop]()
			_playback.Stop()
			tmrTimer.Enabled = False
		End Sub

		Public Overrides Sub Unload()
			Me.Stop()
			_playback.Unload()
		End Sub

		Public Overrides Sub Load()
			_playback.Load(_sequence)
		End Sub
	End Class
End Namespace
