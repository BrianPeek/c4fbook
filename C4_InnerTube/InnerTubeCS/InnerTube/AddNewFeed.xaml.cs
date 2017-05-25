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
using System.Collections.ObjectModel;


namespace InnerTube
{
    /// <summary>
    /// Interaction logic for AddNewItem.xaml
    /// </summary>
    public partial class AddNewFeed : Window
    {
        public AddNewFeed()
        {
            InitializeComponent();
        }

        public static string[] GetTimeValues()
        { 
            return Utilities.GetEnumNames(typeof(InnerTubeTime));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] test = GetTimeValues();
            DataContext = test;
        }

        private void btnAddFeeds_Click(object sender, RoutedEventArgs e)
        {
            ValidateMostViewed();
            ValidateTopRated();
            ValidateTopFavorited();
            ValidateFavoritesByUser();
            ValidateSubscriptionsByUser();
            ValidateCustomSearch();
            App.UpdateFeeds = true; 

        }

        private void ValidateCustomSearch()
        {
            if (chkSearch.IsChecked.Value)
            {
                if (!String.IsNullOrEmpty(Search.Text))
                {
                    string url = InnerTubeService.BuildSearchUrl(Search.Text);
                    AddFeed("Search for: '" + Search.Text + "'", url);
                }
            }   
        }

        private void ValidateSubscriptionsByUser()
        {
            if (chkSubUser.IsChecked.Value)
            {
                if (!String.IsNullOrEmpty(SubUser.Text))
                {
                    string url = InnerTubeService.BuildUserUrl(InnerTubeService.SubscriptionsByUserUrl, SubUser.Text);
                    AddFeed(FavUser.Text + "'s Favorites", url);                    
                }
            }
        }

        private void ValidateFavoritesByUser()
        {
            if (chkFavByUser.IsChecked.Value)
            {
                if (!String.IsNullOrEmpty(FavUser.Text))
                {
                    string url = InnerTubeService.BuildUserUrl(InnerTubeService.FavoritesByUserUrl, FavUser.Text);
                    AddFeed(FavUser.Text + "'s Favorites", url);
                }

            }
        }

        private void ValidateTopFavorited()
        {
            if (chkTopFavorites.IsChecked.Value)
            {
                if (FavTime.SelectedItem != null)
                {
                    InnerTubeTime time = (InnerTubeTime)Enum.Parse(typeof(InnerTubeTime), FavTime.SelectedItem.ToString());
                    string url = InnerTubeService.BuildTimeUrl(InnerTubeService.TopFavoritesUrl, time);
                    AddFeed("Top Favorited " + time.ToString(), url);
                }
                else
                {
                    AddFeed("Top Favorited ", InnerTubeService.TopFavoritesUrl);
                }
            }
        }

        private void ValidateTopRated()
        {
            if (chkTopRated.IsChecked.Value)
            {
                if (RateTime.SelectedItem != null)
                {
                    InnerTubeTime time = (InnerTubeTime)Enum.Parse(typeof(InnerTubeTime), RateTime.SelectedItem.ToString());

                    string url = InnerTubeService.BuildTimeUrl(InnerTubeService.TopRatedUrl, time);

                    AddFeed("Top Rated " + time.ToString(), url);
                }
                else
                {
                    AddFeed("Top Rated ", InnerTubeService.TopRatedUrl);
                }
            }

        }

        private void ValidateMostViewed()
        {
            if (chkMostViewed.IsChecked.Value)
            {
                if (ViewTime.SelectedItem != null)
                {
                    //Add MostViewed by time
                    InnerTubeTime time = (InnerTubeTime)Enum.Parse(typeof(InnerTubeTime), ViewTime.SelectedItem.ToString());

                    string url = InnerTubeService.BuildTimeUrl(InnerTubeService.MostViewedUrl, time);

                    AddFeed("Most Viewed " + time.ToString(), url);
                }
                else
                {
                    AddFeed("Most Viewed ", InnerTubeService.MostViewedUrl);
                }
            }
        }

        private void AddFeed(string label,string url)
        {
            if (IsNew(url))
            {
                InnerTubeFeed feed = new InnerTubeFeed()
                {
                    FeedName = label,
                    FeedUrl = url
                };

                //add a new list of videos
                feed.FeedVideos = new ObservableCollection<InnerTubeVideo>();

                App.InnerTubeFeeds.Add(feed);                
            } ;

            App.SaveFeeds();
            this.Close();            
        }

        private static bool IsNew(string url)
        {
            var x = from s in App.InnerTubeFeeds
                    where s.FeedUrl == url
                    select s;

            if (x.Count() > 0)
            {
                return false;
            }
            else
            {
                return true; 
            }
        }

    }
}
