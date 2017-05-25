using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SharedUtilities;

namespace SharedUtilities
{
    public class InnerTubeFeed : ICloneable
    {
        public string FeedName { get; set; }
        public string FeedUrl { get; set; }
        public DateTime LastUpdated { get; set; }    
        public ObservableCollection<InnerTubeVideo> FeedVideos { get; set; }

        #region ICloneable Members

        public InnerTubeFeed Clone()
        {
            InnerTubeFeed copy = (InnerTubeFeed)((ICloneable)this).Clone();
            ObservableCollection<InnerTubeVideo> feedVideoCopy = new ObservableCollection<InnerTubeVideo>();

            if (this.FeedVideos != null)
            {
                foreach (var v in this.FeedVideos)
                {
                    feedVideoCopy.Add(v.Clone());
                }

                copy.FeedVideos = feedVideoCopy;
                return copy;                                   
            }
            else
            {
                copy.FeedVideos = new ObservableCollection<InnerTubeVideo>();
                return copy;
            }

        }

        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
