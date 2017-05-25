using System;

namespace C4F.VistaP2P.Common
{
    /// <summary>
    /// This is the data that gets passed when a new class showed up.
    /// </summary>
    public class FileChangedEventArgs : EventArgs
    {
        C4F.VistaP2P.Common.Packet mPacket;

        public Packet Packet
        {
            get
            {
                return mPacket;
            }
        }

        public FileChangedEventArgs(Packet cp)
        {
            mPacket = cp;
        }
    }


    /// <summary>
    /// This class like all the other small helper classes (picturePeerChannelHelper, StreamingPeerChannelHelper)
    /// take a one of the ServiceContract (ServiceContract.cs) functions as a parameter to the constructor.
    /// This gives the class the ability to call the service contract function without having access to the entire 
    /// set of PeerChannelWrapper functions.
    //
    // Additionally, this class is responsible for handling incoming events and notifies any functions registered to the event(FileChangedHandler in this case).
    /// </summary>
    public class FileHelper
    {
        //this delegate is used to call the appropriate Operation in the Service Contract.
        public delegate void OperationContractHandler(Packet packet);
        OperationContractHandler mOperationContract;

        public OperationContractHandler OperationContract
        {
            get { return mOperationContract; }
            set { mOperationContract = value; }
        }

        public FileHelper(OperationContractHandler op)
        {
            mOperationContract = op;
        }

        /// <summary>
        /// call this to send a file
        /// </summary>
        /// <param name="packet"></param>
        public void SendFile(Packet packet)
        {
            mOperationContract(packet);
        }

        /// <summary>
        /// notify anyone registered for this event.
        /// </summary>
        /// <param name="packet"></param>
        public void ShareFile(Packet packet)
        {
            if (mFileChanged != null)
            {                
                mFileChanged(this, new FileChangedEventArgs(packet));
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //the event to register for if one is interested in recieving file events
        ///////////////////////////////////////////////////////////////////////////////////////        
        private event EventHandler<FileChangedEventArgs> mFileChanged;

        public event EventHandler<FileChangedEventArgs> FileChanged
        {
            add { mFileChanged += value; }
            remove { mFileChanged -= value; }
        }
    }
}