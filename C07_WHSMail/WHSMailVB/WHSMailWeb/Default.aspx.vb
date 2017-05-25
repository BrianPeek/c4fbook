Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web.UI.WebControls
Imports WHSMailCommon.Entities

Namespace WHSMailWeb
	Public Partial Class _Default
		Inherits BasePage
		Private _folderEntryID As String = String.Empty
		Private _pageNum As Integer = 0

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
			' first time in (tvFolders has viewstate enabled
			If tvFolders.Nodes.Count = 0 Then
				' get teh folder tree
				Dim folderList As List(Of Folder) = Me.WHSMailService.GetFolders()

				' add the root node and expand it
				Dim node As TreeNode = New TreeNode(folderList(0).Name, folderList(0).EntryID)
				node.Expanded = True
				tvFolders.Nodes.Add(node)

				' load up the sub-folders
				LoadNode(folderList(0).Folders, node)

				' get the inbox list and bind it to the grid
				Dim list As List(Of Email) = GetMessages()
				BindMessages(list)

				' save off the default page number and folder ID
				ViewState("PageNum") = _pageNum
				ViewState("FolderID") = _folderEntryID
			End If

			_pageNum = Integer.Parse(ViewState("PageNum").ToString())
			_folderEntryID = ViewState("FolderID").ToString()
		End Sub

		Private Sub LoadNode(ByVal folders As List(Of Folder), ByVal node As TreeNode)
			For Each f As Folder In folders
				' add the node with the format of Folder (Unread/Total)
				Dim subNode As TreeNode = New TreeNode(f.Name & " (" & f.UnreadMessages & "/" & f.TotalMessages & ")", f.EntryID)

				' expand and select the inbox
				subNode.Selected = (f.Name = "Inbox")
				subNode.Expanded = subNode.Selected
				node.ChildNodes.Add(subNode)

				' load the subfolders
				If Not f.Folders Is Nothing Then
					LoadNode(f.Folders, subNode)
				End If
			Next f
		End Sub

		Private Function GetMessages() As List(Of Email)
			' get a group of messages based on the current page number and size
			Return Me.WHSMailService.GetMessages(_folderEntryID, GridView1.PageSize, _pageNum)
		End Function

		Private Sub BindMessages(ByVal list As List(Of Email))
			' load the grid
			GridView1.DataSource = list
			GridView1.DataBind()

			' save off the new page number
			ViewState("PageNum") = _pageNum
		End Sub

		Protected Sub tvFolders_SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs)
			' new folder, so reset the view
			_pageNum = 0
			_folderEntryID = Me.tvFolders.SelectedNode.Value
			ViewState("FolderID") = _folderEntryID

			Dim list As List(Of Email) = GetMessages()
			BindMessages(list)
		End Sub

		Protected Sub btnLink_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
			Dim email As Email = Me.WHSMailService.GetMessage(e.CommandArgument.ToString())

			' fill out the header with some basic info
			lblFrom.Text = email.FromName & "&nbsp;(" & email.From & ")"
			lblSubject.Text = email.Subject
			lblReceived.Text = email.Received.ToString()

			' when a message is selected, write out the content
			msgContent.InnerHtml = email.Body
		End Sub

		Protected Sub btnPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
			' first page
			If _pageNum > 0 Then
				_pageNum -= 1
			End If

			Dim list As List(Of Email) = GetMessages()
			BindMessages(list)
		End Sub

		Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As EventArgs)
			' next page
			_pageNum += 1
			Dim list As List(Of Email) = GetMessages()

			' if we're out of messages, go back to the previous page
			If list Is Nothing OrElse list.Count = 0 Then
				_pageNum -= 1
				list = GetMessages()
			End If
			BindMessages(list)
		End Sub
	End Class
End Namespace
