using System.ComponentModel;

namespace PeerCast
{
    public static class BackgroundWorkerUtility
    {
        public static BackgroundWorker Create()
        {
            BackgroundWorker worker = new BackgroundWorker()            
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
            return worker; 
        }
    }
}
