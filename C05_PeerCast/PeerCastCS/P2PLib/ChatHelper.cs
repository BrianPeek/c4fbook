using System;

namespace C4F.VistaP2P.Common
{
    /// <summary>
    /// This Class is set up to handle text messages in the peerchannel.
    /// </summary>
    public class ChatHelper
    {
        //this delegate is used to call the appropriate Operation in the Service Contract.
        public delegate void OperationContractEventHandler(string member, string msg);
        OperationContractEventHandler mOperationContract;

        public ChatHelper(OperationContractEventHandler op)
        {
            mOperationContract = op;
        }

        /// <summary>
        /// Call this function to send a text message
        /// </summary>
        /// <param name="message">the message to send</param>
        public void SendTextMessage(string nodeName, string message)
        {
            mOperationContract(nodeName, message);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="member">the node name that is sending it</param>
        /// <param name="msg">the message to send</param>
        public void ShareChatMessage(string member, string msg)
        {
            if (null != mChatChanged)
            {
                mChatChanged(this, new ChatChangedEventArgs(String.Format("{0} said: {1}", member, msg)));
            }
        }

        /// <summary>
        /// The event delegate pair. Subscribe to the Event TextChangedHandler if one is interested in the text. 
        /// </summary>
        /// <param name="s"></param>        
        private event EventHandler<ChatChangedEventArgs> mChatChanged;

        public event EventHandler<ChatChangedEventArgs> ChatChanged
        {
            add { mChatChanged += value; }
            remove { mChatChanged -= value; }
        }
    }
}