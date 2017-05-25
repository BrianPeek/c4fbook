'////////////////////////////////////////////////////////////////////////////////
'	Channel.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Public Class Channel
		' serial number of the phidget device mapped to this channel
		Private _serialNumber As Integer

		' index into output array of the phidget device mapped to this channel
		Private _outputIndex As Integer

		' overall channel number
		Private _channelNumber As Integer

		Private _MIDIChannel As Integer

		' array of off/on states for each second tick
		Private _data() As Boolean

		Public Sub New(ByVal channelNumber As Integer)
			_channelNumber = channelNumber
		End Sub

		Public Sub New(ByVal channelNumber As Integer, ByVal serialNumber As Integer, ByVal outputIndex As Integer)
			_channelNumber = channelNumber
			_serialNumber = serialNumber
			_outputIndex = outputIndex
		End Sub

		Public Sub New(ByVal channelNumber As Integer, ByVal serialNumber As Integer, ByVal outputIndex As Integer, ByVal midiChannel As Integer, ByVal dataLength As Integer)
			_channelNumber = channelNumber
			_serialNumber = serialNumber
			_outputIndex = outputIndex
			_MIDIChannel = midiChannel
			_data = New Boolean(dataLength - 1){}
		End Sub

		Public Sub New(ByVal channelNumber As Integer, ByVal serialNumber As Integer, ByVal outputIndex As Integer, ByVal dataLength As Integer)
			_channelNumber = channelNumber
			_serialNumber = serialNumber
			_outputIndex = outputIndex
			_data = New Boolean(dataLength - 1){}
		End Sub

		Public Property Number() As Integer
			Get
				Return _channelNumber
			End Get
			Set(ByVal value As Integer)
				_channelNumber = value
			End Set
		End Property

		Public Property SerialNumber() As Integer
			Get
				Return _serialNumber
			End Get
			Set(ByVal value As Integer)
				_serialNumber = value
			End Set
		End Property

		Public Property OutputIndex() As Integer
			Get
				Return _outputIndex
			End Get
			Set(ByVal value As Integer)
				_outputIndex = value
			End Set
		End Property

		Public Property MIDIChannel() As Integer
			Get
				Return Me._MIDIChannel
			End Get
			Set(ByVal value As Integer)
				Me._MIDIChannel = value
			End Set
		End Property

		Public Property Data() As Boolean()
			Get
				Return _data
			End Get
			Set(ByVal value As Boolean())
				_data = value
			End Set
		End Property
	End Class
End Namespace
