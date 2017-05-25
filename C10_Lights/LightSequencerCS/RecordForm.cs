//////////////////////////////////////////////////////////////////////////////////
//	RecordForm.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace LightSequencer
{
	public partial class RecordForm : Form
	{
		// list of all channels in sequence
		private List<Channel> _channels;

		// keypress data that will be copied out later if kept
		private List<bool[]> _tempData = new List<bool[]>();

		private Dictionary<Keys, int> _keyMap = new Dictionary<Keys,int>();
		private Sequence _sequence;
		private Thread _thread;

		// state of current channel
		private bool[] _on;

		// index into tempData array
		private int _tickCount = 0;

		// song still playing?
		private bool _playing;

		// MCIPlayback or MIDIPlayback
		private IPlayback _playback;

		public RecordForm()
		{
			InitializeComponent();

			// setup the key map
			_keyMap.Add(Keys.D1, 0);
			_keyMap.Add(Keys.D2, 1);
			_keyMap.Add(Keys.D3, 2);
			_keyMap.Add(Keys.D4, 3);
			_keyMap.Add(Keys.D5, 4);
			_keyMap.Add(Keys.D6, 5);
			_keyMap.Add(Keys.D7, 6);
			_keyMap.Add(Keys.D8, 7);
			_keyMap.Add(Keys.D9, 8);
			_keyMap.Add(Keys.D0, 9);
			_keyMap.Add(Keys.Q, 10);
			_keyMap.Add(Keys.W, 11);
			_keyMap.Add(Keys.E, 12);
			_keyMap.Add(Keys.R, 13);
			_keyMap.Add(Keys.T, 14);
			_keyMap.Add(Keys.Y, 15);
			_keyMap.Add(Keys.U, 16);
			_keyMap.Add(Keys.I, 17);
			_keyMap.Add(Keys.O, 18);
			_keyMap.Add(Keys.P, 19);
			_keyMap.Add(Keys.A, 20);
			_keyMap.Add(Keys.S, 21);
			_keyMap.Add(Keys.D, 22);
			_keyMap.Add(Keys.F, 23);
			_keyMap.Add(Keys.G, 24);
			_keyMap.Add(Keys.H, 25);
			_keyMap.Add(Keys.J, 26);
			_keyMap.Add(Keys.K, 27);
			_keyMap.Add(Keys.L, 28);
			_keyMap.Add(Keys.OemSemicolon, 29);
			_keyMap.Add(Keys.Z, 30);
			_keyMap.Add(Keys.X, 31);
			_keyMap.Add(Keys.C, 32);
			_keyMap.Add(Keys.V, 33);
			_keyMap.Add(Keys.B, 34);
			_keyMap.Add(Keys.N, 35);
			_keyMap.Add(Keys.M, 36);
			_keyMap.Add(Keys.Oemcomma, 37);
			_keyMap.Add(Keys.OemPeriod, 38);
			_keyMap.Add(Keys.OemQuestion, 39);
		}

		private void ToggleChannel(int channel, bool on)
		{
			// as long as we're in range
			if(channel < _channels.Count)
			{
				// turn the channel on/off
				_on[channel] = on;

				// turn the actual channel on/off if the Phidget is attached
				if(PhidgetHandler.IFKits[_channels[channel].SerialNumber].Attached)
					PhidgetHandler.IFKits[_channels[channel].SerialNumber].outputs[_channels[channel].OutputIndex] = on;
			}
		}

		private DialogResult StopRecording()
		{
			tmrCountdown.Enabled = false;

			_playing = false;

			btnStart.Text = "&Start";

			_playback.Stop();
			lblCountdown.Text = "3";

			DialogResult dr = MessageBox.Show("Do you want to save the recorded data to the main grid?", "Save Recording", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			switch(dr)
			{
				case DialogResult.Yes:
					// save off the channel data to the main grid
					for(int i = 0; i < _tempData.Count; i++)
					{
						for(int j = 0; j < _tempData[i].Length; j++)
						{
							if(_tempData[i][j] || radOverwrite.Checked)
								_channels[i].Data[j] = _tempData[i][j];
						}
					}
					this.Close();
					break;
				case DialogResult.No:
					for(int i = 0; i < _tempData.Count; i++)
						Array.Clear(_tempData[i], 0, _tempData[i].Length);
					break;
			}

			return dr;
		}

		private void RecordForm_KeyDown(object sender, KeyEventArgs e)
		{
			// otherwise, modify data for channel based on key pressed
			if(_playing && _keyMap.ContainsKey(e.KeyData))
				ToggleChannel(_keyMap[e.KeyData], true);
		}

		private void RecordForm_KeyUp(object sender, KeyEventArgs e)
		{
			// key released, set back to false
			if(_playing && _keyMap.ContainsKey(e.KeyData))
				ToggleChannel(_keyMap[e.KeyData], false);
		}

		private void RecordForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			// stop if person presses escape (needs to be handled here so the form doesn't close automatically)
			if(e.KeyChar == (char)Keys.Escape)
			{
				btnStart.PerformClick();
				e.Handled = true;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// when start button is clicked, start a quick countdown and then enable the recording timer
		private void btnStart_Click(object sender, EventArgs e)
		{
			string cmd = string.Empty;

			_tickCount = 0;

			if(_playing)
			{
				chkPlay.Enabled = true;
				radAppend.Enabled = true;
				radOverwrite.Enabled = true;

				if(StopRecording() != DialogResult.Cancel)
					this.Close();
			}
			else
			{
				_playing = true;
				btnStart.Text = "Stop";
				lblCountdown.Text = "3";
				tmrCountdown.Enabled = true;

				chkPlay.Enabled = false;
				radAppend.Enabled = false;
				radOverwrite.Enabled = false;
			}
		}

		private void tmrCountdown_Tick(object sender, EventArgs e)
		{
			// countdown from 5-0
			lblCountdown.Text = (int.Parse(lblCountdown.Text) - 1).ToString();

			if(lblCountdown.Text == "0")
			{
				tmrCountdown.Enabled = false;

				_thread = new Thread(new ThreadStart(RecordThread));
				_thread.Start();
			}
		}

		private void RecordThread()
		{
			float last = 0;
			Stopwatch s = new Stopwatch();

			_playback.Load(_sequence);

			s.Start();
			_playback.Start();

			_playing = true;

			while(_playing)
			{
				// if we've elapsed enough time, process keys
				if((s.ElapsedMilliseconds - last) > _sequence.Interval)
				{
					// if we're at the end, bail out
					if(_tickCount >= _channels[0].Data.Length)
						btnStart.PerformClick();

					// for each channel, set the ticks on and off as keys are pressed
					for(int i = 0; i < _channels.Count; i++)
					{
						if(_on[i])
							_tempData[i][_tickCount] = _on[i];

						// every time we tick, set the output port for the current channel on or off
						if(chkPlay.Checked && !_on[i])
							PhidgetHandler.IFKits[_channels[i].SerialNumber].outputs[_channels[i].OutputIndex] = _channels[i].Data[_tickCount];
					}
					_tickCount++;

					// maintain the last time we did this
					last = (s.ElapsedMilliseconds - ((s.ElapsedMilliseconds - last) - _sequence.Interval));
				}
			}

			_playback.Stop();
		}

		public Sequence Sequence
		{
			get { return _sequence; }
			set
			{
				_sequence = value;
				_channels = _sequence.Channels;
				_on = new bool[_channels.Count];

				// create some temporary arrays to store our new data
				for(int i = 0; i < _channels.Count; i++)
					_tempData.Add(new bool[_channels[i].Data.Length]);

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

		private void RecordForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(_playing)
				e.Cancel = true;
			_playback.Unload();
		}
	}
}