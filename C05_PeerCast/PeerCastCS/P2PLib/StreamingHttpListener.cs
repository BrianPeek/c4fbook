using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace C4F.VistaP2P.Common
{
    public class StreamingHttpListener
    {

        Stream mPseudoStream;
        //a thread safe stream
        public Stream mSyncStream;

        //used to remember the position that we last wrote to in the stream.
        long mSyncStreamLastWrittenPos;
        //used to remember the last position that we have read from the stream.
        long mSyncStreamLastReadPos;
        //true while we are receiving an audio stream.
        bool mIncommingStream;
        String mReceivingNodeName, mSafeFileName;
        private BackgroundWorker mHttpListenerThread;
        System.Collections.Hashtable mHashTable;
        int mDesiredChunk;
        Guid mReceivingGuid;
        Timer mTimer;
        private static readonly object Lock = new object();
        bool mSending;
        String mUniqueStreamName;
        //MediaElement mMediaElement;

        public delegate void WorkerThreadFinished(object sender, RunWorkerCompletedEventArgs e);

        //register for messages on streams being started and finished if interested.
        public delegate void LogMessage(String s);
        LogMessage mLogMessage;

        //delegate to call when we start streaming a new file 
        public delegate void StartStream(String filename);
        StartStream mStartStream;

        //delegate to call when we start streaming a new file 
        public delegate bool SendStream();
        SendStream mSendStream;

        /// <summary>
        /// Multiple threads are required in order to send and receive streamed data if the user interface is going to be responsive.
        /// The best way I could seperate the visual content from the multiple threads required to send and receive streamed data was via the use
        /// of delegates. I could have passed the classes in such as MainWindow or mP2P, however this would have greatly increased the dependencies.
        /// By using delegates this class can stand on it's own without using MainWindow or mP2P.
        /// </summary>
        /// <param name="nodeName">This Node's name</param>
        /// <param name="threadFinishedDelegate">The delegate to respond when the Thread has finished (this will only happen when it is canceled via </param>
        public StreamingHttpListener(String uniqueStreamName, WorkerThreadFinished threadFinishedDelegate,
                                        LogMessage msg, StartStream startStream, SendStream sendStream)
        {
            
            mLogMessage = msg;
            mUniqueStreamName = uniqueStreamName;
            mStartStream = startStream;
            mSendStream = sendStream;
          
            //start the audioHttpListenerThread. This thread contains the AudioHttpServer and responds to requests from
            //the mAudioPlayer  
            mHttpListenerThread = new System.ComponentModel.BackgroundWorker();
            mHttpListenerThread.DoWork += new System.ComponentModel.DoWorkEventHandler(PseudoHttpServer);
            mHttpListenerThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(threadFinishedDelegate);
            mHttpListenerThread.WorkerSupportsCancellation = true;
            mHttpListenerThread.RunWorkerAsync(null);

            //initialize the audio parameters.
            mIncommingStream = false;
            mSyncStreamLastWrittenPos = 0;
            mPseudoStream = new MemoryStream();
            mSyncStream = Stream.Synchronized(mPseudoStream);
            mHashTable = new System.Collections.Hashtable();

            mTimer = new Timer(new TimerCallback(TimerCallback));
            mTimer.Change(0, 3500);//every 3500ms get a callback
        }

        /// <summary>
        /// Call this when you want the thread to close. It will notify the caller when the thread has exited via the 
        /// delegate passed in the constructor.
        /// </summary>
        public void Quit()
        {
            mHttpListenerThread.CancelAsync();
        }

        public void SendStreamingData(String fullFileName)
        {
            //send it in another thread so the UI thread remains responsive...  
            BackgroundWorker worker;
            worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(SendBGWorker);
            worker.RunWorkerAsync(fullFileName);
            mSending = true;
           // SetButtonContent((object)"Cancel");
        }



        public void SendBGWorker(object sender, DoWorkEventArgs e)
        {

            if (mLogMessage != null)
                mLogMessage(String.Format("Sending Media File: {0}\r\n", (String)e.Argument));

            mStartStream((String)e.Argument);
            while (false == mSendStream()) ;
            if (mLogMessage != null)
                mLogMessage("Finished Sending Media\r\n");            
            mSending = false;
        }

        delegate void SetBufferLabelHandler(Object obj);
        public void SetBufferLabelContent(object obj)
        {
            //if (mBufferingLabel.CheckAccess())
            //{
            //    // The calling thread owns the dispatcher, and hence the UI element
            //    mBufferingLabel.Content = obj;
            //}
            //else
            //{
            //    // Invocation required
            //    mBufferingLabel.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new SetBufferLabelHandler(SetBufferLabelContent), obj);
            //}
        }


        delegate void SetButtonHandler(Object obj);
        public void SetButtonContent(object obj)
        {

            //if (mButtonLabel.CheckAccess())
            //{
            //    if (mDefaultLabelContent == null)
            //    {
            //        mDefaultLabelContent = mButtonLabel.Content;
            //    }

            //    // The calling thread owns the dispatcher, and hence the UI element
            //    mButtonLabel.Content = obj;
            //}
            //else
            //{
            //    // Invocation required
            //    mButtonLabel.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new SetButtonHandler(SetButtonContent), obj);
            //}
        }




        public void StreamedChanged(StreamedChangedEventArgs e)
        {
            
            //if it's a new file, clear the cache. test this via the guid attached
            if (e.StreamedPacket.guid != mReceivingGuid)
            {
                lock (Lock)
                {
                    mSyncStreamLastWrittenPos = 0;

                    mSyncStream.Close();
                    mSyncStream.Flush();
                    mPseudoStream.Close();
                    mPseudoStream = new MemoryStream();
                    mSyncStream = Stream.Synchronized(mPseudoStream);
                }
                mReceivingGuid = e.StreamedPacket.guid;
                mReceivingNodeName = e.StreamedPacket.fileName;
                mHashTable.Clear();
                mDesiredChunk = 0;
            }


            if (e.StreamedPacket.packetNumber != mDesiredChunk)
            {
                //Audio showed up in the wrong order store it away until the correct one shows up
                mHashTable.Add(e.StreamedPacket.packetNumber, e.StreamedPacket);
                return;
            }
            else
            {
                //process the current one
                UpdateAudioStream(e.StreamedPacket);
                mDesiredChunk++;
                //process any others that we have cached..
                while (mHashTable.Contains(mDesiredChunk))
                {
                    UpdateAudioStream((StreamedPacket)mHashTable[mDesiredChunk]);
                    mDesiredChunk++;
                }
            }

            if (e.StreamedPacket.packetNumber == 0)
            {
                mSafeFileName = SafeHttpString(e.StreamedPacket.fileName);
            }
        }

        protected void UpdateAudioStream(StreamedPacket packet)
        {

            //ok we found the chunk we're looking for lets process it and fire off any others we also may need to.
            lock (Lock)
            {
                mSyncStream.Position = mSyncStreamLastWrittenPos;
                StreamingHelper.CopyStream(packet.stream, mSyncStream);
                mSyncStreamLastWrittenPos = mSyncStream.Position;
                mIncommingStream = !packet.endOfStream;

                if (mIncommingStream == false)
                {
                    mLogMessage("Finished Receiving Media File\r\n");
                }
            }
        }



        /// <summary>
        /// The background thread that sets up the HttpListener so we can stream the incoming audio file to the MediaElement via
        /// a local URL.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PseudoHttpServer(object sender, DoWorkEventArgs e)
        {
            HttpListener listener = new HttpListener();
            IAsyncResult result = null;
            try
            {
                //Ensure that the HttpListener is supported.
                if (!HttpListener.IsSupported)
                {
                    System.Diagnostics.Debug.WriteLine("Windows Vista, XP SP2 or Server 2003 is required to use the HttpListener class.");
                    return;
                }

                //Set up the URL that the HttpLister will listen to for requests.
                listener.Prefixes.Add("http://localhost:8088/" + mUniqueStreamName);
                listener.Start();

                //only handles one request at a time...could fire off another thread if we wanted to handle more
                //than one request at a time.
                result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);                

                while (true)
                {
                    //result.AsyncWaitHandle.WaitOne(500, false);
                    System.Threading.Thread.Sleep(500);
                    if (mHttpListenerThread.CancellationPending)
                    {
                        //someone told us to quit.
                        break;
                    }
                }

                //shut down the httpListener.
                listener.Prefixes.Clear();
                listener.Stop();
                listener.Close();
            }      
            catch(System.Net.HttpListenerException)
            {
                System.Windows.Forms.MessageBox.Show(
                    "An error occured while trying to start the Audio/Video threads. The Audio/Video portions " +
                    "of the application will not work. The error occured because the the application did not " +
                    "have the appropriate permissions." +
                    "\n\nDevelopers: See the Streaming portion of the PeerChannelTechnical.doc for information on how to resolve this.");
            }
            catch (Exception ex)
            {
               
                System.Diagnostics.Debug.WriteLine(String.Format("Exception occurred in mP2P_PseudoAudioHttpServer: {0}", ex.Message));
            }
        }       

        public void ListenerCallback(IAsyncResult result)
        {          
            HttpListener listener = (HttpListener)result.AsyncState;
            if (listener.IsListening)
            {
                try
                {

                    // Call EndGetContext to complete the asynchronous operation.
                    HttpListenerContext context = listener.EndGetContext(result);
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;

                    response.KeepAlive = true;
                    response.SendChunked = true;
                    response.ContentType = "audio/x-ms-wma";
                    response.AddHeader("Content-Disposition", "attachment;filename=" + mSafeFileName);
                    response.SendChunked = true;
                    System.IO.Stream outstream = response.OutputStream;
                    System.Diagnostics.Debug.WriteLine(String.Format("{0} :Start Serving", DateTime.Now));

                    //Read from the synchronized audio stream and write to the requesting 
                    //client (a MediaElement in our case).
                    //Continue to write to the client until we get an endOfStream message, or <todo> timeout .
                    byte[] buffer = new byte[12000];
                    int len = 1;
                    mSyncStreamLastReadPos = 0;

                    while (len > 0 || mIncommingStream == true)
                    {
                        if (len == 0)
                        {
                            //no new data sleep and wait for it.
                            //maybe this loop  should be updated to work with more of an event delegate pair rather
                            //than spinning waiting for the stream.
                            System.Threading.Thread.Sleep(200);//don't sit and hog the processor...while we wait for the streem
                        }

                        //obtain a lock so we can read some, and then update the position.
                        lock (Lock)
                        {
                            
                            mSyncStream.Position = mSyncStreamLastReadPos;
                            
                            len = mSyncStream.Read(buffer, 0, 12000);
                            
                            if (outstream.CanWrite)
                            {
                            
                                outstream.Write(buffer, 0, len);
                                mSyncStreamLastReadPos += len;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine(String.Format("{0} Unable to Write.", DateTime.Now));
                            }
                        }
                    }                    
                    outstream.Flush();
                    outstream.Close();
                    response.Close();

                    System.Diagnostics.Debug.WriteLine(String.Format("{0} End Serving", DateTime.Now));

                    //listen for the next audio request...
                    if (listener.IsListening)
                        listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("{0} Exception...", DateTime.Now));
                    //someone just changed the audio file to play on us so the outstream is no longer valid..just ignore 
                    //response.Close();
                    if (listener.IsListening)
                        listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                }


            
            }
        }

        public void ReceiveTimeout()
        {
            mIncommingStream = false;
            if (mLogMessage != null)
                mLogMessage("Stream Timed out...");

           // SetBufferLabelContent((object)"");

        }

        delegate void StopHandler();
        public void Stop()
        {

            //if (mMediaElement.CheckAccess())
            //{
            //    // The calling thread owns the dispatcher, and hence the UI element
            //    mMediaElement.Stop();
            //    mMediaElement.Close();
            //    lock (Lock)
            //    {
            //        if (null != mSyncStream)
            //        {
            //            mSyncStream.Close();
            //            mSyncStream = null;
            //        }
            //    }
            //}
            //else
            //{
            //    // Invocation required
            //    mMediaElement.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new StopHandler(Stop), null);
            //}
        }

        delegate void TimerHandler(object state);
        public void TimerCallback(object state)
        {

            //if (mMediaElement.CheckAccess())
            //{
            //    if (mMediaElement.IsBuffering)
            //    {
            //        mBufferingLabel.Visibility = Visibility.Visible;
            //        mBufferingLabel.Content = String.Format("Buffering...{0}%", mMediaElement.BufferingProgress * 100);
            //    }
            //    else
            //    {
            //        mBufferingLabel.Visibility = Visibility.Hidden;
            //        mBufferingLabel.Content = "";
            //    }
            //}
            //else
            //{
            //    mMediaElement.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new TimerHandler(TimerCallback), state);
            //}
        }




        /// <summary>
        /// true if currently sending the stream. This needs to be set to false if the stream is canceled.
        /// </summary>
        public bool Sending
        {
            get { return mSending; }
        }


        /// <summary>
        /// This is a little helper function, to remove invalid characters in a http string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String SafeHttpString(String input)
        {
            if (null == input)
                return null;

            //get rid of white space
            String safe = input.Trim();
            safe = safe.Replace(" ", "").ToLower();

            //remove bad characters.
            safe = Regex.Replace(safe, "[^a-z0-9\\.]", "");
            return safe;
        }
    }
}