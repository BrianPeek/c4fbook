'////////////////////////////////////////////////////////////////////////////////
'	MCIPlayback.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports System.Text

Namespace LightSequencer
	Public Class MCIPlayback
		Implements IPlayback
		' dll export to MCI for playing music
		<DllImport("winmm.dll")> _
		Shared Function mciSendString(ByVal command As String, ByVal buffer As StringBuilder, ByVal bufferSize As Int32, ByVal hwndCallback As IntPtr) As Int32
		End Function

		Public Sub Load(ByVal seq As Sequence) Implements IPlayback.Load
			Dim cmd As String = "open """ & seq.MusicFile & """ type mpegvideo alias MediaFile"
			mciSendString(cmd, Nothing, 0, IntPtr.Zero)
		End Sub

		Public Sub Unload() Implements IPlayback.Unload
			Dim cmd As String = "close MediaFile"
			mciSendString(cmd, Nothing, 0, IntPtr.Zero)
		End Sub

		Public Sub Start() Implements IPlayback.Start
			Dim cmd As String = "play MediaFile from 0"
			mciSendString(cmd, Nothing, 0, IntPtr.Zero)
		End Sub

		Public Sub [Stop]() Implements IPlayback.Stop
			Dim cmd As String = "stop MediaFile"
			mciSendString(cmd, Nothing, 0, IntPtr.Zero)
		End Sub
	End Class
End Namespace
