using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace SharedUtilities
{

    public enum ZuneMonitoredFolders
    {
        MonitoredAudioFolders,
        MonitoredVideoFolders,
        MonitoredPhotoFolders,
        MonitoredPodcastFolders
    }

    public class ZuneSync : IVideoService
    {
        
        public static string[] GetZuneMonitoredFolders(ZuneMonitoredFolders folder)
        { 
            string hive = @"Software\Microsoft\Zune\Groveler\";
            string[] values = (string[])Registry.CurrentUser.OpenSubKey(hive).GetValue(folder.ToString());
            return values;
        }


        #region VideoService Members
        public void Sync(string filePath)
        {
            string[] currentFolders = ZuneSync.GetZuneMonitoredFolders(ZuneMonitoredFolders.MonitoredVideoFolders);

            bool found = currentFolders.Contains(filePath);
            //check if we are already added the files to the folder
            if (!found)
            {
                //copy the files to the first specified directory   
                if (currentFolders.Length > 0)
                {
                    string destinationPath = currentFolders[0];
                    string[] Files = Directory.GetFiles(filePath, "*.wmv", SearchOption.TopDirectoryOnly);
                    foreach (var f in Files)
                    {
                        File.Copy(f, destinationPath, true);
                    }

                }
                else
                {
                    throw new ArgumentException("Zune is not configured to monitor *any* " +
                    "folders, to fix this, open zune.exe, click settings, and add a video folder");
                }                                
            }



        }


        #endregion

    }


}



