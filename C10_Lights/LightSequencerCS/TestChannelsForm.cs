//////////////////////////////////////////////////////////////////////////////////
//	TestChannelsForm.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace LightSequencer
{
	public partial class TestChannelsForm : Form
	{
		private Sequence _sequence;

		public TestChannelsForm()
		{
			InitializeComponent();
		}

		private void ToggleChannel(int channel, bool on)
		{
			if(channel < _sequence.Channels.Count)
				PhidgetHandler.IFKits[_sequence.Channels[channel].SerialNumber].outputs[_sequence.Channels[channel].OutputIndex] = on;
		}

		private void TestChannelsForm_KeyDown(object sender, KeyEventArgs e)
		{
			// otherwise, modify data for channel 1-9 based on key pressed
			if(e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9)
				ToggleChannel((e.KeyValue - (int)Keys.D0) - 1, true);

			if(e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9)
				ToggleChannel((e.KeyValue - (int)Keys.NumPad0) - 1, true);
		}

		private void TestChannelsForm_KeyUp(object sender, KeyEventArgs e)
		{
			// key released, set back to false
			if(e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9)
				ToggleChannel((e.KeyValue - (int)Keys.D0) - 1, false);

			if(e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9)
				ToggleChannel((e.KeyValue - (int)Keys.NumPad0) - 1, false);
		}

		public Sequence Sequence
		{
			get { return _sequence; }
			set { _sequence = value; }
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void chkAllOn_CheckedChanged(object sender, EventArgs e)
		{
			for(int i = 0; i < _sequence.Channels.Count; i++)
				ToggleChannel(i, chkAllOn.Checked);
		}
	}
}