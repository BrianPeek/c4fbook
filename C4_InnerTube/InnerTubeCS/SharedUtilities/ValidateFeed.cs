using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace SharedUtilities
{
    public static class ValidateFeed
    {
        public static ObservableCollection<InnerTubeFeed> UpdateLinks(ObservableCollection<InnerTubeFeed> feeds, Setting settings)
        {
            for (int i = 0; i < feeds.Count; i++)
            {
                for (int j = 0; j < feeds[i].FeedVideos.Count; j++)
                {
                    //Update download                    
                    if (File.Exists(feeds[i].FeedVideos[j].DownloadedFlv))
                    {                        
                        feeds[i].FeedVideos[j].IsDownloaded = true;
                    }

                    //Update Converted Videos
                    if (settings.iTunesInstalled)
                    {
                        if (File.Exists(feeds[i].FeedVideos[j].DownloadedWmv) && 
                            File.Exists(feeds[i].FeedVideos[j].DownloadedMp4))
                        {
                            feeds[i].FeedVideos[j].IsConverted = true; 
                        }

                    }
                    else
                    {
                        if (File.Exists(feeds[i].FeedVideos[j].DownloadedWmv))
                        {
                            feeds[i].FeedVideos[j].IsConverted = true; 
                        }
                    }
                }
            }

            return feeds;
        
        }

    }
}
