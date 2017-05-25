Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.IO
Imports WiimoteLib

Namespace WiimoteWhiteboard
	Partial Public Class Form1
		Inherits Form
		Private Const CalibrationFilename As String = "calibration.dat"

		'instance of the wii remote
		Private _wm As New Wiimote()
		Private _lastWiiState As New WiimoteState() 'helps with event firing

		Private _cursorControl As Boolean
		Private _screenSize As Size

		Private _calibrationState As Integer
		Private _calibrationMargin As Single =.1f 'specifies how far in to draw the crosshairs

		Private _smoothingCount As Integer = 0 'track the number of points stored
		Private _smoothingPoints As Integer = 5 'store the number of points to average out (1-10)
		Private _smoothingX(9) As Single 'store the past coordinates for smoothing
		Private _smoothingY(9) As Single

		Private cf As CalibrationForm

		Private _warper As New Warper()
		Private _srcX(3) As Single
		Private _srcY(3) As Single
		Private _dstX(3) As Single
		Private _dstY(3) As Single

		'declare consts for mouse messages
		Public Const INPUT_MOUSE As Integer = 0
		Public Const MOUSEEVENTF_MOVE As Integer = &H01
		Public Const MOUSEEVENTF_LEFTDOWN As Integer = &H02
		Public Const MOUSEEVENTF_LEFTUP As Integer = &H04
		Public Const MOUSEEVENTF_RIGHTDOWN As Integer = &H08
		Public Const MOUSEEVENTF_RIGHTUP As Integer = &H10
		Public Const MOUSEEVENTF_ABSOLUTE As Integer = &H8000

		'declare consts for key scan codes
		Public Const VK_LEFT As Byte = &H25
		Public Const VK_UP As Byte = &H26
		Public Const VK_RIGHT As Byte = &H27
		Public Const VK_DOWN As Byte = &H28

		Public Const KEYEVENTF_KEYUP As Integer = &H02

		Public Structure MOUSEINPUT '24 bytes
			Public dx As Integer '4
			Public dy As Integer '4
			Public mouseData As UInteger '4
			Public dwFlags As UInteger '4
			Public time As UInteger '4
			Public dwExtraInfo As IntPtr '4
		End Structure

		Public Structure INPUT '28 bytes
			Public type As Integer ' 4 bytes
			Public mi As MOUSEINPUT '24 bytes
		End Structure

		Private Const INPUT_SIZE As Integer = 28

		Private _buffer(1) As INPUT

		'for firing mouse events
		<DllImport("user32.dll", SetLastError := True)> _
		Shared Function SendInput(ByVal nInputs As UInteger, ByVal pInputs() As INPUT, ByVal cbSize As Integer) As UInteger
		End Function

		'imports keybd_event function from user32.dll
		<DllImport("user32.dll", SetLastError := True)> _
		Public Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)
		End Sub

		' delegate for updating the UI on the original thread
		Private Delegate Sub UpdateUIDelegate(ByVal args As WiimoteState)

		' delegate for updating tracking info
		Private Delegate Sub UpdateTrackingUtilizationDelegate(ByVal utilStatus As String)

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			_screenSize.Width = Screen.GetBounds(Me).Width
			_screenSize.Height= Screen.GetBounds(Me).Height

			Try
				'connect to wii remote
				_wm.Connect()

				'set what features you want to enable for the remote, look at Wiimote.InputReport for options
				_wm.SetReportType(InputReport.IRAccel, True)

				'set wiiremote LEDs with this enumerated ID
				_wm.SetLEDs(True, False, False, False)
			Catch x As Exception
				MessageBox.Show("Exception: " & x.Message)
				Me.Close()
			End Try

			'add event listeners to changes in the wiiremote
			'fired for every input report - usually 100 times per second if accelerometer is enabled
			AddHandler _wm.WiimoteChanged, AddressOf wm_WiimoteChanged

			LoadCalibrationData()
		End Sub

		Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
			'disconnect the wiimote
			_wm.Disconnect()
		End Sub

		Private Sub btnCalibrate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCalibrate.Click
			If cf Is Nothing OrElse cf.IsDisposed Then
				cf = New CalibrationForm()
			End If

			cf.Show()
			_cursorControl = False
			_calibrationState = 1
			DoCalibration()
		End Sub

		Private Sub cbCursorControl_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbCursorControl.CheckedChanged
			_cursorControl = cbCursorControl.Checked
		End Sub

		Private Sub wm_WiimoteChanged(ByVal sender As Object, ByVal e As WiimoteChangedEventArgs)
			'extract the wiimote state
			Dim ws As WiimoteState = e.WiimoteState

			If ws.IRState.IRSensors(0).Found Then
				Dim x As Integer = ws.IRState.IRSensors(0).RawPosition.X
				Dim y As Integer = ws.IRState.IRSensors(0).RawPosition.Y
				Dim warpedX As Single = x
				Dim warpedY As Single = y

				_warper.Warp(x, y, warpedX, warpedY)

				If (Not _lastWiiState.IRState.IRSensors(0).Found) Then 'mouse down
					_lastWiiState.IRState.IRSensors(0).Found = ws.IRState.IRSensors(0).Found

					If _cursorControl Then
						_buffer(0).type = INPUT_MOUSE
						_buffer(0).mi.dx = CInt(Fix(warpedX * 65535.0f / _screenSize.Width))
						_buffer(0).mi.dy = CInt(Fix(warpedY * 65535.0f / _screenSize.Height))
						_buffer(0).mi.mouseData = 0
						_buffer(0).mi.dwFlags = MOUSEEVENTF_ABSOLUTE Or MOUSEEVENTF_MOVE
						_buffer(0).mi.time = 0
						_buffer(0).mi.dwExtraInfo = CType(0, IntPtr)

						_buffer(1).type = INPUT_MOUSE
						_buffer(1).mi.dx = 0
						_buffer(1).mi.dy = 0
						_buffer(1).mi.mouseData = 0
						_buffer(1).mi.dwFlags = MOUSEEVENTF_LEFTDOWN
						_buffer(1).mi.time = 10
						_buffer(1).mi.dwExtraInfo = CType(0, IntPtr)

						SendInput(2, _buffer, INPUT_SIZE)

					End If 'cusor control

					Select Case _calibrationState
						Case 1
							_srcX(_calibrationState - 1) = x
							_srcY(_calibrationState - 1) = y
							_calibrationState = 2
							DoCalibration()
						Case 2
							_srcX(_calibrationState - 1) = x
							_srcY(_calibrationState - 1) = y
							_calibrationState = 3
							DoCalibration()
						Case 3
							_srcX(_calibrationState - 1) = x
							_srcY(_calibrationState - 1) = y
							_calibrationState = 4
							DoCalibration()
						Case 4
							_srcX(_calibrationState - 1) = x
							_srcY(_calibrationState - 1) = y
							_calibrationState = 5
							DoCalibration()
						Case Else
							Exit Select
					End Select 'calibtation state 'mouse down
				Else
					If _cursorControl Then 'dragging
						Dim sumX As Single = warpedX, sumY As Single = warpedY 'initialize the sum
						_smoothingCount += 1 'track number of stored values

						If _smoothingCount > _smoothingPoints Then
							_smoothingCount = _smoothingPoints
						End If

						For i As Integer = 0 To _smoothingPoints - 2
							'shift stored points
							_smoothingX(i) = _smoothingX(i + 1)
							_smoothingY(i) = _smoothingY(i + 1)

							sumX += _smoothingX(i) 'adding sum
							sumY += _smoothingY(i)
						Next i

						'add new points to the last position
						_smoothingX(_smoothingPoints - 1) = warpedX
						_smoothingY(_smoothingPoints - 1) = warpedY

						'calculate the averaged values
						Dim smoothWarpedX As Single = sumX / (_smoothingCount)
						Dim smoothWarpedY As Single = sumY / (_smoothingCount)

						'mouse drag with the averaged coordinate
						_buffer(0).type = INPUT_MOUSE
						_buffer(0).mi.dx = CInt(Fix(smoothWarpedX * 65535.0f / _screenSize.Width))
						_buffer(0).mi.dy = CInt(Fix(smoothWarpedY * 65535.0f / _screenSize.Height))
						_buffer(0).mi.mouseData = 0
						_buffer(0).mi.dwFlags = MOUSEEVENTF_ABSOLUTE Or MOUSEEVENTF_MOVE
						_buffer(0).mi.time = 0
						_buffer(0).mi.dwExtraInfo = CType(0, IntPtr)
						SendInput(1, _buffer, INPUT_SIZE)
					End If
				End If 'ir visible
			Else
				If _lastWiiState.IRState.IRSensors(0).Found Then 'mouse up
					If _cursorControl Then
						_buffer(0).type = INPUT_MOUSE
						_buffer(0).mi.dx = 0
						_buffer(0).mi.dy = 0
						_buffer(0).mi.mouseData = 0
						_buffer(0).mi.dwFlags = MOUSEEVENTF_LEFTUP
						_buffer(0).mi.time = 0
						_buffer(0).mi.dwExtraInfo = CType(0, IntPtr)

						_buffer(1).type = INPUT_MOUSE
						_buffer(1).mi.dx = 0
						_buffer(1).mi.dy = 0
						_buffer(1).mi.mouseData = 0
						_buffer(1).mi.dwFlags = MOUSEEVENTF_MOVE
						_buffer(1).mi.time = 0
						_buffer(1).mi.dwExtraInfo = CType(0, IntPtr)
						SendInput(2, _buffer, INPUT_SIZE)

						'clear smoothing data when mouse up
						ResetCursorSmoothing()
					End If
				End If 'ir lost
			End If

			_lastWiiState.IRState.IRSensors(0).Found = ws.IRState.IRSensors(0).Found

			If (Not _lastWiiState.ButtonState.A) AndAlso ws.ButtonState.A Then
				BeginInvoke(CType(AddressOf AnonymousMethod1, MethodInvoker))
			End If

			_lastWiiState.ButtonState.A = ws.ButtonState.A

			If (Not _lastWiiState.ButtonState.Up) AndAlso ws.ButtonState.Up Then
				keybd_event(VK_UP, &H45, 0, 0)
			End If
			If _lastWiiState.ButtonState.Up AndAlso (Not ws.ButtonState.Up) Then
				keybd_event(VK_UP, &H45, KEYEVENTF_KEYUP, 0)
			End If
			_lastWiiState.ButtonState.Up = ws.ButtonState.Up

			If (Not _lastWiiState.ButtonState.Down) AndAlso ws.ButtonState.Down Then
				keybd_event(VK_DOWN, &H45, 0, 0)
			End If
			If _lastWiiState.ButtonState.Down AndAlso (Not ws.ButtonState.Down) Then
				keybd_event(VK_DOWN, &H45, KEYEVENTF_KEYUP, 0)
			End If
			_lastWiiState.ButtonState.Down = ws.ButtonState.Down

			If (Not _lastWiiState.ButtonState.Left) AndAlso ws.ButtonState.Left Then
				keybd_event(VK_LEFT, &H45, 0, 0)
			End If
			If _lastWiiState.ButtonState.Left AndAlso (Not ws.ButtonState.Left) Then
				keybd_event(VK_LEFT, &H45, KEYEVENTF_KEYUP, 0)
			End If
			_lastWiiState.ButtonState.Left = ws.ButtonState.Left

			If (Not _lastWiiState.ButtonState.Right) AndAlso ws.ButtonState.Right Then
				keybd_event(VK_RIGHT, &H45, 0, 0)
			End If
			If _lastWiiState.ButtonState.Right AndAlso (Not ws.ButtonState.Right) Then
				keybd_event(VK_RIGHT, &H45, KEYEVENTF_KEYUP, 0)
			End If
			_lastWiiState.ButtonState.Right = ws.ButtonState.Right

			BeginInvoke(New UpdateUIDelegate(AddressOf UpdateUI), e.WiimoteState)
		End Sub
		Private Sub AnonymousMethod1()
			btnCalibrate.PerformClick()
		End Sub

		Private Sub UpdateUI(ByVal ws As WiimoteState)
			'draw battery value on GUI
			If ws.Battery > &Hc8 Then
				pbBattery.Value = (&Hc8)
			Else
				pbBattery.Value = (CInt(Fix(ws.Battery)))
			End If
			Dim f As Single = (((100.0f * 48.0f * CSng(ws.Battery / 48.0f))) / 192.0f)
			lblBattery.Text = f.ToString("F")

			'check the GUI check boxes if the IR dots are visible
			Dim irstatus As String = "Visible IR dots: "
			If ws.IRState.IRSensors(0).Found Then
				irstatus &= "1 "
			End If
			If ws.IRState.IRSensors(1).Found Then
				irstatus &= "2 "
			End If
			If ws.IRState.IRSensors(2).Found Then
				irstatus &= "3 "
			End If
			If ws.IRState.IRSensors(3).Found Then
				irstatus &= "4 "
			End If

			lblIRVisible.Text = irstatus
		End Sub

		Private Sub UpdateTrackingUtilization()
			'area of ideal calibration coordinates (to match the screen)
			Dim idealArea As Single = (1 - 2*_calibrationMargin) * 1024 * (1 - 2*_calibrationMargin) * 768

			'area of quadrliatera
			Dim actualArea As Single = 0.5f * Math.Abs((_srcX(1) - _srcX(2)) * (_srcY(0) - _srcY(3)) - (_srcX(0) - _srcX(3)) * (_srcY(1) - _srcY(2)))

			Dim util As Single = (actualArea / idealArea)*100
			Dim utilstatus As String = "Tracking Utilization: " & util.ToString("f0") & "%"

			BeginInvoke(New UpdateTrackingUtilizationDelegate(AddressOf UpdateTrackingUtilizationUI), utilstatus)
		End Sub

		Private Sub UpdateTrackingUtilizationUI(ByVal utilStatus As String)
			lblTrackingUtil.Text = utilStatus
		End Sub

		Public Sub LoadCalibrationData()
			' create reader & open file
			Try
				Dim tr As TextReader = New StreamReader(CalibrationFilename)
				For i As Integer = 0 To 3
					_srcX(i) = Single.Parse(tr.ReadLine())
					_srcY(i) = Single.Parse(tr.ReadLine())
				Next i

				' close the stream
				tr.Close()
			Catch e1 As FileNotFoundException
				'no prexsting calibration
				Return
			End Try

			_warper.SetDestination(_screenSize.Width * _calibrationMargin, _screenSize.Height * _calibrationMargin, _screenSize.Width * (1.0f-_calibrationMargin), _screenSize.Height * _calibrationMargin, _screenSize.Width * _calibrationMargin, _screenSize.Height * (1.0f - _calibrationMargin), _screenSize.Width * (1.0f - _calibrationMargin), _screenSize.Height * (1.0f - _calibrationMargin))
			_warper.SetSource(_srcX(0), _srcY(0), _srcX(1), _srcY(1), _srcX(2), _srcY(2), _srcX(3), _srcY(3))

			_warper.ComputeWarp()
			_cursorControl = True

			cbCursorControl.Checked = _cursorControl

			UpdateTrackingUtilization()
		End Sub

		Public Sub SaveCalibrationData()
			Dim tw As TextWriter = New StreamWriter(CalibrationFilename)

			' write a line of text to the file
			For i As Integer = 0 To 3
				tw.WriteLine(_srcX(i))
				tw.WriteLine(_srcY(i))
			Next i

			' close the stream
			tw.Close()
		End Sub

		Public Sub DoCalibration()
			If cf Is Nothing Then
				Return
			End If

			Dim x As Integer
			Dim y As Integer
			Dim size As Integer = 25
			Dim p As New Pen(Color.Red)

			Select Case _calibrationState
				Case 1
					x = CInt(Fix(_screenSize.Width * _calibrationMargin))
					y = CInt(Fix(_screenSize.Height * _calibrationMargin))
					cf.ShowCalibration(x, y, size, p)
					_dstX(_calibrationState - 1) = x
					_dstY(_calibrationState - 1) = y
				Case 2
					x = _screenSize.Width - CInt(Fix(_screenSize.Width * _calibrationMargin))
					y = CInt(Fix(_screenSize.Height * _calibrationMargin))
					cf.ShowCalibration(x, y, size, p)
					_dstX(_calibrationState - 1) = x
					_dstY(_calibrationState - 1) = y
				Case 3
					x = CInt(Fix(_screenSize.Width * _calibrationMargin))
					y = _screenSize.Height -CInt(Fix(_screenSize.Height * _calibrationMargin))
					cf.ShowCalibration(x, y, size, p)
					_dstX(_calibrationState - 1) = x
					_dstY(_calibrationState - 1) = y
				Case 4
					x = _screenSize.Width - CInt(Fix(_screenSize.Width * _calibrationMargin))
					y = _screenSize.Height -CInt(Fix(_screenSize.Height * _calibrationMargin))
					cf.ShowCalibration(x, y, size, p)
					_dstX(_calibrationState - 1) = x
					_dstY(_calibrationState - 1) = y
				Case 5
					'compute warp
					_warper.SetDestination(_dstX(0), _dstY(0), _dstX(1), _dstY(1), _dstX(2), _dstY(2), _dstX(3), _dstY(3))
					_warper.SetSource(_srcX(0), _srcY(0), _srcX(1), _srcY(1), _srcX(2), _srcY(2), _srcX(3), _srcY(3))
					_warper.ComputeWarp()
					_calibrationState = 0
					_cursorControl = True
					BeginInvoke(CType(AddressOf AnonymousMethod2, MethodInvoker))
					SaveCalibrationData()
					UpdateTrackingUtilization()
				Case Else
			End Select
		End Sub
		Private Sub AnonymousMethod2()
			cf.Close()
			cbCursorControl.Checked = _cursorControl
		End Sub

		Private Sub ResetCursorSmoothing() 'reset smoothing data
			_smoothingCount = 0
			For i As Integer = 0 To 9
			   _smoothingX(i) = 0
			   _smoothingY(i) = 0
			Next i
		End Sub
	End Class
End Namespace