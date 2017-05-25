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
Imports System.Windows.Controls.Primitives
Imports SharedUtilities
Imports System.IO


Namespace InnerTube
    ''' <summary>
    ''' Interaction logic for MediaPlayer.xaml
    ''' </summary>
    Partial Public Class MediaPlayer
        Inherits UserControl
        Private isVideo As Boolean = False

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Play(ByVal sender As Object, ByVal args As RoutedEventArgs)
            Dim tb As ToggleButton = CType(sender, ToggleButton)


            If isVideo Then

                'Hide image if it's a video
                Me.PreviewImage.Visibility = Visibility.Hidden

                If CBool(tb.IsChecked) Then

                    VideoPlayer.Play()
                Else
                    VideoPlayer.Pause()
                End If
            End If
        End Sub





        Public Property InnerTubeVideoFile() As InnerTubeVideo
            Get
                Return CType(GetValue(InnerTubeVideoProperty), InnerTubeVideo)
            End Get

            Set(ByVal value As InnerTubeVideo)
                'Don't write code here it won't execute
            End Set
        End Property

        Public Shared ReadOnly InnerTubeVideoProperty As DependencyProperty = DependencyProperty.Register("InnerTubeVideoFile", GetType(InnerTubeVideo), GetType(MediaPlayer), New UIPropertyMetadata(AddressOf MediaPlayer.InnerTubeVideoFileChanged))


        Private Shared Sub InnerTubeVideoFileChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            Dim player As MediaPlayer = CType(d, MediaPlayer)
            Dim newVideo As InnerTubeVideo = CType(e.NewValue, InnerTubeVideo)

            'reset UI
            player.PreviewImage.Visibility = Visibility.Visible
            player.PlayButton.IsChecked = False
            player.PreviewImage.Source = Nothing

            If newVideo IsNot Nothing Then

                'Set Image
                Dim imageConvert As New ImageSourceConverter()
                If File.Exists(newVideo.DownloadedImage) Then
                    player.PreviewImage.Source = CType(imageConvert.ConvertFromString(newVideo.DownloadedImage), ImageSource)
                Else
                    Dim defaultImage As String = System.IO.Path.Combine(App.Settings.SubPath, FileHelper.DefaultImage)
                    If File.Exists(defaultImage) Then
                        player.PreviewImage.Source = CType(imageConvert.ConvertFromString(defaultImage), ImageSource)
                    Else
                        player.PreviewImage.Source = Nothing
                    End If
                End If


                'Set and enable Video if we have one
                If (Not String.IsNullOrEmpty(newVideo.DownloadedWmv)) AndAlso (Not File.Exists(newVideo.DownloadedWmv)) Then
                    player.isVideo = False
                    player.SetEnabledPlayButton(False)
                Else
                    'Set Video File
                    player.VideoPlayer.Source = New Uri(newVideo.DownloadedWmv)

                    player.isVideo = True
                    player.SetEnabledPlayButton(True)
                End If

            End If
        End Sub





        Private Sub SetEnabledPlayButton(ByVal value As Boolean)
            Me.PlayButton.IsEnabled = value

        End Sub




    End Class
End Namespace
