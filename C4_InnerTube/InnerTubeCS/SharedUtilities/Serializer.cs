using System.Xml.Serialization;
using System.IO;

namespace SharedUtilities
{
    public class Serializer<T>
    {
        public void Serialize(T value, string path)
        {
            //Tell the serializer what type we're serializing
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (TextWriter write = new StreamWriter(path))
            {
                xs.Serialize(write, value);
            }          

        }

        public T Deserialize(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            
            using (TextReader read = new StreamReader(path))
            {
                return (T)xs.Deserialize(read);
            }
        }
    }
}
