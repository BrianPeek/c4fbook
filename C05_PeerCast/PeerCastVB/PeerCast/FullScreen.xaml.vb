Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Media

Namespace PeerCast
	Partial Public Class FullScreen
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub

		Public Sub SetMediaPlayer(ByVal videoPath As Uri)
			MediaPlayer.Source = videoPath
			MediaPlayer.Play()
		End Sub
	End Class
End Namespace
