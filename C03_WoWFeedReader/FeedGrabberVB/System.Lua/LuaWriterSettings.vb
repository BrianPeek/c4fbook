Imports Microsoft.VisualBasic
Imports System
Namespace System.Lua
	Public Class LuaWriterSettings
		Private Const defaultIndentChars As String = "    "

		Public Sub New()
			Me.TableOnNewLine = False
			Me.TableFieldOnNewLine = True
			Me.EndTableOnNewLine = True

			Me.Indent = True
			Me.IndentChars = defaultIndentChars
			Me.NewLineChars = Environment.NewLine

			Me.ValidateContent = True
		End Sub

		Private privateIndent As Boolean
		Public Property Indent() As Boolean
			Get
				Return privateIndent
			End Get
			Set(ByVal value As Boolean)
				privateIndent = value
			End Set
		End Property

		Private privateIndentChars As String
		Public Property IndentChars() As String
			Get
				Return privateIndentChars
			End Get
			Set(ByVal value As String)
				privateIndentChars = value
			End Set
		End Property

		Private privateNewLineChars As String
		Public Property NewLineChars() As String
			Get
				Return privateNewLineChars
			End Get
			Set(ByVal value As String)
				privateNewLineChars = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets a value indicating whether to start tables on a new line.
		''' </summary>
		Private privateTableOnNewLine As Boolean
		Public Property TableOnNewLine() As Boolean
			Get
				Return privateTableOnNewLine
			End Get
			Set(ByVal value As Boolean)
				privateTableOnNewLine = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets a value indicating whether a table field should be on a new line.
		''' </summary>
		Private privateTableFieldOnNewLine As Boolean
		Public Property TableFieldOnNewLine() As Boolean
			Get
				Return privateTableFieldOnNewLine
			End Get
			Set(ByVal value As Boolean)
				privateTableFieldOnNewLine = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets a value indicating whether to close tables on a new line.
		''' </summary>
		Private privateEndTableOnNewLine As Boolean
		Public Property EndTableOnNewLine() As Boolean
			Get
				Return privateEndTableOnNewLine
			End Get
			Set(ByVal value As Boolean)
				privateEndTableOnNewLine = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets a value indicating whether content should be validated and only valid Lua
		''' code emitted.
		''' </summary>
		Private privateValidateContent As Boolean
		Public Property ValidateContent() As Boolean
			Get
				Return privateValidateContent
			End Get
			Set(ByVal value As Boolean)
				privateValidateContent = value
			End Set
		End Property
	End Class
End Namespace
