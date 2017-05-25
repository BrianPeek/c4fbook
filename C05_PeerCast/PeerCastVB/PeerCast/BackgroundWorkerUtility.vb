Imports Microsoft.VisualBasic
Imports System.ComponentModel

Namespace PeerCast
	Public NotInheritable Class BackgroundWorkerUtility
        Public Shared Function Create() As BackgroundWorker
            Dim worker As New BackgroundWorker() With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}
            Return worker
        End Function
	End Class
End Namespace
