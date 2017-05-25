Imports Microsoft.VisualBasic
Imports System
Namespace WiimoteWhiteboard
	Public Class Warper
		Private _srcX(3) As Single
		Private _srcY(3) As Single
		Private _dstX(3) As Single
		Private _dstY(3) As Single
		Private _srcMat(15) As Single
		Private _dstMat(15) As Single
		Private _warpMat(15) As Single
		Private _dirty As Boolean

		Public Sub New()
			SetIdentity()
		End Sub

		Public Sub SetIdentity()
			SetSource(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f)

			SetDestination(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f)
			ComputeWarp()
		End Sub

		Public Sub SetSource(ByVal x0 As Single, ByVal y0 As Single, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal x3 As Single, ByVal y3 As Single)
			_srcX(0) = x0
			_srcY(0) = y0
			_srcX(1) = x1
			_srcY(1) = y1
			_srcX(2) = x2
			_srcY(2) = y2
			_srcX(3) = x3
			_srcY(3) = y3
			_dirty = True
		End Sub

		Public Sub SetDestination(ByVal x0 As Single, ByVal y0 As Single, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal x3 As Single, ByVal y3 As Single)
			_dstX(0) = x0
			_dstY(0) = y0
			_dstX(1) = x1
			_dstY(1) = y1
			_dstX(2) = x2
			_dstY(2) = y2
			_dstX(3) = x3
			_dstY(3) = y3
			_dirty = True
		End Sub

		Public Sub ComputeWarp()
			ComputeSquareToQuad(_dstX(0), _dstY(0), _dstX(1), _dstY(1), _dstX(2), _dstY(2), _dstX(3), _dstY(3), _dstMat)
			ComputeQuadToSquare(_srcX(0), _srcY(0), _srcX(1), _srcY(1), _srcX(2), _srcY(2), _srcX(3), _srcY(3), _srcMat)
			MultiplyMatrices(_srcMat, _dstMat, _warpMat)
			_dirty = False
		End Sub

		Public Sub MultiplyMatrices(ByVal srcMat() As Single, ByVal dstMat() As Single, ByVal resMat() As Single)
			' [ a b ]   [w x]   [(aw + by) (ax + bz)] 
			' [ c d ] * [y z] = [(cw + dy) (cz + dz)]
			For row As Integer = 0 To 3
				Dim rowIndex As Integer = row * 4
				For col As Integer = 0 To 3
					resMat(rowIndex + col) = (srcMat(rowIndex) * dstMat(col) + srcMat(rowIndex + 1) * dstMat(col + 4) + srcMat(rowIndex + 2) * dstMat(col + 8) + srcMat(rowIndex + 3) * dstMat(col + 12))
				Next col
			Next row
		End Sub

		Public Sub ComputeSquareToQuad(ByVal x0 As Single, ByVal y0 As Single, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal x3 As Single, ByVal y3 As Single, ByVal mat() As Single)

			Dim dx1 As Single = x1 - x2, dy1 As Single = y1 - y2
			Dim dx2 As Single = x3 - x2, dy2 As Single = y3 - y2
			Dim sx As Single = x0 - x1 + x2 - x3
			Dim sy As Single = y0 - y1 + y2 - y3
			Dim g As Single = (sx * dy2 - dx2 * sy) / (dx1 * dy2 - dx2 * dy1)
			Dim h As Single = (dx1 * sy - sx * dy1) / (dx1 * dy2 - dx2 * dy1)
			Dim a As Single = x1 - x0 + g * x1
			Dim b As Single = x3 - x0 + h * x3
			Dim c As Single = x0
			Dim d As Single = y1 - y0 + g * y1
			Dim e As Single = y3 - y0 + h * y3
			Dim f As Single = y0

			mat(0) = a
			mat(1) = d
			mat(2) = 0
			mat(3) = g
			mat(4) = b
			mat(5) = e
			mat(6) = 0
			mat(7) = h
			mat(8) = 0
			mat(9) = 0
			mat(10) = 1
			mat(11) = 0
			mat(12) = c
			mat(13) = f
			mat(14) = 0
			mat(15) = 1
		End Sub

		Public Sub ComputeQuadToSquare(ByVal x0 As Single, ByVal y0 As Single, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal x3 As Single, ByVal y3 As Single, ByVal mat() As Single)
			ComputeSquareToQuad(x0, y0, x1, y1, x2, y2, x3, y3, mat)

			' invert through adjoint
			' ignore 3rd column
			Dim a As Single = mat(0), d As Single = mat(1), g As Single = mat(3)
			Dim b As Single = mat(4), e As Single = mat(5), h As Single = mat(7)
			' ignore 3rd row 
			Dim c As Single = mat(12), f As Single = mat(13)

			Dim a1 As Single = e - f * h
			Dim b1 As Single = c * h - b
			Dim c1 As Single = b * f - c * e
			Dim d1 As Single = f * g - d
			Dim e1 As Single = a - c * g
			Dim f1 As Single = c * d - a * f
			Dim g1 As Single = d * h - e * g
			Dim h1 As Single = b * g - a * h
			Dim i1 As Single = a * e - b * d

			Dim idet As Single = 1.0f / (a * a1 + b * d1 + c * g1)

			mat(0) = a1 * idet
			mat(1) = d1 * idet
			mat(2) = 0
			mat(3) = g1 * idet
			mat(4) = b1 * idet
			mat(5) = e1 * idet
			mat(6) = 0
			mat(7) = h1 * idet
			mat(8) = 0
			mat(9) = 0
			mat(10) = 1
			mat(11) = 0
			mat(12) = c1 * idet
			mat(13) = f1 * idet
			mat(14) = 0
			mat(15) = i1 * idet
		End Sub

		Public Function GetWarpMatrix() As Single()
			Return _warpMat
		End Function

		Public Sub Warp(ByVal srcX As Single, ByVal srcY As Single, ByRef dstX As Single, ByRef dstY As Single)
			If _dirty Then
				ComputeWarp()
			End If

			Warp(_warpMat, srcX, srcY, dstX, dstY)
		End Sub

		Public Shared Sub Warp(ByVal mat() As Single, ByVal srcX As Single, ByVal srcY As Single, ByRef dstX As Single, ByRef dstY As Single)
			Dim result(3) As Single
			Dim z As Single = 0
			result(0) = srcX * mat(0) + srcY*mat(4) + z*mat(8) + mat(12)
			result(1) = srcX * mat(1) + srcY*mat(5) + z*mat(9) + mat(13)
			result(2) = srcX * mat(2) + srcY*mat(6) + z*mat(10) + mat(14)
			result(3) = srcX * mat(3) + srcY*mat(7) + z*mat(11) + mat(15)
			dstX = result(0)/result(3)
			dstY = result(1)/result(3)
		End Sub
	End Class
End Namespace
