using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SharedUtilities
{
    public class InnerTubeVideo : ICloneable, IComparable<InnerTubeVideo>
    {      
        public string Id { get; set; }
        public bool IsProtected { get; set; }
        public string Author { get; set; }
        public Collection<string> Categories { get; set; }
        public string ThumbnailLink { get; set; }
        public string Link { get; set; }
        public string EmbedLink { get; set; }
        public string DownloadLink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public int NumRaters { get; set; }
        public float AvgRating { get; set; }
        public int Comments { get; set; }
        public int Duration { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Path { get; set; }

        public DateTime DownloadTime { get; set; }
        public string DownloadedFlv { get; set; }
        public string DownloadedWmv { get; set; }
        public string DownloadedMp4 { get; set; }
        public string DownloadedImage { get; set; }
        public bool IsConverted { get; set; }
        public bool IsDownloaded { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ID: " + this.Id + Environment.NewLine);
            sb.Append("Author: " + this.Author + Environment.NewLine);
            sb.Append("Title: " + this.Title + Environment.NewLine);
            sb.Append("Download Link: " + this.DownloadLink + Environment.NewLine);
            return sb.ToString();
        }


        #region ICloneable Members

        public InnerTubeVideo Clone()
        {
            InnerTubeVideo copy = (InnerTubeVideo)((ICloneable)this).Clone();
            Collection<string> categoryCopy = new Collection<string>();
            foreach (var c in this.Categories)
            {
                categoryCopy.Add((string)c.Clone());
            }
            copy.Categories = categoryCopy;
            return copy;
        }


        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion



        #region IComparable<InnerTubeVideo> Members

        public int CompareTo(InnerTubeVideo other)
        {
            return this.Id.CompareTo(other.Id);
        }

        #endregion
    }
}
