using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedUtilities
{
    public static class SettingService
    {

        public static string VideoPath { get; set; }
        public static string SubPath;
        public static string SettingPath { get; set; }


        public static Setting Load()
        {
            SetPath();

            if (File.Exists(SettingPath))
            {
                var serial = new Serializer<Setting>();
                return serial.Deserialize(SettingPath);
            }
            else
            {
                return BuildDefaultSettings();
            }
        }

        public static void Save(Setting changes)
        {
            SetPath();
            var serial = new Serializer<Setting>();
            serial.Serialize(changes, SettingPath);
        }

        private static void SetPath()
        {
            if (String.IsNullOrEmpty(VideoPath))
            {
                VideoPath = FileHelper.BuildPath(ref SubPath);
            }

            if (String.IsNullOrEmpty(SubPath))
            {
                SubPath = Path.Combine(SubPath, FileHelper.DefaultSettingsName);
            }

            SettingPath = Path.Combine(SubPath, FileHelper.DefaultSettingsName);
        }

        private static Setting BuildDefaultSettings()
        {
            Setting s = new Setting();
            s.AppName = FileHelper.DefaultAppName;
            s.iTunesInstalled = false;
            s.ZuneInstalled = false;
            s.FirstRun = true;
            s.SubPath = SubPath;
            s.InnerTubeFeedFile = Path.Combine(SubPath, FileHelper.BuildXmlName());
            s.VideoPath = VideoPath;
            s.UpdateFeedPoolThreads = 3;
            s.DownloadPoolThreads = 4;
            s.ConversionPoolThreads = 2;

            return s;
        }

    }
}
