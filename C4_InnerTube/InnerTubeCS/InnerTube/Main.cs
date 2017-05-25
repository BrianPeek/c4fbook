using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace InnerTube
{
    public static class Startup
    {
        [System.STAThread]
        public static void Main()
        {
            bool mutexIsNew;

            using (System.Threading.Mutex m = new System.Threading.Mutex(true, "InnerTube", out mutexIsNew))
            {
                if (mutexIsNew)
                {
                    InnerTube.App app = new InnerTube.App();
                    app.InitializeComponent();
                    app.Run();           
                }
                else
                {
                    MessageBox.Show("InnerTube is already running");
                }
            }



        }
    }
}
