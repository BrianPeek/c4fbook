Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text

Namespace SharedUtilities
    Public Module ExtensionMethods

        <System.Runtime.CompilerServices.Extension()> _
        Public Function ToCollection(Of T)(ByVal source As IEnumerable(Of T)) As Collection(Of T)
            Dim result As Collection(Of T) = New Collection(Of T)()

            For Each item In source
                result.Add(item)
            Next item
            Return result
        End Function

        <System.Runtime.CompilerServices.Extension()> _
        Public Function ToObservableCollection(Of T)(ByVal source As IEnumerable(Of T)) As ObservableCollection(Of T)
            Dim result As ObservableCollection(Of T) = New ObservableCollection(Of T)()

            For Each item In source
                result.Add(item)
            Next item
            Return result
        End Function

    End Module
End Namespace
