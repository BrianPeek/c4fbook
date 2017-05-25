using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using WHSMailCommon.Entities;

namespace WHSMailWeb
{
	public partial class _Default : BasePage
	{
		private string _folderEntryID = string.Empty;
		private int _pageNum = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			// first time in (tvFolders has viewstate enabled
			if(tvFolders.Nodes.Count == 0)
			{
				// get teh folder tree
				List<Folder> folderList = this.WHSMailService.GetFolders();

				// add the root node and expand it
				TreeNode node = new TreeNode(folderList[0].Name, folderList[0].EntryID);
				node.Expanded = true;
				tvFolders.Nodes.Add(node);

				// load up the sub-folders
				LoadNode(folderList[0].Folders, node);

				// get the inbox list and bind it to the grid
				List<Email> list = GetMessages();
				BindMessages(list);

				// save off the default page number and folder ID
				ViewState["PageNum"] = _pageNum;
				ViewState["FolderID"] = _folderEntryID;
			}

			_pageNum = int.Parse(ViewState["PageNum"].ToString());
			_folderEntryID = ViewState["FolderID"].ToString();
		}

		private void LoadNode(List<Folder> folders, TreeNode node)
		{
			foreach(Folder f in folders)
			{
				// add the node with the format of Folder (Unread/Total)
				TreeNode subNode = new TreeNode(f.Name + " (" + f.UnreadMessages + "/" + f.TotalMessages + ")", f.EntryID);

				// expand and select the inbox
				subNode.Expanded = subNode.Selected = (f.Name == "Inbox");
				node.ChildNodes.Add(subNode);

				// load the subfolders
				if(f.Folders != null)
					LoadNode(f.Folders, subNode);
			}
		}

		private List<Email> GetMessages()
		{
			// get a group of messages based on the current page number and size
			return this.WHSMailService.GetMessages(_folderEntryID, GridView1.PageSize, _pageNum);
		}

		private void BindMessages(List<Email> list)
		{
			// load the grid
			GridView1.DataSource = list;
			GridView1.DataBind();

			// save off the new page number
			ViewState["PageNum"] = _pageNum;
		}

		protected void tvFolders_SelectedNodeChanged(object sender, EventArgs e)
		{
			// new folder, so reset the view
			_pageNum = 0;
			_folderEntryID = this.tvFolders.SelectedNode.Value;
			ViewState["FolderID"] = _folderEntryID;

			List<Email> list = GetMessages();
			BindMessages(list);
		}

		protected void btnLink_Command(object sender, CommandEventArgs e)
		{
			Email email = this.WHSMailService.GetMessage(e.CommandArgument.ToString());

			// fill out the header with some basic info
			lblFrom.Text = email.FromName + "&nbsp;(" + email.From + ")";
			lblSubject.Text = email.Subject;
			lblReceived.Text = email.Received.ToString();

			// when a message is selected, write out the content
			msgContent.InnerHtml = email.Body;
		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
			// first page
			if(_pageNum > 0)
				_pageNum--;

			List<Email> list = GetMessages();
			BindMessages(list);
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			// next page
			_pageNum++;
			List<Email> list = GetMessages();

			// if we're out of messages, go back to the previous page
			if(list == null || list.Count == 0)
			{
				_pageNum--;
				list = GetMessages();
			}
			BindMessages(list);
		}
	}
}
