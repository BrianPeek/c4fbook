Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Shapes
Imports SharedUtilities
Imports System.Collections.ObjectModel



Namespace InnerTube
    ''' <summary>
    ''' Interaction logic for AddNewItem.xaml
    ''' </summary>
    Partial Public Class AddNewFeed
        Inherits Window
        Public Sub New()
            InitializeComponent()
        End Sub

        Public Shared Function GetTimeValues() As String()
            Return Utilities.GetEnumNames(GetType(InnerTubeTime))
        End Function

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim test() As String = GetTimeValues()
            DataContext = test
        End Sub

        Private Sub btnAddFeeds_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ValidateMostViewed()
            ValidateTopRated()
            ValidateTopFavorited()
            ValidateFavoritesByUser()
            ValidateSubscriptionsByUser()
            ValidateCustomSearch()
            App.UpdateFeeds = True

        End Sub

        Private Sub ValidateCustomSearch()
            If chkSearch.IsChecked.Value Then
                If (Not String.IsNullOrEmpty(Search.Text)) Then
                    Dim url As String = InnerTubeService.BuildSearchUrl(Search.Text)
                    AddFeed("Search for: '" & Search.Text & "'", url)
                End If
            End If
        End Sub

        Private Sub ValidateSubscriptionsByUser()
            If chkSubUser.IsChecked.Value Then
                If (Not String.IsNullOrEmpty(SubUser.Text)) Then
                    Dim url As String = InnerTubeService.BuildUserUrl(InnerTubeService.SubscriptionsByUserUrl, SubUser.Text)
                    AddFeed(FavUser.Text & "'s Favorites", url)
                End If
            End If
        End Sub

        Private Sub ValidateFavoritesByUser()
            If chkFavByUser.IsChecked.Value Then
                If (Not String.IsNullOrEmpty(FavUser.Text)) Then
                    Dim url As String = InnerTubeService.BuildUserUrl(InnerTubeService.FavoritesByUserUrl, FavUser.Text)
                    AddFeed(FavUser.Text & "'s Favorites", url)
                End If

            End If
        End Sub

        Private Sub ValidateTopFavorited()
            If chkTopFavorites.IsChecked.Value Then
                If FavTime.SelectedItem IsNot Nothing Then
                    Dim time As InnerTubeTime = CType(System.Enum.Parse(GetType(InnerTubeTime), FavTime.SelectedItem.ToString()), InnerTubeTime)
                    Dim url As String = InnerTubeService.BuildTimeUrl(InnerTubeService.TopFavoritesUrl, time)
                    AddFeed("Top Favorited " & time.ToString(), url)
                Else
                    AddFeed("Top Favorited ", InnerTubeService.TopFavoritesUrl)
                End If
            End If
        End Sub

        Private Sub ValidateTopRated()
            If chkTopRated.IsChecked.Value Then
                If RateTime.SelectedItem IsNot Nothing Then
                    Dim time As InnerTubeTime = CType(System.Enum.Parse(GetType(InnerTubeTime), RateTime.SelectedItem.ToString()), InnerTubeTime)

                    Dim url As String = InnerTubeService.BuildTimeUrl(InnerTubeService.TopRatedUrl, time)

                    AddFeed("Top Rated " & time.ToString(), url)
                Else
                    AddFeed("Top Rated ", InnerTubeService.TopRatedUrl)
                End If
            End If

        End Sub

        Private Sub ValidateMostViewed()
            If chkMostViewed.IsChecked.Value Then
                If ViewTime.SelectedItem IsNot Nothing Then
                    'Add MostViewed by time
                    Dim time As InnerTubeTime = CType(System.Enum.Parse(GetType(InnerTubeTime), ViewTime.SelectedItem.ToString()), InnerTubeTime)

                    Dim url As String = InnerTubeService.BuildTimeUrl(InnerTubeService.MostViewedUrl, time)

                    AddFeed("Most Viewed " & time.ToString(), url)
                Else
                    AddFeed("Most Viewed ", InnerTubeService.MostViewedUrl)
                End If
            End If
        End Sub

        Private Sub AddFeed(ByVal label As String, ByVal url As String)
            If IsNew(url) Then
                Dim feed As New InnerTubeFeed() With {.FeedName = label, .FeedUrl = url}

                'add a new list of videos
                feed.FeedVideos = New ObservableCollection(Of InnerTubeVideo)()

                App.InnerTubeFeeds.Add(feed)
            End If

            App.SaveFeeds()
            Me.Close()
        End Sub

        Private Shared Function IsNew(ByVal url As String) As Boolean
            Dim x = From s In App.InnerTubeFeeds _
                    Where s.FeedUrl = url _
                    Select s

            If x.Count() > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

    End Class
End Namespace
