using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using C4F.VistaP2P.Common;



namespace PeerCast
{

    public partial class MainWindow : Window
    {
        #region Properties
        //net is the wrapper for all async/P2P work
        private NetworkManager net = new NetworkManager();
        FullScreen fullWindow;  
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            FirstLoad();
        }
        
        private void FirstLoad()
        {
            //Set Text
            if (App.IsServerMode)
            {
                AppModeLabel.Content = "Server Mode";
                FilePath.Content = Properties.Settings.Default.FileDirectory;
                ClientControls.Visibility = Visibility.Hidden;
            }
            else
            {
                AppModeLabel.Content = "Client Mode";
                ServerControls.Visibility = Visibility.Hidden; 
            }
            //Setup Event handlers as all messages from NetworkManager comes through here
            net.P2pWorker.ProgressChanged += new ProgressChangedEventHandler(P2PWorker_ProgressChanged);
            net.P2pWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(P2PBackgroundWorkerCompleted);            
        }


        //Sign In Button starts Connection
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (net.CurrentState == SystemState.LoggedOut)
            {
                if (ValidateFields())
                {
                    net.StartConnection(NetworkName.Text, Password.Text);
                }
            }
            else
            {
                net.EndConnection();
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(NetworkName.Text) && NetworkName.Text.Trim().Contains(' '))
            {
                MessageBox.Show("Please Enter a non-null NetworkName that contains only alpha-numeric characters");
                return false;
            }
            return true;
        }

        #region Network Manager Commands
        
        private void UpdateVideoList(string serializedList)
        {
            List<string> videos = Serializer.DeserializeFileList(serializedList);
            //clear list
            MediaList.Items.Clear();

            //add each item
            foreach (string s in videos)
            {
                MediaList.Items.Add(s);
            }
        }

        public void PlayVideo(string url)
        {
            if (fullWindow == null)
            {
                MediaPlayer.Source = new Uri(url);
                MediaPlayer.Play();
            }
            else
            {
                fullWindow.SetMediaPlayer(new Uri(url));
                fullWindow.Show();                 
            }
        }

        private void ToggleUi()
        {
            if (net.CurrentState == SystemState.LoggedIn || net.CurrentState == SystemState.LoggingIn)
            {
                SignInButton.Content = "Sign Out";
                StatusValue.Content = "Signing In...";
                GetList.IsEnabled = true;
                Play.IsEnabled = true;
                FullScreen.IsEnabled = true; 
                MediaList.IsEnabled = true;
            }
            else
            {
                SignInButton.Content = "Sign In";
                StatusValue.Content = "Disconnected";
                GetList.IsEnabled = false;
                Play.IsEnabled = false;
                FullScreen.IsEnabled = false; 
                MediaList.IsEnabled = false;                
            }
        }

        #endregion

        #region P2P Background Worker Events
        void P2PWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //update progress bar
            progressBar1.Value = e.ProgressPercentage;

            //Read messages from the Network Manager
            if (e.UserState != null)
            {
                ThreadMessage message = (ThreadMessage)(e.UserState);
                switch (message.MessageType)
                {
                    case UiMessage.UpdateStatus:
                        StatusValue.Content = message.Message;
                        break;
                    case UiMessage.Log:
                        Messages.Items.Add(message.Message);
                        break;
                    case UiMessage.StreamVideo:
                        PlayVideo(message.Message);
                        break;
                    case UiMessage.ReceivedVideoList:
                        UpdateVideoList(message.Message);
                        break;
                    case UiMessage.ToggleUi:
                        ToggleUi();
                        break;
                }
            }
        }

        protected void P2PBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //error occured
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("An error occurred in the background thread: {0}", e.Error.Message));
            }
            else
            {
                net.EndConnection();
            }
        } 
        #endregion

        #region Client Only Buttons

        private void GetList_Click(object sender, RoutedEventArgs e)
        {
            //make a request to get the server's video list
            net.SendChatMessage(ChatMessage.Create(ChatCommand.GetList, string.Empty)); 
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            //make a request to play a video
            net.SendChatMessage(ChatMessage.Create(ChatCommand.StreamVideo, (string)MediaList.SelectedItem));
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            //open a new window full screen
            if (fullWindow == null)
            {
                fullWindow = new FullScreen();
                fullWindow.Closing += new CancelEventHandler(fullWindow_Closing);
            }
            net.SendChatMessage(ChatMessage.Create(ChatCommand.StreamVideo, (string)MediaList.SelectedItem));
        }

        void fullWindow_Closing(object sender, CancelEventArgs e)
        {
            //assign to null so that we won't try to play a video with a Closed window which throws an exception
            fullWindow = null; 
        }

        #endregion    

        #region Server Only Button
        private void OpenDialog_Click(object sender, RoutedEventArgs e)
        {
            var folder = new System.Windows.Forms.FolderBrowserDialog();
            folder.RootFolder = Environment.SpecialFolder.MyComputer;

            System.Windows.Forms.DialogResult r = folder.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.FileDirectory = folder.SelectedPath;
                Properties.Settings.Default.Save();
                FilePath.Content = folder.SelectedPath;
            }
        }
        #endregion

        #region Other Form Events
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (net.CurrentState == SystemState.LoggingIn ||
                net.CurrentState == SystemState.LoggedIn)
            {
                net.EndConnection();
            }
        }

        private void ClearMessages_Click(object sender, RoutedEventArgs e)
        {
            Messages.Items.Clear();
        } 
        #endregion


    }


}
