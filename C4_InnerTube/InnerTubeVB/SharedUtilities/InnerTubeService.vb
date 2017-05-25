Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Linq
Imports System.Net
Imports System.Diagnostics
Imports System.Xml.Linq
Imports System.Collections.Specialized
Imports System.Web

Namespace SharedUtilities
    Public Enum InnerTubeTime
        today
        this_week
        this_month
        all_time
    End Enum

    Public Class InnerTubeService
#Region "YouTube URL Properties"
        'Top Rated Videos
        Private Shared _TopRatedUrl As String = "http://gdata.youtube.com/feeds/api/standardfeeds/top_rated"
        Public Shared ReadOnly Property TopRatedUrl() As String
            Get
                Return _TopRatedUrl
            End Get
        End Property

        'Top Favorited Videos
        Private Shared _TopFavoritesUrl As String = "http://gdata.youtube.com/feeds/api/standardfeeds/top_favorites"
        Public Shared ReadOnly Property TopFavoritesUrl() As String
            Get
                Return _TopFavoritesUrl
            End Get
        End Property

        'Most Viewed
        Private Shared _MostViewedUrl As String = "http://gdata.youtube.com/feeds/api/standardfeeds/most_viewed"
        Public Shared ReadOnly Property MostViewedUrl() As String
            Get
                Return _MostViewedUrl
            End Get
        End Property

        'Favorites by User
        Private Shared _FavoritesByUserUrl As String = "http://gdata.youtube.com/feeds/api/users/username/favorites"
        Public Shared ReadOnly Property FavoritesByUserUrl() As String
            Get
                Return _FavoritesByUserUrl
            End Get
        End Property

        'Subscriptions By User
        Private Shared _SubscriptionsByUserUrl As String = "http://gdata.youtube.com/feeds/api/users/username/subscriptions"
        Public Shared ReadOnly Property SubscriptionsByUserUrl() As String
            Get
                Return _SubscriptionsByUserUrl
            End Get
        End Property

        'Search
        Private Shared _SearchUrl As String = "http://gdata.youtube.com/feeds/api/videos"
        Public Shared ReadOnly Property SearchUrl() As String
            Get
                Return _SearchUrl
            End Get
        End Property

        'baseUrl Embed Url, just append videoID
        Private Shared _baseEmbedUrl As String = "http://www.youtube.com/v/"
        Public Shared ReadOnly Property BaseEmbedUrl() As String
            Get
                Return _baseEmbedUrl
            End Get
        End Property

        'baseUrl Watch Url, just append videoID
        Private Shared _BasewatchUrl As String = "http://www.youtube.com/watch?v="
        Public Shared ReadOnly Property BaseWatchUrl() As String
            Get
                Return _BasewatchUrl
            End Get
        End Property

        'baseUrl Download Url, just append videoID
        Private Shared _BaseDownloadUrl As String = "http://www.youtube.com/get_video?video_id="
        Public Shared ReadOnly Property BaseDownloadUrl() As String
            Get
                Return _BaseDownloadUrl
            End Get
        End Property

        'baseUrl Thumbnail Url, just append videoID
        Private Shared _BaseThumbnailUrl As String = "http://img.youtube.com/vi/"
        Public Shared ReadOnly Property BaseThumbnailUrl() As String
            Get
                Return _BaseThumbnailUrl
            End Get
        End Property


        'Single Video Url, just append videoID
        Private Shared _SingleVideoUrl As String = "http://gdata.youtube.com/feeds/api/videos/"
        Public Shared ReadOnly Property SingleVideoUrl() As String
            Get
                Return _SingleVideoUrl
            End Get
        End Property
#End Region

#Region "Public Methods"
        Public Shared Function GetTopRated() As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(_TopRatedUrl)
        End Function

        Public Shared Function GetTopRated(ByVal time As InnerTubeTime) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(BuildTimeUrl(_TopRatedUrl, time))
        End Function

        Public Shared Function GetMostViewed() As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(MostViewedUrl)
        End Function

        Public Shared Function GetMostViewed(ByVal time As InnerTubeTime) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(BuildTimeUrl(MostViewedUrl, time))
        End Function

        Public Shared Function GetTopFavorites() As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(TopFavoritesUrl)
        End Function

        Public Shared Function GetTopFavorites(ByVal time As InnerTubeTime) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(BuildTimeUrl(TopFavoritesUrl, time))
        End Function

        Public Shared Function GetFavoritesByUser(ByVal userId As String) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(BuildUserUrl(_FavoritesByUserUrl, userId))
        End Function

        Public Shared Function GetSubscriptionsByUser(ByVal userId As String) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(BuildUserUrl(_SubscriptionsByUserUrl, userId))
        End Function

        Public Shared Function Search(ByVal query As String) As ObservableCollection(Of InnerTubeVideo)

            'TODO: Add url escaping for query
            Return ConvertYouTubeXmlToObjects(BuildSearchUrl(query))
        End Function

        Public Shared Function GetSingleVideoById(ByVal Id As String) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(_SingleVideoUrl & Id)
        End Function

        Public Shared Function GetSingleVideo(ByVal watchUrl As String) As ObservableCollection(Of InnerTubeVideo)
            Dim queryParam As String = "v"

            Dim urlParts As NameValueCollection = HttpUtility.ParseQueryString(watchUrl)

            If String.IsNullOrEmpty(urlParts(queryParam)) Then
                Throw New Exception("Unable to parse URL, please use format: http://www.youtube.com/watch?v=oBmbZmrsz6U")
            End If

            Return GetSingleVideoById(urlParts(queryParam))
        End Function

        Public Shared Function ConvertYouTubeXmlToObjects(ByVal youTubeUrl As String) As ObservableCollection(Of InnerTubeVideo)
            Return ConvertYouTubeXmlToObjects(New Uri(youTubeUrl))
        End Function

        Public Shared Function ConvertYouTubeXmlToObjects(ByVal youTubeUrl As Uri) As ObservableCollection(Of InnerTubeVideo)

            Dim nsBase As XNamespace = "http://www.w3.org/2005/Atom"
            Dim nsGData As XNamespace = "http://schemas.google.com/g/2005"
            Dim nsYouTube As XNamespace = "http://gdata.youtube.com/schemas/2007"

            'call service
            Dim wc As New WebClient()

            'Get Data
            Dim xr As New XmlTextReader(wc.OpenRead(youTubeUrl))
            Dim rawData As XDocument = XDocument.Load(xr)

            Dim query = From entry In rawData.Descendants(nsBase + "entry") _
               Select New InnerTubeVideo With _
               { _
                .Author = entry.Element(nsBase + "author").Element(nsBase + "name").Value, _
                .Categories = ParseCategories(entry.Elements(nsBase + "category")), _
                .Id = ParseID(entry.Element(nsBase + "id").Value), _
                .Published = DateTime.Parse(entry.Element(nsBase + "published").Value), _
                .Updated = DateTime.Parse(entry.Element(nsBase + "updated").Value), _
                .Title = entry.Element(nsBase + "title").Value, _
                .Description = entry.Element(nsBase + "content").Value, _
                .ThumbnailLink = _BaseThumbnailUrl & ParseID(entry.Element(nsBase + "id").Value) & "/0.jpg", _
                .Link = _BasewatchUrl & ParseID(entry.Element(nsBase + "id").Value), _
                .EmbedLink = _baseEmbedUrl & ParseID(entry.Element(nsBase + "id").Value), _
                .DownloadLink = _BaseDownloadUrl & ParseID(entry.Element(nsBase + "id").Value), _
                .Views = Integer.Parse(entry.Element(nsYouTube + "statistics").Attribute("viewCount").Value), _
                .AvgRating = Single.Parse(entry.Element(nsGData + "rating").Attribute("average").Value), _
                .NumRaters = Integer.Parse(entry.Element(nsGData + "rating").Attribute("numRaters").Value) _
               }

            Return query.ToObservableCollection()

        End Function

        Public Shared Function ConvertYouTubeXmlToObjects(ByVal youTubeUrl As Uri, ByVal setting As Setting) As ObservableCollection(Of InnerTubeVideo)

            Dim nsBase As XNamespace = "http://www.w3.org/2005/Atom"
            Dim nsGData As XNamespace = "http://schemas.google.com/g/2005"
            Dim nsYouTube As XNamespace = "http://gdata.youtube.com/schemas/2007"

            'Use to call service
            Dim wc As New WebClient()

            'Get Data
            Dim xr As New XmlTextReader(wc.OpenRead(youTubeUrl))
            Dim rawData As XDocument = XDocument.Load(xr)


            Dim query = From entry In rawData.Descendants(nsBase + "entry") _
               Select New InnerTubeVideo With _
               { _
                .Author = entry.Element(nsBase + "author").Element(nsBase + "name").Value, _
                .Categories = ParseCategories(entry.Elements(nsBase + "category")), _
                .Id = ParseID(entry.Element(nsBase + "id").Value), _
                .Published = DateTime.Parse(entry.Element(nsBase + "published").Value), _
                .Updated = DateTime.Parse(entry.Element(nsBase + "updated").Value), _
                .Title = entry.Element(nsBase + "title").Value, _
                .Description = entry.Element(nsBase + "content").Value, _
                .ThumbnailLink = _BaseThumbnailUrl & ParseID(entry.Element(nsBase + "id").Value) & "/0.jpg", _
                .Link = _BasewatchUrl & ParseID(entry.Element(nsBase + "id").Value), _
                .EmbedLink = _baseEmbedUrl & ParseID(entry.Element(nsBase + "id").Value), _
                .DownloadLink = _BaseDownloadUrl & ParseID(entry.Element(nsBase + "id").Value), _
                .Views = Integer.Parse(entry.Element(nsYouTube + "statistics").Attribute("viewCount").Value), _
                .AvgRating = Single.Parse(entry.Element(nsGData + "rating").Attribute("average").Value), _
                .NumRaters = Integer.Parse(entry.Element(nsGData + "rating").Attribute("numRaters").Value), _
                .DownloadedImage = FileHelper.BuildFileName(setting.SubPath, ParseID(entry.Element(nsBase + "id").Value), FileType.Image), _
                .DownloadedFlv = FileHelper.BuildFileName(setting.SubPath, entry.Element(nsBase + "title").Value, FileType.Flv), _
                .DownloadedMp4 = FileHelper.BuildFileName(setting.VideoPath, entry.Element(nsBase + "title").Value, FileType.Mp4), .DownloadedWmv = FileHelper.BuildFileName(setting.VideoPath, entry.Element(nsBase + "title").Value, FileType.Wmv) _
               }

            Return query.ToObservableCollection()

        End Function


#End Region

#Region "Private Methods"

        Private Shared Function ParseComments(ByVal p As String) As Integer 'not called
            If String.IsNullOrEmpty(p) Then
                Return -1
            Else
                Return Integer.Parse(p)
            End If
        End Function

        Public Shared Function BuildSearchUrl(ByVal query As String) As String
            Return _SearchUrl & "vq=" & query
        End Function

        Public Shared Function BuildTimeUrl(ByVal baseUrl As String, ByVal time As InnerTubeTime) As String
            Dim build As New UriBuilder(baseUrl)
            build.Query = "time=" & time.ToString()
            Return build.Uri.ToString()
        End Function

        Public Shared Function BuildUserUrl(ByVal url As String, ByVal userId As String) As String
            Dim val As String = url.Replace("username", userId)
            Return val
        End Function

        Private Shared Function ParseTime(ByVal seconds As String) As TimeSpan 'not called
            Dim timevalue As Integer = Integer.Parse(seconds)
            Debug.WriteLine("timevalue=" & timevalue)

            Try
                Dim t As New TimeSpan(0, 0, timevalue)
                Return t
            Catch ex As Exception
                Debug.WriteLine("msg" & ex.Message)
                Debug.WriteLine(ex.Source)
                Throw
            End Try

        End Function

        Private Shared Function ParseCategories(ByVal Categories As IEnumerable(Of XElement)) As Collection(Of String)
            Dim vals = From c In Categories.Attributes("term") _
                       Select c.Value
            Return vals.ToCollection()

        End Function

        Private Shared Function ParseID(ByVal url As String) As String
            'Format: "http://gdata.youtube.com/feeds/api/videos/cI1AwZN4ZYg";
            Dim x = url.Split("/"c)
            Return x.Last()
        End Function

#End Region

    End Class

End Namespace
