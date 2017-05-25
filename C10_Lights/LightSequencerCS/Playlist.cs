//////////////////////////////////////////////////////////////////////////////////
//	Playlist.cs
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
	public class Playlist
	{
		private const string FILE_VERSION = "1";

		public int Version = int.Parse(FILE_VERSION);
		public List<string> Filenames = new List<string>();

		public Playlist()
		{
		}

		public Playlist(string filename)
		{
			Load(filename);
		}

		public void Save(string filename)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;

			XmlWriter xml = XmlWriter.Create(filename, settings);

			xml.WriteStartDocument();

			switch(this.Version)
			{
				default:
					SavePlaylist(xml);
					break;
			}

			xml.WriteEndDocument();
		
			xml.Flush();
			xml.Close();
		}

		private void SavePlaylist(XmlWriter xml)
		{
			StringBuilder sb = new StringBuilder();

			xml.WriteComment("Playlist file for LightSequencer");

			xml.WriteStartElement("Playlist");

				// write base config info
				xml.WriteStartElement("config");
					xml.WriteElementString("fileVersion", FILE_VERSION);
					xml.WriteElementString("numSequences", this.Filenames.Count.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("sequences");

				foreach(string file in this.Filenames)
				{
					// write channel specific info
					xml.WriteStartElement("sequence");
						xml.WriteElementString("fileName", file);
					xml.WriteEndElement();
				}
				xml.WriteEndElement();
			xml.WriteEndElement();
		}

		public void Load(string filename)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filename);

			// grab the base config info
			this.Version = int.Parse(xmlDoc.SelectSingleNode("Playlist/config/fileVersion").InnerText);
			switch(this.Version)
			{
				default:
					LoadPlaylist(xmlDoc);
					break;
			}
		}

		public void LoadPlaylist(XmlDocument xmlDoc)
		{
			XmlNodeList nodes = xmlDoc.SelectNodes("Playlist/sequences/sequence");
			foreach(XmlNode node in nodes)
				this.Filenames.Add(node["fileName"].InnerText);
		}
	}
}
