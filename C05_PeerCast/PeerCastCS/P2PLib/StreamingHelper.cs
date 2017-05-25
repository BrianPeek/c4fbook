using System;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace C4F.VistaP2P.Common
{


    public class StreamingHelper
    {
        StreamedStateType streamedState;   //the state of the streamed class.      
        Timer mWatchDog;                    //timeout if we don't receive a packet after the set time...
        Guid mCurrentGuid;                  //Generate a new guid with each file we send so we can determine on the receiving side what packet group the file should go with.
        String mCurrentFileName;
        Stream mCurrentStream;
        int mSendingPacketNo;               //for a given stream, increment this after every packet is sent
        bool mStopStream;                   //set to true to stop sending the stream.
        string mNodeName;

        // Declare a delegate that takes a single string parameter
        // and has no return type.
        public delegate void ShareStreamHandler(StreamedPacket packet);
        ShareStreamHandler del;

        public StreamingHelper(IPeerChannelWrapper participant, string nodeName, ShareStreamHandler s)
        {
            mNodeName = nodeName;
            streamedState = StreamedStateType.Initial;
            mChunk = 0;
            mLastChunk = -1;
            del = s;

            //////////////////////////////////////////////////////////////////////////
            // Start up an Audio Watchdog timer so if we don't receive a packet after the hard-coded 
            // 20000ms we'll timeout and assume the sending node was lost.
            //////////////////////////////////////////////////////////////////////////            
            mWatchDog = new Timer(new TimerCallback(Watchdog));
            mWatchDog.Change(0, 20000);//every 20000ms get a callback       
        }

        /// <summary>
        /// The Audio event.
        /// Read here for more details on events and delgates: http://www.akadia.com/services/dotnet_delegates_and_events.html
        /// </summary>
        /// <param name="e"></param>        
        public event EventHandler<StreamedChangedEventArgs> StreamChanged;

        /// <summary>
        /// The timeout event.  After 500ms (a hard coded value set in the PeerChannelWrapper constructor)
        /// without receiving a new packet, the audio stream assumes that the sending node has been lost.
        /// anyone who is registered for the event AudioReceiveTimeoutHandler will also be notified.
        /// </summary>        
        private event EventHandler mReceiveTimeoutChanged;

        public event EventHandler ReceiveTimeoutChanged
        {
            add { mReceiveTimeoutChanged += value; }
            remove { mReceiveTimeoutChanged -= value; }
        }

        //Get the current state of the Audio (either initial, or communicating)
        public StreamedStateType StreamedState
        {
            get
            {
                return streamedState;
            }
        }

        /// <summary>
        /// Implements the ShareStreamedAudio function specified in the IPeerChannel service contract above.
        /// </summary>
        /// <param name="mdp">the audio packet to send</param>
        public void ShareStream(StreamedPacket mdp)
        {
            //The audio can be in one of two states: Initial, meaning it is ready to receive a new audio stream,
            //or communicating meaning it is either receiving or transmitting.
            switch (streamedState)
            {
                case StreamedStateType.Initial:
                    //Initialize the data packet;
                    streamedState = StreamedStateType.Communicating;
                    mLastChunk = -1;

                    ////write in the textbox that someone is sending us an audio file.
                    //ShareTextMessage(mdp.senderNodeName, String.Format(" is Sending you an audio file"));
                    break;

                case StreamedStateType.Communicating:

                    //update the most recent packet received so we know that the sender hasn't 
                    //disappeared from the network, or suddenly stopped sending.
                    mChunk = mdp.packetNumber;

                    //found the end of the stream, reset.
                    if (mdp.endOfStream)
                    {
                        streamedState = StreamedStateType.Initial;
                    }
                    break;
            }

            //notify anyone registered for the event.
            if (null != StreamChanged)
            {                
                StreamChanged(this, new StreamedChangedEventArgs(mdp));
            }
        }

        /// <summary>
        /// When sending an audio file, call this first to initialize the data.
        /// </summary>
        /// <param name="filename">The full path of the filename to open.</param>
        public void StartStream(String filename)
        {
            do
            {
                if (mStopStream == false)
                {
                    //Generate a new guid with each file we send so we can determine on the receiving side what packet group the file should go with.
                    mCurrentGuid = System.Guid.NewGuid();
                    mCurrentFileName = filename;
                    mCurrentStream = System.IO.File.OpenRead(filename);
                    mSendingPacketNo = 0;
                    break;
                    
                }                                
                Thread.Sleep(500);                
            } while (true);
            
        }

        /// <summary>
        /// cancel sending the audio file.
        /// </summary>
        public void CancelSendingStream()
        {
            mStopStream = true;
        }

        /// <summary>
        /// This function actually sends a packet out. Continue to call this function until it returns true. 
        /// </summary>
        /// <returns>true when the entire file is sent</returns>
        public bool SendStream()
        {
            StreamedPacket packet = new StreamedPacket();
            Byte[] b = new Byte[12000];
            int len = 0; //the number of bytes read so far. 
            bool finished;

            //only send the meta data on the first packet to conserve bandwidth
            //I'm sure there is still some overhead of sending null strings for the rest of the data.
            if (mSendingPacketNo == 0)
            {
                packet.fileName = System.IO.Path.GetFileName(mCurrentFileName);
                MetadataFileInfo metaData = new MetadataFileInfo();
                metaData.ReadMetaData(mCurrentFileName);
                packet.album = metaData.AlbumName;
                packet.artist = metaData.ArtistName;
                packet.title = metaData.Title;
            }
            //debug put it in each packet...
            packet.fileName = System.IO.Path.GetFileName(mCurrentFileName);

            //read from the file
            len = mCurrentStream.Read(b, 0, 12000);
            //write it to the packet's stream.
            packet.stream.Write(b, 0, len);
            packet.stream.Position = 0;              //set the stream back to the initial position.
            packet.guid = mCurrentGuid;    //set the guid.

            if (mCurrentStream.Length != mCurrentStream.Position && false == mStopStream)
            {
                finished = false;
                packet.endOfStream = false;
            }
            else
            {
                //we are at the end of the file, or someone canceled the file via mStopAudioStream
                //set the file to be finished and send out a endOfStream packet.           
                finished = true;
                mStopStream = false;
                packet.endOfStream = true; //either reached the end or we canceled the stream.
                mCurrentStream.Close();
            }

            //include the node name
            packet.senderNodeName = mNodeName;
            //set the packet number, and increment the sending packet number.
            packet.packetNumber = mSendingPacketNo++;

            try
            {
                if (del != null)
                    del(packet);
            }
            catch (Exception)
            {
                mStopStream = false;
            }
            return finished;
        }

        int mLastChunk, mChunk; //These member variables should never have the same number..otherwise the incomming audio has timed out and the sender is presumed to be disconnected.
        /// <summary>
        /// The callback setup in the constructor. This will get called once every n number of seconds(hard coded in the constructor)
        /// If the numbers are the same, it means we didn't receive a packet inbetween calls.  Therefore we presume that the sending node
        /// has either stopped sending, or we do not have a connection with them.
        /// </summary>
        /// <param name="state"></param>
        public void Watchdog(object state)
        {
            if (streamedState == StreamedStateType.Communicating)
            {
                //could time out...
                if (mChunk == mLastChunk)
                {
                    //timed out..reset the stream back go back to the initial state.                    
                    streamedState = StreamedStateType.Initial;
                    //notify anyone else that wants to know.
                    if (null != mReceiveTimeoutChanged)
                    {
                        mReceiveTimeoutChanged(this, new EventArgs());
                    }
                }
                else
                {
                    mLastChunk = mChunk;//new data received update it.
                }
            }
        }


        /// <summary>
        /// A helper function which copies data from one stream to another.
        /// </summary>
        /// <param name="instream">the stream to read from</param>
        /// <param name="outstream">the stream to write to</param>
        public static void CopyStream(Stream inStream, Stream outStream)
        {
            //read from the input stream in 4K chunks
            //and save to output stream
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            int bytecount = 0;
            int counter = 0;
            while ( true )
            {
                if(false == inStream.CanRead)
                {
                    //timed out..reset the stream back go back to the initial state.                                        
                    Console.WriteLine("CAN't READ****");
                    break; //bail out..someone switched the stream on us.
                }
                if((count = inStream.Read(buffer, 0, bufferLen)) > 0)
                {
                    counter++;
                    outStream.Write(buffer, 0, count);
                    bytecount += count;
                }else
                {
                    break;
                }

            }   
        }
    }
}