'////////////////////////////////////////////////////////////////////////////////
'	PhidgetHandler.cs
'	Light Sequencer
'	Written by Brian Peek (http://www.brianpeek.com/)
'	for the Animated Holiday Lights article
'		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
'////////////////////////////////////////////////////////////////////////////////


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports Phidgets
Imports Phidgets.Events
Imports System.ComponentModel

Namespace LightSequencer
	Public NotInheritable Class PhidgetHandler
		' hashtable mapping serial number to interface kit
		Public Shared IFKits As Dictionary(Of Integer, InterfaceKit) = New Dictionary(Of Integer,InterfaceKit)()
		Private Shared _phidgetsManager As Manager

		Public Shared Event PhidgetsChanged As EventHandler

		Private Sub New()
		End Sub
		Public Shared Sub Init()
			' create a new phidgets manager to find the devices connected
			_phidgetsManager = New Manager()
			AddHandler _phidgetsManager.Attach, AddressOf _phidgetsManager_Attach
			AddHandler _phidgetsManager.Detach, AddressOf _phidgetsManager_Detach
			_phidgetsManager.open()
		End Sub

		Private Shared Sub _phidgetsManager_Attach(ByVal sender As Object, ByVal e As AttachEventArgs)
			If e.Device.Type = "PhidgetInterfaceKit" Then
				' if we haven't opened it already, do so now
				If (Not IFKits.ContainsKey(e.Device.SerialNumber)) Then
					Dim ik As New InterfaceKit()
					AddHandler ik.Attach, AddressOf ik_Attach
					ik.open(e.Device.SerialNumber)
					IFKits(e.Device.SerialNumber) = ik

				End If
			End If
		End Sub

		Private Shared Sub ik_Attach(ByVal sender As Object, ByVal e As AttachEventArgs)
			If PhidgetsChangedEvent IsNot Nothing Then
				For Each AttachHandler As EventHandler In PhidgetsChangedEvent.GetInvocationList()
					Dim syncInvoke As ISynchronizeInvoke = TryCast(AttachHandler.Target, ISynchronizeInvoke)
					If (syncInvoke IsNot Nothing) AndAlso (syncInvoke.InvokeRequired) Then
						syncInvoke.Invoke(AttachHandler, New Object() { Nothing, Nothing })
					Else
						AttachHandler(Nothing, Nothing)
					End If
				Next AttachHandler
			End If
		End Sub

		Private Shared Sub _phidgetsManager_Detach(ByVal sender As Object, ByVal e As DetachEventArgs)
			Dim serial As Integer = e.Device.SerialNumber
			IFKits(serial).close()
			IFKits.Remove(serial)

			If PhidgetsChangedEvent IsNot Nothing Then
				For Each AttachHandler As EventHandler In PhidgetsChangedEvent.GetInvocationList()
					Dim syncInvoke As ISynchronizeInvoke = TryCast(AttachHandler.Target, ISynchronizeInvoke)
					If (syncInvoke IsNot Nothing) AndAlso (syncInvoke.InvokeRequired) Then
						syncInvoke.Invoke(AttachHandler, New Object() { Nothing, Nothing })
					Else
						AttachHandler(Nothing, Nothing)
					End If
				Next AttachHandler
			End If
		End Sub
	End Class
End Namespace
