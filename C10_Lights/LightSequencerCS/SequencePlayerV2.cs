//////////////////////////////////////////////////////////////////////////////////
//	SequencePlayerV2.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Threading;

namespace LightSequencer
{
	public class SequencePlayerV2 : SequencePlayer
	{
		private IPlayback _playback;
		private Sequence _sequence;
		private int _tickCount;
		private Stopwatch _stopWatch;
		private Thread _playbackThread;
		private bool _playing;

		public SequencePlayerV2(Sequence s)
		{
			_sequence = s;

			switch(_sequence.MusicType)
			{
				case MusicType.Sample:
					_playback = new MCIPlayback();
					break;
				case MusicType.MIDI:
					_playback = new MIDIPlayback();
					break;
			}

			// close any open music files
			_playback.Unload();

			_playback.Load(s);
		}

		public override void Start()
		{
			_playbackThread = new Thread(new ThreadStart(ThreadHandler));
			_playbackThread.Name = "V2 playback thread";
			_playbackThread.Start();
		}

		public override void Stop()
		{
			_playing = false;
		}

		private void ThreadHandler()
		{
			float last = 0;
			_tickCount = 0;

			_playback.Load(_sequence);

			_stopWatch = new Stopwatch();
			_stopWatch.Start();

			_playback.Start();

			_playing = true;

			while(_playing)
			{
				// if we hit our interval
				if((_stopWatch.ElapsedMilliseconds - last) >= _sequence.Interval)
				{
					// make sure we're still inside the bounds of the song
					if(_tickCount >= _sequence.Channels[0].Data.Length)
					{
						// dump out and let anyone listening know it's all over
						Thread.Sleep(100);
						_stopWatch.Stop();
						_playback.Stop();
						OnSequenceStopped(new EventArgs());
						return;
					}

					// every time we tick, set the output port for the current channel on or off
					foreach(Channel c in _sequence.Channels)
					{
						if(PhidgetHandler.IFKits[c.SerialNumber].outputs[c.OutputIndex] != c.Data[_tickCount])
							PhidgetHandler.IFKits[c.SerialNumber].outputs[c.OutputIndex] = c.Data[_tickCount];
					}

					_tickCount++;
					last = (_stopWatch.ElapsedMilliseconds - ((_stopWatch.ElapsedMilliseconds - last) - _sequence.Interval));
				}
				else
					Thread.Sleep((int)(_stopWatch.ElapsedMilliseconds - last));	// give the CPU a break until it's time to act again
			}
			Thread.Sleep(100);
			_stopWatch.Stop();
			_playback.Stop();
		}

		public override void Unload()
		{
			Stop();
			_playback.Unload();
		}

		public override void Load()
		{
			switch(_sequence.MusicType)
			{
				case MusicType.Sample:
					_playback = new MCIPlayback();
					break;
				case MusicType.MIDI:
					_playback = new MIDIPlayback();
					break;
			}
		}
	}
}
