﻿#ExternalChecksum("..\..\MainWindow.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","5C98FF1305DAF68DE5602243BA343584")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes

Namespace PeerCast
    
    '''<summary>
    '''MainWindow
    '''</summary>
    <Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
    Partial Public Class MainWindow
        Inherits System.Windows.Window
        Implements System.Windows.Markup.IComponentConnector
        
        
        #ExternalSource("..\..\MainWindow.xaml",20)
        Friend WithEvents Login As System.Windows.Controls.Grid
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",34)
        Friend WithEvents AppModeLabel As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",37)
        Friend WithEvents NetworkNameLabel As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",38)
        Friend WithEvents NetworkName As System.Windows.Controls.TextBox
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",41)
        Friend WithEvents PasswordLabel As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",42)
        Friend WithEvents Password As System.Windows.Controls.TextBox
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",45)
        Friend WithEvents SignInButton As System.Windows.Controls.Button
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",47)
        Friend WithEvents progressBar1 As System.Windows.Controls.ProgressBar
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",48)
        Friend WithEvents StatusValue As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",53)
        Friend WithEvents MessageCell As System.Windows.Controls.StackPanel
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",55)
        Friend WithEvents Messages As System.Windows.Controls.ListBox
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",56)
        Friend WithEvents ClearMessages As System.Windows.Controls.Button
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",60)
        Friend WithEvents ClientControls As System.Windows.Controls.Grid
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",73)
        Friend WithEvents FilesToPlay As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",74)
        Friend WithEvents MediaList As System.Windows.Controls.ListBox
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",88)
        Friend WithEvents GetList As System.Windows.Controls.Button
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",89)
        Friend WithEvents Play As System.Windows.Controls.Button
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",90)
        Friend WithEvents FullScreen As System.Windows.Controls.Button
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",94)
        Friend WithEvents ServerControls As System.Windows.Controls.StackPanel
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",95)
        Friend WithEvents OpenDialog As System.Windows.Controls.Button
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",96)
        Friend WithEvents ChooseDirectory As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",97)
        Friend WithEvents FilePath As System.Windows.Controls.Label
        
        #End ExternalSource
        
        
        #ExternalSource("..\..\MainWindow.xaml",100)
        Friend WithEvents MediaPlayer As System.Windows.Controls.MediaElement
        
        #End ExternalSource
        
        Private _contentLoaded As Boolean
        
        '''<summary>
        '''InitializeComponent
        '''</summary>
        <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
            If _contentLoaded Then
                Return
            End If
            _contentLoaded = true
            Dim resourceLocater As System.Uri = New System.Uri("/PeerCast;component/mainwindow.xaml", System.UriKind.Relative)
            
            #ExternalSource("..\..\MainWindow.xaml",1)
            System.Windows.Application.LoadComponent(Me, resourceLocater)
            
            #End ExternalSource
        End Sub
        
        <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
         System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")>  _
        Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
            If (connectionId = 1) Then
                Me.Login = CType(target,System.Windows.Controls.Grid)
                Return
            End If
            If (connectionId = 2) Then
                Me.AppModeLabel = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 3) Then
                Me.NetworkNameLabel = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 4) Then
                Me.NetworkName = CType(target,System.Windows.Controls.TextBox)
                Return
            End If
            If (connectionId = 5) Then
                Me.PasswordLabel = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 6) Then
                Me.Password = CType(target,System.Windows.Controls.TextBox)
                Return
            End If
            If (connectionId = 7) Then
                Me.SignInButton = CType(target,System.Windows.Controls.Button)
                
                #ExternalSource("..\..\MainWindow.xaml",45)
                AddHandler Me.SignInButton.Click, New System.Windows.RoutedEventHandler(AddressOf Me.SignInButton_Click)
                
                #End ExternalSource
                Return
            End If
            If (connectionId = 8) Then
                Me.progressBar1 = CType(target,System.Windows.Controls.ProgressBar)
                Return
            End If
            If (connectionId = 9) Then
                Me.StatusValue = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 10) Then
                Me.MessageCell = CType(target,System.Windows.Controls.StackPanel)
                Return
            End If
            If (connectionId = 11) Then
                Me.Messages = CType(target,System.Windows.Controls.ListBox)
                Return
            End If
            If (connectionId = 12) Then
                Me.ClearMessages = CType(target,System.Windows.Controls.Button)
                
                #ExternalSource("..\..\MainWindow.xaml",56)
                AddHandler Me.ClearMessages.Click, New System.Windows.RoutedEventHandler(AddressOf Me.ClearMessages_Click)
                
                #End ExternalSource
                Return
            End If
            If (connectionId = 13) Then
                Me.ClientControls = CType(target,System.Windows.Controls.Grid)
                Return
            End If
            If (connectionId = 14) Then
                Me.FilesToPlay = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 15) Then
                Me.MediaList = CType(target,System.Windows.Controls.ListBox)
                Return
            End If
            If (connectionId = 16) Then
                Me.GetList = CType(target,System.Windows.Controls.Button)
                
                #ExternalSource("..\..\MainWindow.xaml",88)
                AddHandler Me.GetList.Click, New System.Windows.RoutedEventHandler(AddressOf Me.GetList_Click)
                
                #End ExternalSource
                Return
            End If
            If (connectionId = 17) Then
                Me.Play = CType(target,System.Windows.Controls.Button)
                
                #ExternalSource("..\..\MainWindow.xaml",89)
                AddHandler Me.Play.Click, New System.Windows.RoutedEventHandler(AddressOf Me.Play_Click)
                
                #End ExternalSource
                Return
            End If
            If (connectionId = 18) Then
                Me.FullScreen = CType(target,System.Windows.Controls.Button)
                
                #ExternalSource("..\..\MainWindow.xaml",90)
                AddHandler Me.FullScreen.Click, New System.Windows.RoutedEventHandler(AddressOf Me.FullScreen_Click)
                
                #End ExternalSource
                Return
            End If
            If (connectionId = 19) Then
                Me.ServerControls = CType(target,System.Windows.Controls.StackPanel)
                Return
            End If
            If (connectionId = 20) Then
                Me.OpenDialog = CType(target,System.Windows.Controls.Button)
                
                #ExternalSource("..\..\MainWindow.xaml",95)
                AddHandler Me.OpenDialog.Click, New System.Windows.RoutedEventHandler(AddressOf Me.OpenDialog_Click)
                
                #End ExternalSource
                Return
            End If
            If (connectionId = 21) Then
                Me.ChooseDirectory = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 22) Then
                Me.FilePath = CType(target,System.Windows.Controls.Label)
                Return
            End If
            If (connectionId = 23) Then
                Me.MediaPlayer = CType(target,System.Windows.Controls.MediaElement)
                Return
            End If
            Me._contentLoaded = true
        End Sub
    End Class
End Namespace
