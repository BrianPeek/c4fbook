Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.ServiceModel
Imports WHSMailCommon.Entities

Namespace WHSMailCommon.Contracts
	' list of methods of the WHSMailService service
	<ServiceContract()> _
	Public Interface IWHSMailService
		<OperationContract> _
		Function GetFolders() As List(Of Folder)

		<OperationContract> _
		Function GetMessages(ByVal entryID As String, ByVal numPerPage As Integer, ByVal pageNum As Integer) As List(Of Email)

		<OperationContract> _
		Function GetMessage(ByVal entryID As String) As Email
	End Interface
End Namespace
