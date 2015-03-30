﻿Imports TGUI
Imports SFML.Window
Imports SFML.Graphics

Public Class GameActionCase
    Inherits AnimatedPicture

    Private Const PADDING = 5

    Private Shared selectedGameActionCase As GameActionCase

    Private InDragAndDrop As Boolean
    Private isEmptyCase As Boolean
    Private canDragDrop As Boolean
    Private tmpLocation As Vector2f
    Private tmpPaddingSelection As Vector2f
    Private hitbox As IntRect
    Private cliped As Boolean
    Private caseParent As GameActionCase

    Public Sub New()
        Me.InDragAndDrop = False
        Me.isEmptyCase = True
        Me.canDragDrop = True
        Me.tmpLocation = Me.Position
        Me.tmpPaddingSelection = New Vector2f
        Me.hitbox = New IntRect(Me.Position.X, Me.Position.Y, Me.Size.X, Me.Size.Y)
        Me.cliped = False
    End Sub

    Private Sub GameActionCase_LeftMouseClick() Handles Me.LeftMouseClickedCallback
        If (Not IsEmpty()) Then
            ' Do Something
        End If
    End Sub

    Private Sub GameActionCase_LeftMousePressed() Handles Me.LeftMousePressedCallback
        If (Not IsEmpty()) Then
            If (CanDragAndDrop() And Not InDragAndDrop) Then
                Me.tmpPaddingSelection = New Vector2f(Mouse.GetPosition(Main.window).X - Me.Position.X, Mouse.GetPosition(Main.window).Y - Me.Position.Y)
                Me.tmpLocation = Me.Position
                Me.InDragAndDrop = True
                selectedGameActionCase = Me
            End If
        End If
    End Sub

    Private Sub GameActionCase_LeftMouseReleased() Handles Me.LeftMouseReleasedCallback
        If (Not Me.IsEmpty()) Then
            If (Me.CanDragAndDrop()) Then
                Me.InDragAndDrop = False

                If (Me.cliped) Then ' Si composant clipsé dans un autre
                    Me.tmpLocation = Me.Position
                    Me.IsEmpty = False
                    selectedGameActionCase.ParentCase = Me
                Else
                    Me.Position = Me.tmpLocation
                End If

                selectedGameActionCase = Nothing
            End If
        End If
    End Sub

    ''' <summary>
    ''' Déplace le composant vers la position spécifiée en prenant en compte la position relative de selection
    ''' </summary>
    ''' <param name="X">Position horizontale de la selection</param>
    ''' <param name="Y">Position verticale de la selection</param>
    ''' <remarks></remarks>
    Protected Sub Move(X As Integer, Y As Integer)
        Me.Position = New Vector2f(X - tmpPaddingSelection.X, Y - tmpPaddingSelection.Y)
    End Sub

    Protected Overrides Sub OnUpdate()
        MyBase.OnUpdate()

        ' Update DragAndDrop current location
        If (Me.InDragAndDrop And Not Me.cliped And Mouse.IsButtonPressed(Mouse.Button.Left) And Me.Focused) Then
            Me.Move(Mouse.GetPosition(Main.window).X, Mouse.GetPosition(Main.window).Y)
        End If

        If (Not Me.GetHitBox.Contains(Mouse.GetPosition(Main.window).X, Mouse.GetPosition(Main.window).Y)) Then
            If (Not IsNothing(selectedGameActionCase)) Then
                If (Not selectedGameActionCase.Equals(Me)) Then
                    selectedGameActionCase.cliped = False

                    Me.IsEmpty = True
                    selectedGameActionCase.ParentCase = Nothing
                End If
            End If
        Else
            If (Me.IsEmpty And Mouse.IsButtonPressed(Mouse.Button.Left)) Then
                If (Not IsNothing(selectedGameActionCase)) Then
                    If (Not selectedGameActionCase.Equals(Me) And Not selectedGameActionCase.cliped) Then
                        selectedGameActionCase.cliped = True
                        selectedGameActionCase.Position = Me.Position
                    End If
                End If
            End If
        End If
    End Sub

    Public Function GetHitBox() As IntRect
        Me.hitbox.Left = Me.Position.X
        Me.hitbox.Top = Me.Position.Y
        Me.hitbox.Width = Me.Size.X
        Me.hitbox.Height = Me.Size.Y
        Return Me.hitbox
    End Function

    ''' <summary>
    ''' Défini ou obtient l'état du composant : Possibilité d'aquérir ou non un composant par Glisser Et Déposer
    ''' </summary>
    ''' <value>True si composant parent, False si composant enfant</value>
    ''' <returns>True si composant parent, False si composant enfant</returns>
    ''' <remarks></remarks>
    Public Property IsEmpty() As Boolean
        Get
            Return Me.isEmptyCase
        End Get
        Set(value As Boolean)
            Me.isEmptyCase = value
            If (value) Then
                Me.MoveToBack()
            End If
        End Set
    End Property

    Public Property CanDragAndDrop() As Boolean
        Get
            Return Me.canDragDrop
        End Get
        Set(value As Boolean)
            Me.canDragDrop = value
        End Set
    End Property

    Public Property ParentCase() As GameActionCase
        Get
            Return Me.caseParent
        End Get
        Private Set(value As GameActionCase)
            Me.caseParent = value
        End Set
    End Property

    Public Overrides Property Size As Vector2f
        Get
            Return MyBase.Size
        End Get
        Set(value As Vector2f)
            MyBase.Size = value
        End Set
    End Property

    Public Overrides Property Position As Vector2f
        Get
            Return MyBase.Position
        End Get
        Set(value As Vector2f)
            MyBase.Position = value
        End Set
    End Property
End Class
