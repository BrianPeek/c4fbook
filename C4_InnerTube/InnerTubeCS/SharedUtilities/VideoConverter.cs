using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharedUtilities;
using System.Net;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace SharedUtilities
{
    public enum ConversionType
    {
        Mp4,
        Wmv
    }

    public static class VideoConverter
    {
        public static void ConvertFlv(string sourceFile, string destinationFile, ConversionType conversion)
        {

            string cmdLineArgs = String.Empty;

            //build command line for ffmpeg
            switch (conversion)
            {
                case ConversionType.Mp4:
                    cmdLineArgs = String.Format(" -i  \"{0}\" \"{1}\"", sourceFile, destinationFile);
                    break;
                case ConversionType.Wmv:
                    cmdLineArgs = String.Format(" -i  \"{0}\" -vcodec wmv2 \"{1}\"", sourceFile, destinationFile);
                    break;
            }

            ConvertFlv(sourceFile, destinationFile, cmdLineArgs);
        }

        public static void ConvertFlv(InnerTubeVideo source, ConversionType conversion)
        {

            string title = FileHelper.ReplaceIllegalCharacters(source.Title);
            string author = FileHelper.ReplaceIllegalCharacters(source.Author);
            string description = FileHelper.ReplaceIllegalCharacters(source.Description);

            //set values based on switch
            string cmdLineArgs = String.Empty;
            string destination = String.Empty; 

            switch (conversion)
            {

                case ConversionType.Mp4:
                    //ffmpeg.exe -title "Chocolate Rain" -author "TayZonday" -comment "Original Song by Tay Zonday" -i "Chocolate Rain.flv" "Chocolate Rain.mp4"
                    destination = source.DownloadedMp4;
                    cmdLineArgs = String.Format(" -title \"{0}\" -author \"{1}\" -comment \"{2}\" -i  \"{3}\" \"{4}\"", 
                                title, author, description, source.DownloadedFlv, destination);
                    break;
                case ConversionType.Wmv:
                    //ffmpeg.exe -title "Chocolate Rain" -author "TayZonday" -comment "Original Song by Tay Zonday" -i "Chocolate Rain.flv" -vcodec wmv2 "Chocolate Rain.wmv"                    
                    destination = source.DownloadedWmv;
                    cmdLineArgs = String.Format(" -title \"{0}\" -author \"{1}\" -comment \"{2}\" -i  \"{3}\" -vcodec wmv2 \"{4}\"", 
                        title, author, description, source.DownloadedFlv, destination);
                    break;
            }
            ConvertFlv(source.DownloadedFlv, destination, cmdLineArgs);
        }

        private static void ConvertFlv(string sourceFile, string destination, string cmdLineArgs)
        {
            //point to ffmpeg conversion tool
            string exePath = Path.Combine(Environment.CurrentDirectory, @"ffmpeg\ffmpeg.exe");

            //ensure sourceFile files exist and the destination doesn't
            if (File.Exists(sourceFile) && File.Exists(exePath) && !File.Exists(destination))
            {

                //Start a Process externally as we're converting from the command line
                using (Process convert = new Process())
                {
                    //Set properties
                    convert.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    convert.StartInfo.CreateNoWindow = true;
                    convert.StartInfo.RedirectStandardOutput = true;
                    convert.StartInfo.UseShellExecute = false;
                    convert.StartInfo.Arguments = cmdLineArgs;
                    convert.StartInfo.FileName = exePath;

                    convert.Start();
                    convert.WaitForExit();
                }
            }
        }
    }
}
