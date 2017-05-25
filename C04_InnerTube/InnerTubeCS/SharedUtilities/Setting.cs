using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedUtilities
{
    public class Setting
    {
        public string AppName { get; set; }
        public string VideoPath { get; set; }
        public string SubPath { get; set; }
        public string InnerTubeFeedFile { get; set; }
        public bool FirstRun { get; set; }
        public bool ZuneInstalled { get; set; }
        public bool iTunesInstalled { get; set; }
        public int UpdateFeedPoolThreads { get; set; }
        public int DownloadPoolThreads { get; set; }
        public int ConversionPoolThreads { get; set; }
    }
}
