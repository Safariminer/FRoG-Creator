﻿Imports SFML.Window

Public Class GameDirection
    Private vector As Vector2f
    Private index As Byte

    Public Sub New(vector As Vector2f, index As Byte)
        Me.vector = vector
        Me.index = index
    End Sub

    Public Function GetVector()
        Return Me.vector
    End Function

    Public Function GetIndex()
        Return Me.index
    End Function

    Public Shared DOWN As New GameDirection(New Vector2f(0, 1), 0)
    Public Shared UP As New GameDirection(New Vector2f(0, -1), 3)
    Public Shared RIGHT As New GameDirection(New Vector2f(1, 0), 2)
    Public Shared LEFT As New GameDirection(New Vector2f(-1, 0), 1)
End Class