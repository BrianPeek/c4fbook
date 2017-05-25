namespace PeerCast
{   
    // Message types that the client will receive
    public enum UiMessage
    {
        StreamVideo,
        UpdateStatus,
        ReceivedVideoList,
        ToggleUi,
        Log
    }

    // Message types that are sent
    public enum ChatCommand
    {
        GetList,
        ReturnList,
        StreamVideo
    }

}