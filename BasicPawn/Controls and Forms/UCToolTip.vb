﻿'BasicPawn
'Copyright(C) 2017 TheTimocop

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program. If Not, see < http: //www.gnu.org/licenses/>.


Public Class UCToolTip
    Private g_mFormMain As FormMain

    Private g_mLocation As Point
    Private g_mMoveLocation As Point
    Private g_iMoveStep As Integer = 16
    Private g_iSizeSpace As Integer = 8
    Private g_iMoveSpeed As Integer = 10
    Private g_iIdleSpeed As Integer = 100

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        TextEditorControl_ToolTip.IsReadOnly = True
        TextEditorControl_ToolTip.ActiveTextAreaControl.TextEditorProperties.IndentationSize = 0
        TextEditorControl_ToolTip.ActiveTextAreaControl.AutoScroll = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.AutoSize = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.HScrollBar.Visible = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.VScrollBar.Visible = False

        If (ClassTools.ClassOperatingSystem.GetWineVersion Is Nothing) Then
            'Block the Text Editor, but still make it visible
            Dim mAlphaPanel As New ClassPanelAlpha With {
                .Parent = Me,
                .Dock = DockStyle.Fill,
                .m_Opacity = 0,
                .m_TransparentBackColor = Color.White
            }
            Me.Controls.Add(mAlphaPanel)
            mAlphaPanel.BringToFront()
        End If

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Timer_Move_Tick(sender As Object, e As EventArgs) Handles Timer_Move.Tick
        If (MoveWindow(Not ClassSettings.g_iSettingsUseWindowsToolTipAnimations)) Then
            Timer_Move.Interval = g_iMoveSpeed
        Else
            Timer_Move.Interval = g_iIdleSpeed
        End If
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)

        Timer_Move.Enabled = Me.Visible
    End Sub

    Property m_Location As Point
        Get
            Return g_mLocation
        End Get
        Set(value As Point)
            Dim bWasVisible = Me.Visible
            Me.Visible = False

            g_mLocation = value
            Me.Location = value
            g_mMoveLocation = New Point()
            MoveWindow(True)

            Me.Visible = bWasVisible
        End Set
    End Property

    Private Function MoveWindow(bInstant As Boolean) As Boolean
        Dim bMoved As Boolean = False

        While True
            Dim mCursorPoint As Point = Cursor.Position

            Dim mMePoint As Point = Me.PointToScreen(Point.Empty)

            If (g_mMoveLocation.Y < 500 AndAlso
                    (mCursorPoint.X + g_iSizeSpace > mMePoint.X AndAlso mCursorPoint.Y + g_iSizeSpace > mMePoint.Y AndAlso
                        mCursorPoint.X < mMePoint.X + Me.Width + g_iSizeSpace AndAlso mCursorPoint.Y < mMePoint.Y + Me.Height + g_iSizeSpace)) Then
                g_mMoveLocation.Y += g_iMoveStep

                Me.Location = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

                bMoved = True

                If (bInstant) Then
                    Continue While
                End If

            ElseIf (g_mMoveLocation.Y > 0 AndAlso
                        Not (mCursorPoint.X + g_iSizeSpace + g_iMoveStep > mMePoint.X AndAlso mCursorPoint.Y + g_iSizeSpace + g_iMoveStep > mMePoint.Y AndAlso
                                mCursorPoint.X < mMePoint.X + Me.Width + g_iSizeSpace + g_iMoveStep AndAlso mCursorPoint.Y < mMePoint.Y + Me.Height + g_iSizeSpace + g_iMoveStep)) Then
                g_mMoveLocation.Y -= g_iMoveStep
                g_mMoveLocation.Y = Math.Max(g_mMoveLocation.Y, 0)

                Me.Location = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

                bMoved = True

                If (bInstant) Then
                    Continue While
                End If
            End If

            Exit While
        End While

        Return bMoved
    End Function

    Private Sub TextEditorControl_ToolTip_TextChanged(sender As Object, e As EventArgs) Handles TextEditorControl_ToolTip.TextChanged
        'TODO: Better DPI, Border detection, or size in general
        Dim textSize = TextRenderer.MeasureText(TextEditorControl_ToolTip.Document.TextContent, TextEditorControl_ToolTip.ActiveTextAreaControl.Font)
        Me.Width = CInt(textSize.Width * 1.25)
        Me.Height = CInt(textSize.Height * 1.25)
    End Sub
End Class
