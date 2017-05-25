namespace PeerCast
{
    public class ThreadMessage
    {
        public UiMessage MessageType { get; set; }
        public string Message { get; set; }

        public static ThreadMessage Create(UiMessage messageType, string message)
        {
            return new ThreadMessage() { MessageType = messageType, Message = message }; 
        }
    }
}
