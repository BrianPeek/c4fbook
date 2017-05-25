using System;
using System.Windows.Forms;
using WowFeedGrabber.Properties;
using System.Drawing;
using Microsoft.Win32;
using System.IO;

namespace WowFeedGrabber
{
    class Program
    {
        private static NotifyIcon notifyIcon;
        private static FeedGrabber feedGrabber;

        private static string[] wowRegistryKeyPaths = new []
                                                      {
                                                          @"SOFTWARE\Blizzard Entertainment\World of Warcraft",
                                                          @"SOFTWARE\Wow6432Node\Blizzard Entertainment\World of Warcraft"
                                                      };

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get the path where the SavedVariables file resides
            string savedVariablesPath = Program.GetSavedVariablesPath();
            if (savedVariablesPath == null)
            {
                MessageBox.Show("World of Warcraft Feed Grabber", "Could not find World of Warcraft installation or no accounts available.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Initialize feed grabber
            feedGrabber = new FeedGrabber(savedVariablesPath);

            // Enable background updates
            feedGrabber.EnableBackgroundUpdates();

            // Explicitly save feeds on start-up
            feedGrabber.SaveFeeds();

            // Create NotifyIcon and make it visible
            notifyIcon = Program.CreateNotifyIcon();
            notifyIcon.Visible = true;

            // Show balloon tip that we're running
            notifyIcon.ShowBalloonTip(5000, "World of Warcraft Feed Grabber", "Feed Grabber is running in the background...", ToolTipIcon.Info);

            // Run message loop
            Application.Run();

            // Dispose NotifyIcon
            notifyIcon.Dispose();
        }

        private static string GetSavedVariablesPath()
        {
            foreach (var wowRegistryKeyPath in wowRegistryKeyPaths)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(wowRegistryKeyPath))
                {
                    if (key == null)
                        continue;

                    string installationPath = (string)key.GetValue("InstallPath", null);
                    if (installationPath == null)
                        return null;

                    string accountPath = Path.Combine(installationPath, @"WTF\Account");
                    string[] accounts = Directory.GetDirectories(accountPath);
                    if (accounts.Length == 0)
                        return null;

                    return Path.Combine(accounts[0], @"SavedVariables\FeedReader.lua");
                }
            }

            return null;
        }

        private static NotifyIcon CreateNotifyIcon()
        {
            var contextMenuStrip = new ContextMenuStrip();

            contextMenuStrip.Items.Add(Program.CreateMenuItem(Resources.RefreshText, OnClickRefresh, true));
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(Program.CreateMenuItem(Resources.ExitText, OnClickExit, false));

            return new NotifyIcon
                   {
                       Text = Resources.NotifyIconText,
                       Icon = Resources.Feed,
                       ContextMenuStrip = contextMenuStrip
                   };

        }


        private static ToolStripMenuItem CreateMenuItem(string text, EventHandler clickHandler, bool bold)
        {
            var menuItem = new ToolStripMenuItem() { Text = text };

            if (bold)
                menuItem.Font = new Font(menuItem.Font, FontStyle.Bold);
            
            menuItem.Click += clickHandler;

            return menuItem;
        }

        private static void OnClickRefresh(object sender, EventArgs e)
        {
            feedGrabber.SaveFeeds();
        }

        private static void OnClickExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
