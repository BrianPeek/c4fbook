using System;
using System.Collections.Generic;

namespace WHSMailCommon.Entities
{
	// entity object representing an Folder
	[Serializable]
	public class Folder : IComparable
	{
		public Folder(string entryID, string name, int unreadMessages, int totalMessages)
		{
			EntryID = entryID;
			Name = name;
			UnreadMessages = unreadMessages;
			TotalMessages = totalMessages;
		}

		// MAPI unique identifier
		public string EntryID { get; set; }

		// subfolders of this folder
		public List<Folder> Folders { get; set; }
		public string Name { get; set; }
		public int UnreadMessages { get; set; }
		public int TotalMessages { get; set; }

		// used so we can sort the folders alphabetically later on
		public int CompareTo(object obj)
		{
			return string.Compare(this.Name, ((Folder)obj).Name);
		}
	}
}
