'////////////////////////////////////////////////////////////////////////////////
'	SequencePlayer.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System

Namespace LightSequencer
	Public MustInherit Class SequencePlayer
		Public MustOverride Sub Start()
		Public MustOverride Sub [Stop]()
		Public MustOverride Sub Load()
		Public MustOverride Sub Unload()

		Public Event SequenceStopped As EventHandler

		Protected Overridable Sub OnSequenceStopped(ByVal e As EventArgs)
			RaiseEvent SequenceStopped(Me, e)
		End Sub
	End Class
End Namespace
