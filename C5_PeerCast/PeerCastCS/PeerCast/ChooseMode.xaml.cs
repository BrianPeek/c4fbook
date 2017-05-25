using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PeerCast
{
    public partial class ChooseMode : Window
    {
        public ChooseMode()
        {
            InitializeComponent();
            checkIfPathExists();
        }

        private void checkIfPathExists()
        {
            if (!Directory.Exists(Properties.Settings.Default.FileDirectory))
            {
                Properties.Settings.Default.FileDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void SetMode(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.Name == "Server")
            {
                App.IsServerMode = true; 
            }
            else
            {
                App.IsServerMode = false; 
            }

            //open Main Window
            MainWindow w = new MainWindow();
            w.Show();

            //close current Window
            this.Close(); 
        }
    }
}
