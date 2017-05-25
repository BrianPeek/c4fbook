using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTunesLib;
using System.IO;

namespace SharedUtilities
{
    public class iTunesSync : IVideoService
    {
        iTunesApp iTunes = new iTunesApp();


        #region IVideoService Members

        public void Sync(string filePath)
        {
            //only get MP4 files
            string[] fileList = Directory.GetFiles(filePath, "*.mp4",SearchOption.TopDirectoryOnly);
            try
            {
                foreach (var f in fileList)
                {
                    //Add file                    
                    iTunes.LibraryPlaylist.AddFile(f);

                    

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("iTunes error: " + ex.Message); 
            }
            finally
            {
                this.iTunes = null; 
            }            
        }

        #endregion

        public void UpdateIPod()
        {
            iTunes.UpdateIPod();
        }


        ~iTunesSync()
        {
            if (this.iTunes != null)
            {
                //cleanup
                this.iTunes = null; 
            }
            

        }
    }
}
