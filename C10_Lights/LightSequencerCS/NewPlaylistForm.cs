//////////////////////////////////////////////////////////////////////////////////
//	NewPlaylistForm.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LightSequencer
{
	public partial class NewPlaylistForm : Form
	{
		public struct ListBoxSequence
		{
			public Sequence Sequence;
			public string File;

			public ListBoxSequence(string file, Sequence sequence)
			{
				File = file;
				Sequence = sequence;
			}

			public override string ToString()
			{
				return string.IsNullOrEmpty(Sequence.Title) ? Path.GetFileName(File) : Sequence.Title;
			}
		}

		private SequencePlayer _player;
		private int _index;
		private bool playing;

		public NewPlaylistForm() : this(null)
		{
		}

		public NewPlaylistForm(string file)
		{
			InitializeComponent();

			if(!string.IsNullOrEmpty(file))
			{
				Playlist p = new Playlist(file);
				RefreshList(p.Filenames);
			}
		}

		private void RefreshList(IList<string> files)
		{
			foreach(string file in files)
			{
				Sequence s = new Sequence(file);
				lbSequences.Items.Add(new ListBoxSequence(file, s));
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Multiselect = true;
			ofd.Title = "Locate sequences...";
			ofd.Filter = "Sequences (*.seq)|*.seq|All files (*.*)|*.*";
			if(ofd.ShowDialog() == DialogResult.OK)
				RefreshList(ofd.FileNames);
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			// Get the collection of selected indexes in the ListBox.  
			ListBox.SelectedIndexCollection sic = this.lbSequences.SelectedIndices;
		 
			// Loop through the ListBox.
			for (int i = 0; i <= this.lbSequences.Items.Count - 1; i++)
			{
				// If this index is in the collection.
				if ((sic.Contains(i)) && (i > 0))
				{
					// Move the item one index upward.
					object item = this.lbSequences.Items[i];
					this.lbSequences.Items.RemoveAt(i);
					this.lbSequences.Items.Insert(i-1, item);
					this.lbSequences.SetSelected(i-1, true);
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			DialogResult dr = DialogResult.OK;

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Save Playlist File...";
			sfd.Filter = "Light Sequencer Playlists (*.lsp)|*.lsp";
			sfd.AddExtension = true;
			dr = sfd.ShowDialog();

			if(dr == DialogResult.OK)
			{
				Playlist p = new Playlist();
				foreach(ListBoxSequence lbs in lbSequences.Items)
					p.Filenames.Add(lbs.File);
				p.Save(sfd.FileName);
				MessageBox.Show("Playlist saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void btnPlay_Click(object sender, EventArgs e)
		{
			if(!playing)
			{
				btnPlay.Text = "&Stop";
				_index = 0;
				PlaySong(_index);
			}
			else
			{
				btnPlay.Text = "&Play";
				lblStatus.Text = "Stopped...";
				_player.Unload();
			}

			gbPlaylist.Enabled = playing;
			playing = !playing;
			btnNext.Enabled = btnPrev.Enabled = playing;
		}

		void _sp_SequenceStopped(object sender, EventArgs e)
		{
			this._player.Unload();

			// move to the next song
			if(_index < lbSequences.Items.Count-1)
				_index++;
			else
			{
				// if repeat checked, start over, otherwse get out and don't play a song
				if(chkRepeat.Checked)
					_index = 0;
				else
				{
					// stop the list
					this.BeginInvoke(new MethodInvoker(delegate() {btnPlay.PerformClick();}));
					return;
				}
			}

			PlaySong(_index);
		}

		void PlaySong(int index)
		{
			ListBoxSequence lbs = (ListBoxSequence)lbSequences.Items[index];
			this.BeginInvoke(new MethodInvoker(delegate(){lblStatus.Text = "Playing " + lbs.ToString();}));

			switch(lbs.Sequence.Version)
			{
				case 1:
					this._player = new SequencePlayerV1(lbs.Sequence);
					this._player.SequenceStopped += new EventHandler(_sp_SequenceStopped);
					this._player.Start();
					break;
				case 2:
					this._player = new SequencePlayerV2(lbs.Sequence);
					this._player.SequenceStopped += new EventHandler(_sp_SequenceStopped);
					this._player.Start();
					break;
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			this._player.Unload();
			if(_index < lbSequences.Items.Count-1)
				_index++;
			else
				_index = 0;

			PlaySong(_index);
		}

		private void btnPrev_Click(object sender, EventArgs e)
		{
			this._player.Unload();
			if(_index == 0)
				_index = lbSequences.Items.Count-1;
			else
				_index--;

			PlaySong(_index);
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection col = this.lbSequences.SelectedIndices; 

			for (int i = this.lbSequences.Items.Count - 1; i >= 0; i--)
			{
				if ((col.Contains(i)) && (i < this.lbSequences.Items.Count-1))
				{
					object item = this.lbSequences.Items[i];
					this.lbSequences.Items.RemoveAt(i);
					this.lbSequences.Items.Insert(i+1, item);
					this.lbSequences.SetSelected(i+1, true);
				}
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			this.lbSequences.Items.RemoveAt(lbSequences.SelectedIndex);
		}
	}
}
