Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports Microsoft.Win32
Imports System.IO

Namespace WowFeedGrabber
	Friend Class Program
		Private Shared notifyIcon As NotifyIcon
		Private Shared feedGrabber As FeedGrabber

		Private Shared wowRegistryKeyPaths() As String = { "SOFTWARE\Blizzard Entertainment\World of Warcraft", "SOFTWARE\Wow6432Node\Blizzard Entertainment\World of Warcraft" }

		<STAThread> _
		Shared Sub Main(ByVal args() As String)
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)

			' Get the path where the SavedVariables file resides
			Dim savedVariablesPath As String = Program.GetSavedVariablesPath()
			If savedVariablesPath Is Nothing Then
				MessageBox.Show("World of Warcraft Feed Grabber", "Could not find World of Warcraft installation or no accounts available.", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return
			End If

			' Initialize feed grabber
			feedGrabber = New FeedGrabber(savedVariablesPath)

			' Enable background updates
			feedGrabber.EnableBackgroundUpdates()

			' Explicitly save feeds on start-up
			feedGrabber.SaveFeeds()

			' Create NotifyIcon and make it visible
			notifyIcon = Program.CreateNotifyIcon()
			notifyIcon.Visible = True

			' Show balloon tip that we're running
			notifyIcon.ShowBalloonTip(5000, "World of Warcraft Feed Grabber", "Feed Grabber is running in the background...", ToolTipIcon.Info)

			' Run message loop
			Application.Run()

			' Dispose NotifyIcon
			notifyIcon.Dispose()
		End Sub

		Private Shared Function GetSavedVariablesPath() As String
			For Each wowRegistryKeyPath In wowRegistryKeyPaths
				Using key = Registry.LocalMachine.OpenSubKey(wowRegistryKeyPath)
					If key Is Nothing Then
						Continue For
					End If

					Dim installationPath As String = CStr(key.GetValue("InstallPath", Nothing))
					If installationPath Is Nothing Then
						Return Nothing
					End If

					Dim accountPath As String = Path.Combine(installationPath, "WTF\Account")
					Dim accounts() As String = Directory.GetDirectories(accountPath)
					If accounts.Length = 0 Then
						Return Nothing
					End If

					Return Path.Combine(accounts(0), "SavedVariables\FeedReader.lua")
				End Using
			Next wowRegistryKeyPath

			Return Nothing
		End Function

		Private Shared Function CreateNotifyIcon() As NotifyIcon
			Dim contextMenuStrip = New ContextMenuStrip()

            contextMenuStrip.Items.Add(Program.CreateMenuItem(My.Resources.RefreshText, AddressOf OnClickRefresh, True))
			contextMenuStrip.Items.Add(New ToolStripSeparator())
            contextMenuStrip.Items.Add(Program.CreateMenuItem(My.Resources.ExitText, AddressOf OnClickExit, False))

            Return New NotifyIcon With {.Text = My.Resources.NotifyIconText, .Icon = My.Resources.Feed, .ContextMenuStrip = contextMenuStrip}

		End Function


		Private Shared Function CreateMenuItem(ByVal text As String, ByVal clickHandler As EventHandler, ByVal bold As Boolean) As ToolStripMenuItem
			Dim menuItem = New ToolStripMenuItem() With {.Text = text}

			If bold Then
				menuItem.Font = New Font(menuItem.Font, FontStyle.Bold)
			End If

			AddHandler menuItem.Click, clickHandler

			Return menuItem
		End Function

		Private Shared Sub OnClickRefresh(ByVal sender As Object, ByVal e As EventArgs)
			feedGrabber.SaveFeeds()
		End Sub

		Private Shared Sub OnClickExit(ByVal sender As Object, ByVal e As EventArgs)
			Application.Exit()
		End Sub
	End Class
End Namespace
