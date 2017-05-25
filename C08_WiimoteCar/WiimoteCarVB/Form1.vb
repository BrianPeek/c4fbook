Imports System
Imports System.Windows.Forms
Imports Phidgets
Imports WiimoteLib

Namespace WiimoteCar
	Partial Public Class Form1
		Inherits Form
		' instance of the connected Wiimote
		Private _wiimote As Wiimote

		' instance of the connected Phidget Interface Kit
		Private _interfaceKit As InterfaceKit

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			' create the InterfaceKit object and open access to the board
			_interfaceKit = New InterfaceKit()
			_interfaceKit.open()
			_interfaceKit.waitForAttachment(5000)

			' create the Wiimote object
			_wiimote = New Wiimote()

			' setup an event handler to be notified when the Wiimote sends a packet of data
			AddHandler _wiimote.WiimoteChanged, AddressOf Wiimote_WiimoteChanged

			' connect the Wiimote
			_wiimote.Connect()

			' set the report type to Buttons and Accelerometer data only
			_wiimote.SetReportType(InputReport.ButtonsAccel, True)
		End Sub

		Private Sub Wiimote_WiimoteChanged(ByVal sender As Object, ByVal e As WiimoteChangedEventArgs)
			' get the current Wiimote state
			Dim ws As WiimoteState = e.WiimoteState

			' if button 1 is pressed, toggle the Forward output
			_interfaceKit.outputs(0) = ws.ButtonState.One

			' if button 2 is pressed, toggle the Backward output
			_interfaceKit.outputs(1) = ws.ButtonState.Two

			' if the Wiimote is tilted far enough to the left, toggle the Left output
			_interfaceKit.outputs(2) = (ws.AccelState.Values.Y < -0.07f)

			' if the Wiimote is tilted far enough to the right, toggle the Reft output
			_interfaceKit.outputs(3) = (ws.AccelState.Values.Y > 0.07f)
		End Sub
	End Class
End Namespace
