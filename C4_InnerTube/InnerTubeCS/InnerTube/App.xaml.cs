using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SharedUtilities;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace InnerTube
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        #region Fields
        public static ObservableCollection<InnerTubeFeed> InnerTubeFeeds { get; set; }
        public static Setting Settings { get; set; }
        public static bool UpdateFeeds;

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            LoadSettings();
            LoadFeeds();
            base.OnStartup(e);
        }

        private static void LoadSettings()
        {
            Settings = SettingService.Load();
        }

        private static void LoadFeeds()
        {
            try
            {
                var serial = new Serializer<ObservableCollection<InnerTubeFeed>>();                
                ObservableCollection<InnerTubeFeed> feeds = serial.Deserialize(Settings.InnerTubeFeedFile);
                if (feeds.Count > 0)
                {
                    App.InnerTubeFeeds = feeds;
                }
                else
                {
                    App.InnerTubeFeeds = BuildInitialCriteria();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                App.InnerTubeFeeds = BuildInitialCriteria();
            }

        }

        public static ObservableCollection<InnerTubeFeed> BuildInitialCriteria()
        {
            ObservableCollection<InnerTubeFeed> sc = new ObservableCollection<InnerTubeFeed>();
            sc.Add(new InnerTubeFeed()
                {
                    FeedName = "Most Viewed Videos",
                    FeedUrl = @"http://gdata.youtube.com/feeds/api/standardfeeds/most_viewed"
                }
            );

            return sc;
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            SaveFeeds();
            base.OnExit(e);
        }

        public static void SaveFeeds()
        {
            SettingService.Save(Settings);
            var serial = new Serializer<ObservableCollection<InnerTubeFeed>>();
            serial.Serialize(App.InnerTubeFeeds, Settings.InnerTubeFeedFile);
        }        




    }
}
