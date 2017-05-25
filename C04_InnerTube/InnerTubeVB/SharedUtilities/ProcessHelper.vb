Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Diagnostics
Imports System.IO

Namespace SharedUtilities
	Public Enum ProcessType
		iTunes
		Zune
	End Enum

	Public NotInheritable Class ProcessHelper
		Private Shared iTunesExe As String = "iTunesHelper"
		Private Shared ZuneExe As String = "ZuneLauncher"

        Public Shared Function IsProcessRunning(ByVal name As String) As Boolean
            Dim x = Process.GetProcesses()
            Dim result = From p In x _
                         Where p.ProcessName = name _
                         Select p

            If result.Count() > 0 Then
                Return True
            End If
            Return False
        End Function

		Public Shared Function IsProcessRunning(ByVal process As ProcessType) As Boolean
			Select Case process
				Case ProcessType.iTunes
					Return IsProcessRunning(iTunesExe)
				Case ProcessType.Zune
					Return IsProcessRunning(ZuneExe)
				Case Else
					Return False

			End Select
		End Function

	End Class
End Namespace
