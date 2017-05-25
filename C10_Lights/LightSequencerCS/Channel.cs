//////////////////////////////////////////////////////////////////////////////////
//	Channel.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

namespace LightSequencer
{
	public class Channel
	{
		// serial number of the phidget device mapped to this channel
		private int _serialNumber;

		// index into output array of the phidget device mapped to this channel
		private int _outputIndex;

		// overall channel number
		private int _channelNumber;

		private int _MIDIChannel;

		// array of off/on states for each second tick
		private bool[] _data;

		public Channel(int channelNumber)
		{
			_channelNumber = channelNumber;
		}

		public Channel(int channelNumber, int serialNumber, int outputIndex)
		{
			_channelNumber = channelNumber;
			_serialNumber = serialNumber;
			_outputIndex = outputIndex;
		}

		public Channel(int channelNumber, int serialNumber, int outputIndex, int midiChannel, int dataLength)
		{
			_channelNumber = channelNumber;
			_serialNumber = serialNumber;
			_outputIndex = outputIndex;
			_MIDIChannel = midiChannel;
			_data = new bool[dataLength];
		}

		public Channel(int channelNumber, int serialNumber, int outputIndex, int dataLength)
		{
			_channelNumber = channelNumber;
			_serialNumber = serialNumber;
			_outputIndex = outputIndex;
			_data = new bool[dataLength];
		}

		public int Number
		{
			get { return _channelNumber; }
			set { _channelNumber = value; }
		}

		public int SerialNumber
		{
			get { return  _serialNumber; }
			set { _serialNumber = value; }
		}

		public int OutputIndex
		{
			get { return _outputIndex; }
			set { _outputIndex = value; }
		}

		public int MIDIChannel
		{
			get { return this._MIDIChannel; }
			set { this._MIDIChannel = value; }
		}

		public bool[] Data
		{
			get { return _data; }
			set { _data = value; }
		}
	}
}
