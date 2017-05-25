//////////////////////////////////////////////////////////////////////////////////
//	MainForm.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Phidgets;

namespace LightSequencer
{
	public partial class MainForm : Form
	{
		private enum CutCopy
		{
			None,
			Cut,
			Copy
		};

		// width of each column in the grid
		const int COL_WIDTH = 20;

		// information regarding the current sequence
		private Sequence _sequence = new Sequence();
		private string _sequenceFile = string.Empty;

		// number of columns displayed per second
		private int _colsPerSec = 0;

		// sequence playing?
		private bool _playing = false;

		private SequencePlayer _sequencePlayer;

		// cut/copy mode
		private CutCopy _cutCopy = CutCopy.None;

		// selected cells during a cut/copy
		private DataGridViewSelectedCellCollection _cutCopyCells = null;

		public MainForm()
		{
			InitializeComponent();
			PhidgetHandler.PhidgetsChanged += new EventHandler(PhidgetHandler_PhidgetsChanged);
			PhidgetHandler.Init();
			EnableMenus(false);
		}

		void PhidgetHandler_PhidgetsChanged(object sender, EventArgs e)
		{
			Debug.WriteLine(PhidgetHandler.IFKits.Count);
		}

		private void NewSequence()
		{
			if(CheckSave())
				return;

			NewSequenceForm nsf = new NewSequenceForm();
			if(nsf.ShowDialog() == DialogResult.OK)
			{
				_sequence = nsf.Sequence;

				CreateSequence();
				RedrawSequence();
			}
		}

		private void CreateSequence()
		{
			// clear out the current grids
			dgvHeader.Rows.Clear();
			dgvHeader.Columns.Clear();
			dgvMain.Rows.Clear();
			dgvMain.Columns.Clear();

			// make our grids invisible while drawing to speed it up
			dgvMain.Visible	= false;
			dgvHeader.Visible = false;

			// columns per second = 1 second / length of time for each square
			_colsPerSec = 1000 / (int)_sequence.Interval;

			// draw the grid
			switch(_sequence.Version)
			{
				case 1:
				case 2:
					// for the length of the music track
					for(int sec = 0; sec < _sequence.MusicLength; sec++)
					{
						// for each second in the time specified, create a column in the header
						string header = "{0:0#}:{1:0#}.00";
						dgvHeader.Columns.Add(sec.ToString(), String.Format(header, sec/60, sec%60));
						dgvHeader.Columns[sec].FillWeight = 0.0001f;	// this value must be < 65535 for the entire grid (?)
						// set the width to an appropriate size for display
						dgvHeader.Columns[sec].Width = _colsPerSec*COL_WIDTH;

						// now draw the squares contained within each second
						for(int partSec = 0; partSec < _colsPerSec; partSec++)
						{
							// no name, FillWeight requirement, set the appropriate width
							dgvMain.Columns.Add("", "");
							dgvMain.Columns[(sec * _colsPerSec) + partSec].FillWeight = 0.0001f; 
							dgvMain.Columns[(sec * _colsPerSec) + partSec].Width = COL_WIDTH;
						}
					}
					break;
				default:
					throw new Exception("Unknown sequence version!");
			}

			// add the row headers (Ch. 1, Ch. 2, etc.)
			for(int channel = 0; channel < _sequence.Channels.Count; channel++)
			{
				// create the row and text
				DataGridViewRow dvr = new DataGridViewRow();
				dvr.HeaderCell.Value = "Ch. " + (channel+1);

				// store away the actual Channel object for later use
				dvr.Tag = _sequence.Channels[channel];

				// add the row
				dgvMain.Rows.Add(dvr);
			}

			// set the width of those row headers
			dgvMain.RowHeadersWidth = 100;
			dgvHeader.RowHeadersWidth = 100;

			// turn the grids back on
			dgvMain.Visible	= true;
			dgvHeader.Visible = true;

			// load up the sequence player
			if(_sequencePlayer != null)
				_sequencePlayer.Unload();

			switch(_sequence.Version)
			{
				case 1:
					_sequencePlayer = new SequencePlayerV1(_sequence);
					break;
				case 2:
					_sequencePlayer = new SequencePlayerV2(_sequence);
					break;
				default:
					throw new ApplicationException("Unknown sequence version!");
			}

			// setup an event handler for when the sequence has ended
			_sequencePlayer.SequenceStopped += new EventHandler(_sequencePlayer_SequenceStopped);

			// turn on the appropriate menus
			EnableMenus(true);
		}

		void _sequencePlayer_SequenceStopped(object sender, EventArgs e)
		{
			this.BeginInvoke(new MethodInvoker(delegate() {recordSequenceToolStripMenuItem.Enabled = true;recordToolStripButton.Enabled = true;}));

			_playing = false;

			TurnOffDevices();
		}

		private void OpenSequence()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Open Sequence File...";
			ofd.Filter = "Sequence files (*.seq)|*.seq";

			if(ofd.ShowDialog() == DialogResult.OK)
			{
				_sequence.Load(ofd.FileName);
				_sequenceFile = ofd.FileName;

				CreateSequence();

				// add in the channel data
				for(int i = 0; i < _sequence.Channels.Count; i++)
				{
					bool[] data = _sequence.Channels[i].Data;

					// draw the existing data
					for(int j = 0; j < data.Length; j++)
						HighlightCell(data[j], i, j);
				}
			}
		}

		private DialogResult SaveSequence()
		{
			DialogResult dr = DialogResult.OK;

			if(_sequenceFile == string.Empty)
			{
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Title = "Save Sequence File...";
				sfd.Filter = "Sequence files (*.seq)|*.seq";
				sfd.AddExtension = true;
				dr = sfd.ShowDialog();
				_sequenceFile = sfd.FileName;
			}

			if(dr == DialogResult.OK)
			{
				_sequence.Save(_sequenceFile);
				MessageBox.Show("Sequence file saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			return dr;
		}

		private void PlaySequence()
		{
			if(!_playing)
			{
				recordSequenceToolStripMenuItem.Enabled = false;
				recordToolStripButton.Enabled = false;

				_sequencePlayer.Start();
				_playing = true;
			}
		}

		private void StopSequence()
		{
			recordSequenceToolStripMenuItem.Enabled = true;
			recordToolStripButton.Enabled = true;

			_sequencePlayer.Stop();
			_playing = false;

			TurnOffDevices();
		}

		private void RecordSequence()
		{
			if(_sequence.MusicType == MusicType.MIDI)
				_sequencePlayer.Unload();

			RecordForm rf = new RecordForm();
			rf.Sequence = _sequence;
			rf.ShowDialog();

			TurnOffDevices();

			// data is updated in the RecordForm dialog...redraw the sequence when it closes
			RedrawSequence();

			if(_sequence.MusicType == MusicType.MIDI)
				_sequencePlayer.Load();
		}

		private void RedrawSequence()
		{
			for(int i = 0; i < _sequence.Channels.Count; i++)
			{
				for(int j = 0; j < _sequence.Channels[i].Data.Length; j++)
					dgvMain.Rows[i].Cells[j].Style.BackColor = (_sequence.Channels[i].Data[j]) ? Color.Blue : Color.White;
			}
		}

		private void HighlightCell(bool on, int row, int col)
		{
			// turn a specific cell off or on
			_sequence.Channels[row].Data[col] = on;
			dgvMain.Rows[row].Cells[col].Style.BackColor = on ? Color.Blue : Color.White;
		}

		private void HighlightCells(bool on, DataGridViewSelectedCellCollection cells)
		{
			// turn a group of cells off or on
			foreach(DataGridViewCell dgc in cells)
				HighlightCell(on, dgc.RowIndex, dgc.ColumnIndex);
		}

		private void HighlightCells(bool on)
		{
			HighlightCells(on, dgvMain.SelectedCells);
		}

		// enable or disable menu items
		private void EnableMenus(bool enabled)
		{
			saveToolStripButton.Enabled = enabled;
			playToolStripButton.Enabled = enabled;
			stopToolStripButton.Enabled = enabled;
			recordToolStripButton.Enabled = enabled;
			copyToolStripButton.Enabled = enabled;
			cutToolStripButton.Enabled = enabled;
			pasteToolStripButton.Enabled = enabled;


			saveToolStripMenuItem.Enabled = enabled;
			saveAsToolStripMenuItem.Enabled = enabled;
			startSequenceToolStripMenuItem.Enabled = enabled;
			stopSequenceToolStripMenuItem.Enabled = enabled;
			recordSequenceToolStripMenuItem.Enabled = enabled;
			copyToolStripMenuItem.Enabled = enabled;
			cutToolStripMenuItem.Enabled = enabled;
			pasteToolStripMenuItem.Enabled = enabled;
			editSequencePropertiesToolStripMenuItem.Enabled = enabled;

			testChannelsToolStripMenuItem.Enabled = enabled;
		}

		private void TurnOffDevices()
		{
			// turn off all output ports
			foreach(Channel c in _sequence.Channels)
			{
			    if(PhidgetHandler.IFKits[c.SerialNumber].Attached && c.SerialNumber != -1)
			        PhidgetHandler.IFKits[c.SerialNumber].outputs[c.OutputIndex] = false;
			}
		}

		private void NewPlaylist()
		{
			if(_sequencePlayer != null)
				_sequencePlayer.Unload();
			new NewPlaylistForm().ShowDialog();
			if(_sequencePlayer != null)
				_sequencePlayer.Load();
		}

		private void OpenPlaylist()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Open Playlist File...";
			ofd.Filter = "Playlist files (*.lsp)|*.lsp";

			if(ofd.ShowDialog() == DialogResult.OK)
				new NewPlaylistForm(ofd.FileName).ShowDialog();
		}

		private void HandleCut(object sender, EventArgs e)
		{
			if(dgvMain.SelectedCells == null)
				return;

			// grab the selected cells
			_cutCopyCells = dgvMain.SelectedCells;
			_cutCopy = CutCopy.Cut;
		}

		private void HandleCopy(object sender, EventArgs e)
		{
			if(dgvMain.SelectedCells == null)
				return;

			// grab the selected cells
			_cutCopyCells = dgvMain.SelectedCells;
			_cutCopy = CutCopy.Copy;
		}

		private void HandleSelectAll(object sender, EventArgs e)
		{
			dgvMain.SelectAll();
		}

		private void HandlePaste(object sender, EventArgs e)
		{
			// since the selected cells collection from the cut/copy are not in any particular, we'll need to find
			// the top-left-most cell to start from
			int row = int.MaxValue, col = int.MaxValue;

			if(dgvMain.SelectedCells == null || dgvMain.SelectedCells.Count == 0)
				return;

			// force a single cell selection
			if(dgvMain.SelectedCells.Count > 1)
			{
				MessageBox.Show("Please select a single cell at which the paste should begin.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// get the currently selected sell to start pasting to
			DataGridViewCell pasteCell = dgvMain.SelectedCells[0];

			// find the top-left most cell in the collection
			foreach(DataGridViewCell cell in this._cutCopyCells)
			{
				if(cell.RowIndex < row)
					row = cell.RowIndex;
				if(cell.ColumnIndex < col)
					col = cell.ColumnIndex;
			}

			foreach(DataGridViewCell cell in this._cutCopyCells)
			{
				// with each cell in the collection, normalize it to the current selection point and change the background color appropriately
				int destRow = pasteCell.RowIndex + (cell.RowIndex-row);
				int destCol = pasteCell.ColumnIndex + (cell.ColumnIndex - col);

				if(destRow < dgvMain.Rows.Count && destCol < dgvMain.Rows[destRow].Cells.Count)
				{
					DataGridViewCell newCell = dgvMain.Rows[pasteCell.RowIndex + (cell.RowIndex-row)].Cells[pasteCell.ColumnIndex + (cell.ColumnIndex - col)];
					newCell.Style.BackColor = cell.Style.BackColor;
				}

				// if we're coming from a cut operation, blank out the original cells
				if(_cutCopy == CutCopy.Cut)
					cell.Style.BackColor = Color.White;
			}
		}

#region Menu/Toolbar Handlers
		// menu/toolbar handlers
		private void sequenceToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			NewSequence();
		}

		private void newToolStripButton_Click(object sender, EventArgs e)
		{
			NewSequence();
		}

		private void sequenceToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			OpenSequence();
		}

		private void openToolStripButton_Click(object sender, EventArgs e)
		{
			OpenSequence();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSequence();
		}

		private void saveToolStripButton_Click(object sender, EventArgs e)
		{
			SaveSequence();
		}

		private void playToolStripButton_Click(object sender, EventArgs e)
		{
			PlaySequence();
		}

		private void stopToolStripButton_Click(object sender, EventArgs e)
		{
			StopSequence();
		}

		private void onToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HighlightCells(true);
		}

		private void offToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HighlightCells(false);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutForm().ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void startSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PlaySequence();
		}

		private void stopSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StopSequence();
		}

		private void recordToolStripButton_Click(object sender, EventArgs e)
		{
			RecordSequence();
		}

		private void recordSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RecordSequence();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string file = _sequenceFile;
			_sequenceFile = string.Empty;
			SaveSequence();
			if(_sequenceFile.Length == 0)
				_sequenceFile = file;
		}

		private void testChannelsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TestChannelsForm tcf = new TestChannelsForm();
			tcf.Sequence = _sequence;
			tcf.ShowDialog();
		}

		private void editSequencePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewSequenceForm nsf = new NewSequenceForm(_sequence);
			if(nsf.ShowDialog() == DialogResult.OK)
			{
				_sequence = nsf.Sequence;
				SaveSequence();
				CreateSequence();
				RedrawSequence();
			}
		}

		private void playlistToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewPlaylist();
		}

		private void newPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewPlaylist();
		}

		private void playlistToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			OpenPlaylist();
		}

		private void playlistToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			OpenPlaylist();
		}
#endregion

#region Events
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			TurnOffDevices();
		}

		private void dgvMain_Scroll(object sender, ScrollEventArgs e)
		{
			// when the main grid is scrolled, scroll the header by the same amount
			dgvHeader.HorizontalScrollingOffset = e.NewValue;
		}

		private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			HighlightCell(true, e.RowIndex, e.ColumnIndex);
		}

		private void dgvMain_KeyDown(object sender, KeyEventArgs e)
		{
			// O == On
			if(e.KeyCode == Keys.O)
				HighlightCells(true);

			// F == Off
			else if(e.KeyCode == Keys.F)
				HighlightCells(false);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = CheckSave();
		}

		private bool CheckSave()
		{
			if(_sequence.Channels.Count > 0)
			{
				DialogResult dr = MessageBox.Show("Do you want to save the current sequence before exiting?", "Save Sequence?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				switch(dr)
				{
					case DialogResult.Yes:
						if(SaveSequence() == DialogResult.Cancel)
							return true;
						break;
					case DialogResult.No:
						break;
					case DialogResult.Cancel:
						return true;
				}
			}
			return false;
		}
#endregion
	}
}
