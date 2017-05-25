using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.PeerResolvers;

///////////////////////////////////////////////////////////////////////////////
// S U M M A R Y
//
// This is is a wrapper around the PeerChannel API. A simple example is included in the 
// SimpleP2P project. The other classes are present to help this class.
//
// A R C H I T E C T U R E 
//
// Windows Communication Foundation (WCF) is an SDK for developing and deploying services. A WCF Service
// is defined by three components: 
//      1) A service Contract (what operations can be performed, ie. what functions one can call on a remote computer)
//      2) A Binding (How to communicate with the service, NetPeerTcpBinding P2P)
//      3) An address (defines where the service is, how to find it.)
//
// When these three things are combined it's refered to as an Endpoint. This endpoint is the way one communicates
// with the service. To create a p2p application the binding is NetPeerTcpBinding. The address for all NetPeerTcpBindings start with "net.p2p://".
// I follow it by the network name the user specifies.  The third portion is the Service contract which is specified in ServiceContract.cs
// 
// In this application we demonstrate several different types of data (audio, pictures, text, files, video). I could have made an endpoint for each type of data, 
// however it would have then required maintaining the network for each data type (joining, leaving, and online/offline). Handling cases where some of the 
// meshes were connected and some were not would have been a difficult user Scenario (how to report to the user when they have connected), and I think less 
// reliable. The alternative is to have a single endpoint
// endpoint that supported them all. _One_ class must implement the Service Contract Interface.  In order to keep things
// understandable this class PeerChannelHelper is responsible for creating the endpoint, and closing the endpoint. All of the other messages are passed
// off to the appropriate helper classes. IE. ShareFile is passed onto FilePeerChannelHelper.     
//
// An additional consideration was for others to be able to use this wrapper to create PeerChannel Applications.  
// With this in mind all of the OperationContract messages can be received by subscribing to the event. Any events that are not subscribed to are
// simply ignored. An simple example of this for a text messaging application is provided in the SimpleP2P Project.
//
// A D D I T I O N A L   C O N S I D E R A T I O N S  
//
// Many of the design decisions were based on the requirement that we be able to stream audio and video.
// Streaming could be interpreted two ways:
//              1) The data be converted to a Stream data type and sent 
//              2) The data will be sent and start playing before the entire file is received.
//  
//  We wanted the second one.  This proved to be the more difficult task.
//  By default in Windows Communication Foundation, all data is sent in a buffered mode, meaning that the 
//  message must be completely delivered before a receiver can read it.  Unfortunately only BasicHttpBinding, 
//  NetTcpBinding and NetNamedPipeBinding (see http://msdn2.microsoft.com/en-us/library/ms751463.aspx) support a Streamed mode.  
//  This means that even if we create a service contract with a Stream as a parameter all of the data will be 
//  transfered to the receiving node before our receiving 
//  service contract function even gets called. To put it even more simply if a 300 mb file is sent all 300mb will be received
//  before the receiving node's function is called.
//
//  This left us with two choices: write a custom binding or break the data up into packets and re-assemble them. Breaking the
//  data into packets and re-assembling the data seemed more straightforward and so we implemented this option.
//
//
//  U S A G E
//
//  Create/Connect to a network by creating an instance of PeerChannelWrapper and calling the constructor:
//        PeerChannelWrapper(String nodeName, String networkName, String password)
//
//  Events can be listened for by registering for the callbacks.  For example to register for a StatusChanged event if
//  your PeerChannelWrapper instance is named mP2PLib.  
//           mP2PLib.StatusChanged += new PeerChannelWrapper.PeerChannelWrapper.StatusChangedHandler(mP2PLib_StatusChanged);
//
//  Then in the class have the function mP2PLib_StatusChanged that has a parameter of
//          PeerChannelWrapper.PeerChannelWrapper.StatusChangedArgs e
//
//  In this way, you can register for the events you are interested in, and ignore the rest.
//
//  To Leave the network call Close()
///////////////////////////////////////////////////////////////////////////////
namespace C4F.VistaP2P.Common
{
    //The actual wrapper class.
    //Maybe could use this instead of threading and will force to another thread? see; 
    //http://www.code-magazine.com/article.aspx?quickid=0701041&page=3
    //[ServiceBehavior(UseSynchronizationContext=false)]

    
    public class P2PLib : IPeerChannel, IDisposable
    {
        //////////////////////////////////////////////////////////////////////////
        //Member Variables
        //////////////////////////////////////////////////////////////////////////                       
        string mNodeName;                                   //the node name passed in with the constructor
        string mPassword;                                   //the password for this network
        string mNetworkName;                                //the network name.
        InstanceContext mInstanceContext;                   //Instance Context        
        DuplexChannelFactory<IPeerChannelWrapper> mFactory; //used to create a channel that supports IPeerChannelWrapper
        IPeerChannelWrapper mParticipant;                   //our interface to the channel        


        public P2PLib()
        {
            mNodeName = mPassword = mNetworkName = null;
            mFactory = null;
            mParticipant = null;
            mDisposed = false;
        }

        /// <summary>
        /// The constructor for this.  In addition to initializing all of the parameters it attempts to open and connect
        /// to the network.  This is done asynchronously with ProcessOpen being called either after a timeout (hard coded to 1 minute)
        /// or when a response is received.
        /// </summary>
        /// <param name="nodeName">the unique nodename</param>
        /// <param name="networkName">the name of the network</param>
        /// <param name="password">the password for the network</param>
        public bool PeerChannelWrapperStart(String nodeName, String networkName, String password)
        {
            CheckDisposed();
            //////////////////////////////////////////////////////////////////////////
            //Initialize all the parameters...
            //////////////////////////////////////////////////////////////////////////                       
            mNodeName = nodeName;
            mNetworkName = networkName;

            //password cannot be empty otherwise mFactory.CreateChannel() below will throw an exception
            if (string.IsNullOrEmpty(password))
            {
                mPassword = "pass";
            }
            else
            {
                mPassword = password.Trim();
            }

            //Construct InstanceContext to handle messages on callback interface. 
            mInstanceContext = new InstanceContext(this);

            //////////////////////////////////////////////////////////////////////////
            //Create the PeerTcpBinding
            //////////////////////////////////////////////////////////////////////////            
            NetPeerTcpBinding p2pBinding = new NetPeerTcpBinding();
            p2pBinding.Name = "BindingDefault";
            p2pBinding.Port = 0; //dynamic port
            p2pBinding.MaxReceivedMessageSize = 70000000; //set the max message size in bytes (70mb)          
            p2pBinding.Resolver.Mode = PeerResolverMode.Pnrp; //use the standard pnrp for our resolver to find the network  

            //////////////////////////////////////////////////////////////////////////
            //create the Endpoint Address and Service Endpoint.
            //////////////////////////////////////////////////////////////////////////            
            String bindingInfo = "net.p2p://";  //the endpoint address for a NetPeerTcpBinding must start with net.p2p
            EndpointAddress epa = new EndpointAddress(String.Concat(bindingInfo, mNetworkName));
            ServiceEndpoint serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(IPeerChannel)), p2pBinding, epa);

            //////////////////////////////////////////////////////////////////////////            
            // Create the participant with the given endpoint configuration
            // Each participant opens a duplex channel to the mesh
            // participant is an instance of the channel to the mesh
            //////////////////////////////////////////////////////////////////////////            
            mFactory = new DuplexChannelFactory<IPeerChannelWrapper>(mInstanceContext, serviceEndpoint);                            
            mFactory.Credentials.Peer.MeshPassword = mPassword;            
            mParticipant = mFactory.CreateChannel();
           
            mPicture = new PictureHelper(mParticipant.SharePicture, mNodeName);
            mTextPeerChannelHelper = new ChatHelper(mParticipant.ShareTextMessage);
            mFilePeerChannelHelper = new FileHelper(mParticipant.ShareFile);
            mStreamedAudio = new StreamingHelper(mParticipant, mNodeName, mParticipant.ShareStreamedAudio);
            mStreamedVideo = new StreamingHelper(mParticipant, mNodeName, mParticipant.ShareStreamedVideo);
           
            //////////////////////////////////////////////////////////////////////////
            //Start an asynchronous call that will try to open a connection to the network.
            //if one is not found, but the pnrp resolver is running then a new network will be
            //created.
            //////////////////////////////////////////////////////////////////////////            
            try
            {
                AsyncCallback callBack = new AsyncCallback(ProcessOpen);
                TimeSpan ts = new TimeSpan(0, 1, 0); //wait a minute
                mParticipant.BeginOpen(ts, callBack, this);
            }
            catch (CommunicationException e)
            {
                System.Windows.MessageBox.Show(String.Format("Error while joining the network: {0}", e.Message));                
                mParticipant = null;
                return false;
            }
            return true;
        }          

        /// <summary>
        /// Async call back from beginning to open the graph. 
        /// </summary>
        /// <param name="result">the result returned</param>
        void ProcessOpen(IAsyncResult result)
        {
            //if they canceled out while we were connecting the object will be disposed.
            //if so just do nothing.
            if (false == mDisposed)
            {
                try
                {
                    mParticipant.EndOpen(result);
                }
                catch (TimeoutException)
                {
                    System.Windows.MessageBox.Show(String.Format("Unable to join the Network because the maximum time allowed was exceeded"));
                    return;
                }
                catch (Exception e)
                {
                    if (null != e.InnerException)
                        System.Windows.MessageBox.Show(String.Format("Unable to join the network because: {0}", e.InnerException.Message));
                    mParticipant = null;
                    return;
                }
                mParticipant.Join(mNodeName); //make a service call so the other nodes know we have just joined.            
            }
        }

        /// <summary>
        /// Destructor...just call close to shut everything down and leave the network
        /// </summary>
        ~P2PLib()
        {
            Dispose(false);  
        }

        /// <summary>
        /// The actual function that closes the node if we were connected.
        /// </summary>
        public void Close()
        {
            (this as IDisposable).Dispose();
        }
      
    

        ///////////////////////////////////////////////////////////////////////////////////////
        //V I D E O
        ///////////////////////////////////////////////////////////////////////////////////////
        private StreamingHelper mStreamedVideo;

        public StreamingHelper StreamedVideo
        {
            get { CheckDisposed(); return mStreamedVideo; }
            set { CheckDisposed(); mStreamedVideo = value; }
        }

        /// <summary>
        /// Implements the ShareStreamedVideo function specified in the IPeerChannel service contract above.
        /// </summary>
        /// <param name="mdp">the audio packet to send</param>
        public void ShareStreamedVideo(StreamedPacket packet)
        {
            CheckDisposed();
            if (mStreamedVideo.StreamedState == StreamedStateType.Initial)
            {
                ShareTextMessage(packet.senderNodeName, String.Format(" is Sending you an video file"));
            }
            mStreamedVideo.ShareStream(packet);
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        //A U D I O
        ///////////////////////////////////////////////////////////////////////////////////////
        private StreamingHelper mStreamedAudio;

        public StreamingHelper StreamedAudio
        {
            get { CheckDisposed(); return mStreamedAudio; }
            set { CheckDisposed(); mStreamedAudio = value; }
        }

        /// <summary>
        /// Implements the ShareStreamedAudio function specified in the IPeerChannel service contract above.
        /// </summary>
        /// <param name="mdp">the audio packet to send</param>
        public void ShareStreamedAudio(StreamedPacket packet)
        {
            CheckDisposed();
            if (mStreamedAudio.StreamedState == StreamedStateType.Initial)
            {
                ShareTextMessage(packet.senderNodeName, String.Format(" is Sending you an audio file"));
            }
            mStreamedAudio.ShareStream(packet);
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //T E X T
        ///////////////////////////////////////////////////////////////////////////////////////
        private ChatHelper mTextPeerChannelHelper;

        public ChatHelper TextPeerChannelHelper
        {
            get { CheckDisposed(); return mTextPeerChannelHelper; }
            set { CheckDisposed(); mTextPeerChannelHelper = value; }
        }
        public void ShareTextMessage(string member, string msg)
        {
            CheckDisposed();
            mTextPeerChannelHelper.ShareChatMessage(member, msg);
        }             

        ///////////////////////////////////////////////////////////////////////////////////////
        //P I C T U R E
        ///////////////////////////////////////////////////////////////////////////////////////
        private PictureHelper mPicture;

        public PictureHelper Picture
        {
            get { CheckDisposed(); return mPicture; }
            set { CheckDisposed(); mPicture = value; }
        }
        public void SharePicture(PicturePacket packet)
        {
            CheckDisposed();
            mPicture.SharePicture(packet);
        }
           
        ///////////////////////////////////////////////////////////////////////////////////////
        //F I L E
        ///////////////////////////////////////////////////////////////////////////////////////
        private FileHelper mFilePeerChannelHelper;

        public FileHelper FilePeerChannelHelper
        {
            get { CheckDisposed(); return mFilePeerChannelHelper; }
            set { CheckDisposed(); mFilePeerChannelHelper = value; }
        }
        public void ShareFile(Packet packet)
        {
            CheckDisposed();
            //ignore files sent to ourselves...
           
            mFilePeerChannelHelper.ShareFile(packet);
         
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //N E T W O R K   S T A T U S
        ///////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////
        //The event to register for if you are interested in knowing
        //about when other nodes join and leave.
        //////////////////////////////////////////////////////////////////////////                                
        private event EventHandler<StatusChangedEventArgs> mStatusChanged;

        public event EventHandler<StatusChangedEventArgs> StatusChanged
        {
            add { CheckDisposed(); mStatusChanged += value; }
            remove { CheckDisposed(); mStatusChanged -= value; }
        }
              
        
        /// <summary>
        /// Implement the join interface.
        /// Notify anyone that is registered for the 
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void Join(string member)
        {
            CheckDisposed();
            if (null != mStatusChanged)
            {                
                mStatusChanged(this, new StatusChangedEventArgs(member, true, false, false, false));
            }
        }

        /// <summary>
        /// Implements the Leave interface
        /// </summary>
        /// <param name="member"></param>
        public void Leave(string member)
        {
            //CheckDisposed();
            if (null != mStatusChanged)
            {                
                mStatusChanged(this, new StatusChangedEventArgs(member, false, true, false, false));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns true if any other node is also in the network</returns>
        public bool IsOnline()
        {
            CheckDisposed();
            return mParticipant.GetProperty<IOnlineStatus>().IsOnline;            
        }

        


        private bool mDisposed;
        protected void CheckDisposed()
        {
            if (mDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            try
            {                                
                if (mParticipant != null)
                {
                    mParticipant.Leave(mNodeName);
                    //mParticipant.Close();
                    //mParticipant.Abort();
                }

               
            }
            catch (InvalidOperationException)
            {
                //safely ignore. This exception occurs when we are in the process of opening when they quit the application.
                //catch the exception to ignore it.
            }

            if (mFactory != null)
            {
                mFactory.Close();
            }

            mParticipant.Abort();
            mParticipant.Dispose();
            mParticipant = null;
            mDisposed = true;           
        }

        virtual protected void Dispose(bool disposeAll)
        {
            (this as IDisposable).Dispose();        
        }
    }


}
