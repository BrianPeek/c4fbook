Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms

Namespace WiimoteWhiteboard
	Partial Public Class CalibrationForm
		Inherits Form
		Private _bmpCalibration As Bitmap
		Private _gfxCalibration As Graphics

		Public Sub New()
			InitializeComponent()

			Dim rect As Rectangle = Screen.GetWorkingArea(Me)

			Me.Size = New Size(rect.Width, rect.Height)
			Me.Text = "Calibration - Working area:" & Screen.GetWorkingArea(Me).ToString() & " || Real area: " & Screen.GetBounds(Me).ToString()

			_bmpCalibration = New Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb)
			_gfxCalibration = Graphics.FromImage(_bmpCalibration)
			pbCalibrate.Left = 0
			pbCalibrate.Top = 0
			pbCalibrate.Size = New Size(rect.Width, rect.Height)
			pbCalibrate.Image = _bmpCalibration

			_gfxCalibration.Clear(Color.White)
		End Sub

		Public Sub ShowCalibration(ByVal x As Integer, ByVal y As Integer, ByVal size As Integer, ByVal p As Pen)
			_gfxCalibration.Clear(Color.White)

			' draw crosshair
			_gfxCalibration.DrawEllipse(p, x - size \ 2, y - size \ 2, size, size)
			_gfxCalibration.DrawLine(p, x-size, y, x+size, y)
			_gfxCalibration.DrawLine(p, x, y-size, x, y+size)

			BeginInvoke(CType(AddressOf AnonymousMethod1, MethodInvoker))
		End Sub
		Private Sub AnonymousMethod1()
			pbCalibrate.Image = _bmpCalibration
		End Sub

		Private Sub CalibrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
			If e.KeyCode = Keys.Escape Then
				Me.Close()
			End If
		End Sub
	End Class
End Namespace