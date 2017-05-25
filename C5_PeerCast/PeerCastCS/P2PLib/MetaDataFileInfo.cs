using System;
using System.IO;

namespace C4F.VistaP2P.Common
{

    /// <summary>
    /// The purpose of this class is to retreive meta data associated with the file.  It does this primarily through a call to 
    /// Shell32.Folder.GetFolderInfo.
    /// </summary>
    public class MetadataFileInfo
    {
        private string mFileName;
        private string mArtistName;
        private string mAlbumName;
        private int mTrackNumber;
        private string mTitle;
        private string mFrameRate;

        public string FileName
        {
            get { return mFileName; }
            set { mFileName = value; }
        }
        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }
        public string AlbumName
        {
            get { return mAlbumName; }
            set { mAlbumName = value; }
        }
        public string ArtistName
        {
            get { return mArtistName; }
            set { mArtistName = value; }
        }

        public int TrackNumber
        {
            get { return mTrackNumber; }
            set { mTrackNumber = value; }
        }

        public String FrameRate
        {
            get { return mFrameRate; }
            set { mFrameRate = value; }
        }

        //actually read the meta data and fill in the appropriate strings.
        public void ReadMetaData(String fullFilePath)
        {
            
            Shell32.Shell shell = new Shell32.ShellClass();
            Shell32.Folder folder = shell.NameSpace(Path.GetDirectoryName(fullFilePath));
            Shell32.FolderItem folderItem = folder.ParseName(Path.GetFileName(fullFilePath));

            if (folderItem != null)
            {
                FileName = Path.GetFileName(fullFilePath);
                AlbumName = folder.GetDetailsOf(folderItem, (int)FileInfo.Album);
                ArtistName = folder.GetDetailsOf(folderItem, (int)FileInfo.Artists);
                Title = folder.GetDetailsOf(folderItem, (int)FileInfo.Title);
                FrameRate = folder.GetDetailsOf(folderItem, (int)FileInfo.Frame_rate);
            }
            folderItem = null;
            folder = null;
            shell = null;
        }
    }
}