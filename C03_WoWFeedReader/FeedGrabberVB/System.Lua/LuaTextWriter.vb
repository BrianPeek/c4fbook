Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Namespace System.Lua
	Public Class LuaTextWriter
		Inherits LuaWriter
		Private ReadOnly settings As New LuaWriterSettings()
		Private ReadOnly textWriter As TextWriter

		Private indent_Renamed As Integer

		''' <summary>
		''' Creates an instance of the <see cref="LuaTextWriter"/> class using the specified <see cref="TextWriter"/>.
		''' </summary>
		''' <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
		Public Sub New(ByVal textWriter As TextWriter)
			If textWriter Is Nothing Then
				Throw New ArgumentNullException("textWriter")
			End If

			Me.textWriter = textWriter
		End Sub

		''' <summary>
		''' Creates an instance of the <see cref="LuaTextWriter"/> class using the specified stream and encoding.
		''' </summary>
		''' <param name="stream">The stream to write.</param>
		''' <param name="encoding">The encoding to generate. If <c>null</c>, UTF-8 encoding is used.</param>
		Public Sub New(ByVal stream As Stream, ByVal encoding As Encoding)
			If stream Is Nothing Then
				Throw New ArgumentNullException("stream")
			End If

			If encoding IsNot Nothing Then
				textWriter = New StreamWriter(stream, encoding)
			Else
				textWriter = New StreamWriter(stream)
			End If
		End Sub

		''' <summary>
		''' Creates an instance of the <see cref="LuaTextWriter"/> class using the specified file.
		''' </summary>
		''' <param name="filename">The filename to write to. If the file exists, it truncates it and overwrites it with the new content. </param>
		''' <param name="encoding">The encoding to generate. If <c>null</c>, UTF-8 encoding is used.</param>
		Public Sub New(ByVal filename As String, ByVal encoding As Encoding)
			Me.New(filename, encoding, New LuaWriterSettings())
		End Sub

		Public Sub New(ByVal filename As String, ByVal encoding As Encoding, ByVal settings As LuaWriterSettings)
			Me.New(New FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), encoding)
			If settings Is Nothing Then
				Throw New ArgumentNullException("settings")
			End If

			Me.settings = settings
		End Sub

		Public Overrides Sub WriteStartAssignment(ByVal local As Boolean, ParamArray ByVal variableNames() As String)
			If variableNames Is Nothing Then
				Throw New ArgumentNullException("variableNames")
			End If

			If local Then
				textWriter.Write("local ")
			End If

			For i As Integer = 0 To variableNames.Length - 1
				If i <> 0 Then
					textWriter.Write(", ")
				End If

				textWriter.Write(variableNames(i))
			Next i

			textWriter.Write(" = ")
		End Sub

		Public Overrides Sub WriteLiteralExpression(ByVal value As Object)
			If value Is Nothing Then
				value = "nil"
			ElseIf TypeOf value Is String Then
				Dim text As String = CType(value, String)

				' Depending on whether the text value contains new line, use long bracket format
				If text.Contains(Constants.vbLf) OrElse text.Contains(Constants.vbCr) OrElse text.Contains(Constants.vbCrLf) OrElse text.Contains("""") Then
					value = String.Format("[[{0}]]", text)
				Else
					value = String.Format("""{0}""", text)
				End If
			ElseIf TypeOf value Is Boolean Then
				value = value.ToString().ToLower()
			End If

			textWriter.Write(value)
		End Sub

		Public Overrides Sub WriteExpressionListDelimiter()
			textWriter.Write(", ")
		End Sub

		Public Overrides Sub WriteLiteralExpressionList(ParamArray ByVal values() As Object)
			If values Is Nothing Then
				Throw New ArgumentNullException("values")
			End If

			For i As Integer = 0 To values.Length - 1
				If i <> 0 Then
					Me.WriteExpressionListDelimiter()
				End If

				Me.WriteLiteralExpression(values(i))
			Next i
		End Sub

		Public Overrides Sub WriteEndAssignment()
			textWriter.WriteLine()
		End Sub

		Public Overrides Sub WriteStartTable()
			If settings.TableOnNewLine Then
				Me.Indent()
				textWriter.Write(Environment.NewLine)
				Me.WriteIndent()
			End If

			textWriter.Write("{ ")
		End Sub


		Public Overrides Sub WriteStartTableField(ByVal name As String, ByVal evaluateAsExpression As Boolean)
			If name IsNot Nothing AndAlso name.Length = 0 Then
				Throw New ArgumentException("Zero length field names are not allowed. Use null for unnamed fields.")
			End If
			If evaluateAsExpression AndAlso String.IsNullOrEmpty(name) Then
				Throw New ArgumentException("Cannot evaluate null or empty name as expression.")
			End If

			If settings.TableFieldOnNewLine Then
				Me.Indent()
				textWriter.Write(Environment.NewLine)
				Me.WriteIndent()
			End If

			If name IsNot Nothing Then
				If evaluateAsExpression Then
					textWriter.Write("[")
				End If

				textWriter.Write(name)

				If evaluateAsExpression Then
					textWriter.Write("]")
				End If

				textWriter.Write(" = ")
			End If
		End Sub

		Public Overrides Sub WriteEndTableField()
			textWriter.Write(", ")

			If settings.TableFieldOnNewLine Then
				Me.Unindent()
			End If
		End Sub

		Public Overrides Sub WriteEndTable()
			If settings.EndTableOnNewLine Then
				textWriter.Write(Environment.NewLine)
				Me.WriteIndent()
				textWriter.Write("}")
			End If

			If settings.TableOnNewLine Then
				Me.Unindent()
			End If
		End Sub

		''' <summary>
		''' Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
		''' </summary>
		Public Overrides Sub Flush()
			textWriter.Flush()
		End Sub

		''' <summary>
		''' Closes the stream and the underlying stream used by the <see cref="LuaWriter"/>.
		''' </summary>
		Public Overrides Sub Close()
			textWriter.Close()
		End Sub

		Private Sub Indent()
			indent_Renamed += 1
		End Sub

		Private Sub Unindent()
			indent_Renamed -= 1
		End Sub

		Private Sub WriteIndent()
			If settings.Indent Then
				For i As Integer = 0 To indent_Renamed - 1
					textWriter.Write(settings.IndentChars)
				Next i
			End If
		End Sub
	End Class
End Namespace