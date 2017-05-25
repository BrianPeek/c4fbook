Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Windows
Imports SharedUtilities
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.IO


Namespace InnerTube
    Public Class App


#Region "Fields"
        Private Shared privateInnerTubeFeeds As ObservableCollection(Of InnerTubeFeed)
        Public Shared Property InnerTubeFeeds() As ObservableCollection(Of InnerTubeFeed)
            Get
                Return privateInnerTubeFeeds
            End Get
            Set(ByVal value As ObservableCollection(Of InnerTubeFeed))
                privateInnerTubeFeeds = value
            End Set
        End Property

        Private Shared privateSettings As Setting
        Public Shared Property Settings() As Setting
            Get
                Return privateSettings
            End Get
            Set(ByVal value As Setting)
                privateSettings = value
            End Set
        End Property
        Public Shared UpdateFeeds As Boolean

#End Region

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            LoadSettings()
            LoadFeeds()
            MyBase.OnStartup(e)
        End Sub

        Private Shared Sub LoadSettings()
            Settings = SettingService.Load()
        End Sub

        Private Shared Sub LoadFeeds()
            Try
                Dim serial = New Serializer(Of ObservableCollection(Of InnerTubeFeed))()
                Dim feeds As ObservableCollection(Of InnerTubeFeed) = serial.Deserialize(Settings.InnerTubeFeedFile)
                If feeds.Count > 0 Then
                    App.InnerTubeFeeds = feeds
                Else
                    App.InnerTubeFeeds = BuildInitialCriteria()
                End If
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                App.InnerTubeFeeds = BuildInitialCriteria()
            End Try

        End Sub

        Public Shared Function BuildInitialCriteria() As ObservableCollection(Of InnerTubeFeed)
            Dim sc As ObservableCollection(Of InnerTubeFeed) = New ObservableCollection(Of InnerTubeFeed)()
            sc.Add(New InnerTubeFeed With _
                             { _
                                 .FeedName = "Most Viewed Videos", _
                                 .FeedUrl = "http://gdata.youtube.com/feeds/api/standardfeeds/most_viewed" _
                             } _
               )

            Return sc
        End Function

        'Protected Overrides Sub OnExit(ByVal e As ExitEventArgs)
        '    SaveFeeds()
        '    MyBase.OnExit(e)
        'End Sub

        Public Shared Sub SaveFeeds()
            SettingService.Save(Settings)
            Dim serial = New Serializer(Of ObservableCollection(Of InnerTubeFeed))()
            serial.Serialize(App.InnerTubeFeeds, Settings.InnerTubeFeedFile)
        End Sub


    End Class

End Namespace
