using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PeerCast
{
    public static class FileUtility
    {
        public static List<string> GetVideoList()
        {
            List<string> videos = Directory.GetFiles(Properties.Settings.Default.FileDirectory, 
                                        "*.wmv", SearchOption.TopDirectoryOnly).ToList();
            return videos; 
        }
    }
}
