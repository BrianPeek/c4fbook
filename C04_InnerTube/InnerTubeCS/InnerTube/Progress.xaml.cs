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
using System.Collections.ObjectModel;
using SharedUtilities;

namespace InnerTube
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {
        public Progress()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();

if (!InnerTubeFeedWorker.IsRunningAsync())
{
    InnerTubeFeedWorker iWork = InnerTubeFeedWorker.GetInstance(App.InnerTubeFeeds, App.Settings);
    iWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(iWork_RunWorkerCompleted);
    iWork.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(iWork_ProgressChanged);

    iWork.RunWorkerAsync(WorkType.All);                
}


        }

        private void ResetControls()
        {
            btnDownload.IsEnabled = false;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.Maximum = CalculateMax();

            StatusList.Items.Clear();
        }

        private double CalculateMax()
        {
            double max = 0;

            foreach (var feed in App.InnerTubeFeeds)
            {
                //+1 for each feed
                max++; 
                
                //add all feed videos 2x for download + convert
                max += feed.FeedVideos.Count * 2;

            }
            max++; //add 1 more for updating

            return max;

        }

        void iWork_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            string s = (string)(e.UserState);
            StatusList.Items.Add(s);
            progressBar1.Value += 1;
        }

        void iWork_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnDownload.IsEnabled = true;
            progressBar1.Visibility = Visibility.Hidden; 

            App.InnerTubeFeeds = (ObservableCollection<InnerTubeFeed>)e.Result;
            
            var serial = new Serializer<ObservableCollection<InnerTubeFeed>>();
            serial.Serialize(App.InnerTubeFeeds, App.Settings.InnerTubeFeedFile);

            string path = System.IO.Path.Combine(App.Settings.VideoPath,App.Settings.AppName);

            StringBuilder sb = new StringBuilder();
            foreach (var item in StatusList.Items)
            {
                sb.Append(item); 
            }
            FileHelper.WriteLogFile(path, sb.ToString());

            App.UpdateFeeds = false;
        }
    }
}


