//////////////////////////////////////////////////////////////////////////////////
//	MCIPlayback.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using Sanford.Multimedia.Midi;
using Midi = Sanford.Multimedia.Midi;

namespace LightSequencer
{
	public class MIDIPlayback : IPlayback
	{
		private Midi.Sequencer _MIDISequencer;
		private Midi.Sequence _MIDISequence;
		private Midi.OutputDevice _MIDIOutDevice;

		public MIDIPlayback()
		{
			_MIDISequencer = new Midi.Sequencer();
			_MIDISequence = new Midi.Sequence();

			// event handlers for various message types
			_MIDISequencer.ChannelMessagePlayed += sequencer_ChannelMessagePlayed;
			_MIDISequencer.Chased += sequencer_Chased;
			_MIDISequencer.Stopped += sequencer_Stopped;
		}

		public void Start()
		{
			_MIDISequencer.Start();
		}

		public void Load(Sequence seq)
		{
			// grab the first MIDI device
			if(_MIDIOutDevice == null)
				_MIDIOutDevice = new Midi.OutputDevice(0);

			// load the MIDI file
			_MIDISequence.Load(seq.MusicFile);
			_MIDISequencer.Sequence = _MIDISequence;
		}

		public void Unload()
		{
			if(_MIDISequence != null)
				_MIDISequence.Clear();
			if(_MIDIOutDevice != null)
				_MIDIOutDevice.Close();
		}

		public void Stop()
		{
			_MIDISequencer.Stop();
		}

		void sequencer_Stopped(object sender, StoppedEventArgs e)
		{
			// send "stop" messages to the sound card
            foreach(ChannelMessage message in e.Messages)
            {
				if(!_MIDIOutDevice.IsDisposed)
					_MIDIOutDevice.Send(message);
            }
		}

		void sequencer_Chased(object sender, Sanford.Multimedia.Midi.ChasedEventArgs e)
		{
			// send "chased" messages to the sound card
            foreach(ChannelMessage message in e.Messages)
            {
				if(!_MIDIOutDevice.IsDisposed)
					_MIDIOutDevice.Send(message);
            }
		}

		void sequencer_ChannelMessagePlayed(object sender, Sanford.Multimedia.Midi.ChannelMessageEventArgs e)
		{
			// send each MIDI command to the sound card
			if(!_MIDIOutDevice.IsDisposed)
				_MIDIOutDevice.Send(e.Message);
		}
	}
}
