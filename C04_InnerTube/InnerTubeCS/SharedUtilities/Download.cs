using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using SharedUtilities;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Web;
using System.Collections.Specialized;


namespace SharedUtilities
{
    public static class Download
    {

        public static void DownloadVideo(InnerTubeVideo source, string destination)
        {
            if (!File.Exists(destination))
            {
                UriBuilder final = new UriBuilder(source.DownloadLink);
                final.Query = "video_id=" + source.Id + "&" + CreateTokenRequest(source);

                WebRequest request = WebRequest.Create(final.ToString());
                request.Timeout = 500000;

                try
                {
                    WebResponse response = request.GetResponse();

                    using (Stream webStream = response.GetResponseStream())
                    {
                        try
                        {
                            int _bufferSize = 65536;

                            using (FileStream fs = File.Create(destination, _bufferSize))
                            {
                                int readBytes = -1;
                                byte[] inBuffer = new byte[_bufferSize];

                                //Loop until we hit the end
                                while (readBytes != 0)
                                {
                                    //read data from web into filebuffer, then write to file
                                    readBytes = webStream.Read(inBuffer, 0, _bufferSize);
                                    fs.Write(inBuffer, 0, readBytes);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error in Buffer Download");
                            Debug.Indent();
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in request.GetResponse()");
                    Debug.Indent();
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private static string CreateTokenRequest(InnerTubeVideo video)
        {
            //YouTube variables
            const string jsVariable = "swfArgs";
            const string argName = "t";

            //get raw html from YouTube video page
            string rawHtml;
            using (WebClient wc = new WebClient())
            {
                rawHtml = wc.DownloadString(video.Link);
            }

            //extract the JavaScript name/value pairs
            int jsIndex = rawHtml.IndexOf(jsVariable);
            int startIndex = rawHtml.IndexOf("{", jsIndex);
            int endIndex = rawHtml.IndexOf("}", startIndex);
            string fullString = rawHtml.Substring(startIndex + 1, endIndex - startIndex - 1);

            //remove all quotes (") 
            fullString = fullString.Replace("\"", "");

            //split all values
            string[] allArgs = fullString.Split(',');

            //loop through javascript parameters
            foreach (string swfArg in allArgs)
            {
                if (swfArg.Trim().StartsWith(argName))
                {
                    var nameValuePair = swfArg.Split(':');
                    return string.Format("{0}={1}", argName, nameValuePair[1].Trim());
                }
            }

            throw new Exception(string.Format("token not found in swfArgs, make sure {0} is accessible", video.Link));
        }

        public static void DownloadVideo(string watchUrl, string destination)
        {
            //Get links for video, then download
            ObservableCollection<InnerTubeVideo> Videos = InnerTubeService.GetSingleVideo(watchUrl);
            DownloadVideo(Videos[0], destination);

        }

        public static void DownloadImage(string watchUrl, string destination)
        {

            ObservableCollection<InnerTubeVideo> Videos = InnerTubeService.GetSingleVideo(watchUrl);
            DownloadImage(Videos[0], destination);
        }

        public static void DownloadImage(InnerTubeVideo source)
        {
            DownloadImage(source, source.DownloadedImage);
        }

        public static void DownloadImage(InnerTubeVideo source, string destination)
        {
            //if we haven't downloaded the image yet, download it
            if (!File.Exists(destination))
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(source.ThumbnailLink), destination);
                }
            }
        }

        public static void DownloadVideo(InnerTubeVideo source)
        {
            DownloadVideo(source, source.DownloadedFlv);
        }

    }
}

