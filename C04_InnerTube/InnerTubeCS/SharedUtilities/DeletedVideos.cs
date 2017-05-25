using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedUtilities
{
    public static class Deleted
    {      

        public static void Delete(System.Collections.ObjectModel.ObservableCollection<InnerTubeFeed> feedList, InnerTubeVideo deleteMe)
        {
            for (int i = 0; i < feedList.Count; i++)
            {
                feedList[i].FeedVideos.Remove(deleteMe);
            }
            
        }

        public static void Delete(System.Collections.ObjectModel.ObservableCollection<InnerTubeFeed> feedList, InnerTubeFeed deleteFeed)
        {
            feedList.Remove(deleteFeed);   
        }
    }
}
