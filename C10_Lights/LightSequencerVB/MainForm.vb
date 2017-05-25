'////////////////////////////////////////////////////////////////////////////////
'	MainForm.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Threading
Imports System.Windows.Forms
Imports Phidgets

Namespace LightSequencer
	Partial Public Class MainForm
		Inherits Form
		Private Enum CutCopy
			None
			Cut
			Copy
		End Enum

		' width of each column in the grid
		Private Const COL_WIDTH As Integer = 20

		' information regarding the current sequence
		Private _sequence As New Sequence()
		Private _sequenceFile As String = String.Empty

		' number of columns displayed per second
		Private _colsPerSec As Integer = 0

		' sequence playing?
		Private _playing As Boolean = False

		Private _sequencePlayer As SequencePlayer

		' cut/copy mode
		Private _cutCopy As CutCopy = CutCopy.None

		' selected cells during a cut/copy
		Private _cutCopyCells As DataGridViewSelectedCellCollection = Nothing

		Public Sub New()
			InitializeComponent()
			AddHandler PhidgetHandler.PhidgetsChanged, AddressOf PhidgetHandler_PhidgetsChanged
			PhidgetHandler.Init()
			EnableMenus(False)
		End Sub

		Private Sub PhidgetHandler_PhidgetsChanged(ByVal sender As Object, ByVal e As EventArgs)
			Debug.WriteLine(PhidgetHandler.IFKits.Count)
		End Sub

		Private Sub NewSequence()
			If CheckSave() Then
				Return
			End If

			Dim nsf As New NewSequenceForm()
			If nsf.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				_sequence = nsf.Sequence

				CreateSequence()
				RedrawSequence()
			End If
		End Sub

		Private Sub CreateSequence()
			' clear out the current grids
			dgvHeader.Rows.Clear()
			dgvHeader.Columns.Clear()
			dgvMain.Rows.Clear()
			dgvMain.Columns.Clear()

			' make our grids invisible while drawing to speed it up
			dgvMain.Visible = False
			dgvHeader.Visible = False

			' columns per second = 1 second / length of time for each square
			_colsPerSec = 1000 / CInt(Fix(_sequence.Interval))

			' draw the grid
			Select Case _sequence.Version
				Case 1, 2
					' for the length of the music track
					For sec As Integer = 0 To _sequence.MusicLength - 1
						' for each second in the time specified, create a column in the header
						Dim header As String = "{0:0#}:{1:0#}.00"
						dgvHeader.Columns.Add(sec.ToString(), String.Format(header, sec\60, sec Mod 60))
						dgvHeader.Columns(sec).FillWeight = 0.0001f ' this value must be < 65535 for the entire grid (?)
						' set the width to an appropriate size for display
						dgvHeader.Columns(sec).Width = _colsPerSec*COL_WIDTH

						' now draw the squares contained within each second
						For partSec As Integer = 0 To _colsPerSec - 1
							' no name, FillWeight requirement, set the appropriate width
							dgvMain.Columns.Add("", "")
							dgvMain.Columns((sec * _colsPerSec) + partSec).FillWeight = 0.0001f
							dgvMain.Columns((sec * _colsPerSec) + partSec).Width = COL_WIDTH
						Next partSec
					Next sec
				Case Else
					Throw New Exception("Unknown sequence version!")
			End Select

			' add the row headers (Ch. 1, Ch. 2, etc.)
			For channel As Integer = 0 To _sequence.Channels.Count - 1
				' create the row and text
				Dim dvr As New DataGridViewRow()
				dvr.HeaderCell.Value = "Ch. " & (channel+1)

				' store away the actual Channel object for later use
				dvr.Tag = _sequence.Channels(channel)

				' add the row
				dgvMain.Rows.Add(dvr)
			Next channel

			' set the width of those row headers
			dgvMain.RowHeadersWidth = 100
			dgvHeader.RowHeadersWidth = 100

			' turn the grids back on
			dgvMain.Visible = True
			dgvHeader.Visible = True

			' load up the sequence player
			If _sequencePlayer IsNot Nothing Then
				_sequencePlayer.Unload()
			End If

			Select Case _sequence.Version
				Case 1
					_sequencePlayer = New SequencePlayerV1(_sequence)
				Case 2
					_sequencePlayer = New SequencePlayerV2(_sequence)
				Case Else
					Throw New ApplicationException("Unknown sequence version!")
			End Select

			' setup an event handler for when the sequence has ended
			AddHandler _sequencePlayer.SequenceStopped, AddressOf _sequencePlayer_SequenceStopped

			' turn on the appropriate menus
			EnableMenus(True)
		End Sub

		Private Sub _sequencePlayer_SequenceStopped(ByVal sender As Object, ByVal e As EventArgs)
			Me.BeginInvoke(New MethodInvoker(AddressOf AnonymousMethod1))

			_playing = False

			TurnOffDevices()
		End Sub
		Private Sub AnonymousMethod1()
			recordSequenceToolStripMenuItem.Enabled = True
			recordToolStripButton.Enabled = True
		End Sub

		Private Sub OpenSequence()
			Dim ofd As New OpenFileDialog()
			ofd.Title = "Open Sequence File..."
			ofd.Filter = "Sequence files (*.seq)|*.seq"

			If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				_sequence.Load(ofd.FileName)
				_sequenceFile = ofd.FileName

				CreateSequence()

				' add in the channel data
				For i As Integer = 0 To _sequence.Channels.Count - 1
					Dim data() As Boolean = _sequence.Channels(i).Data

					' draw the existing data
					For j As Integer = 0 To data.Length - 1
						HighlightCell(data(j), i, j)
					Next j
				Next i
			End If
		End Sub

		Private Function SaveSequence() As DialogResult
			Dim dr As DialogResult = System.Windows.Forms.DialogResult.OK

			If _sequenceFile Is String.Empty Then
				Dim sfd As New SaveFileDialog()
				sfd.Title = "Save Sequence File..."
				sfd.Filter = "Sequence files (*.seq)|*.seq"
				sfd.AddExtension = True
				dr = sfd.ShowDialog()
				_sequenceFile = sfd.FileName
			End If

			If dr = System.Windows.Forms.DialogResult.OK Then
				_sequence.Save(_sequenceFile)
				MessageBox.Show("Sequence file saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If

			Return dr
		End Function

		Private Sub PlaySequence()
			If (Not _playing) Then
				recordSequenceToolStripMenuItem.Enabled = False
				recordToolStripButton.Enabled = False

				_sequencePlayer.Start()
				_playing = True
			End If
		End Sub

		Private Sub StopSequence()
			recordSequenceToolStripMenuItem.Enabled = True
			recordToolStripButton.Enabled = True

			_sequencePlayer.Stop()
			_playing = False

			TurnOffDevices()
		End Sub

		Private Sub RecordSequence()
			If _sequence.MusicType = MusicType.MIDI Then
				_sequencePlayer.Unload()
			End If

			Dim rf As New RecordForm()
			rf.Sequence = _sequence
			rf.ShowDialog()

			TurnOffDevices()

			' data is updated in the RecordForm dialog...redraw the sequence when it closes
			RedrawSequence()

			If _sequence.MusicType = MusicType.MIDI Then
				_sequencePlayer.Load()
			End If
		End Sub

		Private Sub RedrawSequence()
			For i As Integer = 0 To _sequence.Channels.Count - 1
				For j As Integer = 0 To _sequence.Channels(i).Data.Length - 1
					If (_sequence.Channels(i).Data(j)) Then
						dgvMain.Rows(i).Cells(j).Style.BackColor = Color.Blue
					Else
						dgvMain.Rows(i).Cells(j).Style.BackColor = Color.White
					End If
				Next j
			Next i
		End Sub

		Private Sub HighlightCell(ByVal [on] As Boolean, ByVal row As Integer, ByVal col As Integer)
			' turn a specific cell off or on
			_sequence.Channels(row).Data(col) = [on]
			If [on] Then
				dgvMain.Rows(row).Cells(col).Style.BackColor = Color.Blue
			Else
				dgvMain.Rows(row).Cells(col).Style.BackColor = Color.White
			End If
		End Sub

		Private Sub HighlightCells(ByVal [on] As Boolean, ByVal cells As DataGridViewSelectedCellCollection)
			' turn a group of cells off or on
			For Each dgc As DataGridViewCell In cells
				HighlightCell([on], dgc.RowIndex, dgc.ColumnIndex)
			Next dgc
		End Sub

		Private Sub HighlightCells(ByVal [on] As Boolean)
			HighlightCells([on], dgvMain.SelectedCells)
		End Sub

		' enable or disable menu items
		Private Sub EnableMenus(ByVal enabled As Boolean)
			saveToolStripButton.Enabled = enabled
			playToolStripButton.Enabled = enabled
			stopToolStripButton.Enabled = enabled
			recordToolStripButton.Enabled = enabled
			copyToolStripButton.Enabled = enabled
			cutToolStripButton.Enabled = enabled
			pasteToolStripButton.Enabled = enabled


			saveToolStripMenuItem.Enabled = enabled
			saveAsToolStripMenuItem.Enabled = enabled
			startSequenceToolStripMenuItem.Enabled = enabled
			stopSequenceToolStripMenuItem.Enabled = enabled
			recordSequenceToolStripMenuItem.Enabled = enabled
			copyToolStripMenuItem.Enabled = enabled
			cutToolStripMenuItem.Enabled = enabled
			pasteToolStripMenuItem.Enabled = enabled
			editSequencePropertiesToolStripMenuItem.Enabled = enabled

			testChannelsToolStripMenuItem.Enabled = enabled
		End Sub

		Private Sub TurnOffDevices()
			' turn off all output ports
			For Each c As Channel In _sequence.Channels
				If PhidgetHandler.IFKits(c.SerialNumber).Attached AndAlso c.SerialNumber <> -1 Then
					PhidgetHandler.IFKits(c.SerialNumber).outputs(c.OutputIndex) = False
				End If
			Next c
		End Sub

		Private Sub NewPlaylist()
			If _sequencePlayer IsNot Nothing Then
				_sequencePlayer.Unload()
			End If
			CType(New NewPlaylistForm(), NewPlaylistForm).ShowDialog()
			If _sequencePlayer IsNot Nothing Then
				_sequencePlayer.Load()
			End If
		End Sub

		Private Sub OpenPlaylist()
			Dim ofd As New OpenFileDialog()
			ofd.Title = "Open Playlist File..."
			ofd.Filter = "Playlist files (*.lsp)|*.lsp"

			If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				CType(New NewPlaylistForm(ofd.FileName), NewPlaylistForm).ShowDialog()
			End If
		End Sub

		Private Sub HandleCut(ByVal sender As Object, ByVal e As EventArgs) Handles cutToolStripMenuItem.Click, cutToolStripButton.Click, toolStripMenuItem2.Click
			If dgvMain.SelectedCells Is Nothing Then
				Return
			End If

			' grab the selected cells
			_cutCopyCells = dgvMain.SelectedCells
			_cutCopy = CutCopy.Cut
		End Sub

		Private Sub HandleCopy(ByVal sender As Object, ByVal e As EventArgs) Handles copyToolStripMenuItem.Click, copyToolStripButton.Click, toolStripMenuItem3.Click
			If dgvMain.SelectedCells Is Nothing Then
				Return
			End If

			' grab the selected cells
			_cutCopyCells = dgvMain.SelectedCells
			_cutCopy = CutCopy.Copy
		End Sub

		Private Sub HandleSelectAll(ByVal sender As Object, ByVal e As EventArgs) Handles selectAllToolStripMenuItem.Click, toolStripMenuItem5.Click
			dgvMain.SelectAll()
		End Sub

		Private Sub HandlePaste(ByVal sender As Object, ByVal e As EventArgs) Handles pasteToolStripMenuItem.Click, pasteToolStripButton.Click, toolStripMenuItem4.Click
			' since the selected cells collection from the cut/copy are not in any particular, we'll need to find
			' the top-left-most cell to start from
			Dim row As Integer = Integer.MaxValue, col As Integer = Integer.MaxValue

			If dgvMain.SelectedCells Is Nothing OrElse dgvMain.SelectedCells.Count = 0 Then
				Return
			End If

			' force a single cell selection
			If dgvMain.SelectedCells.Count > 1 Then
				MessageBox.Show("Please select a single cell at which the paste should begin.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
				Return
			End If

			' get the currently selected sell to start pasting to
			Dim pasteCell As DataGridViewCell = dgvMain.SelectedCells(0)

			' find the top-left most cell in the collection
			For Each cell As DataGridViewCell In Me._cutCopyCells
				If cell.RowIndex < row Then
					row = cell.RowIndex
				End If
				If cell.ColumnIndex < col Then
					col = cell.ColumnIndex
				End If
			Next cell

			For Each cell As DataGridViewCell In Me._cutCopyCells
				' with each cell in the collection, normalize it to the current selection point and change the background color appropriately
				Dim destRow As Integer = pasteCell.RowIndex + (cell.RowIndex-row)
				Dim destCol As Integer = pasteCell.ColumnIndex + (cell.ColumnIndex - col)

				If destRow < dgvMain.Rows.Count AndAlso destCol < dgvMain.Rows(destRow).Cells.Count Then
					Dim newCell As DataGridViewCell = dgvMain.Rows(pasteCell.RowIndex + (cell.RowIndex-row)).Cells(pasteCell.ColumnIndex + (cell.ColumnIndex - col))
					newCell.Style.BackColor = cell.Style.BackColor
				End If

				' if we're coming from a cut operation, blank out the original cells
				If _cutCopy = CutCopy.Cut Then
					cell.Style.BackColor = Color.White
				End If
			Next cell
		End Sub

#Region "Menu/Toolbar Handlers"
		' menu/toolbar handlers
		Private Sub sequenceToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles sequenceToolStripMenuItem1.Click
			NewSequence()
		End Sub

		Private Sub newToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles newToolStripButton.ButtonClick
			NewSequence()
		End Sub

		Private Sub sequenceToolStripMenuItem2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles sequenceToolStripMenuItem2.Click
			OpenSequence()
		End Sub

		Private Sub openToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles openToolStripButton.ButtonClick
			OpenSequence()
		End Sub

		Private Sub saveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveToolStripMenuItem.Click
			SaveSequence()
		End Sub

		Private Sub saveToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveToolStripButton.Click
			SaveSequence()
		End Sub

		Private Sub playToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles playToolStripButton.Click
			PlaySequence()
		End Sub

		Private Sub stopToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopToolStripButton.Click
			StopSequence()
		End Sub

		Private Sub onToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles onToolStripMenuItem.Click
			HighlightCells(True)
		End Sub

		Private Sub offToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles offToolStripMenuItem.Click
			HighlightCells(False)
		End Sub

		Private Sub aboutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles aboutToolStripMenuItem.Click
			CType(New AboutForm(), AboutForm).ShowDialog()
		End Sub

		Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
			Me.Close()
		End Sub

		Private Sub startSequenceToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startSequenceToolStripMenuItem.Click
			PlaySequence()
		End Sub

		Private Sub stopSequenceToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopSequenceToolStripMenuItem.Click
			StopSequence()
		End Sub

		Private Sub recordToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles recordToolStripButton.Click
			RecordSequence()
		End Sub

		Private Sub recordSequenceToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles recordSequenceToolStripMenuItem.Click
			RecordSequence()
		End Sub

		Private Sub saveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveAsToolStripMenuItem.Click
			Dim file As String = _sequenceFile
			_sequenceFile = String.Empty
			SaveSequence()
			If _sequenceFile.Length = 0 Then
				_sequenceFile = file
			End If
		End Sub

		Private Sub testChannelsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles testChannelsToolStripMenuItem.Click
			Dim tcf As New TestChannelsForm()
			tcf.Sequence = _sequence
			tcf.ShowDialog()
		End Sub

		Private Sub editSequencePropertiesToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editSequencePropertiesToolStripMenuItem.Click
			Dim nsf As New NewSequenceForm(_sequence)
			If nsf.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				_sequence = nsf.Sequence
				SaveSequence()
				CreateSequence()
				RedrawSequence()
			End If
		End Sub

		Private Sub playlistToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles playlistToolStripMenuItem.Click
			NewPlaylist()
		End Sub

		Private Sub newPlaylistToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles newPlaylistToolStripMenuItem.Click
			NewPlaylist()
		End Sub

		Private Sub playlistToolStripMenuItem2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles playlistToolStripMenuItem2.Click
			OpenPlaylist()
		End Sub

		Private Sub playlistToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles playlistToolStripMenuItem1.Click
			OpenPlaylist()
		End Sub
#End Region

#Region "Events"
		Private Sub MainForm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
			TurnOffDevices()
		End Sub

		Private Sub dgvMain_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) Handles dgvMain.Scroll
			' when the main grid is scrolled, scroll the header by the same amount
			dgvHeader.HorizontalScrollingOffset = e.NewValue
		End Sub

		Private Sub dgvMain_CellDoubleClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvMain.CellDoubleClick
			HighlightCell(True, e.RowIndex, e.ColumnIndex)
		End Sub

		Private Sub dgvMain_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles dgvMain.KeyDown
			' O == On
			If e.KeyCode = Keys.O Then
				HighlightCells(True)

			' F == Off
			ElseIf e.KeyCode = Keys.F Then
				HighlightCells(False)
			End If
		End Sub

		Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
			e.Cancel = CheckSave()
		End Sub

		Private Function CheckSave() As Boolean
			If _sequence.Channels.Count > 0 Then
				Dim dr As DialogResult = MessageBox.Show("Do you want to save the current sequence before exiting?", "Save Sequence?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

				Select Case dr
					Case System.Windows.Forms.DialogResult.Yes
						If SaveSequence() = System.Windows.Forms.DialogResult.Cancel Then
							Return True
						End If
					Case System.Windows.Forms.DialogResult.No
					Case System.Windows.Forms.DialogResult.Cancel
						Return True
				End Select
			End If
			Return False
		End Function
#End Region
	End Class
End Namespace
