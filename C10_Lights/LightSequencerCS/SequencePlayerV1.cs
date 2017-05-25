//////////////////////////////////////////////////////////////////////////////////
//	SequencePlayerV1.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace LightSequencer
{
	public class SequencePlayerV1 : SequencePlayer
	{
		private IPlayback _playback;
		private Timer tmrTimer;
		private Sequence _sequence;
		private int _tickCount;

		public SequencePlayerV1(Sequence s)
		{
			_sequence = s;

			// V1 sequence only knows about the MCI engine
			_playback = new MCIPlayback();

			// close any open music files
			_playback.Unload();

			_playback.Load(s);

			tmrTimer = new Timer();
			tmrTimer.Interval = (int)_sequence.Interval;
			tmrTimer.Tick += new EventHandler(tmrTimer_Tick);
		}

		void tmrTimer_Tick(object sender, EventArgs e)
		{
			if(_tickCount >= _sequence.Channels[0].Data.Length)
			{
				_playback.Stop();
				tmrTimer.Enabled = false;
				OnSequenceStopped(new EventArgs());
				return;
			}

			// every time we tick, set the output port for the current channel on or off
			foreach(Channel c in _sequence.Channels)
				PhidgetHandler.IFKits[c.SerialNumber].outputs[c.OutputIndex] = c.Data[_tickCount];

			_tickCount++;
		}

		public override void Start()
		{
			_tickCount = 0;
			Application.DoEvents();	// the timer doesn't want to start all the time...
			tmrTimer.Enabled = true;
			_playback.Start();
		}

		public override void Stop()
		{
			_playback.Stop();
			tmrTimer.Enabled = false;
		}

		public override void Unload()
		{
			Stop();
			_playback.Unload();
		}

		public override void Load()
		{
			_playback.Load(_sequence);
		}
	}
}
