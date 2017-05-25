Imports Microsoft.VisualBasic
Imports System
Namespace LightSequencer
	Partial Public Class MainForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
			Dim dataGridViewCellStyle1 As New System.Windows.Forms.DataGridViewCellStyle()
			Dim dataGridViewCellStyle2 As New System.Windows.Forms.DataGridViewCellStyle()
			Me.mnuMenu = New System.Windows.Forms.MenuStrip()
			Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.newToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.sequenceToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
			Me.playlistToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.openToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.sequenceToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
			Me.playlistToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
			Me.saveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.saveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
			Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.editToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.cutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.copyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.pasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
			Me.selectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.sequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.startSequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.stopSequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.recordSequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
			Me.editSequencePropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.testChannelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.helpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.aboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.tlsToolStrip = New System.Windows.Forms.ToolStrip()
			Me.newToolStripButton = New System.Windows.Forms.ToolStripSplitButton()
			Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
			Me.newPlaylistToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.openToolStripButton = New System.Windows.Forms.ToolStripSplitButton()
			Me.sequenceToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
			Me.playlistToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
			Me.saveToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator()
			Me.cutToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.copyToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.pasteToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
			Me.playToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.stopToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.recordToolStripButton = New System.Windows.Forms.ToolStripButton()
			Me.dgvHeader = New System.Windows.Forms.DataGridView()
			Me.dgvMain = New System.Windows.Forms.DataGridView()
			Me.cmsContext = New System.Windows.Forms.ContextMenuStrip(Me.components)
			Me.onToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.offToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
			Me.toolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
			Me.mnuMenu.SuspendLayout()
			Me.tlsToolStrip.SuspendLayout()
			CType(Me.dgvHeader, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.cmsContext.SuspendLayout()
			Me.SuspendLayout()
			' 
			' mnuMenu
			' 
			Me.mnuMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.fileToolStripMenuItem, Me.editToolStripMenuItem, Me.sequenceToolStripMenuItem, Me.toolsToolStripMenuItem, Me.helpToolStripMenuItem})
			Me.mnuMenu.Location = New System.Drawing.Point(0, 0)
			Me.mnuMenu.Name = "mnuMenu"
			Me.mnuMenu.Size = New System.Drawing.Size(764, 24)
			Me.mnuMenu.TabIndex = 0
			' 
			' fileToolStripMenuItem
			' 
			Me.fileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.newToolStripMenuItem, Me.openToolStripMenuItem, Me.toolStripSeparator3, Me.saveToolStripMenuItem, Me.saveAsToolStripMenuItem, Me.toolStripSeparator4, Me.exitToolStripMenuItem})
			Me.fileToolStripMenuItem.Name = "fileToolStripMenuItem"
			Me.fileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
			Me.fileToolStripMenuItem.Text = "&File"
			' 
			' newToolStripMenuItem
			' 
			Me.newToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.sequenceToolStripMenuItem1, Me.playlistToolStripMenuItem})
			Me.newToolStripMenuItem.Image = (CType(resources.GetObject("newToolStripMenuItem.Image"), System.Drawing.Image))
			Me.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.newToolStripMenuItem.Name = "newToolStripMenuItem"
			Me.newToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys))
			Me.newToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.newToolStripMenuItem.Text = "&New"
			' 
			' sequenceToolStripMenuItem1
			' 
			Me.sequenceToolStripMenuItem1.Name = "sequenceToolStripMenuItem1"
			Me.sequenceToolStripMenuItem1.Size = New System.Drawing.Size(125, 22)
			Me.sequenceToolStripMenuItem1.Text = "&Sequence"
'			Me.sequenceToolStripMenuItem1.Click += New System.EventHandler(Me.sequenceToolStripMenuItem1_Click);
			' 
			' playlistToolStripMenuItem
			' 
			Me.playlistToolStripMenuItem.Name = "playlistToolStripMenuItem"
			Me.playlistToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
			Me.playlistToolStripMenuItem.Text = "&Playlist"
'			Me.playlistToolStripMenuItem.Click += New System.EventHandler(Me.playlistToolStripMenuItem_Click);
			' 
			' openToolStripMenuItem
			' 
			Me.openToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.sequenceToolStripMenuItem2, Me.playlistToolStripMenuItem1})
			Me.openToolStripMenuItem.Image = (CType(resources.GetObject("openToolStripMenuItem.Image"), System.Drawing.Image))
			Me.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.openToolStripMenuItem.Name = "openToolStripMenuItem"
			Me.openToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys))
			Me.openToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.openToolStripMenuItem.Text = "&Open"
			' 
			' sequenceToolStripMenuItem2
			' 
			Me.sequenceToolStripMenuItem2.Name = "sequenceToolStripMenuItem2"
			Me.sequenceToolStripMenuItem2.Size = New System.Drawing.Size(125, 22)
			Me.sequenceToolStripMenuItem2.Text = "&Sequence"
'			Me.sequenceToolStripMenuItem2.Click += New System.EventHandler(Me.sequenceToolStripMenuItem2_Click);
			' 
			' playlistToolStripMenuItem1
			' 
			Me.playlistToolStripMenuItem1.Name = "playlistToolStripMenuItem1"
			Me.playlistToolStripMenuItem1.Size = New System.Drawing.Size(125, 22)
			Me.playlistToolStripMenuItem1.Text = "&Playlist"
'			Me.playlistToolStripMenuItem1.Click += New System.EventHandler(Me.playlistToolStripMenuItem1_Click);
			' 
			' toolStripSeparator3
			' 
			Me.toolStripSeparator3.Name = "toolStripSeparator3"
			Me.toolStripSeparator3.Size = New System.Drawing.Size(143, 6)
			' 
			' saveToolStripMenuItem
			' 
			Me.saveToolStripMenuItem.Image = (CType(resources.GetObject("saveToolStripMenuItem.Image"), System.Drawing.Image))
			Me.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.saveToolStripMenuItem.Name = "saveToolStripMenuItem"
			Me.saveToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys))
			Me.saveToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.saveToolStripMenuItem.Text = "&Save"
'			Me.saveToolStripMenuItem.Click += New System.EventHandler(Me.saveToolStripMenuItem_Click);
			' 
			' saveAsToolStripMenuItem
			' 
			Me.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem"
			Me.saveAsToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.saveAsToolStripMenuItem.Text = "Save &As"
'			Me.saveAsToolStripMenuItem.Click += New System.EventHandler(Me.saveAsToolStripMenuItem_Click);
			' 
			' toolStripSeparator4
			' 
			Me.toolStripSeparator4.Name = "toolStripSeparator4"
			Me.toolStripSeparator4.Size = New System.Drawing.Size(143, 6)
			' 
			' exitToolStripMenuItem
			' 
			Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
			Me.exitToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
			Me.exitToolStripMenuItem.Text = "E&xit"
'			Me.exitToolStripMenuItem.Click += New System.EventHandler(Me.exitToolStripMenuItem_Click);
			' 
			' editToolStripMenuItem
			' 
			Me.editToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.cutToolStripMenuItem, Me.copyToolStripMenuItem, Me.pasteToolStripMenuItem, Me.toolStripSeparator7, Me.selectAllToolStripMenuItem})
			Me.editToolStripMenuItem.Name = "editToolStripMenuItem"
			Me.editToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
			Me.editToolStripMenuItem.Text = "&Edit"
			' 
			' cutToolStripMenuItem
			' 
			Me.cutToolStripMenuItem.Image = (CType(resources.GetObject("cutToolStripMenuItem.Image"), System.Drawing.Image))
			Me.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.cutToolStripMenuItem.Name = "cutToolStripMenuItem"
			Me.cutToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys))
			Me.cutToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
			Me.cutToolStripMenuItem.Text = "Cu&t"
'			Me.cutToolStripMenuItem.Click += New System.EventHandler(Me.HandleCut);
			' 
			' copyToolStripMenuItem
			' 
			Me.copyToolStripMenuItem.Image = (CType(resources.GetObject("copyToolStripMenuItem.Image"), System.Drawing.Image))
			Me.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.copyToolStripMenuItem.Name = "copyToolStripMenuItem"
			Me.copyToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys))
			Me.copyToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
			Me.copyToolStripMenuItem.Text = "&Copy"
'			Me.copyToolStripMenuItem.Click += New System.EventHandler(Me.HandleCopy);
			' 
			' pasteToolStripMenuItem
			' 
			Me.pasteToolStripMenuItem.Image = (CType(resources.GetObject("pasteToolStripMenuItem.Image"), System.Drawing.Image))
			Me.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem"
			Me.pasteToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys))
			Me.pasteToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
			Me.pasteToolStripMenuItem.Text = "&Paste"
'			Me.pasteToolStripMenuItem.Click += New System.EventHandler(Me.HandlePaste);
			' 
			' toolStripSeparator7
			' 
			Me.toolStripSeparator7.Name = "toolStripSeparator7"
			Me.toolStripSeparator7.Size = New System.Drawing.Size(161, 6)
			' 
			' selectAllToolStripMenuItem
			' 
			Me.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem"
			Me.selectAllToolStripMenuItem.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys))
			Me.selectAllToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
			Me.selectAllToolStripMenuItem.Text = "Select &All"
'			Me.selectAllToolStripMenuItem.Click += New System.EventHandler(Me.HandleSelectAll);
			' 
			' sequenceToolStripMenuItem
			' 
			Me.sequenceToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.startSequenceToolStripMenuItem, Me.stopSequenceToolStripMenuItem, Me.recordSequenceToolStripMenuItem, Me.toolStripSeparator2, Me.editSequencePropertiesToolStripMenuItem})
			Me.sequenceToolStripMenuItem.Name = "sequenceToolStripMenuItem"
			Me.sequenceToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
			Me.sequenceToolStripMenuItem.Text = "&Sequence"
			' 
			' startSequenceToolStripMenuItem
			' 
			Me.startSequenceToolStripMenuItem.Name = "startSequenceToolStripMenuItem"
			Me.startSequenceToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
			Me.startSequenceToolStripMenuItem.Text = "&Start Sequence"
'			Me.startSequenceToolStripMenuItem.Click += New System.EventHandler(Me.startSequenceToolStripMenuItem_Click);
			' 
			' stopSequenceToolStripMenuItem
			' 
			Me.stopSequenceToolStripMenuItem.Name = "stopSequenceToolStripMenuItem"
			Me.stopSequenceToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
			Me.stopSequenceToolStripMenuItem.Text = "S&top Sequence"
'			Me.stopSequenceToolStripMenuItem.Click += New System.EventHandler(Me.stopSequenceToolStripMenuItem_Click);
			' 
			' recordSequenceToolStripMenuItem
			' 
			Me.recordSequenceToolStripMenuItem.Name = "recordSequenceToolStripMenuItem"
			Me.recordSequenceToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
			Me.recordSequenceToolStripMenuItem.Text = "&Record Sequence"
'			Me.recordSequenceToolStripMenuItem.Click += New System.EventHandler(Me.recordSequenceToolStripMenuItem_Click);
			' 
			' toolStripSeparator2
			' 
			Me.toolStripSeparator2.Name = "toolStripSeparator2"
			Me.toolStripSeparator2.Size = New System.Drawing.Size(201, 6)
			' 
			' editSequencePropertiesToolStripMenuItem
			' 
			Me.editSequencePropertiesToolStripMenuItem.Enabled = False
			Me.editSequencePropertiesToolStripMenuItem.Name = "editSequencePropertiesToolStripMenuItem"
			Me.editSequencePropertiesToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
			Me.editSequencePropertiesToolStripMenuItem.Text = "&Edit Sequence Properties"
'			Me.editSequencePropertiesToolStripMenuItem.Click += New System.EventHandler(Me.editSequencePropertiesToolStripMenuItem_Click);
			' 
			' toolsToolStripMenuItem
			' 
			Me.toolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.testChannelsToolStripMenuItem})
			Me.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem"
			Me.toolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
			Me.toolsToolStripMenuItem.Text = "&Tools"
			' 
			' testChannelsToolStripMenuItem
			' 
			Me.testChannelsToolStripMenuItem.Name = "testChannelsToolStripMenuItem"
			Me.testChannelsToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
			Me.testChannelsToolStripMenuItem.Text = "&Test Channels"
'			Me.testChannelsToolStripMenuItem.Click += New System.EventHandler(Me.testChannelsToolStripMenuItem_Click);
			' 
			' helpToolStripMenuItem
			' 
			Me.helpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.aboutToolStripMenuItem})
			Me.helpToolStripMenuItem.Name = "helpToolStripMenuItem"
			Me.helpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
			Me.helpToolStripMenuItem.Text = "&Help"
			' 
			' aboutToolStripMenuItem
			' 
			Me.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem"
			Me.aboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
			Me.aboutToolStripMenuItem.Text = "&About..."
'			Me.aboutToolStripMenuItem.Click += New System.EventHandler(Me.aboutToolStripMenuItem_Click);
			' 
			' tlsToolStrip
			' 
			Me.tlsToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.newToolStripButton, Me.openToolStripButton, Me.saveToolStripButton, Me.toolStripSeparator, Me.cutToolStripButton, Me.copyToolStripButton, Me.pasteToolStripButton, Me.toolStripSeparator1, Me.playToolStripButton, Me.stopToolStripButton, Me.recordToolStripButton})
			Me.tlsToolStrip.Location = New System.Drawing.Point(0, 24)
			Me.tlsToolStrip.Name = "tlsToolStrip"
			Me.tlsToolStrip.Size = New System.Drawing.Size(764, 25)
			Me.tlsToolStrip.TabIndex = 1
			' 
			' newToolStripButton
			' 
			Me.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.newToolStripButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.toolStripMenuItem1, Me.newPlaylistToolStripMenuItem})
			Me.newToolStripButton.Image = (CType(resources.GetObject("newToolStripButton.Image"), System.Drawing.Image))
			Me.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.newToolStripButton.Name = "newToolStripButton"
			Me.newToolStripButton.Size = New System.Drawing.Size(32, 22)
			Me.newToolStripButton.Text = "&New"
			Me.newToolStripButton.ToolTipText = "New Sequence"
'			Me.newToolStripButton.ButtonClick += New System.EventHandler(Me.newToolStripButton_Click);
			' 
			' toolStripMenuItem1
			' 
			Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
			Me.toolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
			Me.toolStripMenuItem1.Text = "New &Sequence"
			' 
			' newPlaylistToolStripMenuItem
			' 
			Me.newPlaylistToolStripMenuItem.Name = "newPlaylistToolStripMenuItem"
			Me.newPlaylistToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
			Me.newPlaylistToolStripMenuItem.Text = "New &Playlist"
'			Me.newPlaylistToolStripMenuItem.Click += New System.EventHandler(Me.newPlaylistToolStripMenuItem_Click);
			' 
			' openToolStripButton
			' 
			Me.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.openToolStripButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.sequenceToolStripMenuItem3, Me.playlistToolStripMenuItem2})
			Me.openToolStripButton.Image = (CType(resources.GetObject("openToolStripButton.Image"), System.Drawing.Image))
			Me.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.openToolStripButton.Name = "openToolStripButton"
			Me.openToolStripButton.Size = New System.Drawing.Size(32, 22)
			Me.openToolStripButton.Text = "&Open"
			Me.openToolStripButton.ToolTipText = "Open Sequence"
'			Me.openToolStripButton.ButtonClick += New System.EventHandler(Me.openToolStripButton_Click);
			' 
			' sequenceToolStripMenuItem3
			' 
			Me.sequenceToolStripMenuItem3.Name = "sequenceToolStripMenuItem3"
			Me.sequenceToolStripMenuItem3.Size = New System.Drawing.Size(157, 22)
			Me.sequenceToolStripMenuItem3.Text = "Open &Sequence"
			' 
			' playlistToolStripMenuItem2
			' 
			Me.playlistToolStripMenuItem2.Name = "playlistToolStripMenuItem2"
			Me.playlistToolStripMenuItem2.Size = New System.Drawing.Size(157, 22)
			Me.playlistToolStripMenuItem2.Text = "Open &Playlist"
'			Me.playlistToolStripMenuItem2.Click += New System.EventHandler(Me.playlistToolStripMenuItem2_Click);
			' 
			' saveToolStripButton
			' 
			Me.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.saveToolStripButton.Enabled = False
			Me.saveToolStripButton.Image = (CType(resources.GetObject("saveToolStripButton.Image"), System.Drawing.Image))
			Me.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.saveToolStripButton.Name = "saveToolStripButton"
			Me.saveToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.saveToolStripButton.Text = "&Save"
			Me.saveToolStripButton.ToolTipText = "Save Sequence"
'			Me.saveToolStripButton.Click += New System.EventHandler(Me.saveToolStripButton_Click);
			' 
			' toolStripSeparator
			' 
			Me.toolStripSeparator.Name = "toolStripSeparator"
			Me.toolStripSeparator.Size = New System.Drawing.Size(6, 25)
			' 
			' cutToolStripButton
			' 
			Me.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.cutToolStripButton.Enabled = False
			Me.cutToolStripButton.Image = (CType(resources.GetObject("cutToolStripButton.Image"), System.Drawing.Image))
			Me.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.cutToolStripButton.Name = "cutToolStripButton"
			Me.cutToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.cutToolStripButton.Text = "C&ut"
'			Me.cutToolStripButton.Click += New System.EventHandler(Me.HandleCut);
			' 
			' copyToolStripButton
			' 
			Me.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.copyToolStripButton.Enabled = False
			Me.copyToolStripButton.Image = (CType(resources.GetObject("copyToolStripButton.Image"), System.Drawing.Image))
			Me.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.copyToolStripButton.Name = "copyToolStripButton"
			Me.copyToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.copyToolStripButton.Text = "&Copy"
'			Me.copyToolStripButton.Click += New System.EventHandler(Me.HandleCopy);
			' 
			' pasteToolStripButton
			' 
			Me.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.pasteToolStripButton.Enabled = False
			Me.pasteToolStripButton.Image = (CType(resources.GetObject("pasteToolStripButton.Image"), System.Drawing.Image))
			Me.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.pasteToolStripButton.Name = "pasteToolStripButton"
			Me.pasteToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.pasteToolStripButton.Text = "&Paste"
'			Me.pasteToolStripButton.Click += New System.EventHandler(Me.HandlePaste);
			' 
			' toolStripSeparator1
			' 
			Me.toolStripSeparator1.Name = "toolStripSeparator1"
			Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
			' 
			' playToolStripButton
			' 
			Me.playToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.playToolStripButton.Enabled = False
			Me.playToolStripButton.Image = (CType(resources.GetObject("playToolStripButton.Image"), System.Drawing.Image))
			Me.playToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.playToolStripButton.Name = "playToolStripButton"
			Me.playToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.playToolStripButton.Text = "P&lay"
'			Me.playToolStripButton.Click += New System.EventHandler(Me.playToolStripButton_Click);
			' 
			' stopToolStripButton
			' 
			Me.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.stopToolStripButton.Enabled = False
			Me.stopToolStripButton.Image = (CType(resources.GetObject("stopToolStripButton.Image"), System.Drawing.Image))
			Me.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.stopToolStripButton.Name = "stopToolStripButton"
			Me.stopToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.stopToolStripButton.Text = "&Stop"
'			Me.stopToolStripButton.Click += New System.EventHandler(Me.stopToolStripButton_Click);
			' 
			' recordToolStripButton
			' 
			Me.recordToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
			Me.recordToolStripButton.Enabled = False
			Me.recordToolStripButton.Image = (CType(resources.GetObject("recordToolStripButton.Image"), System.Drawing.Image))
			Me.recordToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.recordToolStripButton.Name = "recordToolStripButton"
			Me.recordToolStripButton.Size = New System.Drawing.Size(23, 22)
			Me.recordToolStripButton.Text = "Record Sequence"
'			Me.recordToolStripButton.Click += New System.EventHandler(Me.recordToolStripButton_Click);
			' 
			' dgvHeader
			' 
			Me.dgvHeader.AllowUserToAddRows = False
			Me.dgvHeader.AllowUserToDeleteRows = False
			Me.dgvHeader.AllowUserToResizeColumns = False
			Me.dgvHeader.AllowUserToResizeRows = False
			Me.dgvHeader.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.dgvHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
			dataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True
			Me.dgvHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1
			Me.dgvHeader.ColumnHeadersHeight = 24
			Me.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
			Me.dgvHeader.Location = New System.Drawing.Point(0, 52)
			Me.dgvHeader.Name = "dgvHeader"
			Me.dgvHeader.ReadOnly = True
			Me.dgvHeader.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
			Me.dgvHeader.Size = New System.Drawing.Size(764, 20)
			Me.dgvHeader.TabIndex = 2
			Me.dgvHeader.VirtualMode = True
			' 
			' dgvMain
			' 
			Me.dgvMain.AllowUserToAddRows = False
			Me.dgvMain.AllowUserToDeleteRows = False
			Me.dgvMain.AllowUserToResizeColumns = False
			Me.dgvMain.AllowUserToResizeRows = False
			Me.dgvMain.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.dgvMain.BorderStyle = System.Windows.Forms.BorderStyle.None
			Me.dgvMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
			dataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True
			Me.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2
			Me.dgvMain.ColumnHeadersHeight = 20
			Me.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
			Me.dgvMain.ContextMenuStrip = Me.cmsContext
			Me.dgvMain.Location = New System.Drawing.Point(0, 68)
			Me.dgvMain.Name = "dgvMain"
			Me.dgvMain.ReadOnly = True
			Me.dgvMain.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
			Me.dgvMain.Size = New System.Drawing.Size(764, 477)
			Me.dgvMain.TabIndex = 3
			Me.dgvMain.VirtualMode = True
'			Me.dgvMain.Scroll += New System.Windows.Forms.ScrollEventHandler(Me.dgvMain_Scroll);
'			Me.dgvMain.CellDoubleClick += New System.Windows.Forms.DataGridViewCellEventHandler(Me.dgvMain_CellDoubleClick);
'			Me.dgvMain.KeyDown += New System.Windows.Forms.KeyEventHandler(Me.dgvMain_KeyDown);
			' 
			' cmsContext
			' 
			Me.cmsContext.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.onToolStripMenuItem, Me.offToolStripMenuItem, Me.toolStripSeparator5, Me.toolStripMenuItem2, Me.toolStripMenuItem3, Me.toolStripMenuItem4, Me.toolStripMenuItem5})
			Me.cmsContext.Name = "cmsContext"
			Me.cmsContext.Size = New System.Drawing.Size(165, 142)
			' 
			' onToolStripMenuItem
			' 
			Me.onToolStripMenuItem.Name = "onToolStripMenuItem"
			Me.onToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
			Me.onToolStripMenuItem.Text = "&On"
'			Me.onToolStripMenuItem.Click += New System.EventHandler(Me.onToolStripMenuItem_Click);
			' 
			' offToolStripMenuItem
			' 
			Me.offToolStripMenuItem.Name = "offToolStripMenuItem"
			Me.offToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
			Me.offToolStripMenuItem.Text = "O&ff"
'			Me.offToolStripMenuItem.Click += New System.EventHandler(Me.offToolStripMenuItem_Click);
			' 
			' toolStripSeparator5
			' 
			Me.toolStripSeparator5.Name = "toolStripSeparator5"
			Me.toolStripSeparator5.Size = New System.Drawing.Size(161, 6)
			' 
			' toolStripMenuItem2
			' 
			Me.toolStripMenuItem2.Image = (CType(resources.GetObject("toolStripMenuItem2.Image"), System.Drawing.Image))
			Me.toolStripMenuItem2.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.toolStripMenuItem2.Name = "toolStripMenuItem2"
			Me.toolStripMenuItem2.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys))
			Me.toolStripMenuItem2.Size = New System.Drawing.Size(164, 22)
			Me.toolStripMenuItem2.Text = "Cu&t"
'			Me.toolStripMenuItem2.Click += New System.EventHandler(Me.HandleCut);
			' 
			' toolStripMenuItem3
			' 
			Me.toolStripMenuItem3.Image = (CType(resources.GetObject("toolStripMenuItem3.Image"), System.Drawing.Image))
			Me.toolStripMenuItem3.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.toolStripMenuItem3.Name = "toolStripMenuItem3"
			Me.toolStripMenuItem3.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys))
			Me.toolStripMenuItem3.Size = New System.Drawing.Size(164, 22)
			Me.toolStripMenuItem3.Text = "&Copy"
'			Me.toolStripMenuItem3.Click += New System.EventHandler(Me.HandleCopy);
			' 
			' toolStripMenuItem4
			' 
			Me.toolStripMenuItem4.Image = (CType(resources.GetObject("toolStripMenuItem4.Image"), System.Drawing.Image))
			Me.toolStripMenuItem4.ImageTransparentColor = System.Drawing.Color.Magenta
			Me.toolStripMenuItem4.Name = "toolStripMenuItem4"
			Me.toolStripMenuItem4.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys))
			Me.toolStripMenuItem4.Size = New System.Drawing.Size(164, 22)
			Me.toolStripMenuItem4.Text = "&Paste"
'			Me.toolStripMenuItem4.Click += New System.EventHandler(Me.HandlePaste);
			' 
			' toolStripMenuItem5
			' 
			Me.toolStripMenuItem5.Name = "toolStripMenuItem5"
			Me.toolStripMenuItem5.ShortcutKeys = (CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys))
			Me.toolStripMenuItem5.Size = New System.Drawing.Size(164, 22)
			Me.toolStripMenuItem5.Text = "Select &All"
'			Me.toolStripMenuItem5.Click += New System.EventHandler(Me.HandleSelectAll);
			' 
			' MainForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(764, 544)
			Me.Controls.Add(Me.dgvHeader)
			Me.Controls.Add(Me.tlsToolStrip)
			Me.Controls.Add(Me.mnuMenu)
			Me.Controls.Add(Me.dgvMain)
			Me.MainMenuStrip = Me.mnuMenu
			Me.Name = "MainForm"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "Light Sequencer"
'			Me.FormClosed += New System.Windows.Forms.FormClosedEventHandler(Me.MainForm_FormClosed);
'			Me.FormClosing += New System.Windows.Forms.FormClosingEventHandler(Me.MainForm_FormClosing);
			Me.mnuMenu.ResumeLayout(False)
			Me.mnuMenu.PerformLayout()
			Me.tlsToolStrip.ResumeLayout(False)
			Me.tlsToolStrip.PerformLayout()
			CType(Me.dgvHeader, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.cmsContext.ResumeLayout(False)
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private tlsToolStrip As System.Windows.Forms.ToolStrip
		Private WithEvents saveToolStripButton As System.Windows.Forms.ToolStripButton
		Private toolStripSeparator As System.Windows.Forms.ToolStripSeparator
		Private WithEvents cutToolStripButton As System.Windows.Forms.ToolStripButton
		Private WithEvents copyToolStripButton As System.Windows.Forms.ToolStripButton
		Private WithEvents pasteToolStripButton As System.Windows.Forms.ToolStripButton
		Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
		Private dgvHeader As System.Windows.Forms.DataGridView
		Private WithEvents dgvMain As System.Windows.Forms.DataGridView
		Private cmsContext As System.Windows.Forms.ContextMenuStrip
		Private WithEvents onToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents offToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents playToolStripButton As System.Windows.Forms.ToolStripButton
		Private WithEvents stopToolStripButton As System.Windows.Forms.ToolStripButton
		Private fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private newToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private openToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private toolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
		Private WithEvents saveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents saveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
		Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private editToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents cutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents copyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents pasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private toolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
		Private WithEvents selectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private helpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents aboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private sequenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents startSequenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents stopSequenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents recordSequenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents recordToolStripButton As System.Windows.Forms.ToolStripButton
		Private mnuMenu As System.Windows.Forms.MenuStrip
		Private toolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents testChannelsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
		Private WithEvents editSequencePropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents sequenceToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents playlistToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents sequenceToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents playlistToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents newToolStripButton As System.Windows.Forms.ToolStripSplitButton
		Private toolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents newPlaylistToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents openToolStripButton As System.Windows.Forms.ToolStripSplitButton
		Private sequenceToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents playlistToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
		Private toolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
		Private WithEvents toolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents toolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents toolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
		Private WithEvents toolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
	End Class
End Namespace

