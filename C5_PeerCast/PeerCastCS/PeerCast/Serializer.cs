using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace PeerCast
{
    public class Serializer
    {
public  static string SerializeFileList()
{            
    List<string> videoList = FileUtility.GetVideoList(); 
   
    MemoryStream contents = new MemoryStream();            
    XmlTextWriter writer = new XmlTextWriter(contents, Encoding.UTF8);
    
    XmlSerializer xs = new XmlSerializer(typeof(List<string>));
    xs.Serialize(writer, videoList);
    
    contents = (MemoryStream)writer.BaseStream;
    UTF8Encoding encoding = new UTF8Encoding();
    
    return encoding.GetString(contents.ToArray());            
}

        public static List<string> DeserializeFileList(string serializedList)
        {
            //deserialize the list of files           
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(serializedList);
            MemoryStream contents = new MemoryStream(byteArray);
            XmlTextWriter writer = new XmlTextWriter(contents, Encoding.UTF8);            
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            
            return (List<string>) xs.Deserialize(contents);            
        }
    }
}
