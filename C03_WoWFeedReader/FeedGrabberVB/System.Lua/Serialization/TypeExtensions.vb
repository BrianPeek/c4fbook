Imports Microsoft.VisualBasic
Imports System.Collections
Namespace System.Lua.Serialization
    Public Module TypeExtensions

        'TODO: INSTANT VB TODO TASK: Extension methods must be defined in modules in VB:
        <System.Runtime.CompilerServices.Extension()> _
        Public Function SerializeAsEnumerable(ByVal type As Type) As Boolean
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            Return (Not type.SerializeAsLiteral()) AndAlso type.GetInterface("IEnumerable") IsNot Nothing
        End Function

        'TODO: INSTANT VB TODO TASK: Extension methods must be defined in modules in VB:
        <System.Runtime.CompilerServices.Extension()> _
        Public Function SerializeAsArray(ByVal type As Type) As Boolean
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            Return type.IsArray
        End Function

        'TODO: INSTANT VB TODO TASK: Extension methods must be defined in modules in VB:
        <System.Runtime.CompilerServices.Extension()> _
        Public Function SerializeAsDictionary(ByVal type As Type) As Boolean
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            Return (Not type.IsArray) AndAlso (type.IsClass AndAlso type IsNot GetType(String))
        End Function

        'TODO: INSTANT VB TODO TASK: Extension methods must be defined in modules in VB:
        <System.Runtime.CompilerServices.Extension()> _
        Public Function SerializeAsLiteral(ByVal type As Type) As Boolean
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            Return type.IsPrimitive OrElse type Is GetType(String)
        End Function
    End Module
End Namespace
