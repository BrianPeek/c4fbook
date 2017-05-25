using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SharedUtilities;
using System.IO;

namespace InnerTube
{
    /// <summary>
    /// Interaction logic for FirstRunWindow.xaml
    /// </summary>
    public partial class FirstRunWindow : Window
    {
        #region Fields
        private bool ZuneInstalled;
        private bool iTunesInstalled;
        private string videoDir;
        private string subDir;  
        #endregion

        public FirstRunWindow()
        {
            InitializeComponent();
            this.Loaded +=new RoutedEventHandler(FirstRunWindow_Loaded);
        }


        void FirstRunWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetupUI();
        }

        private void SetupUI()
        {
            //Zune Checkbox
            ZuneInstalled = ProcessHelper.IsProcessRunning(ProcessType.Zune);            
            chkZune.IsEnabled = ZuneInstalled;
            if (ZuneInstalled)
            {
                chkZune.Foreground = Brushes.Black;
            }

            //iTunes Checkbox
            iTunesInstalled = ProcessHelper.IsProcessRunning(ProcessType.iTunes);
            chkiTunes.IsEnabled = iTunesInstalled;
            if (iTunesInstalled)
            {
                chkiTunes.Foreground = Brushes.Black;
            }
           
            //File Directories
            subDir = App.Settings.AppName;
            videoDir = FileHelper.BuildPath(ref subDir);
            
            //Textboxes
            txtLocation.Text = videoDir;
            txtSubDir.Text = subDir;
            
            btnOK.IsEnabled = true;
        }

 

        private void SetAppSettings()
        {
            App.Settings.FirstRun = false;

            App.Settings.iTunesInstalled = iTunesInstalled;
            App.Settings.ZuneInstalled = ZuneInstalled;

            string xmlFile = FileHelper.BuildXmlName(App.Settings.AppName);

            App.Settings.InnerTubeFeedFile =
                System.IO.Path.Combine(subDir, xmlFile);
            App.Settings.VideoPath = videoDir;
            SettingService.Save(App.Settings);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SetAppSettings();
            this.Close();
        }
    }
}
