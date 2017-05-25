using System;
using System.Windows;
using System.Windows.Media;

namespace PeerCast
{
    public partial class FullScreen : Window
    {
        public FullScreen()
        {
            InitializeComponent();
        }

        public void SetMediaPlayer(Uri videoPath)
        {
            MediaPlayer.Source = videoPath;
            MediaPlayer.Play();         
        }
    }
}
