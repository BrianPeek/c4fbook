Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic

Namespace WHSMailCommon.Entities
	' entity object representing an Folder
	<Serializable> _
	Public Class Folder
		Implements IComparable
		Private _entryID As String
		Private _name As String
		Private _folders As List(Of Folder)
		Private _unreadMessages As Integer
		Private _totalMessages As Integer

		Public Sub New(ByVal entryID As String, ByVal name As String, ByVal unreadMessages As Integer, ByVal totalMessages As Integer)
			_entryID = entryID
			_name = name
			_unreadMessages = unreadMessages
			_totalMessages = totalMessages
		End Sub

		' MAPI unique identifier
		Public Property EntryID() As String
			Get
				Return _entryID
			End Get
			Set(ByVal value As String)
				_entryID = value
			End Set
		End Property

		' subfolders of this folder
		Public Property Folders() As List(Of Folder)
			Get
				Return _folders
			End Get
			Set(ByVal value As List(Of Folder))
				_folders = value
			End Set
		End Property

		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				_name = value
			End Set
		End Property

		Public Property UnreadMessages() As Integer
			Get
				Return Me._unreadMessages
			End Get
			Set(ByVal value As Integer)
				Me._unreadMessages = value
			End Set
		End Property

		Public Property TotalMessages() As Integer
			Get
				Return Me._totalMessages
			End Get
			Set(ByVal value As Integer)
				Me._totalMessages = value
			End Set
		End Property

		' used so we can sort the folders alphabetically later on
		Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
			Return String.Compare(Me.Name, (CType(obj, Folder)).Name)
		End Function
	End Class
End Namespace
