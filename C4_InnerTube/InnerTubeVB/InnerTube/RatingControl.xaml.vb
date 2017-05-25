Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes

Namespace InnerTube
	''' <summary>
	''' Interaction logic for RatingUserControl.xaml
	''' </summary>

	Partial Public Class RatingUserControl
		Inherits System.Windows.Controls.UserControl
		Public Sub New()
			InitializeComponent()
		End Sub


		Public Property StarRating() As Double
			Get
				Return CDbl(GetValue(StarRatingProperty))
			End Get
			Set(ByVal value As Double)
				'SetValue(StarRatingProperty, value);                
			End Set 'can't write custom logic in a Dependency Property setter
		End Property

		' Using a DependencyProperty as the backing store for StarRating.  This enables animation, styling, binding, etc...
        Public Shared ReadOnly StarRatingProperty As DependencyProperty = DependencyProperty.Register("StarRating", GetType(Double), GetType(RatingUserControl), New UIPropertyMetadata(0.0, AddressOf RatingUserControl.RatingPropertyChanged))


	   Private Shared Sub RatingPropertyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			'static method so must get object
			Dim rating As RatingUserControl = CType(d, RatingUserControl)

			Dim newRating As Double
			Dim test As Boolean = Double.TryParse(e.NewValue.ToString(), newRating)

			If test Then
				'You can only have a zero through 5 rating
				If newRating <= 5.0 AndAlso newRating >= 0.0 Then
					'set values
					rating.StarToolTip.Content = newRating.ToString()
					rating.SetStars(newRating)
				End If
			End If
	   End Sub



	   Private Sub SetStars(ByVal newRating As Double)
			Dim StarredIndex As Integer = CalculateIndex(newRating)

			'loop through all the star halves
			For i As Integer = 0 To StarStackPanel.Children.Count - 1
				Dim star As Path = CType(StarStackPanel.Children(i), Path)

				If i <= StarredIndex Then
					SetStyle(star, i, True)
				Else
					SetStyle(star, i, False)
				End If
			Next i

	   End Sub

		Private Sub SetStyle(ByVal star As Path, ByVal i As Integer, ByVal Selected As Boolean)
			'even numbers use "SelectedLeft" style
			If (i Mod 2) = 0 Then
				If Selected Then
					star.Style = CType(Me.Resources("SelectedLeft"), Style)
				Else
					star.Style = CType(Me.Resources("EmptyLeft"), Style)
				End If

			Else
				If Selected Then
					star.Style = CType(Me.Resources("SelectedRight"), Style)
				Else
					star.Style = CType(Me.Resources("EmptyRight"), Style)
				End If

			End If

		End Sub



		Private Function CalculateIndex(ByVal value As Double) As Integer
			'Get the ".yy" part of  "x.yy" (3.14)
			Dim actual = value - Math.Floor(value)

			'compare the newRating and choose to round 
			'to either: 0, 0.5, or 1
			If actual <.25 Then
				actual = 0
			ElseIf actual >=.25 AndAlso actual <.75 Then
				actual =.5
			ElseIf actual >=.75 Then
				actual = 1
			End If

			'add the base number to the rounded number and double
			'whole num + rounded value, then * 2 to get a valid index, 
			'then subtract by 1 (zero-based index)
			Dim newNum As Double = ((Math.Floor(value) + actual) * 2) - 1

			Return CInt(Fix(newNum))
		End Function


	End Class
End Namespace