using System;

namespace C4F.VistaP2P.Common
{

    //This wraps the audio Packet into an EventArg so anyone who is registered for the event can process it.
    public class StreamedArgs : EventArgs
    {
        public StreamedPacket mdp;
        internal StreamedArgs(StreamedPacket mdp)
        {
            this.mdp = mdp;
        }
    }

    /// <summary>
    /// A simple class that just inherits from EventArgs so we can pass the data to any event listeners.
    /// </summary>
    public class PictureChangedEventArgs : EventArgs
    {
        PicturePacket mChunkedPacket;

        public PicturePacket Packet
        {
            get
            {
                return mChunkedPacket;
            }
        }

        public PictureChangedEventArgs(PicturePacket cp)
        {
            mChunkedPacket = cp;
        }
    }

    /// <summary>
    /// A simple class that just inherits from EventArgs so we can pass the data to any event listeners.
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        string member;
        bool joined, left, online, offline;

        public String Member
        {
            get { return member; }
        }

        public bool NewNodeJoined
        {
            get { return joined; }
        }
        public bool NodeLeft
        {
            get { return left; }
        }

        public bool NodeOnline
        {
            get { return online; }
        }

        public bool NodeOffline
        {
            get { return offline; }
        }

        internal StatusChangedEventArgs(String member, bool joined, bool left, bool online, bool offline)
        {
            this.member = member;
            this.joined = joined;
            this.left = left;
            this.online = online;
            this.offline = offline;
        }
    }

    public class ChatChangedEventArgs : EventArgs
    {
        private string msg;

        public ChatChangedEventArgs(string messageData)
        {
            msg = messageData;
        }
        public string Message
        {
            get { return msg; }
            set { msg = value; }
        }
    }

    //This wraps the audio Packet into an EventArg so anyone who is registered for the event can process it.
    public class StreamedChangedEventArgs : EventArgs
    {
        private StreamedPacket mStreamedPacket;

        public StreamedPacket StreamedPacket
        {
            get { return mStreamedPacket; }
            set { mStreamedPacket = value; }
        }
        internal StreamedChangedEventArgs(StreamedPacket mdp)
        {
            this.mStreamedPacket = mdp;
        }
    }

    public class SendStreamEventArgs : EventArgs
    {
        private string mFullFileName;

        public string FullFileName
        {
            get
            {
                return mFullFileName;
            }
        }

        public SendStreamEventArgs(string FullFileName)
        {
            mFullFileName = FullFileName;
        }
    }
}
