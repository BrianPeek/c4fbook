Imports Microsoft.VisualBasic
Imports System
Imports System.ServiceModel
Imports WHSMailCommon.Contracts

Namespace WHSMailWeb
	Public Partial Class BasePage
		Inherits System.Web.UI.Page
		Private _factory As ChannelFactory(Of IWHSMailService) = Nothing
		Private _channel As IWHSMailService = Nothing

		Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
			' create a channel factory and then instantiate a proxy channel
			_factory = New ChannelFactory(Of IWHSMailService)("WHSMailService")
			_channel = _factory.CreateChannel()
		End Sub

		Protected Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs)
			Try
				_factory.Close()
			Catch
				' for the moment, we don't really care what happens here...if it fails, so be it
			End Try
		End Sub

		Public ReadOnly Property WHSMailService() As IWHSMailService
			Get
				Return _channel
			End Get
		End Property

	End Class
End Namespace
