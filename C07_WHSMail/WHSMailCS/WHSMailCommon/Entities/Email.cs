using System;

namespace WHSMailCommon.Entities
{
	// entity object representing an Email
	[Serializable]
	public class Email
	{
		public Email(string entryID, string from, string fromName, string subject, DateTime received, int size)
		{
			EntryID = entryID;
			From = from;
			FromName = fromName;
			Subject = string.IsNullOrEmpty(subject) ? "(no subject)" : subject;
			Received = received;
			Size = size;
		}

		public Email(string entryID, string from, string fromName, string subject, DateTime received, int size, string body) :
			this(entryID, from, fromName, subject, received, size)
		{
			Body = body;
		}

		// MAPI unique ID
		public string EntryID { get; set; }

		// email address of sender
		public string From { get; set; }

		// name of sender
		public string FromName { get; set; }

		public string Subject { get; set; }
		public string Body { get; set; }
		public DateTime Received { get; set; }
		public int Size { get; set; }
	}
}
