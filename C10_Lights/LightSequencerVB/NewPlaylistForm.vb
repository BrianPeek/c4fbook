'////////////////////////////////////////////////////////////////////////////////
'	NewPlaylistForm.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms

Namespace LightSequencer
	Partial Public Class NewPlaylistForm
		Inherits Form
		Public Structure ListBoxSequence
			Public Sequence As Sequence
			Public File As String

			Public Sub New(ByVal file As String, ByVal sequence As Sequence)
				Me.File = file
				Sequence = sequence
			End Sub

			Public Overrides Function ToString() As String
				If String.IsNullOrEmpty(Sequence.Title) Then
					Return Path.GetFileName(File)
				Else
					Return Sequence.Title
				End If
			End Function
		End Structure

		Private _player As SequencePlayer
		Private _index As Integer
		Private playing As Boolean

		Public Sub New()
			Me.New(Nothing)
		End Sub

		Public Sub New(ByVal file As String)
			InitializeComponent()

			If (Not String.IsNullOrEmpty(file)) Then
				Dim p As New Playlist(file)
				RefreshList(p.Filenames)
			End If
		End Sub

		Private Sub RefreshList(ByVal files As IList(Of String))
			For Each file As String In files
				Dim s As New Sequence(file)
				lbSequences.Items.Add(New ListBoxSequence(file, s))
			Next file
		End Sub

		Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
			Dim ofd As New OpenFileDialog()
			ofd.Multiselect = True
			ofd.Title = "Locate sequences..."
			ofd.Filter = "Sequences (*.seq)|*.seq|All files (*.*)|*.*"
			If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				RefreshList(ofd.FileNames)
			End If
		End Sub

		Private Sub btnUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUp.Click
			' Get the collection of selected indexes in the ListBox.  
			Dim sic As ListBox.SelectedIndexCollection = Me.lbSequences.SelectedIndices

			' Loop through the ListBox.
			For i As Integer = 0 To Me.lbSequences.Items.Count - 1
				' If this index is in the collection.
				If (sic.Contains(i)) AndAlso (i > 0) Then
					' Move the item one index upward.
					Dim item As Object = Me.lbSequences.Items(i)
					Me.lbSequences.Items.RemoveAt(i)
					Me.lbSequences.Items.Insert(i-1, item)
					Me.lbSequences.SetSelected(i-1, True)
				End If
			Next i
		End Sub

		Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
			Dim dr As DialogResult = System.Windows.Forms.DialogResult.OK

			Dim sfd As New SaveFileDialog()
			sfd.Title = "Save Playlist File..."
			sfd.Filter = "Light Sequencer Playlists (*.lsp)|*.lsp"
			sfd.AddExtension = True
			dr = sfd.ShowDialog()

			If dr = System.Windows.Forms.DialogResult.OK Then
				Dim p As New Playlist()
				For Each lbs As ListBoxSequence In lbSequences.Items
					p.Filenames.Add(lbs.File)
				Next lbs
				p.Save(sfd.FileName)
				MessageBox.Show("Playlist saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If
		End Sub

		Private Sub btnPlay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPlay.Click
			If (Not playing) Then
				btnPlay.Text = "&Stop"
				_index = 0
				PlaySong(_index)
			Else
				btnPlay.Text = "&Play"
				lblStatus.Text = "Stopped..."
				_player.Unload()
			End If

			gbPlaylist.Enabled = playing
			playing = Not playing
			btnPrev.Enabled = playing
			btnNext.Enabled = btnPrev.Enabled
		End Sub

		Private Sub _sp_SequenceStopped(ByVal sender As Object, ByVal e As EventArgs)
			Me._player.Unload()

			' move to the next song
			If _index < lbSequences.Items.Count-1 Then
				_index += 1
			Else
				' if repeat checked, start over, otherwse get out and don't play a song
				If chkRepeat.Checked Then
					_index = 0
				Else
					' stop the list
					Me.BeginInvoke(New MethodInvoker(AddressOf AnonymousMethod1))
					Return
				End If
			End If

			PlaySong(_index)
		End Sub
		Private Sub AnonymousMethod1()
			btnPlay.PerformClick()
		End Sub

		Private Sub PlaySong(ByVal index As Integer)
			Dim lbs As ListBoxSequence = CType(lbSequences.Items(index), ListBoxSequence)
''TODO: INSTANT VB TODO TASK: Anonymous methods are not converted by Instant VB if local variables of the outer method are referenced within the anonymous method:
'			Me.BeginInvoke(New MethodInvoker(delegate()
'				lblStatus.Text = "Playing " & lbs.ToString()
'		   ))

			Select Case lbs.Sequence.Version
				Case 1
					Me._player = New SequencePlayerV1(lbs.Sequence)
					AddHandler _player.SequenceStopped, AddressOf _sp_SequenceStopped
					Me._player.Start()
				Case 2
					Me._player = New SequencePlayerV2(lbs.Sequence)
					AddHandler _player.SequenceStopped, AddressOf _sp_SequenceStopped
					Me._player.Start()
			End Select
		End Sub

		Private Sub btnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNext.Click
			Me._player.Unload()
			If _index < lbSequences.Items.Count-1 Then
				_index += 1
			Else
				_index = 0
			End If

			PlaySong(_index)
		End Sub

		Private Sub btnPrev_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrev.Click
			Me._player.Unload()
			If _index = 0 Then
				_index = lbSequences.Items.Count-1
			Else
				_index -= 1
			End If

			PlaySong(_index)
		End Sub

		Private Sub btnDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDown.Click
			Dim col As ListBox.SelectedIndexCollection = Me.lbSequences.SelectedIndices

			For i As Integer = Me.lbSequences.Items.Count - 1 To 0 Step -1
				If (col.Contains(i)) AndAlso (i < Me.lbSequences.Items.Count-1) Then
					Dim item As Object = Me.lbSequences.Items(i)
					Me.lbSequences.Items.RemoveAt(i)
					Me.lbSequences.Items.Insert(i+1, item)
					Me.lbSequences.SetSelected(i+1, True)
				End If
			Next i
		End Sub

		Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
			Me.lbSequences.Items.RemoveAt(lbSequences.SelectedIndex)
		End Sub
	End Class
End Namespace
