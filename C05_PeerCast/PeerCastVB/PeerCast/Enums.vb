Imports Microsoft.VisualBasic
Imports System
Namespace PeerCast
	' Message types that the client will receive
	Public Enum UiMessage
		StreamVideo
		UpdateStatus
		ReceivedVideoList
		ToggleUi
		Log
	End Enum

	' Message types that are sent
	Public Enum ChatCommand
		GetList
		ReturnList
		StreamVideo
	End Enum

    'Included here for reference since C4FP2P version is C# only 
    'Public Enum SystemState
    '    LoggedOut
    '    LoggingIn
    '    LoggedIn
    'End Enum
End Namespace