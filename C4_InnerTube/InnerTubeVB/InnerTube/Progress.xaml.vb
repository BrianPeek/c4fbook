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
Imports System.Collections.ObjectModel
Imports SharedUtilities


Namespace InnerTube
    ''' <summary>
    ''' Interaction logic for Progress.xaml
    ''' </summary>
    Partial Public Class Progress
        Inherits Window
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ResetControls()

            If (Not InnerTubeFeedWorker.IsRunningAsync()) Then
                Dim iWork As InnerTubeFeedWorker = InnerTubeFeedWorker.GetInstance(App.InnerTubeFeeds, App.Settings)
                AddHandler iWork.RunWorkerCompleted, AddressOf iWork_RunWorkerCompleted
                AddHandler iWork.ProgressChanged, AddressOf iWork_ProgressChanged

                iWork.RunWorkerAsync(WorkType.All)
            End If


        End Sub

        Private Sub ResetControls()
            btnDownload.IsEnabled = False
            progressBar1.Visibility = Visibility.Visible
            progressBar1.Maximum = CalculateMax()

            StatusList.Items.Clear()
        End Sub

        Private Function CalculateMax() As Double
            Dim max As Double = 0

            For Each feed In App.InnerTubeFeeds
                '+1 for each feed
                max += 1

                'add all feed videos 2x for download + convert
                max += feed.FeedVideos.Count * 2

            Next feed
            max += 1 'add 1 more for updating

            Return max

        End Function

        Private Sub iWork_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
            Dim s As String = CStr(e.UserState)
            StatusList.Items.Add(s)
            progressBar1.Value += 1
        End Sub

        Private Sub iWork_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
            btnDownload.IsEnabled = True
            progressBar1.Visibility = Visibility.Hidden

            App.InnerTubeFeeds = CType(e.Result, ObservableCollection(Of InnerTubeFeed))

            Dim serial = New Serializer(Of ObservableCollection(Of InnerTubeFeed))()
            serial.Serialize(App.InnerTubeFeeds, App.Settings.InnerTubeFeedFile)

            Dim path As String = System.IO.Path.Combine(App.Settings.VideoPath, App.Settings.AppName)
            Dim sb As StringBuilder = New StringBuilder()
            For Each Item In StatusList.Items
                sb.Append(Item)
            Next

            FileHelper.WriteLogFile(path, sb.ToString())

            App.UpdateFeeds = False
        End Sub
    End Class
End Namespace


