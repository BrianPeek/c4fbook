//////////////////////////////////////////////////////////////////////////////////
//	Sequence.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LightSequencer
{
	public enum MusicType
	{
		Sample = 0,
		MIDI = 1
	}

	public class Sequence
	{
		private const string FILE_VERSION = "2";

		public string Title;
		public string Artist;
		public string MusicFile;
		public int MusicLength;
		public float Interval;	// in ms
		public int Version = int.Parse(FILE_VERSION);
		public List<Channel> Channels = new List<Channel>();
		public MusicType MusicType;

		public Sequence() : this(null)
		{
		}

		public Sequence(string filename)
		{
			if(!string.IsNullOrEmpty(filename))
				Load(filename);
		}

		public void Save(string filename)
		{
			// indent our output file
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;

			// create a new XML file
			XmlWriter xml = XmlWriter.Create(filename, settings);

			// write start tags
			xml.WriteStartDocument();

			// save the appropriate version
			switch(this.Version)
			{
				case 1:
					SaveV1Sequence(xml);	
					break;
				default:
					SaveSequence(xml);
					break;
			}

			// write end tags
			xml.WriteEndDocument();
		
			// close it up
			xml.Flush();
			xml.Close();
		}

		private void SaveV1Sequence(XmlWriter xml)
		{
			StringBuilder sb = new StringBuilder();

			xml.WriteComment("Sequence file for LightSequencer");

			xml.WriteStartElement("sequence");

				// write base config info
				xml.WriteStartElement("config");
					xml.WriteElementString("fileVersion", "1");
					xml.WriteElementString("numChannels", this.Channels.Count.ToString());
					xml.WriteElementString("musicFile", this.MusicFile);
					xml.WriteElementString("musicLength", this.MusicLength.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("channels");

				foreach(Channel channel in this.Channels)
				{
					// 1==on, 0==off...lame, but it works. :)
					foreach(bool b in channel.Data)
						sb.Append((b ? "1" : "0"));

					// write channel specific info
					xml.WriteStartElement("channel");
						xml.WriteAttributeString("number", channel.Number.ToString());
						xml.WriteAttributeString("serialNumber", channel.SerialNumber.ToString());
						xml.WriteAttributeString("outputIndex", channel.OutputIndex.ToString());
						xml.WriteString(sb.ToString());
					xml.WriteEndElement();

					sb.Remove(0, sb.Length);
				}

				xml.WriteEndElement();

			xml.WriteEndElement();
		}

		private void SaveSequence(XmlWriter xml)
		{
			StringBuilder sb = new StringBuilder();

			xml.WriteComment("Sequence file for LightSequencer");

			xml.WriteStartElement("sequence");

				// write base config info
				xml.WriteStartElement("config");
					xml.WriteElementString("fileVersion", FILE_VERSION);
					xml.WriteElementString("musicFile", this.MusicFile);
					xml.WriteElementString("musicLength", this.MusicLength.ToString());
					xml.WriteElementString("numChannels", this.Channels.Count.ToString());
					xml.WriteElementString("interval", this.Interval.ToString());
				xml.WriteEndElement();

				// write music information
				xml.WriteStartElement("musicInfo");
					xml.WriteElementString("title", this.Title);
					xml.WriteElementString("artist", this.Artist);
				xml.WriteEndElement();

				// write channels
				xml.WriteStartElement("channels");

				foreach(Channel channel in this.Channels)
				{
					// 1==on, 0==off...lame, but it works. :)
					foreach(bool b in channel.Data)
						sb.Append((b ? "1" : "0"));

					// write channel specific info
					xml.WriteStartElement("channel");
						xml.WriteAttributeString("number", channel.Number.ToString());
						xml.WriteAttributeString("serialNumber", channel.SerialNumber.ToString());
						xml.WriteAttributeString("outputIndex", channel.OutputIndex.ToString());
						xml.WriteAttributeString("midiChannel", channel.MIDIChannel.ToString());
						xml.WriteString(sb.ToString());
					xml.WriteEndElement();

					sb.Remove(0, sb.Length);
				}
				xml.WriteEndElement();
			xml.WriteEndElement();
		}

		public void Load(string filename)
		{
			// load the XML file
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filename);

			// grab the base config info
			this.Version = int.Parse(xmlDoc.SelectSingleNode("sequence/config/fileVersion").InnerText);
			switch(this.Version)
			{
				case 1:
					LoadV1Sequence(xmlDoc);
					break;
				default:
					LoadSequence(xmlDoc);
					break;
			}
		}

		public void LoadV1Sequence(XmlDocument xmlDoc)
		{
			XmlNode	node;

			this.Interval = 50;

			int numChannels = int.Parse(xmlDoc.SelectSingleNode("sequence/config/numChannels").InnerText);
			this.MusicFile = xmlDoc.SelectSingleNode("sequence/config/musicFile").InnerText;
			this.MusicLength = int.Parse(xmlDoc.SelectSingleNode("sequence/config/musicLength").InnerText);
			this.MusicType = MusicType.Sample;

			this.Channels.Clear();

			// add each channel to our internal list for easy tracking
			for(int i = 0; i < numChannels; i++)
			{
				node = xmlDoc.SelectSingleNode("sequence/channels/channel[@number='" + i.ToString() + "']");
				if(node != null)
				{
					int serial = int.Parse(node.Attributes["serialNumber"].Value);
					char[] data = node.InnerText.ToCharArray();
					this.Channels.Add(new Channel(i, serial, int.Parse(node.Attributes["outputIndex"].Value), data.Length));
					for(int j = 0; j < data.Length; j++)
						this.Channels[i].Data[j] = (data[j] == '1');
				}
				else
					this.Channels.Add(new Channel(i, -1, -1));
			}
		}

		public void LoadSequence(XmlDocument xmlDoc)
		{
			XmlNode	node;

			// grab the base config info
			this.MusicFile = xmlDoc.SelectSingleNode("sequence/config/musicFile").InnerText;

			if(this.MusicFile.EndsWith(".mid"))
				this.MusicType = MusicType.MIDI;

			this.MusicLength = int.Parse(xmlDoc.SelectSingleNode("sequence/config/musicLength").InnerText);
			this.Interval = float.Parse(xmlDoc.SelectSingleNode("sequence/config/interval").InnerText);

			int numChannels = int.Parse(xmlDoc.SelectSingleNode("sequence/config/numChannels").InnerText);

			this.Title = xmlDoc.SelectSingleNode("sequence/musicInfo/title").InnerText;
			this.Artist = xmlDoc.SelectSingleNode("sequence/musicInfo/artist").InnerText;

			this.Channels.Clear();

			// add each channel to our internal list for easy tracking
			for(int i = 0; i < numChannels; i++)
			{
				node = xmlDoc.SelectSingleNode("sequence/channels/channel[@number='" + i.ToString() + "']");
				if(node != null)
				{
					int serial = int.Parse(node.Attributes["serialNumber"].Value);
					char[] data = node.InnerText.ToCharArray();

					// add the channel to the channel list
					this.Channels.Add(new Channel(i, serial, int.Parse(node.Attributes["outputIndex"].Value), int.Parse(node.Attributes["midiChannel"].Value), data.Length));

					// read out the data
					for(int j = 0; j < data.Length; j++)
						this.Channels[i].Data[j] = (data[j] == '1');
				}
				else
					this.Channels.Add(new Channel(i));
			}
		}
	}
}
