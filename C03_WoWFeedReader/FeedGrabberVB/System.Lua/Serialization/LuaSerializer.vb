Imports Microsoft.VisualBasic
Imports System.Reflection
Imports System.Collections
Namespace System.Lua.Serialization
	Public Class LuaSerializer
		Public Sub Serialize(ByVal luaWriter As LuaWriter, ByVal o As Object)
			If luaWriter Is Nothing Then
				Throw New ArgumentNullException("luaWriter")
			End If

			Me.SerializeImpl(luaWriter, o)
		End Sub

		Private Sub SerializeImpl(ByVal luaWriter As LuaWriter, ByVal o As Object)
			' If object is null, serialize as literal (to 'nil')
			If o Is Nothing Then
				Me.SerializeLiteral(luaWriter, o)
			Else
				' Get the type of the object
				Dim type As Type = o.GetType()

				If type.SerializeAsEnumerable() Then
					Me.SerializeEnumerable(luaWriter, CType(o, IEnumerable))
				End If
				If type.SerializeAsDictionary() Then
					Me.SerializeDictionary(luaWriter, o)
				End If
				If type.SerializeAsLiteral() Then
					Me.SerializeLiteral(luaWriter, o)
				End If
			End If
		End Sub

		Private Sub SerializeEnumerable(ByVal luaWriter As LuaWriter, ByVal collection As IEnumerable)
			luaWriter.WriteStartTable()

			For Each item In collection
				luaWriter.WriteStartTableField()
				Me.SerializeImpl(luaWriter, item)
				luaWriter.WriteEndTableField()
			Next item

			luaWriter.WriteEndTable()
		End Sub

		Private Sub SerializeDictionary(ByVal luaWriter As LuaWriter, ByVal o As Object)
			Dim type As Type = o.GetType()

			luaWriter.WriteStartTable()

			For Each propertyInfo In type.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
				luaWriter.WriteStartTableField(propertyInfo.Name, False)
				Me.SerializeImpl(luaWriter, propertyInfo.GetValue(o, Nothing))
				luaWriter.WriteEndTableField()
			Next propertyInfo

			luaWriter.WriteEndTable()
		End Sub

		Private Sub SerializeLiteral(ByVal luaWriter As LuaWriter, ByVal value As Object)
			luaWriter.WriteLiteralExpression(value)
		End Sub
	End Class
End Namespace
