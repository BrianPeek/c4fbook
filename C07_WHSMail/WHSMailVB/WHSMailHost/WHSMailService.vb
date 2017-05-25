Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.ServiceModel
Imports WHSMailCommon.Contracts
Imports WHSMailCommon.Entities
Imports Outlook = Microsoft.Office.Interop.Outlook

Namespace WHSMailHost
	Public Class WHSMailService
		Implements IWHSMailService
		Private ReadOnly _nameSpace As Outlook.NameSpace = Nothing

		Public Sub New()
			' get an instance of the MAPI namespace and login
			Dim app As Outlook.Application = New Outlook.ApplicationClass()
			_nameSpace = app.GetNamespace("MAPI")
			_nameSpace.Logon(Nothing, Nothing, False, False)
		End Sub

		Public Function GetFolders() As List(Of Folder) Implements IWHSMailService.GetFolders
			Dim list As List(Of Folder) = New List(Of Folder)()

			' get the inbox and then go up one level...that *should* be the root of the default store
			Dim root As Outlook.MAPIFolder = CType(_nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox).Parent, Outlook.MAPIFolder)

			' add root folder
			Dim folder As Folder = New Folder(root.EntryID, root.Name, root.UnReadItemCount, root.Items.Count)
			list.Add(folder)

			' Enumerate the sub-folders
			EnumerateFolders(root.Folders, folder)

			Return list
		End Function

		Private Sub EnumerateFolders(ByVal folders As Outlook.Folders, ByVal rootFolder As Folder)
			For Each f As Outlook.MAPIFolder In folders
				' ensure it's a folder that contains mail messages (i.e. no contacts, appointments, etc.)
				If f.DefaultItemType = Outlook.OlItemType.olMailItem Then
					If rootFolder.Folders Is Nothing Then
						rootFolder.Folders = New List(Of Folder)()
					End If

					' add the current folder and enumerate all sub-folders
					Dim subFolder As Folder = New Folder(f.EntryID, f.Name, f.UnReadItemCount, f.Items.Count)
					rootFolder.Folders.Add(subFolder)
					If f.Folders.Count > 0 Then
						Me.EnumerateFolders(f.Folders, subFolder)
					End If
				End If
			Next f

			' alphabetize the list (Folder implements IComparable)
			rootFolder.Folders.Sort()
		End Sub

		Public Function GetMessages(ByVal entryID As String, ByVal numPerPage As Integer, ByVal pageNum As Integer) As List(Of Email) Implements IWHSMailService.GetMessages
			Dim list As List(Of Email) = New List(Of Email)()

			Dim f As Outlook.MAPIFolder

			' if no ID specified, open the inbox
			If String.IsNullOrEmpty(entryID) Then
				f = _nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox)
			Else
				f = _nameSpace.GetFolderFromID(entryID, "")
			End If

			' to handle the sorting, one needs to cache their own instance of the items object
			Dim items As Outlook.Items = f.Items

			' sort descending by received time
			items.Sort("[ReceivedTime]", True)

			' pull in the correct number of items based on number of items per page and current page number
			Dim i As Integer = (numPerPage*pageNum)+1
			Do While i <= (numPerPage*pageNum)+numPerPage AndAlso i <= items.Count
				' ensure it's a mail message
				Dim mi As Outlook.MailItem = (TryCast(items(i), Outlook.MailItem))
				If Not mi Is Nothing Then
					list.Add(New Email(mi.EntryID, mi.SenderEmailAddress, mi.SenderName, mi.Subject, mi.ReceivedTime, mi.Size))
				End If
				i += 1
			Loop

			Return list
		End Function

		Public Function GetMessage(ByVal entryID As String) As Email Implements IWHSMailService.GetMessage
			' pull the message
			Dim mi As Outlook.MailItem = (TryCast(_nameSpace.GetItemFromID(entryID, ""), Outlook.MailItem))

			If Not mi Is Nothing Then
				Dim body As String

				' if it's a plain format message, wrap it in <pre> tags for nice output
				If mi.BodyFormat = Outlook.OlBodyFormat.olFormatPlain Then
					body = "<pre>" & mi.Body & "</pre>"
				Else
					body = mi.HTMLBody
				End If

				Return New Email(mi.EntryID, mi.SenderEmailAddress, mi.SenderName, mi.Subject, mi.ReceivedTime, mi.Size, body)
			Else
				Return Nothing
			End If
		End Function
	End Class

	Friend Class MyServiceHost
		Friend Shared myServiceHost As ServiceHost = Nothing

		Friend Shared Sub StartService()
			' Instantiate new ServiceHost 
			myServiceHost = New ServiceHost(GetType(WHSMailService))
			myServiceHost.Open()
		End Sub

		Friend Shared Sub StopService()
			' Call StopService from your shutdown logic (i.e. dispose method)
			If myServiceHost.State <> CommunicationState.Closed Then
				myServiceHost.Close()
			End If
		End Sub
	End Class
End Namespace

