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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using SharedUtilities;
using System.IO;

namespace InnerTube
{
    /// <summary>
    /// Interaction logic for MediaPlayer.xaml
    /// </summary>
    public partial class MediaPlayer : UserControl
    {
        bool isVideo = false; 

        public MediaPlayer()
        {
            InitializeComponent();
        }

        void Play(object sender, RoutedEventArgs args)
        {
            ToggleButton tb = (ToggleButton)sender;
            if (isVideo)
            {

                //Hide image if it's a video
                this.PreviewImage.Visibility = Visibility.Hidden;               

                if ((bool)tb.IsChecked)
                {

                    VideoPlayer.Play();
                }
                else
                {
                    VideoPlayer.Pause();
                }
            }
        }


        public InnerTubeVideo InnerTubeVideoFile
        {
            get { return (InnerTubeVideo)GetValue(InnerTubeVideoProperty); }
            set { /*don't write code here it won't execute*/ }
        }

        public static readonly DependencyProperty InnerTubeVideoProperty =
            DependencyProperty.Register("InnerTubeVideoFile",
                                        typeof(InnerTubeVideo),
                                        typeof(MediaPlayer),
                                        new UIPropertyMetadata(MediaPlayer.InnerTubeVideoFileChanged));


        private static void InnerTubeVideoFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaPlayer player = (MediaPlayer)d;
            InnerTubeVideo newVideo = (InnerTubeVideo)e.NewValue;

            //reset UI
            player.PreviewImage.Visibility = Visibility.Visible;
            player.PlayButton.IsChecked = false;
            player.PreviewImage.Source = null; 

            if (newVideo != null)
            {
                //Set Image
                ImageSourceConverter imageConvert = new ImageSourceConverter();               
                if (File.Exists(newVideo.DownloadedImage))
	            {
                    player.PreviewImage.Source = (ImageSource)imageConvert.ConvertFromString(newVideo.DownloadedImage);
	            }
                else
                {      
              
                    string defaultImage = System.IO.Path.Combine(App.Settings.SubPath, FileHelper.DefaultImage);
                    if (File.Exists(defaultImage))
                    {
                        player.PreviewImage.Source = (ImageSource)imageConvert.ConvertFromString(defaultImage);
                    }
                    else
                    {
                        player.PreviewImage.Source = null;
                    }                                        
                }               
               
                //Set and enable Video if we have one
                if (!String.IsNullOrEmpty(newVideo.DownloadedWmv) 
                 && !File.Exists(newVideo.DownloadedWmv))
                {
                    player.isVideo = false;
                    player.SetEnabledPlayButton(false);
                }
                else
                {
                    //Set Video File
                    player.VideoPlayer.Source = new Uri(newVideo.DownloadedWmv);    
                    
                    player.isVideo = true;
                    player.SetEnabledPlayButton(true); 
                }

            }
        }





        private void SetEnabledPlayButton(bool value)
        {
            this.PlayButton.IsEnabled = value;            
        }




    }
}
