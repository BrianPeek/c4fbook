'////////////////////////////////////////////////////////////////////////////////
'	RecordForm.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Threading
Imports System.Windows.Forms

Namespace LightSequencer
	Partial Public Class RecordForm
		Inherits Form
		' list of all channels in sequence
		Private _channels As List(Of Channel)

		' keypress data that will be copied out later if kept
		Private _tempData As List(Of Boolean()) = New List(Of Boolean())()

		Private _keyMap As Dictionary(Of Keys, Integer) = New Dictionary(Of Keys,Integer)()
		Private _sequence As Sequence
		Private _thread As Thread

		' state of current channel
		Private _on() As Boolean

		' index into tempData array
		Private _tickCount As Integer = 0

		' song still playing?
		Private _playing As Boolean

		' MCIPlayback or MIDIPlayback
		Private _playback As IPlayback

		Public Sub New()
			InitializeComponent()

			' setup the key map
			_keyMap.Add(Keys.D1, 0)
			_keyMap.Add(Keys.D2, 1)
			_keyMap.Add(Keys.D3, 2)
			_keyMap.Add(Keys.D4, 3)
			_keyMap.Add(Keys.D5, 4)
			_keyMap.Add(Keys.D6, 5)
			_keyMap.Add(Keys.D7, 6)
			_keyMap.Add(Keys.D8, 7)
			_keyMap.Add(Keys.D9, 8)
			_keyMap.Add(Keys.D0, 9)
			_keyMap.Add(Keys.Q, 10)
			_keyMap.Add(Keys.W, 11)
			_keyMap.Add(Keys.E, 12)
			_keyMap.Add(Keys.R, 13)
			_keyMap.Add(Keys.T, 14)
			_keyMap.Add(Keys.Y, 15)
			_keyMap.Add(Keys.U, 16)
			_keyMap.Add(Keys.I, 17)
			_keyMap.Add(Keys.O, 18)
			_keyMap.Add(Keys.P, 19)
			_keyMap.Add(Keys.A, 20)
			_keyMap.Add(Keys.S, 21)
			_keyMap.Add(Keys.D, 22)
			_keyMap.Add(Keys.F, 23)
			_keyMap.Add(Keys.G, 24)
			_keyMap.Add(Keys.H, 25)
			_keyMap.Add(Keys.J, 26)
			_keyMap.Add(Keys.K, 27)
			_keyMap.Add(Keys.L, 28)
			_keyMap.Add(Keys.OemSemicolon, 29)
			_keyMap.Add(Keys.Z, 30)
			_keyMap.Add(Keys.X, 31)
			_keyMap.Add(Keys.C, 32)
			_keyMap.Add(Keys.V, 33)
			_keyMap.Add(Keys.B, 34)
			_keyMap.Add(Keys.N, 35)
			_keyMap.Add(Keys.M, 36)
			_keyMap.Add(Keys.Oemcomma, 37)
			_keyMap.Add(Keys.OemPeriod, 38)
			_keyMap.Add(Keys.OemQuestion, 39)
		End Sub

		Private Sub ToggleChannel(ByVal channel As Integer, ByVal [on] As Boolean)
			' as long as we're in range
			If channel < _channels.Count Then
				' turn the channel on/off
				_on(channel) = [on]

				' turn the actual channel on/off if the Phidget is attached
				If PhidgetHandler.IFKits(_channels(channel).SerialNumber).Attached Then
					PhidgetHandler.IFKits(_channels(channel).SerialNumber).outputs(_channels(channel).OutputIndex) = [on]
				End If
			End If
		End Sub

		Private Function StopRecording() As DialogResult
			tmrCountdown.Enabled = False

			_playing = False

			btnStart.Text = "&Start"

			_playback.Stop()
			lblCountdown.Text = "3"

			Dim dr As DialogResult = MessageBox.Show("Do you want to save the recorded data to the main grid?", "Save Recording", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

			Select Case dr
				Case System.Windows.Forms.DialogResult.Yes
					' save off the channel data to the main grid
					For i As Integer = 0 To _tempData.Count - 1
						For j As Integer = 0 To _tempData(i).Length - 1
							If _tempData(i)(j) OrElse radOverwrite.Checked Then
								_channels(i).Data(j) = _tempData(i)(j)
							End If
						Next j
					Next i
					Me.Close()
				Case System.Windows.Forms.DialogResult.No
					For i As Integer = 0 To _tempData.Count - 1
						Array.Clear(_tempData(i), 0, _tempData(i).Length)
					Next i
			End Select

			Return dr
		End Function

		Private Sub RecordForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
			' otherwise, modify data for channel based on key pressed
			If _playing AndAlso _keyMap.ContainsKey(e.KeyData) Then
				ToggleChannel(_keyMap(e.KeyData), True)
			End If
		End Sub

		Private Sub RecordForm_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
			' key released, set back to false
			If _playing AndAlso _keyMap.ContainsKey(e.KeyData) Then
				ToggleChannel(_keyMap(e.KeyData), False)
			End If
		End Sub

		Private Sub RecordForm_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles MyBase.KeyPress
			' stop if person presses escape (needs to be handled here so the form doesn't close automatically)
			If e.KeyChar = ChrW(Keys.Escape) Then
				btnStart.PerformClick()
				e.Handled = True
			End If
		End Sub

		Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
			Me.Close()
		End Sub

		' when start button is clicked, start a quick countdown and then enable the recording timer
		Private Sub btnStart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStart.Click
			Dim cmd As String = String.Empty

			_tickCount = 0

			If _playing Then
				chkPlay.Enabled = True
				radAppend.Enabled = True
				radOverwrite.Enabled = True

				If StopRecording() <> System.Windows.Forms.DialogResult.Cancel Then
					Me.Close()
				End If
			Else
				_playing = True
				btnStart.Text = "Stop"
				lblCountdown.Text = "3"
				tmrCountdown.Enabled = True

				chkPlay.Enabled = False
				radAppend.Enabled = False
				radOverwrite.Enabled = False
			End If
		End Sub

		Private Sub tmrCountdown_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrCountdown.Tick
			' countdown from 5-0
			lblCountdown.Text = (Integer.Parse(lblCountdown.Text) - 1).ToString()

			If lblCountdown.Text = "0" Then
				tmrCountdown.Enabled = False

				_thread = New Thread(New ThreadStart(AddressOf RecordThread))
				_thread.Start()
			End If
		End Sub

		Private Sub RecordThread()
			Dim last As Single = 0
			Dim s As New Stopwatch()

			_playback.Load(_sequence)

			s.Start()
			_playback.Start()

			_playing = True

			Do While _playing
				' if we've elapsed enough time, process keys
				If (s.ElapsedMilliseconds - last) > _sequence.Interval Then
					' if we're at the end, bail out
					If _tickCount >= _channels(0).Data.Length Then
						btnStart.PerformClick()
					End If

					' for each channel, set the ticks on and off as keys are pressed
					For i As Integer = 0 To _channels.Count - 1
						If _on(i) Then
							_tempData(i)(_tickCount) = _on(i)
						End If

						' every time we tick, set the output port for the current channel on or off
						If chkPlay.Checked AndAlso (Not _on(i)) Then
							PhidgetHandler.IFKits(_channels(i).SerialNumber). _
								outputs(_channels(i).OutputIndex) = _channels(i).Data(_tickCount)
						End If
					Next i
					_tickCount += 1

					' maintain the last time we did this
					last = (s.ElapsedMilliseconds - ((s.ElapsedMilliseconds - last) - _sequence.Interval))
				End If
			Loop

			_playback.Stop()
		End Sub

		Public Property Sequence() As Sequence
			Get
				Return _sequence
			End Get
			Set(ByVal value As Sequence)
				_sequence = value
				_channels = _sequence.Channels
				_on = New Boolean(_channels.Count - 1){}

				' create some temporary arrays to store our new data
				For i As Integer = 0 To _channels.Count - 1
					_tempData.Add(New Boolean(_channels(i).Data.Length - 1){})
				Next i

				Select Case _sequence.MusicType
					Case MusicType.Sample
						_playback = New MCIPlayback()
					Case MusicType.MIDI
						_playback = New MIDIPlayback()
				End Select
			End Set
		End Property

		Private Sub RecordForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
			If _playing Then
				e.Cancel = True
			End If
			_playback.Unload()
		End Sub
	End Class
End Namespace