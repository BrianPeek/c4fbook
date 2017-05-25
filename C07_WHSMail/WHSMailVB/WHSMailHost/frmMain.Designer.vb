Imports Microsoft.VisualBasic
Imports System
Namespace WHSMailHost
	Public Partial Class frmMain
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (Not components Is Nothing) Then
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
			Me.niIcon = New System.Windows.Forms.NotifyIcon(Me.components)
			Me.cmsMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
			Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
			Me.cmsMenu.SuspendLayout()
			Me.SuspendLayout()
			' 
			' niIcon
			' 
			Me.niIcon.ContextMenuStrip = Me.cmsMenu
			Me.niIcon.Icon = (CType(resources.GetObject("niIcon.Icon"), System.Drawing.Icon))
			Me.niIcon.Text = "WHS Mail Host"
			Me.niIcon.Visible = True
			' 
			' cmsMenu
			' 
			Me.cmsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.mnuExit})
			Me.cmsMenu.Name = "cmsMenu"
			Me.cmsMenu.Size = New System.Drawing.Size(153, 48)
			' 
			' mnuExit
			' 
			Me.mnuExit.Name = "mnuExit"
			Me.mnuExit.Size = New System.Drawing.Size(152, 22)
			Me.mnuExit.Text = "&Exit"
'			Me.mnuExit.Click += New System.EventHandler(Me.mnuExit_Click);
			' 
			' frmMain
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(284, 264)
			Me.Icon = (CType(resources.GetObject("$this.Icon"), System.Drawing.Icon))
			Me.Name = "frmMain"
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "WHS Mail Host"
			Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
'			Me.Resize += New System.EventHandler(Me.frmMain_Resize);
'			Me.FormClosing += New System.Windows.Forms.FormClosingEventHandler(Me.frmMain_FormClosing);
'			Me.Load += New System.EventHandler(Me.frmMain_Load);
			Me.cmsMenu.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private niIcon As System.Windows.Forms.NotifyIcon
		Private cmsMenu As System.Windows.Forms.ContextMenuStrip
		Private WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
	End Class
End Namespace

