//////////////////////////////////////////////////////////////////////////////////
//	NewSequenceForm.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Phidgets;
using Midi = Sanford.Multimedia.Midi;

namespace LightSequencer
{
	public partial class NewSequenceForm : Form
	{
		// store away information on a MIDI channel
		private struct MIDIChannel
		{
			private int _channel;
			private string _name;

			public int Channel
			{
				get { return this._channel; }
				set { this._channel = value; }
			}

			public string Name
			{
				get { return this._name; }
				set { this._name = value; }
			}

			public string Display
			{
				get { return this.Channel + ": " + this.Name; }
			}

			public MIDIChannel(int c, string n) { _channel = c; _name = n; }
		}

		private int _numChannels = 0;
		private Sequence _sequence;
		private bool _edit = false;

		// binding to the combo box in the device list
		private BindingList<MIDIChannel> _MIDIChannels;

		// map a midi channel to a phidget output
		Dictionary<int, int> _midiMap = new Dictionary<int,int>();

		public NewSequenceForm() : this(null)
		{
		}

		public NewSequenceForm(Sequence s)
		{
			InitializeComponent();

			// respond to change events from the new PhidgetHandler class
			PhidgetHandler.PhidgetsChanged += new EventHandler(PhidgetHandler_PhidgetsChanged);

			if(PhidgetHandler.IFKits.Count == 0)
			{
				btnOK.Enabled = false;
				MessageBox.Show("No Phidget Interface Kits found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			// we're in edit mode...fill in existing data
			if(s != null)
			{
				this.Text = "Edit Title Info";

				txtArtist.Text = s.Artist;
				txtFile.Text = s.MusicFile;
				txtMinutes.Text = (s.MusicLength / 60).ToString();
				txtSeconds.Text = ((s.MusicLength % 60) - 3).ToString();
				txtTitle.Text = s.Title;

				_sequence = s;
				_edit = true;
				txtMinutes.Enabled = false;
				txtSeconds.Enabled = false;
				txtFile.Enabled = false;
				btnBrowse.Enabled = false;
				chkHold.Enabled = false;

				if(_sequence.MusicType == MusicType.MIDI)
				{
					GetMIDIInfo(_sequence.MusicFile, false);
					SwitchGrid(true);
				}
			}

			// setup the device grid
			FillPhidgetGrid();
		}

		void PhidgetHandler_PhidgetsChanged(object sender, EventArgs e)
		{
			FillPhidgetGrid();
		}

		private void FillPhidgetGrid()
		{
			dgvDevices.Rows.Clear();
			_numChannels = 0;

			foreach(InterfaceKit ik in PhidgetHandler.IFKits.Values)
			{
		        int currentChannel = _numChannels;

		        // parse out the number of output channels the device supports (analog in/digitial in/digitial out)
		        _numChannels += ik.outputs.Count;

		        // add device to the grid
		        for(int i = 0; i < ik.outputs.Count; i++)
				{
		            dgvDevices.Rows.Add(ik.Name, ik.SerialNumber, i+1+currentChannel, -1, i);

					if(_edit && i < _sequence.Channels.Count)
					{
						if(_sequence.MusicType == MusicType.MIDI)
							dgvDevices.Rows[i].Cells["colMIDIChannel"].Value = _sequence.Channels[i].MIDIChannel;

						dgvDevices.Rows[i].Cells["colPhidgetOutput"].Value = _sequence.Channels[i].OutputIndex;
					}
					else
					{
						// if we have MIDI info, fill in the defaults
						if(_sequence != null && _sequence.MusicType == MusicType.MIDI && _MIDIChannels != null && i < _MIDIChannels.Count)
							dgvDevices.Rows[i].Cells["colMIDIChannel"].Value = i;
					}
				}
			}

			// enable/disable OK button
			btnOK.Enabled = (dgvDevices.Rows.Count > 0);
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Locate music file...";
			ofd.Filter = "Music files (*.mp3;*.wma;*.wav;*.mid)|*.mp3;*.wma;*.wav;*.mid|All files (*.*)|*.*";
			if(ofd.ShowDialog() == DialogResult.OK)
			{
				txtFile.Text = ofd.FileName;

				// if we have a MIDI file, show the MIDI column
				if(txtFile.Text.EndsWith(".mid"))
				{
					chkHold.Enabled = true;
					GetMIDIInfo(txtFile.Text, true);
					SwitchGrid(true);
				}
				else
				{
					chkHold.Enabled = false;
					SwitchGrid(false);
				}
			}
		}

		private void SwitchGrid(bool midi)
		{
			if(midi)
			{
				// enable MIDI column
				dgvDevices.Columns["colMIDIChannel"].Visible = true;
				dgvDevices.Columns["colChannels"].Visible = false;

				// bind the MIDI channels to the combo
				((DataGridViewComboBoxColumn)dgvDevices.Columns["colMIDIChannel"]).DisplayMember = "Display";
				((DataGridViewComboBoxColumn)dgvDevices.Columns["colMIDIChannel"]).ValueMember = "Channel";
				((DataGridViewComboBoxColumn)dgvDevices.Columns["colMIDIChannel"]).DataSource = _MIDIChannels;

				// default select every channel
				for(int i = 0; i < dgvDevices.Rows.Count; i++)
				{
					if(i < _MIDIChannels.Count)
						dgvDevices.Rows[i].Cells["colMIDIChannel"].Value = i;
				}
			}
			else
			{
				// disable MIDI column
				dgvDevices.Columns["colMIDIChannel"].Visible = false;
				dgvDevices.Columns["colChannels"].Visible = true;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if(txtMinutes.Text.Trim().Length == 0 || txtSeconds.Text.Trim().Length == 0 || int.Parse(txtSeconds.Text) > 59)
			{
				MessageBox.Show("Please enter a valid sequence length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if(!_edit)
				_sequence = new Sequence();

			if(txtFile.Text.EndsWith(".mid"))
				_sequence.MusicType = MusicType.MIDI;
			else
				_sequence.MusicType = MusicType.Sample;

			_sequence.Title = txtTitle.Text;
			_sequence.Artist = txtArtist.Text;

			if(_edit)
			{
				// if we have more channels than our devices can handle, remove the old channels
				if(_sequence.Channels.Count > dgvDevices.Rows.Count)
					_sequence.Channels.RemoveRange(dgvDevices.Rows.Count, _sequence.Channels.Count - dgvDevices.Rows.Count);

				int i = 0;

				// get chanel info
				foreach(DataGridViewRow row in dgvDevices.Rows)
				{
					if(i < _sequence.Channels.Count)
					{
						_sequence.Channels[i].SerialNumber = int.Parse(row.Cells["colSerialNumber"].Value.ToString());
						_sequence.Channels[i].OutputIndex = int.Parse(row.Cells["colPhidgetOutput"].Value.ToString());
						_sequence.Channels[i].MIDIChannel = int.Parse(row.Cells["colMIDIChannel"].Value.ToString());
					}
					i++;
				}
			}
			else
			{
				_sequence.Interval = 50;	// TODO: someday this will be different
				_sequence.MusicFile = txtFile.Text;
				_sequence.MusicLength = ((int.Parse(txtMinutes.Text) * 60) + int.Parse(txtSeconds.Text) + 3);

				// if we're not editing, create new channels
				int i = 0;

				foreach(DataGridViewRow row in dgvDevices.Rows)
				{
					_sequence.Channels.Add(new Channel(i++,
						int.Parse(row.Cells["colSerialNumber"].Value.ToString()),
						int.Parse(row.Cells["colPhidgetOutput"].Value.ToString()),
						int.Parse(row.Cells["colMIDIChannel"].Value.ToString()),
						(int)(1000/_sequence.Interval) * _sequence.MusicLength)
					);
					_midiMap[int.Parse(row.Cells["colMIDIChannel"].Value.ToString())] = int.Parse(row.Cells["colPhidgetOutput"].Value.ToString());
				}

				if(_sequence.MusicType == MusicType.MIDI)
					ProcessMIDI();
			}

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void GetMIDIInfo(string file, bool fillTime)
		{
			Midi.Sequence MIDISequence;

			int tempo = 500000;
			int maxtick = 0;
			float msPerTick = (tempo/48)/1000.0f;
			string title = string.Empty;

			_MIDIChannels = new BindingList<MIDIChannel>();

			// load the MIDI file
			MIDISequence = new Midi.Sequence();
			MIDISequence.Load(file);

			// we don't handle non-PPQN MIDI files
			if(MIDISequence.SequenceType != Sanford.Multimedia.Midi.SequenceType.Ppqn)
			{
				MessageBox.Show("Unsupported MIDI type...sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			foreach(Midi.Track t in MIDISequence)
			{
				// get the command enumerator
				IEnumerator<Midi.MidiEvent> en = t.Iterator().GetEnumerator();
				bool channelAdded = false;
				while(en.MoveNext())
				{
					Midi.MidiEvent e = en.Current;
					switch(e.MidiMessage.MessageType)
					{
						case Sanford.Multimedia.Midi.MessageType.Channel:	// track the # of channels
							Midi.ChannelMessage channel = (Midi.ChannelMessage)e.MidiMessage;
							if(!channelAdded)
							{
								_MIDIChannels.Add(new MIDIChannel(channel.MidiChannel, title));
								channelAdded = true;
							}
							break;

						case Sanford.Multimedia.Midi.MessageType.Meta:
							Midi.MetaMessage meta = (Midi.MetaMessage)e.MidiMessage;
							switch(meta.MetaType)
							{
								// cache away the track name for the grid
								case Sanford.Multimedia.Midi.MetaType.TrackName:
									title = Encoding.ASCII.GetString(meta.GetBytes());
									break;

								// get the tempo and convert to a time value we can use
								case Sanford.Multimedia.Midi.MetaType.Tempo:
									tempo = meta.GetBytes()[0] << 16 | meta.GetBytes()[1] << 8 | meta.GetBytes()[2];
									msPerTick = (tempo/MIDISequence.Division)/1000.0f;
									break;
							}
							break;
					}

					// find the highest time value
					if(e.AbsoluteTicks > maxtick)
						maxtick = e.AbsoluteTicks;
				}
			}
			// and use that value to fill in the minutes/seconds
			if(fillTime)
			{
				txtMinutes.Text =  (((int)(msPerTick * maxtick)/1000) / 60).ToString();
				txtSeconds.Text = (((int)(msPerTick * maxtick)/1000) % 60+1).ToString();
			}
		}

		private void ProcessMIDI()
		{
			Midi.Sequence _MIDISequence;

			// default MIDI tempos/milliseconds per tick
			int tempo = 500000;
			float msPerTick = (tempo/48)/1000.0f;

			_MIDISequence = new Midi.Sequence();
			_MIDISequence.Load(_sequence.MusicFile);

			if(_MIDISequence.SequenceType != Sanford.Multimedia.Midi.SequenceType.Ppqn)
			{
				MessageBox.Show("Unsupported MIDI type...sorry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			foreach(Midi.Track t in _MIDISequence)
			{
				IEnumerator<Midi.MidiEvent> en = t.Iterator().GetEnumerator();

				while(en.MoveNext())
				{
					Midi.MidiEvent e = en.Current;
					switch(e.MidiMessage.MessageType)
					{
						// starta  new channel
						case Sanford.Multimedia.Midi.MessageType.Channel:
							Midi.ChannelMessage channel = (Midi.ChannelMessage)e.MidiMessage;

							// if it's a note on command and it's in our mapping list
							if(channel.Command == Sanford.Multimedia.Midi.ChannelCommand.NoteOn &&
								_midiMap.ContainsKey(channel.MidiChannel) && 
								(int)((e.AbsoluteTicks*msPerTick)/50) < _sequence.Channels[_midiMap[channel.MidiChannel]].Data.Length)
							{
								// this means the note is on
								if(channel.Data2 > 0)
									_sequence.Channels[_midiMap[channel.MidiChannel]].Data[(int)((e.AbsoluteTicks*msPerTick)/50)] = true;
								else
								{
									// the note is off
									_sequence.Channels[_midiMap[channel.MidiChannel]].Data[(int)((e.AbsoluteTicks*msPerTick)/50)] = false;
									if(chkHold.Checked)
									{
										// backfill the grid
										for(int i = (int)((e.AbsoluteTicks*msPerTick)/50); i > 0 && !_sequence.Channels[_midiMap[channel.MidiChannel]].Data[i]; i--)
											_sequence.Channels[_midiMap[channel.MidiChannel]].Data[i] = true;
									}
								}
							}
							// true note off...don't see this used much
							if(channel.Command == Sanford.Multimedia.Midi.ChannelCommand.NoteOff &&
								_midiMap.ContainsKey(channel.MidiChannel))
							{
								_sequence.Channels[_midiMap[channel.MidiChannel]].Data[(int)((e.AbsoluteTicks*msPerTick)/50)] = false;
								if(chkHold.Checked)
								{
									for(int i = (int)((e.AbsoluteTicks*msPerTick)/50); i > 0 && !_sequence.Channels[_midiMap[channel.MidiChannel]].Data[i]; i--)
										_sequence.Channels[_midiMap[channel.MidiChannel]].Data[i] = true;
								}
							}
							break;
						case Sanford.Multimedia.Midi.MessageType.Meta:
							Midi.MetaMessage meta = (Midi.MetaMessage)e.MidiMessage;
							switch(meta.MetaType)
							{
								// again, get the tempo value
								case Sanford.Multimedia.Midi.MetaType.Tempo:
									tempo = meta.GetBytes()[0] << 16 | meta.GetBytes()[1] << 8 | meta.GetBytes()[2];
									msPerTick = (tempo/_MIDISequence.Division)/1000.0f;
									break;
							}
							break;
					}
				}
			}	
		}

		private void txtMinutes_TextChanged(object sender, EventArgs e)
		{
			CheckDigits(txtMinutes);
		}

		private void txtSeconds_TextChanged(object sender, EventArgs e)
		{
			CheckDigits(txtSeconds);
		}

		// ensure valid data is entered into our minutes/seconds boxes
		private void CheckDigits(TextBox tb)
		{
			foreach(char c in tb.Text)
			{
				if(!Char.IsDigit(c))
					tb.Text = string.Empty;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void NewSequenceForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			// remove the event handler
			PhidgetHandler.PhidgetsChanged -= PhidgetHandler_PhidgetsChanged;
		}

		void dgvDevices_DataError(object sender, System.Windows.Forms.DataGridViewDataErrorEventArgs e)
		{
			// I dunno what the deal is with this, but the DGV hates me.
		}

		public Sequence Sequence
		{
			get { return _sequence; }
		}
	}
}