using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedUtilities
{
    public enum FileType
    {
        Image,
        Flv,
        Mp4,
        Wmv
    }

    public static class FileHelper
    {
        public static string DefaultAppName = "InnerTube";
        public static string DefaultSettingsName = "settings.xml";
        public static string DefaultImage = "images/youtube.jpg";
        

        public static string BuildXmlName(string appName)
        {
            return appName + ".xml";
        }
        public static string BuildXmlName()
        {
            return DefaultAppName + ".xml";
        }


        public static string WriteLogFile(string basePath, string contents)
        {
            string time = ReplaceIllegalCharacters(DateTime.Now.ToString(), "_");
            string fileName = String.Format("UpdateLog_{0}.txt", time);
            string fullPath = Path.Combine(basePath, fileName);

            File.WriteAllText(fullPath, contents);
            return fullPath;    
        }

        public static string BuildFileName(string filePath, string fileName, FileType type)
        {
            switch (type)
            {
                case FileType.Image:
                    return Path.Combine(filePath, String.Format("{0}.jpg",fileName));
                case FileType.Flv:
                    return Path.Combine(filePath, String.Format("{0}.flv", FileHelper.ReplaceIllegalCharacters(fileName)));
                case FileType.Mp4:
                    return Path.Combine(filePath, String.Format("{0}.mp4", FileHelper.ReplaceIllegalCharacters(fileName)));
                case FileType.Wmv:
                    return Path.Combine(filePath, String.Format("{0}.wmv", FileHelper.ReplaceIllegalCharacters(fileName)));
                default:
                    return String.Empty;
                    
            }
        }

        public static double FileSize(string file)
        {
            FileInfo f = new FileInfo(file);
            double size = f.Length / 1024;
            return size;
        }

        public static string ReplaceIllegalCharacters(string source, string replacement)
        {

            if (!String.IsNullOrEmpty(replacement))
            {
                //make sure replacement doesn't have an illegal character
                replacement = ReplaceIllegalCharacters(replacement, String.Empty);
            }
            else
            {
                replacement = String.Empty;
            }

            //Using LINQ to replace characters for fun
            var query = from n in new[] { source }

                        //Replace illegal characters in DestinationFile name
                        // The following are illegal: \ / : * ? < > | " 
                        select n.Replace(@"\", replacement)
                                .Replace(@"/", replacement)
                                .Replace(":", replacement)
                                .Replace("*", replacement)
                                .Replace("?", replacement)
                                .Replace("<", replacement)
                                .Replace(">", replacement)
                                .Replace("|", replacement)
                                .Replace("\"", replacement) // quote char is escaped as \"
                                .Replace("-", replacement);

            return query.First();
        
        }

        public static string ReplaceIllegalCharacters(string source)
        {
            return ReplaceIllegalCharacters(source, String.Empty);
        }

        public static string BuildPath(ref string subDir)
        {
            //Environment.SpecialFolder doesn't have a folder for "Videos", 
            // but assume it does
            string newpath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            OperatingSystem os = Environment.OSVersion;

            if (os.Version.Major >= 6) //Vista or higher
            {
                newpath = newpath.Replace("Music", "Videos");
            }
            else if (os.Version.Major == 5) //XP
            {
                newpath = newpath.Replace("My Music", "My Videos");
            }
            else
            {
                throw new NotSupportedException
                    (String.Format("The Operating System Version is not supported:{0}", Environment.OSVersion));                    
            }

            //Ensure the directory exists, if it doesn't, build it
            if (!Directory.Exists(newpath))
            {
                Directory.CreateDirectory(newpath);
            }

            if (String.IsNullOrEmpty(subDir))
            {
                subDir = Path.Combine(newpath, FileHelper.DefaultAppName);
            }
            else
            {
                subDir = Path.Combine(newpath, subDir);
            }           

            if (!Directory.Exists(subDir))
            {
                Directory.CreateDirectory(subDir);
            }               

            return newpath;                
        }

        public static bool ConvertedFilesExist(InnerTubeVideo video, Setting settings)
        {
            //iTunes needs WMV & MP4
            if (settings.iTunesInstalled)
            {
                if (File.Exists(video.DownloadedWmv) && File.Exists(video.DownloadedMp4))
                {
                    return true;
                }
                else
                {
                    return false; 
                }
            }
            else
            {
                if (File.Exists(video.DownloadedWmv))
                {
                    return true;
                }
                else
                {
                    return false; 
                }
            }
        }

    }
}
