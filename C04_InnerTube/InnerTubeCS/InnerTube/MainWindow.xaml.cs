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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using SharedUtilities;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls.Primitives;


namespace InnerTube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public static RoutedCommand AddFeedCommand = new RoutedCommand("AddFeedCommand", typeof(MainWindow));
        public static RoutedCommand UpdateFeedCommand = new RoutedCommand("UpdateFeedCommand", typeof(MainWindow));
        public static RoutedCommand DownloadFeedCommand = new RoutedCommand("DownloadFeedCommand", typeof(MainWindow));

        private void AddFeed(object sender, RoutedEventArgs e)
        {
            AddNewFeed add = new InnerTube.AddNewFeed();
            add.ShowDialog();
        }

        private void UpdateFeed(object sender, RoutedEventArgs e)
        {
            if (!InnerTubeFeedWorker.IsRunningAsync())
            {
                InnerTubeFeedWorker iWork = InnerTubeFeedWorker.GetInstance(App.InnerTubeFeeds, App.Settings);
                iWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(iWork_RunWorkerCompleted);
                iWork.RunWorkerAsync(WorkType.UpdateFeeds);
            }
        }

        void iWork_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            App.InnerTubeFeeds = (ObservableCollection<InnerTubeFeed>)e.Result;
            var serial = new Serializer<ObservableCollection<InnerTubeFeed>>();            
            serial.Serialize(App.InnerTubeFeeds, App.Settings.InnerTubeFeedFile);
            App.UpdateFeeds = false;
            this.DataContext = App.InnerTubeFeeds;
        }


        private void DownloadFeed(object sender, RoutedEventArgs e)
        {
            Progress prog = new Progress();
            prog.ShowDialog();      
        }

        private void DeleteVideo(object sender, RoutedEventArgs e)
        {
            MenuItem currentItem = (MenuItem)sender;
            InnerTubeVideo deleteVideo = (InnerTubeVideo)currentItem.CommandParameter;
            Deleted.Delete(App.InnerTubeFeeds, deleteVideo);
        }


        private void DeleteFeed(object sender, RoutedEventArgs e)
        {
            MenuItem currentItem = (MenuItem)sender;
            InnerTubeFeed deleteFeed = (InnerTubeFeed)currentItem.CommandParameter;
            Deleted.Delete(App.InnerTubeFeeds, deleteFeed);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Settings.FirstRun)
            {
                OpenFirstRunWindow();
            }

            this.DataContext = App.InnerTubeFeeds;
        }

        private static void OpenFirstRunWindow()
        {
            //Show FirstRun Window
            FirstRunWindow fr = new FirstRunWindow();
            fr.ShowDialog();
            
        }


        void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }



    }
}
