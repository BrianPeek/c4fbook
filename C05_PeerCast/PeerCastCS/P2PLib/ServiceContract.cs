using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace C4F.VistaP2P.Common
{
    //Service contract..This defines what operations can be performed on the service.
    //I think of it as what function calls do I want to be able to make on the other peers.
    //
    //Many of the operations have a parameter of Packet such as PicturePacket. I wanted to be
    //able to send a Stream as a parameter.  The OperationContract requires that the stream be the only parameter,
    //and thus SharePicture(Stream data, string fileName) is invalid.  The way around this is to specify 
    //a MessageContract that contains the parameters, including the stream.  
    //
    //Note: All PeerChannel Operation contracts must be OneWay.
    [ServiceContract(CallbackContract = typeof(IPeerChannel))]
    public interface IPeerChannel
    {
        //A node should call this when it joins a network. We do this so we can put the name of the 
        //node that just joined in the message box.
        [OperationContract(IsOneWay = true)]
        void Join(string member);

        //We call this function on the way out so other nodes know if we left 
        //the network gracefully.
        [OperationContract(IsOneWay = true)]
        void Leave(string member);

        //The node name of the person sending the text message, 
        //and then the string containing the message itself 
        [OperationContract(IsOneWay = true)]
        void ShareTextMessage(string member, string msg);

        //Used for sending a picture.
        [OperationContract(IsOneWay = true)]
        void SharePicture(PicturePacket packet);

        //used for sending a chunk of an audio packet.
        //Note: there is no real difference between the Audio and Video implementations 
        //at this level.  They only become apparent when displaying them using the MainWindow.xaml class
        [OperationContract(IsOneWay = true)]
        void ShareStreamedAudio(StreamedPacket packet);

        //used for sending a chunk of a video packet.         
        [OperationContract(IsOneWay = true)]
        void ShareStreamedVideo(StreamedPacket packet);

        //used for sharing a file.
        [OperationContract(IsOneWay = true)]
        void ShareFile(Packet packet);
    }

    /// <summary>
    /// The message contract is here in order to pass a stream in addition to other data.
    /// By default the service contract requires that a Stream be the only parameter in an OperationsContract
    /// </summary>
    [MessageContract]
    public class Packet :IDisposable
    {
        //The node that is sending the packet.
        [MessageHeader]
        public String senderNodeName;

        //the packetNumber, starting at 0. When streaming data the packets are enumerated so we can re-order them if necessary on the 
        //receiving side.
        [MessageHeader]
        public int packetNumber;

        //set to true when this is the final packet in a stream of data such as an audio or video file.
        [MessageHeader]
        public bool endOfStream;

        //The reason we include a guid with each packet can most easily be explained with the following sceneario:
        //Sender starts sending TomPetty.wma ..5 seconds into it they realize they were sending the acustic version, so they cancel the stream
        //and immediately start sending the desired file TomPetty.wma from a different folder. The receiving node has a difficult time determining
        //if the audio was just delayed, or what is going on.  The easiest way to clarify this (at the expense of a little bandwidth to carry the guid)
        //is to include it for each audio or video transfer.  In the above scenario, the second transfer will have a different guid so the receiving
        //node(s) can reset and start playing the second tune.
        [MessageHeader]
        public Guid guid;

        //the filename of the file being sent.
        [MessageHeader]
        public String fileName;

        //The actual stream itself...
        [MessageBodyMember]
        public Stream stream;

        public Packet()
        {
            stream = new MemoryStream();
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
            }
        }
    }

    //Message contract for sending a ouctyre  
    [MessageContract]
    public class PicturePacket
    {
        [MessageBodyMember]
        public Stream s;

        [MessageHeader]
        public String fileName;

        [MessageHeader]
        public String title;

        [MessageHeader]
        public String senderNodeName;
    }


    //Message contract for sending streaming data
    [MessageContract]
    public class StreamedPacket : Packet
    {
        [MessageHeader]
        public String title; //title of the song

        [MessageHeader]
        public String album;

        [MessageHeader]
        public String artist;
    }

   
    //combine IClientChannel and IPeerChannel into one interface
    public interface IPeerChannelWrapper : IPeerChannel, IClientChannel
    {
    }


}