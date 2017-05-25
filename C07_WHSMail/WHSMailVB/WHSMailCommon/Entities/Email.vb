Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace WHSMailCommon.Entities
	' entity object representing an Email
	<Serializable> _
	Public Class Email
		Private _entryID As String
		Private _from As String
		Private _fromName As String
		Private _subject As String
		Private _received As DateTime
		Private _size As Integer
		Private _body As String

		Public Sub New(ByVal entryID As String, ByVal From As String, ByVal fromName As String, ByVal subject As String, ByVal received As DateTime, ByVal size As Integer)
			_entryID = entryID
			_from = From
			_fromName = fromName
			If String.IsNullOrEmpty(subject) Then
				_subject = "(no subject)"
			Else
				_subject = subject
			End If
			_received = received
			_size = size
		End Sub

		Public Sub New(ByVal entryID As String, ByVal From As String, ByVal fromName As String, ByVal subject As String, ByVal received As DateTime, ByVal size As Integer, ByVal body As String)
			Me.New(entryID, From, fromName, subject, received, size)
			_body = body
		End Sub

		' MAPI unique ID
		Public Property EntryID() As String
			Get
				Return _entryID
			End Get
			Set(ByVal value As String)
				_entryID = value
			End Set
		End Property

		' email address of sender
		Public Property From() As String
			Get
				Return _from
			End Get
			Set(ByVal value As String)
				_from = value
			End Set
		End Property

		' name of sender
		Public Property FromName() As String
			Get
				Return _fromName
			End Get
			Set(ByVal value As String)
				_fromName = value
			End Set
		End Property

		Public Property Subject() As String
			Get
				Return _subject
			End Get
			Set(ByVal value As String)
				_subject = value
			End Set
		End Property

		Public Property Body() As String
			Get
				Return _body
			End Get
			Set(ByVal value As String)
				_body = value
			End Set
		End Property

		Public Property Received() As DateTime
			Get
				Return _received
			End Get
			Set(ByVal value As DateTime)
				_received = value
			End Set
		End Property

		Public Property Size() As Integer
			Get
				Return _size
			End Get
			Set(ByVal value As Integer)
				_size = value
			End Set
		End Property
	End Class
End Namespace
