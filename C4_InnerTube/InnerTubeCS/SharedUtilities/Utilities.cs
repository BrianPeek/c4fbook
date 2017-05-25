using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;

namespace SharedUtilities
{


    public static class Utilities
    {
        //public static Dictionary<string, string> ParseQueryStringParameters(string url)
        //{
        //    var result = new Dictionary<string, string>();

        //    //convert to Uri to get Query, then get Query substring 
        //    string queryParams = new Uri(url).Query.Substring(1);

        //    var AmpSplit = queryParams.Split('&');

        //    foreach (var s in AmpSplit)
        //    {
        //        if (!string.IsNullOrEmpty(s))
        //        {
        //            var EqualSplit = s.Split('=');

        //            result.Add(EqualSplit[0], EqualSplit[1]);
        //        }
        //    }
        //    return result;
        //}





        public static string[] GetEnumNames(Type source)
        {
            if (source.BaseType == typeof(System.Enum))
            {
                return Enum.GetNames(source);
            }
            else
            {
                throw new ArgumentException("Type passed in is not an enum");
            }

        }

        public static bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();

        }

        public static List<InnerTubeVideo> Flatten(ObservableCollection<InnerTubeFeed> source)
        {
            Dictionary<string, InnerTubeVideo> vids = new Dictionary<string, InnerTubeVideo>();
            foreach (var feed in source)
            {
                foreach (var vid in feed.FeedVideos)
                {
                    if (!vids.ContainsKey(vid.Id))
                    {
                        vids.Add(vid.Id, vid);
                    }
                }
            }
            return vids.Values.ToList();
        }

    }
}
