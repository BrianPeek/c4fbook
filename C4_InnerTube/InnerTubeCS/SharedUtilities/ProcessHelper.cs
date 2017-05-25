using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace SharedUtilities
{
    public enum ProcessType
    {
        iTunes,
        Zune
    }
    
    public static class ProcessHelper
    {
        private static string iTunesExe = "iTunesHelper";
        private static string ZuneExe = "ZuneLauncher";


        public static bool IsProcessRunning(string name)
        {
            var x = Process.GetProcesses();
            var result = from p in x
                         where p.ProcessName == name
                         select p;

            if (result.Count() > 0)
            {
                return true;
            }
            {
                return false;
            }
        }

        public static bool IsProcessRunning(ProcessType process)
        {
            switch (process)
            {
                case ProcessType.iTunes:
                    return IsProcessRunning(iTunesExe);
                case ProcessType.Zune:
                    return IsProcessRunning(ZuneExe);    
                default:
                    return false;
                    
            }
        }
        
    }
}
