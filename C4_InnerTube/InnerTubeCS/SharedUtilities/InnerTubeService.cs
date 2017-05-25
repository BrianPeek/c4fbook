using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Xml.Linq;
using System.Web;
using System.Collections.Specialized;

namespace SharedUtilities
{
    public enum InnerTubeTime
    {
        today,
        this_week,
        this_month,
        all_time
    }

    public class InnerTubeService
    {
        #region YouTube URL Properties
        //Top Rated Videos
        private static string _TopRatedUrl = "http://gdata.youtube.com/feeds/api/standardfeeds/top_rated";
        public static string TopRatedUrl
        { get { return _TopRatedUrl; } }

        //Top Favorited Videos
        private static string _TopFavoritesUrl = "http://gdata.youtube.com/feeds/api/standardfeeds/top_favorites";
        public static string TopFavoritesUrl
        { get { return _TopFavoritesUrl; } }

        //Most Viewed
        private static string _MostViewedUrl = "http://gdata.youtube.com/feeds/api/standardfeeds/most_viewed";
        public static string MostViewedUrl
        { get { return _MostViewedUrl; } }

        //Favorites by User
        private static string _FavoritesByUserUrl = "http://gdata.youtube.com/feeds/api/users/username/favorites";
        public static string FavoritesByUserUrl
        { get { return _FavoritesByUserUrl; } }

        //Subscriptions By User
        private static string _SubscriptionsByUserUrl = "http://gdata.youtube.com/feeds/api/users/username/subscriptions";
        public static string SubscriptionsByUserUrl
        { get { return _SubscriptionsByUserUrl; } }

        //Search
        private static string _SearchUrl = "http://gdata.youtube.com/feeds/api/videos";
        public static string SearchUrl
        { get { return _SearchUrl; } }

        //baseUrl Embed Url, just append videoID
        private static string _baseEmbedUrl = "http://www.youtube.com/v/";
        public static string BaseEmbedUrl
        { get { return _baseEmbedUrl; } }

        //baseUrl Watch Url, just append videoID
        private static string _BasewatchUrl = "http://www.youtube.com/watch?v=";
        public static string BaseWatchUrl
        { get { return _BasewatchUrl; } }

        //baseUrl Download Url, just append videoID
        private static string _BaseDownloadUrl = "http://www.youtube.com/get_video?video_id=";
        public static string BaseDownloadUrl
        { get { return _BaseDownloadUrl; } }

        //baseUrl Thumbnail Url, just append videoID
        private static string _BaseThumbnailUrl = "http://img.youtube.com/vi/";
        public static string BaseThumbnailUrl
        { get { return _BaseThumbnailUrl; } }


        //Single Video Url, just append videoID
        private static string _SingleVideoUrl = "http://gdata.youtube.com/feeds/api/videos/";
        public static string SingleVideoUrl
        { get { return _SingleVideoUrl; } }
        #endregion

        #region Public Methods
        public static ObservableCollection<InnerTubeVideo> GetTopRated()
        {
            return ConvertYouTubeXmlToObjects(_TopRatedUrl);
        }

        public static ObservableCollection<InnerTubeVideo> GetTopRated(InnerTubeTime time)
        {
            return ConvertYouTubeXmlToObjects(BuildTimeUrl(_TopRatedUrl, time));
        }

        public static ObservableCollection<InnerTubeVideo> GetMostViewed()
        {
            return ConvertYouTubeXmlToObjects(MostViewedUrl);
        }

        public static ObservableCollection<InnerTubeVideo> GetMostViewed(InnerTubeTime time)
        {
            return ConvertYouTubeXmlToObjects(BuildTimeUrl(MostViewedUrl, time));
        }

        public static ObservableCollection<InnerTubeVideo> GetTopFavorites()
        {
            return ConvertYouTubeXmlToObjects(TopFavoritesUrl);
        }

        public static ObservableCollection<InnerTubeVideo> GetTopFavorites(InnerTubeTime time)
        {
            return ConvertYouTubeXmlToObjects(BuildTimeUrl(TopFavoritesUrl, time));
        }

        public static ObservableCollection<InnerTubeVideo> GetFavoritesByUser(string userId)
        {
            return ConvertYouTubeXmlToObjects(BuildUserUrl(_FavoritesByUserUrl, userId));
        }

        public static ObservableCollection<InnerTubeVideo> GetSubscriptionsByUser(string userId)
        {
            return ConvertYouTubeXmlToObjects(BuildUserUrl(_SubscriptionsByUserUrl, userId));
        }

        public static ObservableCollection<InnerTubeVideo> Search(string query)
        {

            //TODO: Add url escaping for query
            return ConvertYouTubeXmlToObjects(BuildSearchUrl(query));
        }

        public static ObservableCollection<InnerTubeVideo> GetSingleVideoById(string Id)
        {
            return ConvertYouTubeXmlToObjects(_SingleVideoUrl + Id);
        }

        public static ObservableCollection<InnerTubeVideo> GetSingleVideo(string watchUrl)
        {
            string queryParam = "v";

            NameValueCollection urlParts = HttpUtility.ParseQueryString(watchUrl);

            if (string.IsNullOrEmpty(urlParts[queryParam]) )
            {
                throw new Exception(@"Unable to parse URL, please use format: http://www.youtube.com/watch?v=oBmbZmrsz6U");
            }

            return GetSingleVideoById(urlParts[queryParam]); 
        }

        public static ObservableCollection<InnerTubeVideo> ConvertYouTubeXmlToObjects(string youTubeUrl)
        {
            return ConvertYouTubeXmlToObjects(new Uri(youTubeUrl));
        }

        public static ObservableCollection<InnerTubeVideo> ConvertYouTubeXmlToObjects(Uri youTubeUrl)
        {

            XNamespace nsBase = @"http://www.w3.org/2005/Atom";
            XNamespace nsGData = @"http://schemas.google.com/g/2005";
            XNamespace nsYouTube = @"http://gdata.youtube.com/schemas/2007";

            //call service
            WebClient wc = new WebClient();

            //Get Data
            XmlTextReader xr = new XmlTextReader(wc.OpenRead(youTubeUrl));
            XDocument rawData = XDocument.Load(xr);

            var query = from entry in rawData.Descendants(nsBase + "entry")
                        select new InnerTubeVideo
                        {
                            Author = entry.Element(nsBase + "author").Element(nsBase + "name").Value,
                            Categories = ParseCategories(entry.Elements(nsBase + "category")),
                            Id = ParseID(entry.Element(nsBase + "id").Value),
                            Published = DateTime.Parse(entry.Element(nsBase + "published").Value),
                            Updated = DateTime.Parse(entry.Element(nsBase + "updated").Value),
                            Title = entry.Element(nsBase + "title").Value,
                            Description = entry.Element(nsBase + "content").Value,
                            ThumbnailLink = _BaseThumbnailUrl + ParseID(entry.Element(nsBase + "id").Value) + @"/0.jpg",
                            Link = _BasewatchUrl + ParseID(entry.Element(nsBase + "id").Value),
                            EmbedLink = _baseEmbedUrl + ParseID(entry.Element(nsBase + "id").Value),
                            DownloadLink = _BaseDownloadUrl + ParseID(entry.Element(nsBase + "id").Value),
                            Views = int.Parse(entry.Element(nsYouTube + "statistics").Attribute("viewCount").Value),
                            AvgRating = float.Parse(entry.Element(nsGData + "rating").Attribute("average").Value),
                            NumRaters = int.Parse(entry.Element(nsGData + "rating").Attribute("numRaters").Value),
                        };

            return query.ToObservableCollection<InnerTubeVideo>();

        }

        public static ObservableCollection<InnerTubeVideo> ConvertYouTubeXmlToObjects(Uri youTubeUrl, Setting setting)
        {

            XNamespace nsBase = @"http://www.w3.org/2005/Atom";
            XNamespace nsGData = @"http://schemas.google.com/g/2005";
            XNamespace nsYouTube = @"http://gdata.youtube.com/schemas/2007";

            //Use to call service
            WebClient wc = new WebClient();

            //Get Data
            XmlTextReader xr = new XmlTextReader(wc.OpenRead(youTubeUrl));
            XDocument rawData = XDocument.Load(xr);

            var query = from entry in rawData.Descendants(nsBase + "entry")
                        select new InnerTubeVideo
                        {
                            Author = entry.Element(nsBase + "author").Element(nsBase + "name").Value,
                            Categories = ParseCategories(entry.Elements(nsBase + "category")),
                            Id = ParseID(entry.Element(nsBase + "id").Value),
                            Published = DateTime.Parse(entry.Element(nsBase + "published").Value),
                            Updated = DateTime.Parse(entry.Element(nsBase + "updated").Value),
                            Title = entry.Element(nsBase + "title").Value,
                            Description = entry.Element(nsBase + "content").Value,
                            ThumbnailLink = _BaseThumbnailUrl + ParseID(entry.Element(nsBase + "id").Value) + @"/0.jpg",
                            Link = _BasewatchUrl + ParseID(entry.Element(nsBase + "id").Value),
                            EmbedLink = _baseEmbedUrl + ParseID(entry.Element(nsBase + "id").Value),
                            DownloadLink = _BaseDownloadUrl + ParseID(entry.Element(nsBase + "id").Value),
                            Views = int.Parse(entry.Element(nsYouTube + "statistics").Attribute("viewCount").Value),
                            AvgRating = float.Parse(entry.Element(nsGData + "rating").Attribute("average").Value),
                            NumRaters = int.Parse(entry.Element(nsGData + "rating").Attribute("numRaters").Value),
                            //set download locations
                            DownloadedImage = FileHelper.BuildFileName(setting.SubPath,
                                                ParseID(entry.Element(nsBase + "id").Value), FileType.Image),
                            DownloadedFlv = FileHelper.BuildFileName(setting.SubPath,
                                                entry.Element(nsBase + "title").Value, FileType.Flv),
                            DownloadedMp4 = FileHelper.BuildFileName(setting.VideoPath,
                                                entry.Element(nsBase + "title").Value, FileType.Mp4),
                            DownloadedWmv = FileHelper.BuildFileName(setting.VideoPath,
                                                entry.Element(nsBase + "title").Value, FileType.Wmv)
                        };

            return query.ToObservableCollection<InnerTubeVideo>();

        }


        #endregion

        #region Private Methods

        private static int ParseComments(string p)  //not called
        {
            if (String.IsNullOrEmpty(p))
            {
                return -1;
            }
            else
            {
                return int.Parse(p);
            }
        }

        public static string BuildSearchUrl(string query)
        {
            return _SearchUrl + "vq=" + query;
        }

        public static string BuildTimeUrl(string baseUrl, InnerTubeTime time)
        {
            UriBuilder build = new UriBuilder(baseUrl);
            build.Query = "time=" + time.ToString();
            return build.Uri.ToString();
        }

        public static string BuildUserUrl(string url, string userId)
        {
            string val = url.Replace("username", userId);
            return val;
        }

        private static TimeSpan ParseTime(string seconds) //not called
        {
            int timevalue = int.Parse(seconds);
            Debug.WriteLine("timevalue=" + timevalue);

            try
            {
                TimeSpan t = new TimeSpan(0, 0, timevalue);
                return t;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("msg" + ex.Message);
                Debug.WriteLine(ex.Source);
                throw;
            }

        }

        private static Collection<string> ParseCategories(IEnumerable<XElement> Categories)
        {
            var vals = from c in Categories.Attributes("term")
                       select c.Value;
            return vals.ToCollection<string>();
        }

        private static string ParseID(string url)
        {
            //Format: "http://gdata.youtube.com/feeds/api/videos/cI1AwZN4ZYg";
            var x = url.Split('/');
            return x.Last();
        }

        #endregion

    }

}
