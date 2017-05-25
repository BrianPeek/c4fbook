Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports System.Globalization

Namespace System.Lua
	Public MustInherit Class LuaWriter
		Implements IDisposable
		''' <summary>
		''' Initializes a new instance of the <see cref="LuaWriter"/> class.
		''' </summary>
		Protected Sub New()
		End Sub

		Public MustOverride Sub WriteStartAssignment(ByVal local As Boolean, ParamArray ByVal variableNames() As String)

		Public Sub WriteStartAssignment(ParamArray ByVal variableNames() As String)
			Me.WriteStartAssignment(False, variableNames)
		End Sub

		Public MustOverride Sub WriteLiteralExpression(ByVal value As Object)

		Public MustOverride Sub WriteExpressionListDelimiter()

		Public MustOverride Sub WriteLiteralExpressionList(ParamArray ByVal values() As Object)

		Public MustOverride Sub WriteEndAssignment()

		Public MustOverride Sub WriteStartTable()

		Public MustOverride Sub WriteStartTableField(ByVal name As String, ByVal evaluateAsExpression As Boolean)

		Public Sub WriteStartTableField(ByVal name As String)
			Me.WriteStartTableField(name, False)
		End Sub

		Public Sub WriteStartTableField()
			Me.WriteStartTableField(Nothing, False)
		End Sub

		Public Sub WriteTableFieldLiteralExpression(ByVal name As String, ByVal evaluateAsExpression As Boolean, ByVal value As Object)
			Me.WriteStartTableField(name, evaluateAsExpression)
			Me.WriteLiteralExpression(value)
			Me.WriteEndTableField()
		End Sub

		Public Sub WriteTableFieldLiteralExpression(ByVal name As String, ByVal value As Object)
			Me.WriteStartTableField(name)
			Me.WriteLiteralExpression(value)
			Me.WriteEndTableField()
		End Sub

		Public Sub WriteTableFieldLiteralExpression(ByVal value As Object)
			Me.WriteStartTableField()
			Me.WriteLiteralExpression(value)
			Me.WriteEndTableField()
		End Sub

		Public MustOverride Sub WriteEndTableField()

		Public MustOverride Sub WriteEndTable()

		''' <summary>
		''' When overridden in a derived class, flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
		''' </summary>
		Public MustOverride Sub Flush()

		''' <summary>
		''' When overridden in a derived class, closes the stream and the underlying stream used by the <see cref="LuaWriter"/>.
		''' </summary>
		Public MustOverride Sub Close()

		''' <summary>
		''' Releases the unmanaged resources used by the <see cref="LuaWriter"/> and optionally releases the managed resources.
		''' </summary>
		''' <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			Try
				Me.Close()
			Catch
			End Try
		End Sub

		''' <summary>
		''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		''' </summary>
		Private Sub Dispose() Implements IDisposable.Dispose
			Me.Dispose(True)
		End Sub

		''' <summary>
		''' Creates a new <see cref="LuaWriter"/> instance using the specified stream.
		''' </summary>
		''' <param name="output">The stream to write to.</param>
		''' <returns>A <see cref="LuaWriter"/> object.</returns>
		Public Shared Function Create(ByVal output As Stream) As LuaWriter
			If output Is Nothing Then
				Throw New ArgumentNullException("output")
			End If

			Return New LuaTextWriter(output, Nothing)
		End Function

		''' <summary>
		''' Creates a new <see cref="LuaWriter"/> instance using the specified <see cref="TextWriter"/>.
		''' </summary>
		''' <param name="output">The <see cref="TextWriter"/> to write to.</param>
		''' <returns>A <see cref="LuaWriter"/> object.</returns>        
		Public Shared Function Create(ByVal output As TextWriter) As LuaWriter
			If output Is Nothing Then
				Throw New ArgumentNullException("output")
			End If

			Return New LuaTextWriter(output)
		End Function

		''' <summary>
		''' Creates a new <see cref="LuaWriter"/> instance using the specified <see cref="TextWriter"/>.
		''' </summary>
		''' <param name="output">The <see cref="StringBuilder"/> to write to. Content written by the <see cref="LuaWriter"/> is appended to the <see cref="StringBuilder"/>.</param>
		''' <returns>A <see cref="LuaWriter"/> object.</returns>      
		Public Shared Function Create(ByVal output As StringBuilder) As LuaWriter
			If output Is Nothing Then
				Throw New ArgumentNullException("output")
			End If

			Return New LuaTextWriter(New StringWriter(output, CultureInfo.InvariantCulture))
		End Function

		''' <summary>
		''' Creates a new <see cref="LuaWriter"/> instance using the specified filename.
		''' </summary>
		''' <param name="outputFilename">The <see cref="TextWriter"/> to write to.</param>
		''' <returns>A <see cref="LuaWriter"/> object.</returns>      
		Public Shared Function Create(ByVal outputFilename As String) As LuaWriter
			If outputFilename Is Nothing Then
				Throw New ArgumentNullException("outputFilename")
			End If

			Return New LuaTextWriter(outputFilename, Nothing)
		End Function

		Public Shared Function Create(ByVal outputFilename As String, ByVal settings As LuaWriterSettings) As LuaWriter
			If outputFilename Is Nothing Then
				Throw New ArgumentNullException("outputFilename")
			End If
			If settings Is Nothing Then
				Throw New ArgumentNullException("settings")
			End If

			Return New LuaTextWriter(outputFilename, Nothing, settings)
		End Function
	End Class
End Namespace
