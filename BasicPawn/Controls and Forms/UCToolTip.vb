'BasicPawn
'Copyright(C) 2016 TheTimocop

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

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        TextEditorControl_ToolTip.IsReadOnly = True
    End Sub

    Private Sub Timer_Move_Tick(sender As Object, e As EventArgs) Handles Timer_Move.Tick
        Dim mCursorPoint As Point = Cursor.Position

        Dim mMePoint As Point = Me.PointToScreen(Point.Empty)

        If (g_mMoveLocation.Y < 500 AndAlso
            (mCursorPoint.X + g_iSizeSpace > mMePoint.X AndAlso mCursorPoint.Y + g_iSizeSpace > mMePoint.Y AndAlso
            mCursorPoint.X < mMePoint.X + Me.Width + g_iSizeSpace AndAlso mCursorPoint.Y < mMePoint.Y + Me.Height + g_iSizeSpace)) Then
            Timer_Move.Interval = g_iMoveSpeed

            g_mMoveLocation.Y += g_iMoveStep
            Me.Location = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

        ElseIf (g_mMoveLocation.Y > 0 AndAlso
                    Not (mCursorPoint.X + g_iSizeSpace + g_iMoveStep > mMePoint.X AndAlso mCursorPoint.Y + g_iSizeSpace + g_iMoveStep > mMePoint.Y AndAlso
                            mCursorPoint.X < mMePoint.X + Me.Width + g_iSizeSpace + g_iMoveStep AndAlso mCursorPoint.Y < mMePoint.Y + Me.Height + g_iSizeSpace + g_iMoveStep)) Then
            Timer_Move.Interval = g_iMoveSpeed

            g_mMoveLocation.Y -= g_iMoveStep
            g_mMoveLocation.Y = Math.Max(g_mMoveLocation.Y, 0)
            Me.Location = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

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
            g_mLocation = value
            Me.Location = value
            g_mMoveLocation = New Point()
        End Set
    End Property
End Class
