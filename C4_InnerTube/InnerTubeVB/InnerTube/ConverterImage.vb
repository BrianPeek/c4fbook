Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Data
Imports System.IO
Imports SharedUtilities


Namespace InnerTube
    <ValueConversion(GetType(Object), GetType(String))> _
    Public Class ConverterImage
        Implements IValueConverter
        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Dim path As String = CStr(value)
            Return SetImage(path)
        End Function

        Public Shared Function SetImage(ByVal imagePath As String) As Object
            'if the path doesn't exist
            If String.IsNullOrEmpty(imagePath) OrElse ((Not File.Exists(imagePath))) Then
                'use default image
                Return Path.Combine(App.Settings.SubPath, FileHelper.DefaultImage)
            Else
                Return imagePath
            End If
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            ' we don't intend this to ever be called
            Return Nothing
        End Function
    End Class




End Namespace
