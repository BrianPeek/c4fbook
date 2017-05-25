using System;
using System.IO;

namespace C4F.VistaP2P.Common
{  
    

    public class PictureHelper
    {

        public static readonly int MAX_IMAGE_SIZE = 70000000;///The maximium size in bytes of a picture.
        ///
        //this delegate is used to call the appropriate Operation in the Service Contract.
        public delegate void OperationContractEventHandler(PicturePacket packet);
        OperationContractEventHandler mOperationContract;

        public OperationContractEventHandler OperationContract
        {
            get { return mOperationContract; }
            set { mOperationContract = value; }
        }
        String mNodeName;
        ///////////////////////////////////////////////////////////////////////////////////////
        //the event for listening to pictures
        ///////////////////////////////////////////////////////////////////////////////////////        
        private event EventHandler<PictureChangedEventArgs> mPictureChanged;

        public event EventHandler<PictureChangedEventArgs> PictureChanged
        {
            add { mPictureChanged += value; }
            remove { mPictureChanged -= value; }
        }

        
        public PictureHelper(OperationContractEventHandler op, string nodeName)
        {
            mNodeName = nodeName;
            mOperationContract = op;
        }

        /// <summary>
        /// The function to call when sending a picture
        /// </summary>
        /// <param name="cp">fill the packet with the appropriate information and write the image to the stream</param>
        public void SendPicture()
        {
            Microsoft.Win32.OpenFileDialog ofd;
            ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "openFileDialog1";
            ofd.Filter = "(*.JPG, *.GIF, *.PNG)|*.jpg;*.gif;*.png|All Files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ofd.RestoreDirectory = true;
            ofd.Title = "Select a Picture";


            //check return value, but how in wpf?
            bool ok = (bool)ofd.ShowDialog();

            if (ok)
            {
                System.IO.Stream stream;
                if ((stream = ofd.OpenFile()) != null)
                {
                    PicturePacket packet = new PicturePacket();
                    packet.s = stream;
                    packet.fileName = System.IO.Path.GetFileName(ofd.FileName);
                    MetadataFileInfo md = new MetadataFileInfo();
                    md.ReadMetaData(ofd.FileName);
                    packet.title = md.Title;
                    packet.senderNodeName = mNodeName;
                    mOperationContract(packet);
                }
            }
        }

        /// <summary>
        /// Make a call to this in order to send a picture.
        /// </summary>
        /// <param name="fullFileName"></param>
        public void SendPicture(String fullFileName)
        {
            if (fullFileName == null)
            {  //send an empty picture...
                PicturePacket packet = new PicturePacket();
                packet.s = new MemoryStream();
                packet.senderNodeName = mNodeName;
                mOperationContract(packet);
                return;
            }

            System.IO.Stream stream;
            if ((stream = System.IO.File.OpenRead(fullFileName)) != null)
            {
                PicturePacket packet = new PicturePacket();
                packet.s = stream;
                packet.fileName = System.IO.Path.GetFileName(fullFileName);
                MetadataFileInfo md = new MetadataFileInfo();
                md.ReadMetaData(fullFileName);
                packet.title = md.Title;
                packet.senderNodeName = mNodeName;
                mOperationContract(packet);
            }
        }

        public void SendPicture(Stream stream, String title)
        {
            PicturePacket packet = new PicturePacket();
            packet.s = stream;
            packet.title = title;            
            packet.senderNodeName = mNodeName;
            mOperationContract(packet);
        }

        public void SendPicture(PicturePacket pp)
        {
            if (pp != null)
            {
                mOperationContract(pp);
            }
        }

        /// <summary>
        /// Implements the Share Picture interface
        /// </summary>
        /// <param name="member"></param>
        public void SharePicture(PicturePacket cp)
        {
            //look to see if anyone is listening, if so perform the callback.
            if (mPictureChanged != null)
            {                
                mPictureChanged(this, new PictureChangedEventArgs(cp));
            }
        }     
    }
}