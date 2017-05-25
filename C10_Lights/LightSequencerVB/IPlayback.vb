'////////////////////////////////////////////////////////////////////////////////
'	IPlayback.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Public Interface IPlayback
		Sub Load(ByVal seq As Sequence)
		Sub Unload()
		Sub Start()
		Sub [Stop]()
	End Interface
End Namespace
