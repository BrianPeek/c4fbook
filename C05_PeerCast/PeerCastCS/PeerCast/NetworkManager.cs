using System;
using System.ComponentModel;
using System.Windows;
using C4F.VistaP2P.Common; 

namespace PeerCast
{
    public class NetworkManager
    {
        #region Variables
        public BackgroundWorker P2pWorker;
        private string userName = "machine" + DateTime.Now.Ticks;
        private string networkName;
        private string password;
        private P2PLib p2p;
        private StreamingHttpListener videoHttpListener;
        private int progressBarValue = 0;
        public SystemState CurrentState = SystemState.LoggedOut;
        #endregion

        public NetworkManager()
        {
            P2pWorker = BackgroundWorkerUtility.Create();
        }

        public void StartConnection(string networkName, string password)
        {
            this.networkName = networkName;
            this.password = password;

            if (ValidateResetConnection())
            {
                //update the network state
                this.CurrentState = SystemState.LoggingIn;
                                
                P2pWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(P2pWorkerAsync);

                //start working
                P2pWorker.RunWorkerAsync();
            }
        }

        //p2p may not "die" when setting to null so need to check here        
        private bool ValidateResetConnection()
        {
            if (p2p != null)
            {
                MessageBox.Show("An Error occurred when trying to initialize the Network. Please restart the application");
                return false;
            }
            return true;
        }
       
        private void P2pWorkerAsync(object sender, DoWorkEventArgs doWorkArgs)
        {
            //Send back a text that we're logging in
            P2pWorker.ReportProgress(0, ThreadMessage.Create(UiMessage.ToggleUi, string.Empty)); 

            //Create new p2p
            p2p = new P2PLib();

            //Connect to the PeerChannel
            bool start = p2p.PeerChannelWrapperStart(userName, networkName, password); 

            //check for connection error
            if (!start)
            {
                EndConnection();
            }

            SubscribeToPeerChannelEvents();

            //PeerChannel connection may take a while so cycle the ProgressBar
            CycleProgressBarUntilConnected();

            SitAndWait();
        }

        private void SubscribeToPeerChannelEvents()
        {
            //hook up the events we care about...text and status for the server
            p2p.StatusChanged += new EventHandler<StatusChangedEventArgs>(P2PLib_StatusChanged);
            p2p.TextPeerChannelHelper.ChatChanged += new EventHandler<ChatChangedEventArgs>(P2PLib_ChatChanged);

            //only setup the listener in client mode
            if (!App.IsServerMode)
            {
                p2p.StreamedVideo.StreamChanged += new EventHandler<StreamedChangedEventArgs>(P2PLib_StreamChanged);
            }

            //required by server for start/send stream, used by client for StreamChanged event defined above)
            videoHttpListener = new StreamingHttpListener(userName + "/video/",
                 MediaFinished,
                 LogMessage,
                 p2p.StreamedVideo.StartStream,
                 p2p.StreamedVideo.SendStream);
            
        }

        private void CycleProgressBarUntilConnected()
        {
            //spin while we log in and update the progress bar.
            while (this.CurrentState == SystemState.LoggingIn)
            {
                System.Threading.Thread.Sleep(100);
                if (progressBarValue >= 100)
                {
                    progressBarValue = 0;
                }
                else
                {
                    progressBarValue++;
                }

                //Send back to UI thread
                P2pWorker.ReportProgress(progressBarValue);
                if (P2pWorker.CancellationPending)
                {
                    P2pWorker.ReportProgress(0);
                }
            }
            //when complete, report 100 as the progress
            P2pWorker.ReportProgress(100);
        }

        private void SitAndWait()
        {
            //continuous loop to sit and wait for PeerChannel or form events
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                if (P2pWorker != null)
                {
                    if (P2pWorker.CancellationPending || CurrentState == SystemState.LoggedOut)
                    {
                        P2pWorker.ReportProgress(0);
                        break;
                    }
                }
            }
        }

        protected void P2PLib_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (userName == e.Member)
            {
                if (e.NewNodeJoined)
                {
                    this.CurrentState = SystemState.LoggedIn;
                    P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.UpdateStatus, "Connected"));                     
                }

                if (e.NodeLeft)
                {
                    P2pWorker.ReportProgress(0, ThreadMessage.Create(UiMessage.UpdateStatus, "Disconnected...")); 
                    this.CurrentState = SystemState.LoggedOut;
                }
            }           
        }
        


        #region Chat Methods

        public void SendChatMessage(ChatMessage message)
        {
            p2p.TextPeerChannelHelper.SendTextMessage(message.CommandType.ToString(), message.Message);
        }
        
        protected void P2PLib_ChatChanged(object sender, ChatChangedEventArgs e)
        {
            //Sends text to chat window
            P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.Log, e.Message));

            if (e.Message.StartsWith("machine"))
            {
                return; //exit
            }

            ChatMessage msg = ChatMessage.P2PLibParse(e.Message);
            if (App.IsServerMode)
            {
                ProcessServerCommand(msg);
            }
            else
            {
                if (msg.CommandType == ChatCommand.ReturnList)
                {
                    ChatMessage.P2PLibParse(e.Message); 
                    P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.ReceivedVideoList, msg.Message));
                }
            }
        }

        private void ProcessServerCommand(ChatMessage msg)
        {
            switch (msg.CommandType)
            {
                case ChatCommand.GetList:
                    string list = Serializer.SerializeFileList();
                    this.SendChatMessage(ChatMessage.Create(ChatCommand.ReturnList, list));
                    break;
                case ChatCommand.StreamVideo:
                    if (videoHttpListener != null)
                    {
                        //Cancel current stream if streaming
                        if (p2p.StreamedVideo.StreamedState == StreamedStateType.Communicating)
                        {
                            p2p.StreamedVideo.CancelSendingStream();
                        }
                        //start streaming file back to the client
                        videoHttpListener.SendStreamingData(msg.Message);
                    }
                    break;
            }
        }
       

        #endregion

        #region Video Streaming Methods

        void P2PLib_StreamChanged(object sender, StreamedChangedEventArgs e)
        {           
            //take packet from server & use videoHttpListener to write to localhost
            videoHttpListener.StreamedChanged(e);
            
            //set client to listen to stream 
            if (e.StreamedPacket.packetNumber == 0)
            {
                string file = StreamingHttpListener.SafeHttpString(e.StreamedPacket.fileName);
                string url = String.Format(@"http://localhost:8088/{0}/video/{1}", userName, file);
                P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.StreamVideo, url));
            }
        }

        protected void MediaFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            //End connection on error
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Error streaming this video:{0}", e.Error.Message));
                EndConnection();
            }
        }

        public void LogMessage(string msg)
        {
            P2pWorker.ReportProgress(100, ThreadMessage.Create(UiMessage.Log, msg));
        }  
        #endregion

        public void EndConnection()
        {
            //make sure we're not calling this multiple times
            if (this.CurrentState != SystemState.LoggedOut)
            {
                this.CurrentState = SystemState.LoggedOut;

                //reset UI after changing SystemState
                P2pWorker.ReportProgress(0, ThreadMessage.Create(UiMessage.ToggleUi, string.Empty));

                //close and set classes to null
                if (p2p != null)
                {
                    p2p.Close();
                    p2p = null;
                }

                if (videoHttpListener != null)
                {
                    videoHttpListener.Stop();
                    videoHttpListener.Quit();
                    videoHttpListener = null;
                }

                if (P2pWorker != null)
                {
                    P2pWorker.CancelAsync();
                }
            }
        }


    }
}
