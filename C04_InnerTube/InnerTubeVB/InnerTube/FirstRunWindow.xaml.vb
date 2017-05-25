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
Imports System.IO


Namespace InnerTube
    ''' <summary>
    ''' Interaction logic for FirstRunWindow.xaml
    ''' </summary>
    Partial Public Class FirstRunWindow
        Inherits Window
#Region "Fields"
        Private ZuneInstalled As Boolean
        Private iTunesInstalled As Boolean
        Private videoDir As String
        Private subDir As String
#End Region

        Public Sub New()
            InitializeComponent()
            AddHandler Loaded, AddressOf FirstRunWindow_Loaded
        End Sub


        Private Sub FirstRunWindow_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SetupUI()
        End Sub

        Private Sub SetupUI()
            'Zune Checkbox
            ZuneInstalled = ProcessHelper.IsProcessRunning(ProcessType.Zune)
            chkZune.IsEnabled = ZuneInstalled
            If ZuneInstalled Then
                chkZune.Foreground = Brushes.Black
            End If

            'iTunes Checkbox
            iTunesInstalled = ProcessHelper.IsProcessRunning(ProcessType.iTunes)
            chkiTunes.IsEnabled = iTunesInstalled
            If iTunesInstalled Then
                chkiTunes.Foreground = Brushes.Black
            End If

            'File Directories
            subDir = App.Settings.AppName
            videoDir = FileHelper.BuildPath(subDir)

            'Textboxes
            txtLocation.Text = videoDir
            txtSubDir.Text = subDir

            btnOK.IsEnabled = True
        End Sub



        Private Sub SetAppSettings()
            App.Settings.FirstRun = False

            App.Settings.iTunesInstalled = iTunesInstalled
            App.Settings.ZuneInstalled = ZuneInstalled

            Dim xmlFile As String = FileHelper.BuildXmlName(App.Settings.AppName)

            App.Settings.InnerTubeFeedFile = System.IO.Path.Combine(subDir, xmlFile)
            App.Settings.VideoPath = videoDir
            SettingService.Save(App.Settings)
        End Sub

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SetAppSettings()
            Me.Close()
        End Sub
    End Class
End Namespace
