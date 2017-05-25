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
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Collections.ObjectModel
Imports SharedUtilities
Imports System.Diagnostics
Imports System.Threading
Imports System.Windows.Controls.Primitives



Namespace InnerTube
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Shared AddFeedCommand As New RoutedCommand("AddFeedCommand", GetType(MainWindow))
        Public Shared UpdateFeedCommand As New RoutedCommand("UpdateFeedCommand", GetType(MainWindow))
        Public Shared DownloadFeedCommand As New RoutedCommand("DownloadFeedCommand", GetType(MainWindow))

        Private Sub AddFeed(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim add As AddNewFeed = New InnerTube.AddNewFeed()
            add.ShowDialog()

        End Sub

        Private Sub UpdateFeed(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not InnerTubeFeedWorker.IsRunningAsync()) Then
                Dim iWork As InnerTubeFeedWorker = InnerTubeFeedWorker.GetInstance(App.InnerTubeFeeds, App.Settings)
                AddHandler iWork.RunWorkerCompleted, AddressOf iWork_RunWorkerCompleted
                iWork.RunWorkerAsync(WorkType.UpdateFeeds)
            End If
        End Sub

        Private Sub iWork_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
            App.InnerTubeFeeds = CType(e.Result, ObservableCollection(Of InnerTubeFeed))
            Dim serial = New Serializer(Of ObservableCollection(Of InnerTubeFeed))()
            serial.Serialize(App.InnerTubeFeeds, App.Settings.InnerTubeFeedFile)
            App.UpdateFeeds = False
            Me.DataContext = App.InnerTubeFeeds
        End Sub


        Private Sub DownloadFeed(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim prog As New Progress()
            prog.ShowDialog()
        End Sub

        Private Sub DeleteVideo(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim currentItem As MenuItem = CType(sender, MenuItem)
            Dim deleteVideo As InnerTubeVideo = CType(currentItem.CommandParameter, InnerTubeVideo)
            Deleted.Delete(App.InnerTubeFeeds, deleteVideo)
        End Sub


        Private Sub DeleteFeed(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim currentItem As MenuItem = CType(sender, MenuItem)
            Dim deleteFeed As InnerTubeFeed = CType(currentItem.CommandParameter, InnerTubeFeed)
            Deleted.Delete(App.InnerTubeFeeds, deleteFeed)
        End Sub


        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If App.Settings.FirstRun Then
                OpenFirstRunWindow()
            End If

            Me.DataContext = App.InnerTubeFeeds
        End Sub

        Private Shared Sub OpenFirstRunWindow()
            'Show FirstRun Window
            Dim fr As New FirstRunWindow()
            fr.ShowDialog()

        End Sub


        Private Sub MenuExit_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.Close()
        End Sub



    End Class
End Namespace
