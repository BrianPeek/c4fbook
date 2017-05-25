using System.Collections.Generic;
using System.ServiceModel;
using WHSMailCommon.Contracts;
using WHSMailCommon.Entities;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace WHSMailHost
{
	public class WHSMailService : IWHSMailService
	{
		private readonly Outlook.NameSpace _nameSpace;

		public WHSMailService()
		{
			// get an instance of the MAPI namespace and login
			Outlook.Application app = new Outlook.ApplicationClass();
			_nameSpace = app.GetNamespace("MAPI");
			_nameSpace.Logon(null, null, false, false);
		}

		public List<Folder> GetFolders()
		{
			List<Folder> list = new List<Folder>();

			// get the inbox and then go up one level...that *should* be the root of the default store
			Outlook.MAPIFolder root = (Outlook.MAPIFolder)_nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox).Parent;

			// add root folder
			Folder folder = new Folder(root.EntryID, root.Name, root.UnReadItemCount, root.Items.Count);
			list.Add(folder);

			// Enumerate the sub-folders
			EnumerateFolders(root.Folders, folder);

			return list;
		}

		private void EnumerateFolders(Outlook.Folders folders, Folder rootFolder)
		{
			foreach(Outlook.MAPIFolder f in folders)
			{
				// ensure it's a folder that contains mail messages (i.e. no contacts, appointments, etc.)
				if(f.DefaultItemType == Outlook.OlItemType.olMailItem)
				{
					if(rootFolder.Folders == null)
						rootFolder.Folders = new List<Folder>();

					// add the current folder and enumerate all sub-folders
					Folder subFolder = new Folder(f.EntryID, f.Name, f.UnReadItemCount, f.Items.Count);
					rootFolder.Folders.Add(subFolder);
					if(f.Folders.Count > 0)
						this.EnumerateFolders(f.Folders, subFolder);
				}
			}

			// alphabetize the list (Folder implements IComparable)
			rootFolder.Folders.Sort();
		}

		public List<Email> GetMessages(string entryID, int numPerPage, int pageNum)
		{
			List<Email> list = new List<Email>();

			Outlook.MAPIFolder f;

			// if no ID specified, open the inbox
			if(string.IsNullOrEmpty(entryID))
				f = _nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
			else
				f = _nameSpace.GetFolderFromID(entryID, "");

			// to handle the sorting, one needs to cache their own instance of the items object
			Outlook.Items items = f.Items;

			// sort descending by received time
			items.Sort("[ReceivedTime]", true);

			// pull in the correct number of items based on number of items per page and current page number
			for(int i = (numPerPage*pageNum)+1; i <= (numPerPage*pageNum)+numPerPage && i <= items.Count; i++)
			{
				// ensure it's a mail message
				Outlook.MailItem mi = (items[i] as Outlook.MailItem);
				if(mi != null)
					list.Add(new Email(mi.EntryID, mi.SenderEmailAddress, mi.SenderName, mi.Subject, mi.ReceivedTime, mi.Size));
			}

			return list;
		}

		public Email GetMessage(string entryID)
		{
			// pull the message
			Outlook.MailItem mi = (_nameSpace.GetItemFromID(entryID, "") as Outlook.MailItem);

			if (mi != null)
			{
				string body;

				// if it's a plain format message, wrap it in <pre> tags for nice output
				if(mi.BodyFormat == Outlook.OlBodyFormat.olFormatPlain)
					body = "<pre>" + mi.Body + "</pre>";
				else
					body = mi.HTMLBody;

				return new Email(mi.EntryID, mi.SenderEmailAddress, mi.SenderName, mi.Subject, mi.ReceivedTime, mi.Size, body);
			}
			else
				return null;
		}
	}

	internal class MyServiceHost
	{
		internal static ServiceHost myServiceHost = null;

		internal static void StartService()
		{
			// Instantiate new ServiceHost 
			myServiceHost = new ServiceHost(typeof(WHSMailService));
			myServiceHost.Open();
		}

		internal static void StopService()
		{
			// Call StopService from your shutdown logic (i.e. dispose method)
			if (myServiceHost.State != CommunicationState.Closed)
				myServiceHost.Close();
		}
	}
}

