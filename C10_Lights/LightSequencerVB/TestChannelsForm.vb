'////////////////////////////////////////////////////////////////////////////////
'	TestChannelsForm.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Namespace LightSequencer
	Partial Public Class TestChannelsForm
		Inherits Form
		Private _sequence As Sequence

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub ToggleChannel(ByVal channel As Integer, ByVal [on] As Boolean)
			If channel < _sequence.Channels.Count Then
				PhidgetHandler.IFKits(_sequence.Channels(channel).SerialNumber).outputs(_sequence.Channels(channel).OutputIndex) = [on]
			End If
		End Sub

		Private Sub TestChannelsForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
			' otherwise, modify data for channel 1-9 based on key pressed
			If e.KeyData >= Keys.D0 AndAlso e.KeyData <= Keys.D9 Then
				ToggleChannel((e.KeyValue - CInt(Fix(Keys.D0))) - 1, True)
			End If

			If e.KeyData >= Keys.NumPad0 AndAlso e.KeyData <= Keys.NumPad9 Then
				ToggleChannel((e.KeyValue - CInt(Fix(Keys.NumPad0))) - 1, True)
			End If
		End Sub

		Private Sub TestChannelsForm_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
			' key released, set back to false
			If e.KeyData >= Keys.D0 AndAlso e.KeyData <= Keys.D9 Then
				ToggleChannel((e.KeyValue - CInt(Fix(Keys.D0))) - 1, False)
			End If

			If e.KeyData >= Keys.NumPad0 AndAlso e.KeyData <= Keys.NumPad9 Then
				ToggleChannel((e.KeyValue - CInt(Fix(Keys.NumPad0))) - 1, False)
			End If
		End Sub

		Public Property Sequence() As Sequence
			Get
				Return _sequence
			End Get
			Set(ByVal value As Sequence)
				_sequence = value
			End Set
		End Property

		Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
			Me.Close()
		End Sub

		Private Sub chkAllOn_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAllOn.CheckedChanged
			For i As Integer = 0 To _sequence.Channels.Count - 1
				ToggleChannel(i, chkAllOn.Checked)
			Next i
		End Sub
	End Class
End Namespace