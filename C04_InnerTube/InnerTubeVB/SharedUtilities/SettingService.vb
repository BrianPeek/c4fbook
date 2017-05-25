Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO

Namespace SharedUtilities
	Public NotInheritable Class SettingService

        Private Shared privateVideoPath As String
		Public Shared Property VideoPath() As String
			Get
				Return privateVideoPath
			End Get
			Set(ByVal value As String)
				privateVideoPath = value
			End Set
		End Property
		Public Shared SubPath As String

        Private Shared privateSettingPath As String
		Public Shared Property SettingPath() As String
			Get
				Return privateSettingPath
			End Get
			Set(ByVal value As String)
				privateSettingPath = value
			End Set
		End Property

		Public Shared Function Load() As Setting
			SetPath()

			If File.Exists(SettingPath) Then
				Dim serial = New Serializer(Of Setting)()
				Return serial.Deserialize(SettingPath)
			Else
				Return BuildDefaultSettings()
			End If
		End Function

		Public Shared Sub Save(ByVal changes As Setting)
			SetPath()
			Dim serial = New Serializer(Of Setting)()
			serial.Serialize(changes, SettingPath)
		End Sub

		Private Shared Sub SetPath()
			If String.IsNullOrEmpty(VideoPath) Then
				VideoPath = FileHelper.BuildPath(SubPath)
			End If

			If String.IsNullOrEmpty(SubPath) Then
				SubPath = Path.Combine(SubPath, FileHelper.DefaultSettingsName)
			End If

			SettingPath = Path.Combine(SubPath, FileHelper.DefaultSettingsName)
		End Sub

		Private Shared Function BuildDefaultSettings() As Setting
			Dim s As New Setting()
			s.AppName = FileHelper.DefaultAppName
			s.iTunesInstalled = False
			s.ZuneInstalled = False
			s.FirstRun = True
			s.SubPath = SubPath
			s.InnerTubeFeedFile = Path.Combine(SubPath, FileHelper.BuildXmlName())
			s.VideoPath = VideoPath
			s.UpdateFeedPoolThreads = 3
            s.DownloadPoolThreads = 5
			s.ConversionPoolThreads = 2

			Return s
		End Function

	End Class
End Namespace
